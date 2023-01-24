using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObserver : MonoBehaviour, IObserver
{
    public void StateUpdate(ISubject subject)
    {
        // 유닛의 변화를 감지하면
        // 관계 상태가 변화되었는지 확인한다.

        // 데이터를 전달 받아야 한다.
    }
}
