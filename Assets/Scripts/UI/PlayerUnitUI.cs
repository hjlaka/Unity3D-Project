using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUnitUI : MonoBehaviour
{

    [SerializeField]
    private UnitButton unitPrefab;

    [SerializeField]
    private Transform contentTransform;


    public void AddUI(Piece piece)
    {
        UnitButton instance = Instantiate(unitPrefab, contentTransform);
        instance.name = piece.character.characterName;
        //instance.characterData = character;
        instance.GetComponentInChildren<TextMeshProUGUI>().text = piece.character.characterName;
    }


}
