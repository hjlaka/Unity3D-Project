using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChessEventable
{
    void Check(Piece subject, Piece target);

    void Threat(Piece subject, Piece target);

    void Attack(Piece subject, Piece target);

    void Defend(Piece subject, Piece target);

    void CheckMate(Piece subject, Piece target);
}
