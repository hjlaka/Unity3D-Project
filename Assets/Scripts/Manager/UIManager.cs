using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pieceCreateUI;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            if (pieceCreateUI.activeSelf)
            {
                pieceCreateUI.SetActive(false);
                //GameManager.Instance.state = GameManager.GameState.SELECTING_PIECE;
            }
            else
            {
                if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
                {
                    pieceCreateUI.SetActive(true);
                    //GameManager.Instance.state = GameManager.GameState.CREATEING_PIECE;
                }
            }
                
            
            
        }
    }
}
