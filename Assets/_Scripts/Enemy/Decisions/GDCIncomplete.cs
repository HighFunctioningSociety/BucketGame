using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/GCDIncomplete")]
public class GDCIncomplete : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool stillOnCooldown = CheckGCD(stateMachine);
        return stillOnCooldown;
    }

    private bool CheckGCD(EnemyStateMachine stateMachine)
    {
        return !stateMachine.Enemy.AbilityManager.globalCooldownComplete;
    }
}
