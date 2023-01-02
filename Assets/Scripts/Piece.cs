using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    public Place place;

    public TeamData team;

    private Renderer render;

    [SerializeField]
    private Color mouseOver;
    //[SerializeField]
    private Color normal;

    private List<Piece> defendFor;
    private List<Piece> attackTo;
    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        defendFor = new List<Piece>();
        attackTo = new List<Piece>();
    }

    private void Start()
    {
        normal = team.normal;
        transform.position = place.transform.position;
        render.material.color = normal;
    }

    public void AddAttack(Piece piece)
    {
        Debug.Log(piece + "�� �����Ѵ�");
        attackTo.Add(piece);
    }
    public void AddDefence(Piece piece)
    {
        Debug.Log(piece + "�� ��ȣ�Ѵ�");
        defendFor.Add(piece);
    }

    public void BeAttacked(Piece piece)
    {
        Debug.Log(piece + "���� ���� ���Ѵ�");
    }

    public void BeThreatened(Piece piece)
    {
        Debug.Log(piece + "���� ���� ���Ѵ�");
    }

    public void BeDefended(Piece piece)
    {
        Debug.Log(piece + "���� ��ȣ �޴´�");
    }


    public void SetInPlace(Place place)
    {
        this.place = place;
        Move();
    }

    public void Move()
    {
        transform.position = place.transform.position;
    }


    public virtual void PieceAction() { }
    public virtual bool IsMovable(Vector2Int location)
    {
        return false;
    }


    public virtual void LookAttackRange() { }

    public virtual void ShowMovable() { }

    private void OnMouseOver()
    {
        //Debug.Log(string.Format("{0} ���콺 ����Ŵ", gameObject.name));
        render.material.color = mouseOver;
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} ������ ���콺 ����", gameObject.name));
        render.material.color = normal;
    }

    private void OnMouseUpAsButton()
    {
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            Debug.Log(string.Format("{0} Ŭ��", gameObject.name));
            PlaceManager.Instance.SelectPiece(this);
        }
        
        else if(GameManager.Instance.state == GameManager.GameState.SELECTING_PLACE)
        {
            // �ڽ��̶��
            if(PlaceManager.Instance.SelectedPiece == this)
                PlaceManager.Instance.SelectedPieceInit(PlaceManager.Instance.SelectedPiece.place);

            // ���� �� �⹰�̶��
            else if (this.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                PlaceManager.Instance.SelectedPieceInit(PlaceManager.Instance.SelectedPiece.place);
                PlaceManager.Instance.SelectPiece(this);
            }
            // �ٸ� �� �⹰�̶��
            else
            {
                // ������ �� �ִٸ�
            }

        }
    }
}

