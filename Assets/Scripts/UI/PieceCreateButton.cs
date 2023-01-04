using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCreateButton : MonoBehaviour
{
    [SerializeField]
    private Piece piecePrefab;
    [SerializeField]
    private TeamData team;

    private Place creatingPlace;
    private void Awake()
    {
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // �� ��ȯ�� �ƴ϶� ������Ʈ �������Ⱑ �³�?
    }

    public void CreatePiece()
    {
        if (creatingPlace == null || creatingPlace.piece != null) return;
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE) return;

        Piece instance = Instantiate(piecePrefab);
        instance.team = team;
        instance.SetInPlace(creatingPlace);
        // �⹰ ����

        // �⹰�� ���û��·� ����
        //PlaceManager.Instance.SelectPiece(instance);

        // ���� ���¸� ��ġ ���� ���·� ���� (�÷��̽� �Ŵ������� ó��)



        // �ٸ� �⹰ ���ý�...? - ���� ���� �⹰�� ����



        
    }

}
