using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    public Actions OnStateEnterAction;
    public Actions[] actions;
    public Transition[] transitions;
    public bool uninterruptable;
    public bool hurtboxDisabled;
    public bool isNeutralState;
    public Color sceneGizmoColor = Color.grey;

    // Start is called before the first frame update
    public void UpdateState(EnemyContainer enemy)
    {
        DoActions(enemy);
        CheckTransitions(enemy);
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
