using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Piece : LifeUnit, IObserver
{
    [Header("InGame")]
    public Place place;
    public TeamData team;


    public int forwardY;
    [SerializeField]
    protected int pieceScore;
    public int PieceScore { get { return pieceScore; } private set { pieceScore = value; } }

    [SerializeField]
    private DecidedStateLists recognized;
    public DecidedStateLists Recognized { get { return recognized; } private set { recognized = value; } }

    [SerializeField]
    private uint moveCount;
    public uint MoveCount { get { return moveCount; } }


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
    }

    private void Start()
    {
        ApplyTeamInfo();

        if (place != null)
        {
            place.BeFilled(this);
            Move();

            PlaceManager.Instance.CalculateInfluence(this);

            place.notifyObserver();
        }
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

    public Place DesireToPlace(ref float will)
    {
        Place targetPlace = decideDesireStrategy.DecidePlace(this, ref will);

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
        Debug.Log(this + "가 " + piece + "를 더이상 보호하지 않는다.");
    }

    public void BeDefended(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "에게 보호 받는다");
    }

    public void EndDefended(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "로부터 더이상 보호받지 않는다.");
    }

    public void EndThreat(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "를 더이상 위협하지 않는다.");
    }

    public void BeThreatened(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "에게 위협 당한다");
    }

    public void EndThreatened(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "로부터 더이상 위협받지 않는다.");
    }*/

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

        Debug.Log(this + "가 " + place.boardIndex + "로 이동했다.");
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
        //Debug.Log("팀 색상으로 변경");
        curNormal = team.normal;
        render.material.color = curNormal;
    }
    public void ChangeColor(Color color)
    {
        //Debug.Log(color + " 색상으로 변경");
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
    void IObserver.StateUpdate()
    {
        PlaceManager.Instance.ReCalculateInfluence(this);
        //Debug.Log("업데이트 수행: " + this);
    }

    public override string GetName()
    {
        return character.name;
    }
}

