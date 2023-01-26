using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ChangeLayerFunc))]
[RequireComponent(typeof(Button))]
public class UnitButton : MonoBehaviour
{
    //public CharacterData characterData;
    private Piece piece;
    public Piece Piece 
    { 
        get { return piece; } 
        set 
        { 
            piece = value;
            if(piece != null)
            {
                Debug.Log("���� �⹰: " + piece);
                this.name = piece.character.characterName;
                textComp.text = value.character.characterName;
                piece.OnPlaced += FinishSetting;
            }
        } 
    }

    // ���� ��� �⹰�� �ǿ� ��ġ��ų �� �ִ�.

    // ��ư�� ���� ���·� ���� �����
    // ���콺�� �⹰ ��ġ ���� �����
    // �⹰�� ��ġ�� �� �ִ� ĭ�� Ȱ��ȭ��

    // �巡�� �� ���?

    private Place creatingPlace;
    private Transform pieceZone;
    private ChangeLayerFunc changeLayer;

    private Button button;

    [SerializeField]
    private TextMeshProUGUI textComp;

    private void Awake()
    {
        pieceZone = GameObject.Find("PieceZone").transform;
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // �� ��ȯ�� �ƴ϶� ������Ʈ �������Ⱑ �³�?
        changeLayer = GetComponent<ChangeLayerFunc>();
        button = GetComponent<Button>();
       
    }
    public void PlacePiece()
    {
        if (GameManager.Instance.state != GameManager.GameState.PREPARING_GAME_ON) return;

        PlaceManager.Instance.SelectPiece(piece);
        

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

    public void FinishSetting()
    {
        button.interactable = false;
        piece.OnPlaced -= FinishSetting;
    }
}
