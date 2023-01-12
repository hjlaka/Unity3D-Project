using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : SingleTon<PlayerDataManager>
{
    private List<Piece> playerPieces;

    public List<Piece> unitList;

    [SerializeField]
    private PlayerUnitUI ui;

    [SerializeField]
    private TeamData playerTeam;
    public TeamData PlayerTeam
    {
        get { return playerTeam;  }
        private set { playerTeam = value; }
    }

    public List<Piece> PlayerPieces 
    { get 
        { 
            return playerPieces; 
        } 
    }

    private void Awake()
    {
        playerPieces = new List<Piece>();
        unitList = new List<Piece>();
    }

    public void AddPlayerPiece(Piece piece)
    {
        playerPieces.Add(piece);

        AddUnit(piece);
    }

    public void AddUnit(Piece piece)
    {
        unitList.Add(piece);
        ui.AddUI(piece);
    }

    public void RemoveUnit(Piece piece)
    {
        unitList.Remove(piece);
    }

    public void EnablePlayerListUI()
    {
        ui.gameObject.SetActive(true);
    }
    public void DisablePlayerListUI()
    {
        ui.gameObject.SetActive(false);
    }
}
