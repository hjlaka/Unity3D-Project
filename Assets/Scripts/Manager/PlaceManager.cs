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
    public void ShowPlaceable()
    {
        Place curPlace = selectedPiece.place;
        Vector2Int curIndex = curPlace.boardIndex;
        Board curBoard = curPlace.board;
       
        if (null == curBoard)                   // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
            return;

        if (!curBoard.FollowRule)               // ��Ģ�� ������ �ʴ� ������ ����
            return;


    }

    public void PostPlaceAction()
    {
        Place newPlace = selectedPiece.place;
        Vector2Int newIndex = newPlace.boardIndex;
        Board curBoard = newPlace.board;

        // �⹰�� �ִ� ���� ���尡 �ƴ϶�� ����
        if (null == curBoard)
            return;

        // ��Ģ�� ������ �ʴ� ������ ����
        if (!curBoard.FollowRule)
            return;

        selectedPiece.IsMovable(selectedPiece.place.boardIndex);

       
    }


    public void ShowPlaceableEnd(Piece endedPiece)
    {
        List<Place> movableList = endedPiece.MovableTo;
        List<Piece> defeceList = endedPiece.DefendFor;
        List<Piece> threatList = endedPiece.ThreatTo;

        for(int i = 0; i < movableList.Count; i++)
        {
            movableList[i].ChangeColor();
        }

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
        List<Place> movableList = leftPiece.MovableTo;
        for (int i = 0; i < movableList.Count; i++)
        {
            movableList[i].HeatPoint--;
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

        // ���� ǥ�õ� ���� �����
        ShowPlaceableEnd(SelectedPiece);

        // ���� ����� ����
        WithDrawInfluence(SelectedPiece);
        selectedPiece.ClearMovable();

        // ���� �ڸ� �⹰ ������ ����
        Debug.Log("�����ڸ�:" + oldPlace + " ������: " + oldPlace.HeatPoint);
        oldPlace.HeatPoint--;

        // Debug.Log(selectedPiece.MovableTo.Count.ToString());

        


        
        selectedPiece.SetInPlace(place);    // �⹰�� ��� ��ġ �����
        place.piece = selectedPiece;


        // ���ο� �ڸ� ������ �߰�
        place.HeatPoint++;

        // ��ġ ���� �� ����� ����
        PostPlaceAction();

        


        // ���� �⹰ ����
        Piece endedPiece = selectedPiece;

        // ���� �ʱ�ȭ
        SelectedPieceInit();

        //OnFinishMove?.Invoke();
        place.board.UpdateHeatHUD();

        // ���� ����
        //OnFinishMove?.Invoke(endedPiece);


        waitToInit = StartCoroutine(PostInfluenceShowEnd(endedPiece));
        //CancleSelectPiece();


    }


    private IEnumerator PostInfluenceShowEnd(Piece endedPiece)
    {
        yield return new WaitForSeconds(1f);

        ShowPlaceableEnd(endedPiece);

    }

    public void SelectPiece(Piece piece)
    {
        SelectedPiece = piece;
        ShowPlaceable();
        GameManager.Instance.state = GameManager.GameState.SELECTING_PLACE;

        OnSelectPiece?.Invoke();

    }

    public void CancleSelectPiece()
    {
        if (null == selectedPiece) return;

        // ���õ� �⹰�� �ٷ� ����ϴ� ���
        ShowPlaceableEnd(selectedPiece);
        SelectedPieceInit();
    }

    public void SelectedPieceInit()
    {
        
        SelectedPiece = null;
        GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;


        OnNonSelectPiece?.Invoke();
    }
}
