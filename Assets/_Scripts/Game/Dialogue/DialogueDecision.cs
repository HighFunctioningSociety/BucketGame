using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDecision : MonoBehaviour
{
    [TextArea(3, 10)]
    public string decisionInputText;

    public GameObject yesDialogueBranch;
    public TriggerableEvent yesEvent;

    public GameObject noDialogueBranch;
    public TriggerableEvent noEvent;
}
