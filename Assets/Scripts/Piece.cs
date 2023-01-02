using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Place place;

    private Renderer render;

    [SerializeField]
    private Color mouseOver;
    [SerializeField]
    private Color normal;

    
    private void Awake()
    {
        render = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        transform.position = place.transform.position;
        render.material.color = normal;
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
        //Debug.Log(string.Format("{0} 클릭", gameObject.name));
        PlaceManager.Instance.selectedPiece = this;
        PlaceManager.Instance.ShowPlaceable();
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

    public virtual void ShowMovable() { }
}
