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
                //Debug.Log("���� �⹰: " + piece);
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

    private Button button;

    [SerializeField]
    private TextMeshProUGUI textComp;

    private void Awake()
    {
        button = GetComponent<Button>();
       
    }
    public void PlacePiece()
    {
        //if (GameManager.Instance.curState != GameManager.GameState.PREPARING_GAME)
        if (GameManager.Instance.curState is not StatePreparingGame)
        {
            Debug.Log("�غ� ���°� �ƴϴ�");
            return;
        }
            


        PlaceManager.Instance.SelectPiece(piece);

    }

    public void FinishSetting()
    {
        if(button != null)
        {
            button.interactable = false;
            piece.OnPlaced -= FinishSetting;
        }
        
    }
}
