using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDialogueTrigger : TriggerableEvent
{
    public Dialogue dialogue; 

    public override void TriggerEvent()
    {
        TriggerDialogue();
    }

    private void TriggerDialogue()
    {
        DialogueManager.dm.StartDialogue(dialogue);
    }
}

