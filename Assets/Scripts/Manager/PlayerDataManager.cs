using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : SingleTon<PlayerDataManager>
{
    [SerializeField]
    private List<Piece> playerPieces = new List<Piece>();

    public List<Piece> unitList;    // 게임에 참여할 수 있는 유닛


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
        unitList = new List<Piece>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            if(ui.gameObject.activeSelf)
            {
                DisablePlayerListUI();
            }
            else
            {
                EnablePlayerListUI();
            }
        }
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
