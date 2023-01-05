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
        Debug.Log("이동 클리어" + MovableTo.Count);
        MovableTo.Clear();
    }

    // 임시 생성 
    public void ClearThreat()
    {
        Debug.Log("위협 클리어" + ThreatTo.Count);
        ThreatTo.Clear();
    }

    public void ClearDefence()
    {
        Debug.Log("방어 클리어" + DefendFor.Count);
        DefendFor.Clear();
    }

    public void ClearInfluence()
    {
        Debug.Log("영향권 클리어" + Influenceable.Count);
        Influenceable.Clear();
    }
    // 여기까지 임시 생성

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
        Debug.Log(this + "가 " + piece + "를 보호한다");
        defendFor.Add(piece);
    }

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

    public void AddThreat(Piece piece)
    {
        Debug.Log(this + "가 " + piece + "을 위협한다");
        ThreatTo.Add(piece);
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




    // ----------------------------------------------------------- 폰 움직임을 위해 추가 { 
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
            Debug.Log("앞에 장애물 없다");

            AddMovable(targetPlace);
            //연출 이동

            return false;
        }
    }

    protected bool RecognizePieceOnlyInfluence(Vector2Int curLocation)
    {
        Piece targetPiece = this.place.board.places[curLocation.x, curLocation.y].piece;
        Place targetPlace = this.place.board.places[curLocation.x, curLocation.y];
        // TODO:
        // 과열도는 영향권 범위의 변경과 함께 연산되도록 엮기
        targetPlace.HeatPoint++;

        if (targetPiece != null)
        {
            RecognizeDefendAndAttack(curLocation, targetPiece, targetPlace);

            return true;
        }
        else
        {
            // 기물이 없는 경우 이동할 수 없음
            // 영향권은 맞음
            AddInfluence(targetPlace);
            return false;
        }
    }
    // } 폰 움직임을 위해 추가 -----------------------------------------------------------

    private void RecognizeDefendAndAttack(Vector2Int curLocation, Piece targetPiece, Place targetPlace)
    {
        if (targetPiece.team.TeamId == team.TeamId)
        {
            AddDefence(targetPiece);
            targetPiece.BeDefended(this);

            // 방어할 수 있는 자리는 이동할 수 없지만 영향권 내의 자리이다.
            AddInfluence(targetPlace);


            //연출
            //PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.DEFENCE);
            //DialogueManager.Instance.ShowDialogueUI("Defend" + targetPiece);
        }
        else
        {
            AddThreat(targetPiece);
            targetPiece.BeThreatened(this);

            // 공격할 수 있는 자리는 이동할 수 있는 자리 이기도 하다.
            // 공격할 수 있는 자리는 영향권 내 자리이다.
            AddMovable(targetPlace);
            AddInfluence(targetPlace);


            //연출
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
        //Debug.Log(string.Format("{0} 마우스 가리킴", gameObject.name));
        render.material.color = mouseOver;
    }

    private void OnMouseExit()
    {
        //Debug.Log(string.Format("{0} 밖으로 마우스 나감", gameObject.name));
        render.material.color = normal;
    }

    private void OnMouseUpAsButton()
    {
        if (GameManager.Instance.state == GameManager.GameState.SELECTING_PIECE)
        {
            Debug.Log(string.Format("{0} 클릭", gameObject.name));
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
            }

        }
    }
}

