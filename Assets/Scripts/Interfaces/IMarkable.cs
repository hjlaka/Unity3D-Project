using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMarkable
{
    void PreShow(Piece selecetedPiece);
    void PreShowEnd(Piece endedPiece);
    void PostShow(Piece finishedPiece);
    void PostShowEnd(Piece endedPiece);
}
