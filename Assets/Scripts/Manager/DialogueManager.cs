using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : SingleTon<DialogueManager>
{

/*    [SerializeField]
    private bool dialogueOn;*/

    [SerializeField]
    private  DialogueUI dialogueUI;

    [SerializeField]
    private float delay;


    [SerializeField]
    private TextMeshProUGUI dialogueName;
    [SerializeField]
    private TextMeshProUGUI dialogueText;


    private Queue<DialogueUnit> dialogueQueue;


    public class DialogueUnit
    {
        public string name;
        public string dialogue;

        public DialogueUnit(string name, string dialogue)
        {
            this.name = name;
            this.dialogue = dialogue;
        }
    }

    private void Awake()
    {
        dialogueQueue = new Queue<DialogueUnit>();

        
        dialogueQueue.Enqueue(new DialogueUnit("샘플", "테스트"));
        dialogueQueue.Enqueue(new DialogueUnit("샘플2", "테스트2"));
    }

    public void CheckDialogueEvent()
    {
        if (IsDialogueExist())
        {
            GameManager.Instance.ChangeGameState(GameManager.GameState.IN_CONVERSATION);
            StartCoroutine(ShowDialogueUIWithDelay());
            SetDialogueText();
        }
        else
            Debug.Log("대화 없음");
    }

    private void SetDialogueText()
    {
        DialogueUnit dialogue = dialogueQueue.Dequeue();
        dialogueName.text = dialogue.name;
        dialogueText.text = dialogue.dialogue;
    }

    public void NextDialogueShow()
    {
        if (!IsDialogueExist())
        {
            EndConversation();
        }
        else
        {
            SetDialogueText();
        }
    }

    public void AddDialogue(ref string name, ref string talk)
    {
        DialogueUnit newDialogue = new DialogueUnit(name, talk);
        dialogueQueue.Enqueue(newDialogue);
        Debug.Log("대화 추가했어요" + dialogueQueue.Count);
    }

    public void AddDialogue(DialogueUnit dialogue)
    {
        dialogueQueue.Enqueue(dialogue);
        Debug.Log("대화 추가했어요" + dialogueQueue.Count);
    }


    private IEnumerator ShowDialogueUIWithDelay()
    {
        yield return new WaitForSeconds(delay);

        dialogueUI.gameObject.SetActive(true);
    }

    private void EndConversation()
    {
        Debug.Log("대화 끝났어요");
        DisableDialogueUI();
        GameManager.Instance.GoBackGameState();
    }


    private void DisableDialogueUI()
    {
        dialogueUI.gameObject.SetActive(false);
    }

    public bool IsDialogueExist()
    {
        if (dialogueQueue.Count > 0)
            return true;
        else
            return false;
    }

}
