using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlaceObserver))]
[RequireComponent (typeof(UnitObserver))]
[RequireComponent(typeof(Subject))]
public class Piece : LifeUnit
{
    [Header("InGame")]
    public Place place;
    public TeamData team;
    [SerializeField]
    private Player belong;
    public Player Belong { get { return belong; } set { belong = value; } }


    public int forwardY;
    [SerializeField]
    protected float pieceScore;
    public float PieceScore { get { return pieceScore; } private set { pieceScore = value; } }

    [SerializeField]
    private DecidedStateLists recognized;
    public DecidedStateLists Recognized { get { return recognized; } private set { recognized = value; } }

    [SerializeField]
    private uint moveCount;
    public uint MoveCount { get { return moveCount; } set { moveCount = value; } }


    public PlaceObserver PlaceObserver { get; private set; }
    public UnitObserver UnitObserver { get; private set; }

    public Subject ChessSubject { get; private set; }
    // TODO: king Ŭ������ subjectAsKing �߰�


    public IReturnHeat returnHeat;

    [Header("Charecter")]
    [SerializeField]
    public CharacterData character;

    private Renderer render;
    private Color curNormal;

    private IDecidePlaceStrategy decideDesireStrategy;

    protected IPieceMovable movePattern;
    public IPieceMovable MovePattern { get { return movePattern; } private set { movePattern = value; } }

    


    protected virtual void Awake()
    {
        render = GetComponentInChildren<Renderer>();

        recognized = new DecidedStateLists();

        PlaceObserver = GetComponent<PlaceObserver>();
        UnitObserver = GetComponent<UnitObserver>();
        ChessSubject = GetComponent<Subject>();
        Debug.Log("������Ʈ �Ҵ�: " + ChessSubject);
    }

    private void Start()
    {
        if (IsFree)
            return;

        moveCount = 0;
        ApplyTeamInfo();

        if (place != null)
        {
            place.BeFilled(this);
            Move();

            PlaceManager.Instance.CalculateInfluence(this);
            PlaceManager.Instance.ApplyInfluence(this);

            place.notifyObserver();
        }
    }
    public void BelongTo(Player player)
    {
        Debug.Log("�޾ƿ� �÷��̾�: " + player);
        this.belong = player;
        targetLocation = belong.homeLocation.position;
        belong.AddPiece(this);
    }

    public virtual void SetFree(bool value)
    {
        IsFree = value;
    }

    public void PlaceToDesire(Place targetPlace)
    {

        //PlaceManager.Instance.SelectPiece(this);

        if(Recognized.movable.Count <= 0)
        {
            Debug.Log("�̵��� �� �ִ� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }
        if(targetPlace != null)
        {
            if(targetPlace.Piece != null)
            {
                Debug.Log("��������!");
                //AttackTo(targetPlace);
                ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, this, targetPlace.Piece));
                ChessEventManager.Instance.GetEvent();
                DialogueManager.Instance.CheckDialogueEvent();
                PlaceManager.Instance.MoveProcess(this, targetPlace);
            }
            else
            {
                Debug.Log("�̵�����!");
                PlaceManager.Instance.MoveProcess(this, targetPlace);
            }   
        }
        else
        {
            Debug.Log("�����̰� ���� ���� ����");
            PlaceManager.Instance.CancleSelectPiece();
        }


    }

    public Placement DesireToPlace(ref float will, out ScoreNode scoreSet)
    {
        Placement targetPlace = decideDesireStrategy.DecidePlace(this, ref will, out scoreSet);

        return targetPlace;
    }

    private void ApplyTeamInfo()
    {
        curNormal = team.normal;
        render.material.color = curNormal;

        if (team.direction == TeamData.Direction.UpToDown)
        {
            forwardY = -1;
            returnHeat = new TopTeam();
            transform.Rotate(Vector3.up * 180);
        } 
        else
        {
            forwardY = 1;
            returnHeat = new BottomTeam();
        }

        decideDesireStrategy = character.DecidePlaceStrategy;
    }

   /* public void EndDefence(Piece piece)
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
    }*/

    public virtual Piece SetInPlace(Place place)
    {
        // �̵��� �ڸ��� ������ �ִ� �⹰
        Piece preSetPiece = place.Piece;
       
        Place oldPlace = this.place;
        this.place = null;
        oldPlace?.BeEmpty();

        this.place = place;
        place.BeFilled(this);

        Move();

        Debug.Log(this + "�� " + place.boardIndex + "�� �̵��ߴ�.");
        if(oldPlace != null && oldPlace.board == place.board && place.board.FollowRule)
            moveCount++;

        if(preSetPiece != null) Debug.Log("������ �ִ� �⹰: " + preSetPiece);

        return preSetPiece;
    }

    public void Move()
    {
        transform.position = place.transform.position;
    }

    public virtual void RecognizeRange(Vector2Int location)
    {
        movePattern.RecognizeRange(location, recognized);
    }

    public void SetDecidePlaceStrategy(IDecidePlaceStrategy decidePlaceStrategy)
    {
        decideDesireStrategy = decidePlaceStrategy;
    }

    public virtual List<Place> ReturnMovablePlaces(Vector2Int location)
    {
        return null;
    }

    private void AttackTo(Place place)
    {
        PlaceManager.Instance.Attack(this, place.Piece);
    }
    protected void BeAttackedBy(Piece piece)
    {
        Place attackPlace = this.place;
        PlaceManager.Instance.ExpelPiece(this);
        PlaceManager.Instance.MoveProcess(piece, attackPlace);
    }
    public void ChangeColor()
    {
        //Debug.Log("�� �������� ����");
        curNormal = team.normal;
        render.material.color = curNormal;
    }
    public void ChangeColor(Color color)
    {
        //Debug.Log(color + " �������� ����");
        curNormal = color;
        render.material.color = curNormal;

    }
    public void ChangeColorTemp(Color color)
    {
        render.material.color = color;

    }
    public void ChangeColorTempBack()
    {
        render.material.color = curNormal;

    }

    public override string GetName()
    {
        return character.name;
    }
}

