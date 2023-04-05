using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyFirstStrategy : IDecidePlaceStrategy
{

    // 이 전략에서는 기물이 움직이지 않을 수도 있다고 가정한다.
    public Placement DecidePlace(Piece piece, ref float will, out ScoreNode scoreSet)
    {
        scoreSet = null;

        return null;
    }

}
