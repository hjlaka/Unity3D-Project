using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUnitUI : MonoBehaviour
{

    [SerializeField]
    private GameObject unitPrefab;

    [SerializeField]
    private Transform contentTransform;


    public void AddUI(CharacterData character)
    {
        GameObject instance = Instantiate(unitPrefab, contentTransform);
        instance.name = character.characterName;
        instance.GetComponentInChildren<TextMeshProUGUI>().text = character.characterName;
    }


}
