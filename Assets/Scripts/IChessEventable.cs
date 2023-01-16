using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChessEventable
{
    void Check();

    void Threat();

    void Attack();

    void Defend();

    void CheckMate();
}
