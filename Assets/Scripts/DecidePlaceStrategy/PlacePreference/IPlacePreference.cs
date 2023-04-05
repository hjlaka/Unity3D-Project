using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacePreference
{
    float ReturnHeatPreference(Piece piece, Place place);

    // 과열도에 따른 위치 선정.
    // 안전한 위치인지, 아군을 믿을 수 있는지 여부가 반영된다.
}
