using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    [FormerlySerializedAs("OnStateEnterAction")]
    [Space]
    [Header("State Actions")]
    public Actions[] OnStateEnterActions;
    public Actions[] OnStateExitActions;
    public Actions[] actions;

    [Space]
    [Header("Transition List")]
    public Transition[] transitions;

    [Space]
    [Header("State Booleans")]
    public bool uninterruptable;
    public bool hurtboxDisabled;
    public bool IsIdleState;
    public bool IsAggroState;

    [Space]
    [Header("Timings")]
    public float TimeToRemainInState;
    [Space]
    [Header("Gizmos")]
    public Color sceneGizmoColor = Color.grey;

    // Called the frame the state is entered
    public void OnStateEnter(EnemyStateMachine stateMachine)
    {
        if (OnStateEnterActions.Length > 0)
        {
            foreach (var action in OnStateEnterActions)
            {
                action.Act(stateMachine);
            }
        }
    }

    // Start is called before the first frame update
    public void UpdateState(EnemyStateMachine stateMachine)
    {
        Debug.LogWarning(("IM UPDATING"));
        DoActions(stateMachine);
        CheckTransitions(stateMachine);
    }

    // Called the frame the state exits
    public void OnStateExit(EnemyStateMachine stateMachine)
    {
        if (OnStateExitActions.Length > 0)
        {
            foreach (var action in OnStateExitActions)
            {
                action.Act(stateMachine);
            }
        }
    }

    // Update is called once per frame
    private void DoActions(EnemyStateMachine stateMachine)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(stateMachine);
        }
    }

    private void CheckTransitions(EnemyStateMachine stateMachine)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(stateMachine);

            if (decisionSucceeded)
            {
                stateMachine.TransitionToState(transitions[i].trueState);
            }
            else
            {
                stateMachine.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
