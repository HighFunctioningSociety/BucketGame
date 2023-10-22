using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasRetreated : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        return RetreatFinished(stateMachine);
    }

    private bool RetreatFinished(EnemyStateMachine stateMachine)
    {
        bool gcdComplete = stateMachine.Enemy.AbilityManager.globalCooldownComplete;
        bool hasRetreated = stateMachine.Enemy.Triggers.retreated;
        return gcdComplete && hasRetreated;
    }
}
