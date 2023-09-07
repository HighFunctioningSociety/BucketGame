using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    [Space]
    [Header("State Actions")]
    public Actions OnStateEnterAction;
    public Actions OnStateExitAction;
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
    public void OnStateEnter(EnemyContainer enemy)
    {
        OnStateEnterAction.Act(enemy);
    }

    // Start is called before the first frame update
    public void UpdateState(EnemyContainer enemy)
    {
        DoActions(enemy);
        CheckTransitions(enemy);
    }

    // Called the frame the state exits
    public void OnStateExit(EnemyContainer enemy)
    {
        OnStateExitAction.Act(enemy);
    }

    // Update is called once per frame
    private void DoActions(EnemyContainer enemy)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(enemy);
        }
    }

    private void CheckTransitions(EnemyContainer enemy)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(enemy);

            if (decisionSucceeded)
            {
                enemy.TransitionToState(transitions[i].trueState);
            }
            else
            {
                enemy.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
