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

    [SerializeField]
    private float skipDelay;

    private TextMeshProUGUI dialogueText;

    private Queue<string> dialogueQueue;

    private void Awake()
    {
        dialogueText = dialogueUI.GetComponentInChildren<TextMeshProUGUI>();
        dialogueQueue = new Queue<string>();
        dialogueQueue.Enqueue("hi");
        dialogueQueue.Enqueue("my name is...");
    }


    public void AddDialogue(ref string talk)
    {
        dialogueQueue.Enqueue(talk);
        Debug.Log("대화 추가했어요" + dialogueQueue.Count);
    }

    private void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.IN_CONVERSATION) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("대화 다음으로 넘어가요");
            SetDialogueText();
        }

        /*string talk = "너를 공격하겠어";
        DialogueManager.Instance.AddDialogue(ref talk);*/
        
    }

    public void StartDialogue()
    {
        if (!SetDialogueText())
        {
            Debug.Log("대화 처음부터 없어요");
            return;
        }
        else
        {
            Debug.Log("대화 시작해요");
            GameManager.Instance.state = GameManager.GameState.IN_CONVERSATION;
            dialogueUI.gameObject.SetActive(true);
        }
        //ShowDialogueUI();
        //StartCoroutine(GetInputSkipKey());
    }

    private IEnumerator GetInputSkipKey()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetDialogueText();
            }
                break;
        }
    }

    public void ShowDialogueUI()
    {
        if (dialogueUI.gameObject.activeSelf) return;
        if (!SetDialogueText()) return;

        StartCoroutine(ShowDialogueUIWithDelay());

    }

    public IEnumerator ShowDialogueUIWithDelay()
    {
        yield return new WaitForSeconds(delay);


        dialogueUI.gameObject.SetActive(true);


        bool nextConversation = true;
        
        while(nextConversation)
        {
            yield return new WaitForSeconds(skipDelay);

            nextConversation = SetDialogueText();
            Debug.Log(dialogueQueue.Count);
        }
        
        EndConversation();


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
       // Debug.Log("");
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
