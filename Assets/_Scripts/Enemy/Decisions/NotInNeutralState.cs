using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/NotInNeutralState")]
public class NotInNeutralState : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool isStateNeutral = CheckIfStateIsNeutral(stateMachine);
        return isStateNeutral;
    }

    private bool CheckIfStateIsNeutral(EnemyStateMachine stateMachine)
    {
        return stateMachine.Enemy.InIdle == false;
    }
}
