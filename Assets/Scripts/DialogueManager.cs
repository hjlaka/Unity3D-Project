using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : SingleTon<DialogueManager>
{

    [SerializeField]
    private  DialogueUI dialogueUI;

    [SerializeField]
    private float delay;

    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        dialogueText = dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
    }
/*    public void ShowDialogueUI()
    {
        dialogueUI.gameObject.SetActive(true);
        dialogueText.text = "�׽�Ʈ �Դϴ�.";
    }*/
    public void ShowDialogueUI(string text = "�׽�Ʈ �Դϴ�.")
    {
        if (dialogueUI.gameObject.activeSelf) return;

        StartCoroutine(ShowDialogueUIWithDelay());

    }

    public IEnumerator ShowDialogueUIWithDelay()
    {
        yield return new WaitForSeconds(delay);

        dialogueUI.gameObject.SetActive(true);
        dialogueText.text = "Test";
    }

    public void DisableDialogueUI()
    {
        dialogueUI.gameObject.SetActive(false);
    }

}
