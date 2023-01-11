using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitButton : MonoBehaviour
{
    public CharacterData characterData;

    // 누를 경우 기물을 판에 배치시킬 수 있다.

    // 버튼이 눌린 상태로 색이 변경됨
    // 마우스가 기물 배치 모드로 변경됨
    // 기물을 배치할 수 있는 칸이 활성화됨

    // 드래그 앤 드랍?
    private Place creatingPlace;

    private void Awake()
    {
        creatingPlace = GameObject.Find("CreatingPlace").GetComponent<Place>();         // 형 변환이 아니라 컴포넌트 가져오기가 맞나?
    }
    public void PlacePiece()
    {
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PIECE) return;

        //TODO: 기물 생성에서 기물 위치 변경으로 기능 변경 고려하기
        Debug.Log("기물을 생성했습니다." + characterData.characterName);
        Piece instance = Instantiate(characterData.piecePrefab);
        instance.team = PlayerDataManager.Instance.PlayerTeam;
        instance.character = characterData;
        instance.SetInPlace(creatingPlace);


        PlaceManager.Instance.SelectPiece(instance);

        Debug.Log("전달한 것: " + instance);

    }
}
