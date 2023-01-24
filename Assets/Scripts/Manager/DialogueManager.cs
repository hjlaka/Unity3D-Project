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

    private DialogueUnit prevDialogue;

    private List<Unit> participations;


    public class DialogueUnit
    {
        public Unit unit;
        public string name;
        public string talk;

        public DialogueUnit(Unit unit, string talk)
        {
            this.unit = unit;
            this.name = unit.GetName();
            this.talk = talk;
        }

        public DialogueUnit(string name, string talk)
        {
            this.name = name;
            this.talk = talk;
            this.unit = null;
        }
    }

    private void Awake()
    {
        dialogueQueue = new Queue<DialogueUnit>();

        
        dialogueQueue.Enqueue(new DialogueUnit("샘플", "테스트"));
        dialogueQueue.Enqueue(new DialogueUnit("샘플2", "테스트2"));

        participations = new List<Unit>();
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
        dialogueText.text = dialogue.talk;
        if(dialogue.unit != null)
        {
            participations.Add(dialogue.unit);
            CameraController.Instance.AddToTargetGroup(dialogue.unit.transform, participations.Count);
            CameraController.Instance.SetFreeCam();
        }
        else
        {
            CameraController.Instance.SetCamToTopDownView();
        }

        prevDialogue = dialogue;
            
    }

    public void NextDialogueShow()
    {
        if (!IsDialogueExist())
        {
            EndConversation();
        }
        else
        {
            GetOutDialogue();
            SetDialogueText();
        }
    }

    private void GetOutDialogue()
    {
        if(prevDialogue != null)
        {
            if(prevDialogue.unit != null)
            {
                CameraController.Instance.RemoveFromTargetGroup(prevDialogue.unit.transform);
            }
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
        DeleteParticipations();
        GameManager.Instance.GoBackGameState();
        CameraController.Instance.SetCamToTopDownView();
    }

    private void DeleteParticipations()
    {
        for(int i = 0; i < participations.Count; i++)
        {
            CameraController.Instance.RemoveFromTargetGroup(participations[i].transform);
        }
        participations.Clear();
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
