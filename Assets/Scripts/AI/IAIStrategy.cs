using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAIStrategy
{
    void AddPossibility(ScoreNode scoreSet, Piece piece, Place place);

    Placement GetBestInOwnWay();
}
