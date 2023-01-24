using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Drama/EventDialogue")]
public class EventDialogue : ScriptableObject
{
    [Serializable]
    public struct EventDialogueUnit
    {
        public int unitID;
        public string unitName;
        public string dialogue;
    }

    public int eventID;
    public List<EventDialogueUnit> eventDialogues;
    
}
