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

        Piece instance = Instantiate(piecePrefab);
        instance.team = team;
        instance.SetInPlace(creatingPlace);
        // 기물 생성

        // 기물을 선택상태로 변경
        //PlaceManager.Instance.SelectPiece(instance);

        // 게임 상태를 위치 선택 상태로 변경 (플레이스 매니저에서 처리)



        // 다른 기물 선택시...? - 선택 중인 기물을 제거



        
    }

}
