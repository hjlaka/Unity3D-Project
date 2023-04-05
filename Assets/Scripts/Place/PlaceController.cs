using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceController : MonoBehaviour
{
    // ��Ʈ�ѷ�. ������
    public enum TeamType { Top, Bottom }

    private PlaceModel model;
    private PlaceView view;

    public enum PlaceType { A, B, V };
    public PlaceType type;     // ��ũ���ͺ� ������Ʈ

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
            Debug.Log("��ġ ���� ��ġ Ŭ�� ����");
            if (PlaceManager.Instance.SelectedPiece != null && model.IsMovableToCurPiece)
            {
                PlaceManager.Instance.MovePiece(PlaceManager.Instance.SelectedPiece, this);
                PlaceManager.Instance.SelectedPieceInit();
                return;
            }
            Debug.Log("���õ� �⹰ ����");
        }
        if (GameManager.Instance.state != GameManager.GameState.SELECTING_PLACE)
        {
            Debug.Log("��ġ ���� ���� ���� �ƴ�");
            return;
        }


        // �⹰�� ���õ� ���¿��� Ŭ����
        if (null == PlaceManager.Instance.SelectedPiece)
        {
            Debug.Log("���õ� �⹰�� ����");
            return;
        }

        if (piece != null)
        {
            Debug.Log("�ڸ��� �⹰�� ���� (������ ���ҽ� �⹰�� ������ ��)");
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
        // ���������� �˸��� ��������, ������ ����� �ٲ� �� �ִ�.
        // ���� ��ϴ�� ���������� �˷����ϹǷ�, ���纻�� ������ �Ѵ�.

        List<IObserver> copyList = new List<IObserver>();

        for (int i = 0; i < model.InfluencingUnit.Count; i++)
        {
            copyList.Add(model.InfluencingUnit[i]);
        }

        for (int i = 0; i < copyList.Count; i++)
        {
            copyList[i].StateUpdate(this);
            //Debug.Log("������Ʈ ȣ��: " + copyList[i]);
        }

        return this;

        */

        return null;
    }
}
