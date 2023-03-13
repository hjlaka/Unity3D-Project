using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlaceObserver))]
[RequireComponent (typeof(UnitObserver))]
[RequireComponent(typeof(Subject))]
public class Piece : LifeUnit, ITargetable
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
    // TODO: king 클래스에 subjectAsKing 추가


    public IReturnHeat returnHeat;

    [Header("Charecter")]
    [SerializeField]
    public CharacterData character;

    private Renderer render;

    private Color curNormal;
    public Color CurNormal { get { return curNormal; }  private set { curNormal = value;  Debug.Log("색 변경"); } }

    private IDecidePlaceStrategy decideDesireStrategy;

    protected IPieceMovable movePattern;
    public IPieceMovable MovePattern { get { return movePattern; } private set { movePattern = value; } }


    public UnityAction OnPlaced;


    protected virtual void Awake()
    {
        render = GetComponentInChildren<Renderer>();

        recognized = new DecidedStateLists();

        PlaceObserver = GetComponent<PlaceObserver>();
        UnitObserver = GetComponent<UnitObserver>();
        ChessSubject = GetComponent<Subject>();
        //Debug.Log("서브젝트 할당: " + ChessSubject);
    }


    public void SetOnGame()
    {
        if (IsFree)
            return;

        moveCount = 0;
        ApplyTeamInfo();

        if (place != null)
        {
            place.BeFilled(this);
            Move();

            PlaceManager.Instance.influenceCalculator.CalculateInfluence(this);
            PlaceManager.Instance.influenceCalculator.ApplyInfluence(this);

            place.notifyObserver();
        }
    }
    public void BelongTo(Player player)
    {
        //Debug.Log("받아온 플레이어: " + player);
        this.belong = player;
        targetLocation = belong.homeLocation.position;
        belong.AddPiece(this);
    }

    public bool IsSameTeam(Piece other)
    {
        return this.team.TeamId == other.team.TeamId;   //Belong으로 판단할 수도 있다.
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
            Debug.Log("이동할 수 있는 곳이 없다");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }
        if(targetPlace != null)
        {
            if(targetPlace.Piece != null)
            {
                Debug.Log("공격하자!");
                //AttackTo(targetPlace);
                ChessEventManager.Instance.SubmitEvent(new ChessEvent(ChessEvent.EventType.ATTACK, this, targetPlace.Piece));
                ChessEventManager.Instance.GetEvent();
                DialogueManager.Instance.CheckDialogueEvent();
                PlaceManager.Instance.MoveProcess(this, targetPlace);
            }
            else
            {
                Debug.Log("이동하자!");
                PlaceManager.Instance.MoveProcess(this, targetPlace);
            }   
        }
        else
        {
            Debug.Log("움직이고 싶은 곳이 없다");
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
        if (null == team)
            return;

        CurNormal = team.normal;
        render.material.color = CurNormal;

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

    public virtual Piece SetInPlace(Place place)
    {
        // 이동할 자리에 기존에 있던 기물
        Piece preSetPiece = place.Piece;
       
        Place oldPlace = this.place;
        this.place = null;
        oldPlace?.BeEmpty();

        this.place = place;
        place.BeFilled(this);

        Move();

        OnPlaced?.Invoke();

        Debug.Log(this + "가 " + place.boardIndex + "로 이동했다.");
        if(oldPlace != null && oldPlace.board == place.board && place.board.FollowRule)
            moveCount++;

        if(preSetPiece != null) Debug.Log("기존에 있던 기물: " + preSetPiece);

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
        Debug.Log("팀 색상으로 변경");
        CurNormal = team.normal;
        render.material.color = CurNormal;
    }
    public void ChangeColor(Color color)
    {
        Debug.Log(color + " 색상으로 변경");
        CurNormal = color;
        render.material.color = CurNormal;

    }
    public void ChangeColorTemp(Color color)
    {
        render.material.color = color;

    }
    public void ChangeColorTempBack()
    {
        render.material.color = CurNormal;

    }

    public override string GetName()
    {
        return character.name;
    }

    public ITargetable.Type React()
    {
        return ITargetable.Type.Attack;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}

