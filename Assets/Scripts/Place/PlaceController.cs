using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceController : MonoBehaviour
{
    // 컨트롤러. 연결자
    public enum TeamType { Top, Bottom }

    private PlaceModel model;
    private PlaceView view;

    public enum PlaceType { A, B, V };
    public PlaceType type;     // 스크립터블 오브젝트

    private void Awake()
    {
        model = GetComponent<PlaceModel>();
        view = GetComponent<PlaceView>();

        model.InfluencingUnit = new List<PlaceObserver>();
    }

    private void Start()
    {
        SetTypeColor();
    }

    private void SetTypeColor()
    {
        switch (type)
        {
            case PlaceType.A:
                model.TypeColor = Color.white;
                break;
            case PlaceType.B:
                model.TypeColor = Color.black;
                break;
            case PlaceType.V:
                model.TypeColor = Color.green;
                break;
        }

        view.ChangeColor(model.TypeColor);
    }

    private void ChangeEffect(float intencity)
    {
        model.Effect.ChangeIntencity(intencity);
    }

    public void BeEmpty()
    {
        model.Piece = null;
    }

    public void BeFilled(Piece piece)
    {
        model.Piece = piece;
    }

    public void AddInfluence(TeamData.Direction type)
    {
        switch(type)
        {
            case TeamData.Direction.UpToDown:
                model.HeatPointTopTeam++;
                break; 
            case TeamData.Direction.DownToUp:
                model.HeatPointBottomTeam++;
                break;
            default:
                break;
        }
    }

    public void CheckClickAction()
    {
        /*
        if (GameManager.Instance.state == GameManager.GameState.PREPARING_GAME_ON)
        {
            Debug.Log("배치 상태 위치 클릭 진입");
            if (PlaceManager.Instance.SelectedPiece != null && model.IsMovableToCurPiece)
            {
                PlaceManager.Instance.MovePiece(PlaceManager.Instance.SelectedPiece, this);
                PlaceManager.Instance.SelectedPieceInit();
                return;
            }
            Debug.Log("선택된 기물 없음");
        }
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PLACE)
        {
            Debug.Log("위치 선택 가능 상태 아님");
            return;
        }


        // 기물이 선택된 상태에서 클릭시
        if (null == PlaceManager.Instance.SelectedPiece)
        {
            Debug.Log("선택된 기물이 없음");
            return;
        }

        if (piece != null)
        {
            Debug.Log("자리에 기물이 있음 (공격을 원할시 기물을 눌러야 함)");
            return;
        }



        PlaceManager.Instance.MoveProcess(PlaceManager.Instance.SelectedPiece, this);

        */
    }

    public void registerObserver(IObserver observer)
    {
        model.InfluencingUnit.Add(observer as PlaceObserver);
    }

    public void removeObserver(IObserver observer)
    {
        model.InfluencingUnit.Remove(observer as PlaceObserver);
    }

    public ISubject notifyObserver()
    {
        /*
        // 옵저버에게 알리는 과정에서, 옵저버 목록이 바뀔 수 있다.
        // 현재 목록대로 옵저버에게 알려야하므로, 복사본을 만들어야 한다.

        List<IObserver> copyList = new List<IObserver>();

        for (int i = 0; i < model.InfluencingUnit.Count; i++)
        {
            copyList.Add(model.InfluencingUnit[i]);
        }

        for (int i = 0; i < copyList.Count; i++)
        {
            copyList[i].StateUpdate(this);
            //Debug.Log("업데이트 호출: " + copyList[i]);
        }

        return this;

        */

        return null;
    }
}
