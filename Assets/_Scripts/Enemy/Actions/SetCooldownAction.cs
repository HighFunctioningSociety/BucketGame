using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/SetCooldown")]
public class SetCooldownAction : Action
{
    public float cooldownValue;
    public override void Act(EnemyStateMachine stateMachine)
    {
        ApplyCooldown(stateMachine);
    }

    private void ApplyCooldown(EnemyStateMachine stateMachine)
    {
        stateMachine.Enemy.AbilityManager.SetCooldown(cooldownValue);
    }
}
