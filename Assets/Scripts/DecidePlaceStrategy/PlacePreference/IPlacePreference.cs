using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacePreference
{
    float ReturnHeatPreference(Piece piece, Place place);

    // �������� ���� ��ġ ����.
    // ������ ��ġ����, �Ʊ��� ���� �� �ִ��� ���ΰ� �ݿ��ȴ�.
}
