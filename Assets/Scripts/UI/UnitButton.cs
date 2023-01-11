using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public CharacterData characterData;

    // ���� ��� �⹰�� �ǿ� ��ġ��ų �� �ִ�.

    // ��ư�� ���� ���·� ���� �����
    // ���콺�� �⹰ ��ġ ���� �����
    // �⹰�� ��ġ�� �� �ִ� ĭ�� Ȱ��ȭ��

    // �巡�� �� ���?
    private Place creatingPlace;

    private void Awake()
    {
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // �� ��ȯ�� �ƴ϶� ������Ʈ �������Ⱑ �³�?
    }
    public void PlacePiece()
    {
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE) return;

        //TODO: �⹰ �������� �⹰ ��ġ �������� ��� ���� ����ϱ�
        Debug.Log("�⹰�� �����߽��ϴ�." + characterData.characterName);
        Piece instance = Instantiate(characterData.piecePrefab);
        instance.team = PlayerDataManager.Instance.PlayerTeam;
        instance.character = characterData;
        instance.SetInPlace(creatingPlace);


        PlaceManager.Instance.SelectPiece(instance);

        Debug.Log("������ ��: " + instance);

    }
}
