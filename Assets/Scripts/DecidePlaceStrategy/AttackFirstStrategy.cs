using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFirstStrategy : IDecidePlaceStrategy
{
    public void DecidePlace(List<Place> movablePlaces)
    {
        Debug.Log("���� �켱 ����");
    }
}
