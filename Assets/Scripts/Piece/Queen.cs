using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    protected override void Awake()
    {
        base.Awake();

        movePattern = new EightWayMove(this);
        // 무브 패턴을 리스트화 하여 여러개 넣는 방식은 어떤가?
    }
}
