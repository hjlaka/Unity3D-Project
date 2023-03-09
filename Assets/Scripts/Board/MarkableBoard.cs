using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MarkableBoard : Board, IMarkable
{

    public enum PlaceType { DEFENCE, ATTACK, MOVABLE, NORMAL, INFLUENCE }

    public void PreShow(Piece selecetedPiece)
    {
        ShowMovable(selecetedPiece);
        ShowThreatAndDefence(selecetedPiece);
    }
    public void PreShowEnd(Piece endedPiece)
    {
        ShowMovableEnd(endedPiece);
        ShowThreatAndDefenceEnd(endedPiece);
    }

    public void PostShow(Piece finishedPiece)
    {
        ShowInfluence(finishedPiece);
        ShowThreatAndDefence(finishedPiece);
    }
    public void PostShowEnd(Piece endedPiece)
    {
        Debug.Log("마무리 표시 해제중: " + endedPiece);
        ShowThreatAndDefenceEnd(endedPiece);
        ShowInfluenceEnd(endedPiece);
    }
    public void ShowMovable(Piece piece)
    {

        if (!FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Place> movablePlaces = piece.Recognized.movable;
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
        List<Place> influencing = piece.Recognized.influenceable;
        for (int i = 0; i < influencing.Count; i++)
        {
            ChangePlaceColor(influencing[i].boardIndex, PlaceType.INFLUENCE);
        }
    }

    public void ShowThreatAndDefence(Piece piece)
    {


        if (!FollowRule)               // 규칙을 따르지 않는 보드라면 종료
            return;

        // 연출
        List<Piece> defending = piece.Recognized.defending;
        List<Piece> threating = piece.Recognized.threating;

        for (int i = 0; i < defending.Count; i++)
        {
            // 다른 보드로 위치가 변경될 시 문제 생길 수 있음
            ChangePlaceColor(defending[i].place.boardIndex, PlaceType.DEFENCE);
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
        List<Place> movableList = endedPiece.Recognized.movable;
        for (int i = 0; i < movableList.Count; i++)
        {
            movableList[i].ChangeColor();
            movableList[i].IsMovableToCurPiece = false;
        }
    }
    public void ShowInfluenceEnd(Piece endedPiece)
    {
        List<Place> influenceList = endedPiece.Recognized.influenceable;
        Debug.Log("영향권 표시 해제 " + influenceList.Count);
        for (int i = 0; i < influenceList.Count; i++)
        {
            influenceList[i].ChangeColor();
        }
    }
    private void ShowThreatAndDefenceEnd(Piece endedPiece)
    {
        List<Piece> defeceList = endedPiece.Recognized.defending;
        List<Piece> threatList = endedPiece.Recognized.threating;

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

            case PlaceType.INFLUENCE:
                places[location.x, location.y].ChangeColor(Color.gray);
                break;
        }
    }
}
