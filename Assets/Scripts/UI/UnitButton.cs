using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ChangeLayerFunc))]
public class UnitButton : MonoBehaviour
{
    //public CharacterData characterData;
    public Piece piece;

    // ���� ��� �⹰�� �ǿ� ��ġ��ų �� �ִ�.

    // ��ư�� ���� ���·� ���� �����
    // ���콺�� �⹰ ��ġ ���� �����
    // �⹰�� ��ġ�� �� �ִ� ĭ�� Ȱ��ȭ��

    // �巡�� �� ���?
    private Place creatingPlace;
    private Transform pieceZone;
    private ChangeLayerFunc changeLayer;

    private void Awake()
    {
        pieceZone = GameObject.Find("PieceZone").transform;
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // �� ��ȯ�� �ƴ϶� ������Ʈ �������Ⱑ �³�?
        changeLayer = GetComponent<ChangeLayerFunc>();
    }
    public void PlacePiece()
    {
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE) return;

        //TODO: �⹰ �������� �⹰ ��ġ �������� ��� ���� ����ϱ�
       /* Debug.Log("�⹰�� �����߽��ϴ�." + piece.character.characterName);
        Piece instance = Instantiate(piece.character.piecePrefab);
        instance.team = PlayerDataManager.Instance.PlayerTeam;
        instance.character = piece.character;
        instance.SetInPlace(creatingPlace);

        changeLayer.ChangeLayerRecursively(instance.transform, pieceZone.gameObject.layer);

        PlaceManager.Instance.SelectPiece(instance);

        Debug.Log("������ ��: " + instance);*/

    }
}
