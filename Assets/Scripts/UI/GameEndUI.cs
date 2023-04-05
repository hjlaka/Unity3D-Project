using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI resultText;
    public void UpdateUI(string resultText)
    {
        this.resultText.text = resultText;
        gameObject.SetActive(true);
    }
}
