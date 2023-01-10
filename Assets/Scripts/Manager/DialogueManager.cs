using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : SingleTon<DialogueManager>
{

    [SerializeField]
    private bool dialogueOn;

    [SerializeField]
    private  DialogueUI dialogueUI;

    [SerializeField]
    private float delay;



    private TextMeshProUGUI dialogueText;

    private Queue<string> dialogueQueue;

    private void Awake()
    {
        dialogueText = dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
        dialogueQueue = new Queue<string>();
        dialogueQueue.Enqueue("hi");
        dialogueQueue.Enqueue("my name is...");
    }


    private void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.IN_CONVERSATION) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("대화 다음으로 넘어가요");
            SetDialogueText();
        }
        
    }

    public void AddDialogue(ref string talk)
    {
        dialogueQueue.Enqueue(talk);
        Debug.Log("대화 추가했어요" + dialogueQueue.Count);
    }

    public void StartDialogue()
    {
        if (!dialogueOn)
        {
            GameManager.Instance.state = GameManager.GameState.TURN_FINISHED;
            return;
        }

        if (!SetDialogueText())
        {
            Debug.Log("대화 처음부터 없어요");
            return;
        }
        else
        {
            Debug.Log("대화 시작해요");
            StartCoroutine(ShowDialogueUIWithDelay());
        }
    }


    public IEnumerator ShowDialogueUIWithDelay()
    {
        yield return new WaitForSeconds(delay);

        dialogueUI.gameObject.SetActive(true);
        GameManager.Instance.state = GameManager.GameState.IN_CONVERSATION;

    }

    private bool SetDialogueText()
    {
        if (dialogueQueue.Count <= 0)
        {
            Debug.Log("대화 끝났어요");
            GameManager.Instance.state = GameManager.GameState.TURN_FINISHED;
            EndConversation();
            return false;
        }
            
        dialogueText.text = dialogueQueue.Dequeue();
        return true;
    }

    public void NextDialogueShow()
    {
        SetDialogueText();
    }

    private void EndConversation()
    {
        DisableDialogueUI();
    }

    public void DisableDialogueUI()
    {
        dialogueUI.gameObject.SetActive(false);

    }

}
