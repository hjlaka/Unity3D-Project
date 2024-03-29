using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
                //Debug.Log("들어온 기물: " + piece);
                this.name = piece.character.characterName;
                textComp.text = value.character.characterName;
                piece.OnPlaced += FinishSetting;
            }
        } 
    }

    // 누를 경우 기물을 판에 배치시킬 수 있다.

    // 버튼이 눌린 상태로 색이 변경됨
    // 마우스가 기물 배치 모드로 변경됨
    // 기물을 배치할 수 있는 칸이 활성화됨

    // 드래그 앤 드랍?

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
            Debug.Log("준비 상태가 아니다");
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
