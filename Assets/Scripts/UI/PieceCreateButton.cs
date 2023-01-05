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
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // 형 변환이 아니라 컴포넌트 가져오기가 맞나?
    }

    public void CreatePiece()
    {
        if (creatingPlace == null || creatingPlace.piece != null) return;
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE) return;

        Debug.Log("기물을 생성했습니다." + piecePrefab.name);
        Piece instance = Instantiate(piecePrefab);
        instance.team = team;
        instance.SetInPlace(creatingPlace);
        
    }

}
