using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChessEventable
{
    void Check(Piece targetPiece);

    void Threat();

    void Attack();

    void Defend(Piece subject, Piece target);

    void CheckMate();
}
