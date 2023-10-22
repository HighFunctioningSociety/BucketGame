using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StartReposition")]
public class RepositionDecision : Decision
{
    [HideInInspector] public EnemyContainer enemy;

    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool shouldReposition = ShouldTriggerReposition(stateMachine);
        return shouldReposition;
    }

    private bool ShouldTriggerReposition(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.AbilityManager.repositionCooldownComplete)
        {
            stateMachine.Enemy.Reposition.InitiateReposition();
            Debug.Log("reposition");
            return true;
        }
        else
        {
            return false;
        }
    }
}
