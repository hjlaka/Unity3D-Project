using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Piece : MonoBehaviour
{
    [Header("InGame")]
    public Place place;
    public TeamData team;
    [SerializeField]
    private Color mouseOver;

    protected int forwardY;


    public IReturnHeat returnHeat;

    [Header("Charector")]
    [SerializeField]
    public CharacterData charactor;

    private Renderer render;
    private Color curNormal;

    private IDecidePlaceStrategy decidePlaceStrategy;


    private List<Piece> defendFor;
    private List<Piece> threatTo;
    private List<Place> movableTo;
    private List<Place> influenceable;

    #region ����Ʈ ������Ƽ
    public List<Piece> DefendFor
    {
        get { return defendFor; }
        private set 
        { 
            defendFor = value;
        }
    }
    
    public List<Piece> ThreatTo
    {
        get { return threatTo; }
        private set 
        { 
            threatTo = value;
        }
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

    #endregion

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
        ApplyTeamInfo();

        if (place != null)
        {
            SetInPlace(place);
        }   
    }

    public void DesireToPlace()
    {

        //PlaceManager.Instance.SelectPiece(this);

        if(MovableTo.Count <= 0)
        {
            Debug.Log("�̵��� �� �ִ� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }

        Place targetPlace = decidePlaceStrategy.DecidePlace(this);
        if(targetPlace != null)
        {
            if(targetPlace.piece != null)
            {
                Debug.Log("��������!");
                //AttackTo(targetPlace);
                PlaceManager.Instance.Attack(this, targetPlace.piece);
            }
            else
            {
                Debug.Log("�̵�����!");
                PlaceManager.Instance.MovePieceTo(this, targetPlace);
            }   
        }
        else
        {
            Debug.Log("�����̰� ���� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
        }
        //Place targetPlace;
        //decidePlaceStrategy.DecidePlace(out targetPlace);

    }

    private void ApplyTeamInfo()
    {
        curNormal = team.normal;
        render.material.color = curNormal;

        if (team.direction == TeamData.Direction.UpToDown)
        {
            forwardY = -1;
            returnHeat = new TopTeam();
        } 
        else
        {
            forwardY = 1;
            returnHeat = new BottomTeam();
        }

        decidePlaceStrategy = charactor.DecidePlaceStrategy;
    }

    #region ����Ʈ ����
    public void ClearMovable()
    {
        //Debug.Log("�̵� Ŭ����" + MovableTo.Count);
        MovableTo.Clear();
    }

    // �ӽ� ���� 
    public void ClearThreat()
    {
        //Debug.Log("���� Ŭ����" + ThreatTo.Count);
        ThreatTo.Clear();
    }

    public void ClearDefence()
    {
        //Debug.Log("��� Ŭ����" + DefendFor.Count);
        DefendFor.Clear();
    }

    public void ClearInfluence()
    {
        //Debug.Log("����� Ŭ����" + Influenceable.Count);
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

        if (team.direction == TeamData.Direction.DownToUp)
            place.HeatPointBottomTeam++;
        else
            place.HeatPointTopTeam++;

    }
    public void AddDefence(Piece piece)
    {
        Debug.Log(this + "�� " + piece + "�� ��ȣ�Ѵ�");
        defendFor.Add(piece);
        DialogueManager.Instance.AddDialogue(ref charactor.defending);
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
        DialogueManager.Instance.AddDialogue(ref charactor.threatening);
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

    #endregion
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

        }
        else
        {
            AddThreat(targetPiece);
            targetPiece.BeThreatened(this);

            // ������ �� �ִ� �ڸ��� �̵��� �� �ִ� �ڸ� �̱⵵ �ϴ�.
            // ������ �� �ִ� �ڸ��� ����� �� �ڸ��̴�.
            AddMovable(targetPlace);
            AddInfluence(targetPlace);

        }
    }

    protected bool RecognizePiece(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = this.place.board.places[curLocation.x, curLocation.y];

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
    }

    public void ChangeColor()
    {
        curNormal = team.normal;
        render.material.color = curNormal;
    }
    public void ChangeColor(Color color)
    {
        curNormal = color;
        render.material.color = curNormal;

    }
    private void OnMouseOver()
    {
        //Debug.Log(string.Format("{0} ���콺 ����Ŵ", gameObject.name));
        render.material.color = mouseOver;
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} ������ ���콺 ����", gameObject.name));
        render.material.color = curNormal;
    }

    private void OnMouseUpAsButton()
    {
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            //Debug.Log(string.Format("{0} Ŭ��", gameObject.name));
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
                if(place.IsAttackableByCurPiece)
                {
                    //���� 
                    //BeAttackedBy(PlaceManager.Instance.SelectedPiece);
                    PlaceManager.Instance.Attack(PlaceManager.Instance.SelectedPiece, this);
                   
                }
            }

        }
    }
    private void AttackTo(Place place)
    {
        PlaceManager.Instance.Attack(this, place.piece);
        //PlaceManager.Instance.ExpelPiece(place.Piece);
        //PlaceManager.Instance.MovePieceTo(this, place);
    }
    private void BeAttackedBy(Piece piece)
    {
        Place attackPlace = this.place;
        PlaceManager.Instance.ExpelPiece(this);
        PlaceManager.Instance.MovePieceTo(piece, attackPlace);
    }

    public void setDecidePlaceStrategy(IDecidePlaceStrategy decidePlaceStrategy)
    {

    }
}

