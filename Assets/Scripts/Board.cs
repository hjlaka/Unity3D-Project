using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Vector2Int size;
    public Vector2Int Size
    {
        get { return size; }
        set { size = value; }
    }


    public enum PlaceType { DEFENCE, ATTACK, MOVABLE, NORMAL, SPECIALMOVE }
    private bool followRule;
    public bool FollowRule { get { return followRule; } set { followRule = value; } }
    public Transform heatPointHUD;
    private List<TextMeshProUGUI> heatHUDList;
    public Place[,] places;
    private float cellSize;

    private void Awake()
    {
        heatHUDList = new List<TextMeshProUGUI>();
        
    }
    private void Start()
    {

        if (null == heatPointHUD)
            return;

        CreateHUD();
        UpdateHeatHUD();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetHeat();
        }
    }
    public void ResetHeat()
    {
        if (null == heatPointHUD)
            return;


        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                places[i, j].HeatPoint = 0;
                heatHUDList[(i * size.x) + j].text = "0";
            }
        }
    }

    private void CreateHUD()
    {
        heatPointHUD.GetComponent<RectTransform>().sizeDelta = new Vector2(20 * size.x, 20 * size.y);

        for (int i = 0; i < size.x * size.y; i++)
        {
            GameObject textUI = Instantiate(new GameObject());
            textUI.transform.SetParent(heatPointHUD.transform, false);
            textUI.AddComponent<TextMeshProUGUI>();
            TextMeshProUGUI text = textUI.GetComponent<TextMeshProUGUI>();
            text.text = i.ToString();
            text.fontSize = 20f;
            heatHUDList.Add(text);
        }

        heatPointHUD.gameObject.SetActive(false);
    }
    public void UpdateHeatHUD()
    {
        if (null == heatPointHUD)
            return;

        for (int i = 0; i < size.x; i++)
        {
            for(int j = 0; j < size.y; j++)
            {
                heatHUDList[(i * size.x) + j].text = places[i, j].HeatPoint.ToString();
            }
        }
    }

   /* public void MovePieceTo(Piece piece, Place place)
    {
        Place oldPlace = piece.place;
        oldPlace.piece = null;

        // 움직일 수 있는 곳인가?

        // 보드 위 연출 지우기
        PreShowEnd(piece);

        // 영향력 제거

        // 기물의 이전 영향권 리스트 초기화

        // 기물의 위치 이동
        piece.SetInPlace(place);

        // 기물의 새로운 영향권 리스트 만들기

        // 보드 위 연출 생성
        PostShow(piece);
        UpdateHeatHUD();

        // 카메라 연출까지 여기에서? - 매니저?
        // 진행할 이벤트가 있으면 실행?? - 매니저?

    }*/


    public void ShowMovable(Piece piece)
    {

        if (!FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Place> movablePlaces = piece.MovableTo;
        for (int i = 0; i < movablePlaces.Count; i++)
        {
            ChangePlaceColor(movablePlaces[i].boardIndex, PlaceType.MOVABLE);
            movablePlaces[i].IsMovableToCurPiece = true;
        }
    }

    public void ShowInfluence(Piece piece)
    {

        if (!FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Place> influencing = piece.Influenceable;
        for (int i = 0; i < influencing.Count; i++)
        {
            //TODO: 영향권을 나타내는 색 따로 설정
            ChangePlaceColor(influencing[i].boardIndex, PlaceType.MOVABLE);
        }
    }

    public void ShowThreatAndDefence(Piece piece)
    {


        if (!FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Piece> defencing = piece.DefendFor;
        List<Piece> threating = piece.ThreatTo;

        for (int i = 0; i < defencing.Count; i++)
        {
            // 다른 보드로 위치가 변경될 시 문제 생길 수 있음
            ChangePlaceColor(defencing[i].place.boardIndex, PlaceType.DEFENCE);
        }
        for (int i = 0; i < threating.Count; i++)
        {
            ChangePlaceColor(threating[i].place.boardIndex, PlaceType.ATTACK);
            threating[i].place.IsAttackableByCurPiece = true;
            threating[i].place.IsMovableToCurPiece = true;
        }

    }


    public void ShowMovableEnd(Piece endedPiece)
    {
        List<Place> movableList = endedPiece.MovableTo;
        for (int i = 0; i < movableList.Count; i++)
        {
            movableList[i].ChangeColor();
            movableList[i].IsMovableToCurPiece = false;
        }
    }
    public void ShowInfluenceEnd(Piece endedPiece)
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
            threatList[i].place.IsAttackableByCurPiece = false;
            threatList[i].place.IsMovableToCurPiece = false;        // 공격 가능하면, 움직일 수도 있게 엮어도 좋을 듯
        }
    }

    public void PostShow(Piece finishedPiece)
    {
        ShowInfluence(finishedPiece);
        ShowThreatAndDefence(finishedPiece);
    }

    public void PreShow(Piece seleceted)
    {
        ShowMovable(seleceted);
        //ShowInfluence(seleceted);
        ShowThreatAndDefence(seleceted);
    }
    public void PreShowEnd(Piece endedPiece)
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


    public void ChangePlaceColor(Vector2Int location, PlaceType placeType)
    {
        switch (placeType)
        {
            case PlaceType.DEFENCE:
                places[location.x, location.y].ChangeColor(Color.blue);
                break;

            case PlaceType.ATTACK:
                places[location.x, location.y].ChangeColor(Color.red);
                break;

            case PlaceType.NORMAL:
                places[location.x, location.y].ChangeColor();
                break;

            case PlaceType.MOVABLE:
                places[location.x, location.y].ChangeColor(PlaceManager.Instance.highlight);
                break;

            case PlaceType.SPECIALMOVE:
                places[location.x, location.y].ChangeColor(Color.gray);
                break;
        }
    }
}
