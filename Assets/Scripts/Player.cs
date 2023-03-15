using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField]
    protected List<Piece> pieceList;
    [SerializeField]
    protected Unit coreUnit;
    public Transform homeLocation;
    public Unit CoreUnit { get { return coreUnit; } set { coreUnit = value; } }


    public virtual void AddPiece(Piece piece)
    {
        pieceList.Add(piece);
    }

    public virtual void DoTurn()
    {

    }

    public void GoToHome()
    {
        for(int i = 0; i < pieceList.Count; i++)
        {
            if (pieceList[i].IsFree) continue;
            pieceList[i].IsFree = true;
        }
    }

    public void ShoutOnGame()
    {
        if(pieceList.Count <= 0) return;

        Piece shoutingPiece = pieceList[Random.Range(0, pieceList.Count)];
        DialogueManager.DialogueUnit shouting = new DialogueManager.DialogueUnit(shoutingPiece, shoutingPiece.character.onGame);
        DialogueManager.Instance.AddDialogue(shouting);
    }

    public bool CheckCoreOnGame()
    {
        if (coreUnit == null)
            return true;
        return coreUnit.IsOnGame;
    }

    public bool CheckActionable()
    {
        for(int i = 0; i <= pieceList.Count; i++)
        {
            if (pieceList[i].Recognized.movable.Count > 0)
                return true;
        }
        return false;
    }

    public void GiveUp()
    {

    }

}
