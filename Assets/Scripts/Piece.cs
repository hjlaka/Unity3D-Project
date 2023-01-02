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
        attackTo.Add(piece);
    }
    public void AddDefence(Piece piece)
    {
        defendFor.Add(piece);
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
            // 같은 팀 기물이라면
            PlaceManager.Instance.SelectedPieceInit();
            PlaceManager.Instance.SelectPiece(this);

            // 다른 팀 기물이라면
            // 공격할 수 있다면
        }
    }
}

