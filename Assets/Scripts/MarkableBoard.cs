using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkableBoard : Board
{

    public enum PlaceType { DEFENCE, ATTACK, MOVABLE, NORMAL, SPECIALMOVE }

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
