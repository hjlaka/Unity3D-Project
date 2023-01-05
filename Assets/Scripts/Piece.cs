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
    private List<Piece> threatTo;
    private List<Place> movableTo;
    private List<Place> influenceable;

    public List<Piece> DefendFor
    {
        get { return defendFor; }
        private set { defendFor = value; }
    }
    
    public List<Piece> ThreatTo
    {
        get { return threatTo; }
        private set { threatTo = value; }
    }

    public List<Place> MovableTo
    {
        get { return movableTo; } 
        private set { movableTo = value;  }
    }

    public List<Place> Influenceable
    {
        get { return influenceable; }
        private set { influenceable = value; }
    }

    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        defendFor = new List<Piece>();
        threatTo = new List<Piece>();
        movableTo = new List<Place>();
        influenceable = new List<Place>();
    }

    private void Start()
    {
        normal = team.normal;
        render.material.color = normal;

        if(place != null)
        {
            SetInPlace(place);
        }
            
    }

    public void ClearMovable()
    {
        Debug.Log("�̵� Ŭ����" + MovableTo.Count);
        MovableTo.Clear();
    }

    // �ӽ� ���� 
    public void ClearThreat()
    {
        Debug.Log("���� Ŭ����" + ThreatTo.Count);
        ThreatTo.Clear();
    }

    public void ClearDefence()
    {
        Debug.Log("��� Ŭ����" + DefendFor.Count);
        DefendFor.Clear();
    }

    public void ClearInfluence()
    {
        Debug.Log("����� Ŭ����" + Influenceable.Count);
        Influenceable.Clear();
    }
    // ������� �ӽ� ����

    public void AddMovable(Place place)
    {
        movableTo.Add(place);
    }
    public void AddInfluence(Place place)
    {
        Influenceable.Add(place);
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
        ThreatTo.Add(piece);
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

    public virtual void SetInPlace(Place place)
    {
        this.place = place;
        place.piece = this;
        Move();
    }

    public void Move()
    {
        transform.position = place.transform.position;
    }


    public virtual void PieceAction() { }
    public virtual void IsMovable(Vector2Int location)
    {
    }

    public virtual List<Place> ReturnMovablePlaces(Vector2Int location)
    {
        return null;
    }


    protected bool IsTopOutLocation(Vector2Int curLocation, int boardHeight)
    {
        if (curLocation.y > boardHeight - 1)
            return true;
        else
            return false;
    }

    protected bool IsBottomOutLocation(Vector2Int curLocation)
    {
        if (curLocation.y < 0)
            return true;
        else
            return false;
    }

    protected bool IsLeftOutLocation(Vector2Int curLocation)
    {
        if (curLocation.x < 0)
            return true;
        else
            return false;
    }

    protected bool IsRightOutLocation(Vector2Int curLocation, int boardWidth)
    {
        if (curLocation.x > boardWidth - 1)
            return true;
        else
            return false;
    }




    // ----------------------------------------------------------- �� �������� ���� �߰� { 
    protected bool RecognizePieceMoveObstacle(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = this.place.board.places[curLocation.x, curLocation.y];

        if(targetPiece != null)
        {
            return true;
        }
        else
        {
            Debug.Log("�տ� ��ֹ� ����");

            AddMovable(targetPlace);
            //���� �̵�

            return false;
        }
    }

    protected bool RecognizePieceOnlyInfluence(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = this.place.board.places[curLocation.x, curLocation.y];
        // TODO:
        // �������� ����� ������ ����� �Բ� ����ǵ��� ����
        targetPlace.HeatPoint++;

        if (targetPiece != null)
        {
            RecognizeDefendAndAttack(curLocation, targetPiece, targetPlace);

            return true;
        }
        else
        {
            // �⹰�� ���� ��� �̵��� �� ����
            // ������� ����
            AddInfluence(targetPlace);
            return false;
        }
    }
    // } �� �������� ���� �߰� -----------------------------------------------------------

    private void RecognizeDefendAndAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == team.TeamId)
        {
            AddDefence(targetPiece);
            targetPiece.BeDefended(this);

            // ����� �� �ִ� �ڸ��� �̵��� �� ������ ����� ���� �ڸ��̴�.
            AddInfluence(targetPlace);


            //����
            //PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.DEFENCE);
            //DialogueManager.Instance.ShowDialogueUI("Defend" + targetPiece);
        }
        else
        {
            AddThreat(targetPiece);
            targetPiece.BeThreatened(this);

            // ������ �� �ִ� �ڸ��� �̵��� �� �ִ� �ڸ� �̱⵵ �ϴ�.
            // ������ �� �ִ� �ڸ��� ����� �� �ڸ��̴�.
            AddMovable(targetPlace);
            AddInfluence(targetPlace);


            //����
            //PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.ATTACK);
            //DialogueManager.Instance.ShowDialogueUI("Attack" + targetPiece);
        }
    }

    protected bool RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = this.place.board.places[curLocation.x, curLocation.y];
        targetPlace.HeatPoint++;

        if (targetPiece != null)
        {
            RecognizeDefendAndAttack(curLocation, targetPiece, targetPlace);

            return true;
        }
        else
        {

            RecognizeMovableVoidPlace(curLocation, targetPlace);

            return false;
        }
    }

    private void RecognizeMovableVoidPlace(Vector2Int curLocation, Place targetPlace)
    {
        AddMovable(targetPlace);
        AddInfluence(targetPlace);
        //PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.MOVABLE);
    }

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

