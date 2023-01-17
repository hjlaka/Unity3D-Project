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

        
        dialogueQueue.Enqueue(new DialogueUnit("����", "�׽�Ʈ"));
        dialogueQueue.Enqueue(new DialogueUnit("����2", "�׽�Ʈ2"));
    }


/*    private void Update()
    {
        if (GameManager.Instance.state != GameManager.GameState.IN_CONVERSATION) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("��ȭ �������� �Ѿ��");
            SetDialogueText();
        }
        
    }*/

    public void AddDialogue(ref string name, ref string talk)
    {
        DialogueUnit newDialogue = new DialogueUnit(name, talk);
        dialogueQueue.Enqueue(newDialogue);
        Debug.Log("��ȭ �߰��߾��" + dialogueQueue.Count);
    }

    public void AddDialogue(DialogueUnit dialogue)
    {
        dialogueQueue.Enqueue(dialogue);
        Debug.Log("��ȭ �߰��߾��" + dialogueQueue.Count);
    }

    public void StartDialogue()
    {

        GameManager.Instance.ChangeGameState(GameManager.GameState.IN_CONVERSATION);

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

    }

    private bool SetDialogueText()
    {
        if (dialogueQueue.Count <= 0)
        {
            EndConversation();
            return false;
        }

        DialogueUnit dialogue = dialogueQueue.Dequeue();
        dialogueName.text = dialogue.name;
        dialogueText.text = dialogue.dialogue;
        return true;
    }

    public void NextDialogueShow()
    {
        SetDialogueText();
    }

    private void EndConversation()
    {
        Debug.Log("��ȭ �������");
        GameManager.Instance.GoBackGameState();
        DisableDialogueUI();
    }

    public void DisableDialogueUI()
    {
        dialogueUI.gameObject.SetActive(false);

    }

}
