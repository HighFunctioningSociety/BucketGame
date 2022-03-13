using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager dm;
    private Queue<string> dialogueQueue;
    private DialogueDecision decision;
    private TriggerableEvent dialogueEvent;

    public Text nameText;
    public Text dialogueText;
    public Text decisionText;
    public GameObject continueButton;
    public GameObject yesButton;
    public GameObject noButton;
    public Animator dialogueAnimator;
    public Animator decisionAnimator;

    private void Awake()
    {
        if (dm == null)
        {
            dm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dialogueQueue = new Queue<string>();
    }

    public void StartDialogue(Dialogue _dialogue)
    {
        Inputs.supressJump = true;
        PrepPlayerForDialogue();
        TargetDialogueContinueButton();
        _UIManager.UIManager.EnableDialogueParent();
        _UIManager.UIManager.MenuFadeOut();

        nameText.text = _dialogue.name;
        dialogueEvent = _dialogue.dialogueEvent;
        decision = _dialogue.decision;
        dialogueQueue.Clear();

        foreach (string sentence in _dialogue.sentences)
        {
            dialogueQueue.Enqueue(sentence);
        }

        StartCoroutine(DialogueDisplayDelay());
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = dialogueQueue.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, dialogueText));
    }

    private void EndDialogue()
    {
        dialogueAnimator.SetBool("IsOpen", false);

        if (dialogueEvent != null)
        {
            dialogueEvent.TriggerEvent();
        }

        dialogueEvent = null;

        if (decision == null)
        {
            StartCoroutine(MenuDisplayDelay());
            _UIManager.UIManager.DisableDialogueParent();
            _GameManager.AcceptPlayerInput();
        }
        else
        {
            StartDecision();
        }
    }

    private void StartDecision()
    {
        decisionAnimator.SetBool("IsOpen", true);
        StartCoroutine(TypeSentence(decision.decisionInputText, decisionText));
        TargetYesDecisionButton();
    }

    public void YesDecision()
    {
        if (decision.yesEvent != null)
        {
            decision.yesEvent.TriggerEvent();
        }

        StartDialogue(decision.yesDialogueBranch.GetComponent<RawDialogue>().dialogue);
        decisionAnimator.SetBool("IsOpen", false);
        decision = null;
    }

    public void NoDecision()
    {
        if (decision.noEvent != null)
        {
            decision.noEvent.TriggerEvent();
        }

        StartDialogue(decision.noDialogueBranch.GetComponent<RawDialogue>().dialogue);
        decisionAnimator.SetBool("IsOpen", false);
        decision = null;
    }


    private void TargetDialogueContinueButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(continueButton);
    }

    private void TargetYesDecisionButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(yesButton);
    }

    private void PrepPlayerForDialogue()
    {
        _GameManager.RelinquishPlayerInput();
        Inputs.DisableHorizontal();
        PlayerContainer.KillVelocity();
    }


    IEnumerator TypeSentence (string sentence, Text textDestination)
    {
        textDestination.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textDestination.text += letter;
            yield return null;
        }
    }

    IEnumerator MenuDisplayDelay()
    {
        yield return new WaitForSeconds(0.7f);
        Inputs.supressJump = false;
        _UIManager.UIManager.MenuFadeIn();
    }

    IEnumerator DialogueDisplayDelay()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueAnimator.SetBool("IsOpen", true);
        DisplayNextSentence();
    }
}
