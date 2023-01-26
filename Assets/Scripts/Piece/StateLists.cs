using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLists
{
    public List<Place> movable;
    public List<Piece> threating;
    public List<Piece> defending;

    public List<Place> influenceable;

    public List<Place> special;

    public StateLists()
    {
        movable = new List<Place>();
        threating = new List<Piece>();
        defending = new List<Piece>();
        influenceable = new List<Place>();
        special = new List<Place>();
    }

    public virtual void AddMovable(Place place)
    {
        movable.Add(place);
    }
    public virtual void AddDefending(Piece piece)
    {
        defending.Add(piece);
    }

    public virtual void AddThreating(Piece piece)
    {
        threating.Add(piece);
    }

    public virtual void AddInfluenceable(Place place)
    {
        influenceable.Add(place);
    }

    public virtual void AddSpecial(Place place)
    {
        special.Add(place);
    }

    public void ClearAllRecognized()
    {
        ClearMovable();
        ClearThreating();
        ClearDefending();
        ClearInfluenceable();
        ClearSpecial();
    }

    public void ClearMovable()
    {
        //Debug.Log("이동 클리어" + MovableTo.Count);
        movable.Clear();
    }

    // 임시 생성 
    public void ClearThreating()
    {
        //Debug.Log("위협 클리어" + ThreatTo.Count);
        threating.Clear();
    }

    public void ClearDefending()
    {
        //Debug.Log("방어 클리어" + DefendFor.Count);
        defending.Clear();
    }

    public void ClearInfluenceable()
    {
        //Debug.Log("영향권 클리어" + Influenceable.Count);
        influenceable.Clear();
    }

    public void ClearSpecial()
    {
        special.Clear();
    }
}
