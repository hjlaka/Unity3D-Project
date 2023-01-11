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


    public void AddUI(CharacterData character)
    {
        UnitButton instance = Instantiate(unitPrefab, contentTransform);
        instance.name = character.characterName;
        instance.characterData = character;
        instance.GetComponentInChildren<TextMeshProUGUI>().text = character.characterName;
    }


}
