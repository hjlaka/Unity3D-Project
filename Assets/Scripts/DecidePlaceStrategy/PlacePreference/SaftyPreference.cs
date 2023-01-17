using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyPreference : MonoBehaviour, IPlacePreference
{
    public float ReturnHeatPreference(Piece piece, Place place)
    {
        //���� �켱:

        // ����� ������ �ϳ��� ������ �켱���� �������� ���
        // ����� ������ �־, �߿䵵�� ���ٸ�(?), ���� �ϰ� �̵��ϴ� ���

        // ���� �� �ִ� �⹰�� ��쿡��... �� �⹰�� �������� �ϳ� ���� �Ѵ�.
        int deltaHeat = piece.returnHeat.ReturnTeamHeat(place) - piece.returnHeat.ReturnOpponentHeat(place) - 1;  // �ڱ� �ڽ��� ������ �߱�
        float heatPreferPoint = 0;
        if (0 == deltaHeat)
        {
            heatPreferPoint = 3;
        }
        else if (deltaHeat == 1)
        {
            heatPreferPoint = 4;
        }
        else if (deltaHeat == -1)
        {
            heatPreferPoint = 1;
        }
        else if (deltaHeat > 1)
        {
            heatPreferPoint = 4;
        }
        else if (deltaHeat < -1)
        {
            heatPreferPoint = 0;
        }

        if (piece.returnHeat.ReturnOpponentHeat(place) > 0)
        {
            heatPreferPoint *= 0.5f;
        }

        return heatPreferPoint;
    }
}
