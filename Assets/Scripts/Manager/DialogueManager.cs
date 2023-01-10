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
            Debug.Log("��ȭ �������� �Ѿ��");
            SetDialogueText();
        }
        
    }

    public void AddDialogue(ref string talk)
    {
        dialogueQueue.Enqueue(talk);
        Debug.Log("��ȭ �߰��߾��" + dialogueQueue.Count);
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
            Debug.Log("��ȭ ó������ �����");
            return;
        }
        else
        {
            Debug.Log("��ȭ �����ؿ�");
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
            Debug.Log("��ȭ �������");
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
