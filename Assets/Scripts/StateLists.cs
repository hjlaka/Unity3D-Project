using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLists
{
    public List<Place> movable;
    public List<Piece> threating;
    public List<Piece> defending;

    public List<Place> influenceable;

    public StateLists()
    {
        movable = new List<Place>();
        threating = new List<Piece>();
        defending = new List<Piece>();
        influenceable = new List<Place>();
    }

    public void AddMovable(Place place)
    {
        movable.Add(place);
    }
    public void AddDefending(Piece piece)
    {
        defending.Add(piece);
    }

    public void AddThreating(Piece piece)
    {
        threating.Add(piece);
    }

    public void AddInfluenceable(Place place)
    {
        influenceable.Add(place);
    }

    public void ClearAllRecognized()
    {
        ClearMovable();
        ClearThreating();
        ClearDefending();
        ClearInfluenceable();
    }

    public void ClearMovable()
    {
        //Debug.Log("�̵� Ŭ����" + MovableTo.Count);
        movable.Clear();
    }

    // �ӽ� ���� 
    public void ClearThreating()
    {
        //Debug.Log("���� Ŭ����" + ThreatTo.Count);
        threating.Clear();
    }

    public void ClearDefending()
    {
        //Debug.Log("��� Ŭ����" + DefendFor.Count);
        defending.Clear();
    }

    public void ClearInfluenceable()
    {
        //Debug.Log("����� Ŭ����" + Influenceable.Count);
        influenceable.Clear();
    }
}
