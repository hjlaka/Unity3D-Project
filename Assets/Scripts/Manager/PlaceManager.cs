using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlaceManager : SingleTon<PlaceManager>
{
    [SerializeField]
    private Piece selectedPiece;

    [SerializeField]
    private CameraController camController;


    public UnityEvent OnSelectPiece;
    public UnityEvent OnNonSelectPiece;
    public UnityEvent OnFinishMove;

    public enum PlaceType { DEFENCE, ATTACK, MOVABLE, NORMAL, SPECIALMOVE}

    private Coroutine waitToInit;
    private Coroutine showEnd;
    public Piece SelectedPiece
    {
        get { return selectedPiece; }
        set 
        { 
            selectedPiece = value;

            if (value != null && SelectedPiece.place.board?.heatPointHUD != null)
            {
                SelectedPiece.place.board.heatPointHUD.gameObject.SetActive(true);

            }
                
        }
    }

    public UnityEvent OnExitSelect;

    public Place selectedPlace;


    [SerializeField]
    private Color highlight;
    [SerializeField]
    private Color attackable;


    public void ShowHUD()
    {
        Transform hud = SelectedPiece.place.board.heatPointHUD;
        if(hud != null) hud.gameObject.SetActive(true);
    }
    public void ShowMovable(Piece piece)
    {
        Place curPlace = piece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;
       
        if (null == curBoard)                   // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
            return;

        if (!curBoard.FollowRule)               // ��Ģ�� ������ �ʴ� ������ ����
            return;

        // ����
        List<Place> movable = selectedPiece.MovableTo;
        for(int i = 0; i < movable.Count; i++)
        {
            ChangePlaceColor(movable[i].boardIndex, PlaceType.MOVABLE);
        }
    }

    public void ShowInfluence(Piece piece)
    {
        Place curPlace = selectedPiece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;

        if (null == curBoard)                   // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
            return;

        if (!curBoard.FollowRule)               // ��Ģ�� ������ �ʴ� ������ ����
            return;

        // ����
        List<Place> influencing = piece.Influenceable;
        for (int i = 0; i < influencing.Count; i++)
        {
            //TODO: ������� ��Ÿ���� �� ���� ����
            ChangePlaceColor(influencing[i].boardIndex, PlaceType.MOVABLE);
        }
    }

    public void ShowThreatAndDefence(Piece piece)
    {
        Place curPlace = piece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;

        if (null == curBoard)                   // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
            return;

        if (!curBoard.FollowRule)               // ��Ģ�� ������ �ʴ� ������ ����
            return;

        // ����
        List<Piece> defencing = selectedPiece.DefendFor;
        List<Piece> threating = selectedPiece.ThreatTo;

        for (int i = 0; i < defencing.Count; i++)
        {
            // �ٸ� ����� ��ġ�� ����� �� ���� ���� �� ����
            ChangePlaceColor(defencing[i].place.boardIndex, PlaceType.DEFENCE);
            //defencing[i].place.ChangeColor();
        }
        for (int i = 0; i < threating.Count; i++)
        {
            ChangePlaceColor(threating[i].place.boardIndex, PlaceType.ATTACK);
            //threating[i].place.ChangeColor();
        }

    }

    public void PostPlaceAction()
    {
        Place newPlace = selectedPiece.place;
        Vector2Int newIndex = newPlace.boardIndex;
        Board curBoard = newPlace.board;

        // ���ο� �ڸ� ������ �߰�
        if (selectedPiece.team.direction == TeamData.Direction.DownToUp)
            newPlace.HeatPointBottomTeam++;
        else
            newPlace.HeatPointTopTeam++;

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        // ��Ģ�� ������ �ʴ� ������ ����
        if (!curBoard.FollowRule)
            return;

        selectedPiece.IsMovable(selectedPiece.place.boardIndex);

    }

    private void ShowMovableEnd(Piece endedPiece)
    {
        List<Place> movableList = endedPiece.MovableTo;
        for (int i = 0; i < movableList.Count; i++)
        {
            movableList[i].ChangeColor();
        }
    }
    private void ShowInfluenceEnd(Piece endedPiece)
    {
        List<Place> influenceList = endedPiece.Influenceable;
        for (int i = 0; i < influenceList.Count; i++)
        {
            influenceList[i].ChangeColor();
        }
    }
    private void ShowThreatAndDefenceEnd(Piece endedPiece)
    {
        List<Piece> defeceList = endedPiece.DefendFor;
        List<Piece> threatList = endedPiece.ThreatTo;

        for (int i = 0; i < defeceList.Count; i++)
        {
            defeceList[i].place.ChangeColor();
        }

        for (int i = 0; i < threatList.Count; i++)
        {
            threatList[i].place.ChangeColor();
        }
    }

    public void WithDrawInfluence(Piece leftPiece)
    {
        //TODO: ����ȭ
        if (leftPiece.team.direction == TeamData.Direction.DownToUp)
        {
            leftPiece.place.HeatPointBottomTeam--;
        }
        else
        {
            leftPiece.place.HeatPointTopTeam--;
        }
        

        List<Place> influencable = leftPiece.Influenceable;

        for (int i = 0; i < influencable.Count; i++)
        {
            if (leftPiece.team.direction == TeamData.Direction.DownToUp)
            {
                influencable[i].HeatPointBottomTeam--;
            }
            else
            {
                influencable[i].HeatPointTopTeam--;
            }
        }

    }

    public void ChangePlaceColor(Vector2Int location, PlaceType placeType)
    {
        switch(placeType)
        {
            case PlaceType.DEFENCE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.blue);
                break;

            case PlaceType.ATTACK:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.red);
                break;

            case PlaceType.NORMAL:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor();
                break;

            case PlaceType.MOVABLE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(highlight);
                break;

            case PlaceType.SPECIALMOVE:
                selectedPiece.place.board.places[location.x, location.y].ChangeColor(Color.gray);
                break;
        }
    }

    public void MovePieceTo(Place place)
    {
        Place oldPlace = selectedPiece.place;
        oldPlace.piece = null;

        // ���� ǥ�õ� ���� ����� ����
        PreShowEnd(selectedPiece);

        // ���� ����� ����
        WithDrawInfluence(SelectedPiece);
        selectedPiece.ClearMovable();

        // ����� �� �� ���� ���׿� ���� �����ϱ�
        // =====�ӽ� ����======

        selectedPiece.ClearThreat();
        selectedPiece.ClearDefence();
        selectedPiece.ClearInfluence();

        // ===================



        // Debug.Log(selectedPiece.MovableTo.Count.ToString());

        
        selectedPiece.SetInPlace(place);    // �⹰�� ��� ��ġ �����




        // ��ġ ���� �� ����� ����
        PostPlaceAction();

        place.board?.UpdateHeatHUD();

        // ����� ����
        PostShow(selectedPiece);

        // ī�޶� ����
        OnFinishMove?.Invoke();

        // ���� ������ �̺�Ʈ�� ������ ����
        DialogueManager.Instance.StartDialogue();


        // ���� �⹰ ����
        Piece endedPiece = selectedPiece;




        StartCoroutine(EndTurn(endedPiece));        // ���� ������ ������ �������� ����, ����ؼ� Ȯ��

    }

    private IEnumerator EndTurn(Piece endedPiece)
    {
        // �⺻ ��� �ð�
        yield return new WaitForSeconds(1.5f);

        while (GameManager.Instance.state != GameManager.GameState.TURN_FINISHED)
        {
            yield return null;

            // ���ʰ� �����ٴ� ��ȣ�� ������ ��ٸ���.
        }
        Debug.Log("���� ���� ���� �Ǿ�����");

        SelectedPieceInit();

        //yield return new WaitForSeconds(1f);
        //PostShowEnd(endedPiece);
        ShowMovableEnd(endedPiece);
        ShowInfluenceEnd(endedPiece);

        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        // ī�޶� ����
        OnNonSelectPiece?.Invoke();
    }


    private void PostShow(Piece finishedPiece)
    {
        ShowInfluence(finishedPiece);
        ShowThreatAndDefence(finishedPiece);
    }

    private void PreShow(Piece seleceted)
    {
        ShowMovable(seleceted);
        //ShowInfluence(seleceted);
        ShowThreatAndDefence(seleceted);
    }
    private void PreShowEnd(Piece endedPiece)
    {
        //ShowInfluence(endedPiece);
        ShowMovableEnd(endedPiece);
        ShowThreatAndDefenceEnd(endedPiece);
    }

    private IEnumerator PostShowEnd(Piece endedPiece)
    {
        yield return new WaitForSeconds(1f);
        //yield return null;

        ShowMovableEnd(endedPiece);
        //ShowThreatAndDefenceEnd(endedPiece);
        ShowInfluenceEnd(endedPiece);
    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;

        // ����
        PreShow(piece);

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // ���õ� �⹰�� �ٷ� ����ϴ� ���

        //����
        PreShowEnd(selectedPiece);

        SelectedPieceInit();
    }

    public void SelectedPieceInit()
    {
        
        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;

        //StartCoroutine(EndPlaceCam());
        
    }

    private IEnumerator EndPlaceCam()
    {
        yield return new WaitForSeconds(1f);

        OnNonSelectPiece?.Invoke();
    }
}
