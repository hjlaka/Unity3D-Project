using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Piece : LifeUnit
{
    [Header("InGame")]
    public Place place;
    public TeamData team;
    [SerializeField]
    private Color mouseOver;

    public int forwardY;

    protected int pieceScore;
    public int PieceScore { get { return pieceScore; } private set { pieceScore = value; } }


    public IReturnHeat returnHeat;

    [Header("Charecter")]
    [SerializeField]
    public CharacterData character;

    private Renderer render;
    private Color curNormal;

    private IDecidePlaceStrategy decideDesireStrategy;

    protected IPieceMovable movePattern;


    private StateLists recognized;
    public StateLists Recognized { get { return recognized; } private set { recognized = value; } }

    //private List<Piece> defendFor;
    //private List<Piece> threatTo;
    //private List<Place> movableTo;
    //private List<Place> influenceable;

    #region 리스트 프로퍼티
    /*public List<Piece> DefendFor
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
    }*/

    /*public List<Place> MovableTo
    {
        get { return movableTo; } 
        private set { movableTo = value;  }
    }*/

   /* public List<Place> Influenceable
    {
        get { return influenceable; }
        private set { influenceable = value; }
    }*/

    #endregion

    protected virtual void Awake()
    {
        render = GetComponentInChildren<Renderer>();
        /*defendFor = new List<Piece>();
        threatTo = new List<Piece>();
        movableTo = new List<Place>();*/
        recognized = new StateLists();
        //influenceable = new List<Place>();
    }

    private void Start()
    {
        ApplyTeamInfo();

        if (place != null)
        {
            SetInPlace(place);
        }   
    }

    public void PlaceToDesire()
    {

        //PlaceManager.Instance.SelectPiece(this);

        if(Recognized.movable.Count <= 0)
        {
            Debug.Log("이동할 수 있는 곳이 없다");
            PlaceManager.Instance.CancleSelectPiece();
            return;
        }

        Place targetPlace = decideDesireStrategy.DecidePlace(this);
        if(targetPlace != null)
        {
            if(targetPlace.piece != null)
            {
                Debug.Log("공격하자!");
                //AttackTo(targetPlace);
                PlaceManager.Instance.Attack(this, targetPlace.piece);
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
        //Place targetPlace;
        //decideDesireStrategy.DecidePlace(out targetPlace);

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

    #region 리스트 관리
    /*public void ClearMovable()
    {
        //Debug.Log("이동 클리어" + MovableTo.Count);
        MovableTo.Clear();
    }

    // 임시 생성 
    public void ClearThreat()
    {
        //Debug.Log("위협 클리어" + ThreatTo.Count);
        ThreatTo.Clear();
    }

    public void ClearDefence()
    {
        //Debug.Log("방어 클리어" + DefendFor.Count);
        DefendFor.Clear();
    }

    public void ClearInfluence()
    {
        //Debug.Log("영향권 클리어" + Influenceable.Count);
        Influenceable.Clear();
    }*/
    // 여기까지 임시 생성

/*    public void AddMovable(Place place)
    {
        movableTo.Add(place);
    }*/
   /* public void AddInfluence(Place place)
    {
        recognized.AddInfluenceable(place);

        if (team.direction == TeamData.Direction.DownToUp)
            place.HeatPointBottomTeam++;
        else
            place.HeatPointTopTeam++;

    }
    public void AddDefence(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "를 보호한다");
        recognized.AddDefending(piece);
        //DialogueManager.Instance.AddDialogue(ref character.characterName, ref character.defending);
    }*/

    public void EndDefence(Piece piece)
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

    /*public void AddThreat(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "을 위협한다");
        ThreatTo.Add(piece);
        //DialogueManager.Instance.AddDialogue(ref character.characterName, ref character.threatening);
    }*/

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
    public virtual void RecognizeRange(Vector2Int location)
    {
        movePattern.RecognizeRange(location);
    }

    public void SetDecidePlaceStrategy(IDecidePlaceStrategy decidePlaceStrategy)
    {
        decideDesireStrategy = decidePlaceStrategy;
    }

    public virtual List<Place> ReturnMovablePlaces(Vector2Int location)
    {
        return null;
    }




    // ----------------------------------------------------------- 폰 움직임을 위해 추가 { 
    
    private void OnMouseOver()
    {
        //Debug.Log(string.Format("{0} 마우스 가리킴", gameObject.name));
        render.material.color = mouseOver;
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} 밖으로 마우스 나감", gameObject.name));
        render.material.color = curNormal;
    }

    private void OnMouseUpAsButton()
    {
        /*if (!GameManager.Instance.isPlayerTurn)
        {
            Debug.Log("플레이어의 차례가 아닙니다.");
            return;
        }*/
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            //Debug.Log(string.Format("{0} 클릭", gameObject.name));
            PlaceManager.Instance.SelectPiece(this);
        }
        
        else if(GameManager.Instance.state == GameManager.GameState.SELECTING_PLACE)
        {
            // 자신이라면
            if (PlaceManager.Instance.SelectedPiece == this)
                PlaceManager.Instance.CancleSelectPiece();

            // 같은 팀 기물이라면
            else if (this.team.TeamId == PlaceManager.Instance.SelectedPiece.team.TeamId)
            {
                PlaceManager.Instance.CancleSelectPiece();
                PlaceManager.Instance.SelectPiece(this);
            }
            // 다른 팀 기물이라면
            else
            {
                // 공격할 수 있다면
                if(place.IsAttackableByCurPiece)
                {
                    //공격 
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
        //PlaceManager.Instance.MoveProcess(this, place);
    }
    private void BeAttackedBy(Piece piece)
    {
        Place attackPlace = this.place;
        PlaceManager.Instance.ExpelPiece(this);
        PlaceManager.Instance.MoveProcess(piece, attackPlace);
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
}

