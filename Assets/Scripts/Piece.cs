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
                

                //연출
                PlaceManager.Instance.ChangePlaceColor(curLocation, PlaceManager.PlaceType.DEFENCE);
                DialogueManager.Instance.ShowDialogueUI("Defend" + targetPiece);
            }
            else
            {
                AddThreat(targetPiece);
                targetPiece.BeThreatened(this);


                //연출
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

