using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceModel : MonoBehaviour
{
    // 데이터 담당


    private Board board;
    private Vector2Int boardIndex;
    private Piece piece;

    private Color typeColor;

    [SerializeField]
    private bool isMovableToCurPiece = false;
    [SerializeField]
    private bool isAttackableByCurPiece = false;
    [SerializeField]
    private List<PlaceObserver> influencingUnit;
    [SerializeField]
    private PlaceEffect effect;

    private SpecialMove moveAction;

    [SerializeField]
    private int heatPointTopTeam;
    [SerializeField]
    private int heatPointBottomTeam;
    [SerializeField]
    private int heatPoint;

    public int HeatPointTopTeam { get { return heatPointTopTeam; } 
        set 
        { 
            heatPointTopTeam = value; 
            heatPoint = heatPointBottomTeam + heatPointTopTeam; 
        } 
    }
    public int HeatPointBottomTeam { get { return heatPointBottomTeam; } 
        set 
        { 
            heatPointBottomTeam = value;
            heatPoint = heatPointBottomTeam + heatPointTopTeam;

        } 
    }

    public int HeatPoint { get { return heatPoint; } }



    public Board Board { get { return board; } set { board = value; } }
    public Vector2Int BoardIndex { get { return boardIndex; } set { boardIndex = value; } }
    public Piece Piece { get { return piece; } set { piece = value; } }

    public Color TypeColor { get { return typeColor; } set { typeColor = value; } }

    public bool IsMovableToCurPiece
    {
        get { return isMovableToCurPiece; }
        set { isMovableToCurPiece = value; }
    }
    public bool IsAttackableByCurPiece
    {
        get { return isAttackableByCurPiece; }
        set { isAttackableByCurPiece = value; }
    }

    public List<PlaceObserver> InfluencingUnit { get { return influencingUnit; } set { influencingUnit = value; } }
    public PlaceEffect Effect { get { return effect; } set { effect = value; } }

    public SpecialMove MoveAction
    {
        get { return moveAction; }
        set { moveAction = value; }
    }
}
