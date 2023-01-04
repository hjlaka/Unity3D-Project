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
    private List<Place> movableTo;

    public List<Place> MovableTo
    {
        get { return movableTo; } private set { movableTo = value;  }
    }

    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        defendFor = new List<Piece>();
        attackTo = new List<Piece>();
        movableTo = new List<Place>();
    }

    private void Start()
    {
        normal = team.normal;
        transform.position = place.transform.position;
        render.material.color = normal;
    }

    public void ClearMovable()
    {
        movableTo.Clear();
    }

    public void AddMovable(Place place)
    {
        movableTo.Add(place);
    }
    public void AddDefence(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ��ȣ�Ѵ�");
        defendFor.Add(piece);
    }

    public void EndDefence(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ���̻� ��ȣ���� �ʴ´�.");
    }

    public void BeDefended(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "���� ��ȣ �޴´�");
    }

    public void EndDefended(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�κ��� ���̻� ��ȣ���� �ʴ´�.");
    }

    public void AddThreat(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� �����Ѵ�");
    }

    public void EndThreat(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ���̻� �������� �ʴ´�.");
    }

    public void BeThreatened(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "���� ���� ���Ѵ�");
    }

    public void EndThreatened(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�κ��� ���̻� �������� �ʴ´�.");
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

    protected void RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        if (targetPiece != null)
        {
            if (targetPiece.team.TeamId == team.TeamId)
            {
                AddDefence(targetPiece);
                targetPiece.BeDefended(this);
                

                //����
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.DEFENCE);
                DialogueManager.Instance.ShowDialogueUI("Defend" + targetPiece);
            }
            else
            {
                AddThreat(targetPiece);
                targetPiece.BeThreatened(this);


                //����
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.ATTACK);
                DialogueManager.Instance.ShowDialogueUI("Attack" + targetPiece);
            }
        }
        else
        {
            Place targetPlace = this.place.board.places[curLocation.x, curLocation.y];
            AddMovable(targetPlace);
            targetPlace.HeatPoint++;
            PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.MOVABLE);
        }
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
            if (PlaceManager.Instance.SelectedPiece == this)
                PlaceManager.Instance.CancleSelectPiece();

            // ���� �� �⹰�̶��
            else if (this.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                PlaceManager.Instance.CancleSelectPiece();
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

