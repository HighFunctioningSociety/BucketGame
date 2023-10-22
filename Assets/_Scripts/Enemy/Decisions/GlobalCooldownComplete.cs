using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/GCDComplete")]
public class GlobalCooldownComplete : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool cooldownComplete = CheckGlobalCooldown(stateMachine);
        return cooldownComplete;
    }

    private bool CheckGlobalCooldown(EnemyStateMachine stateMachine)
    {
        return stateMachine.Enemy.AbilityManager.globalCooldownComplete;
    }
}
