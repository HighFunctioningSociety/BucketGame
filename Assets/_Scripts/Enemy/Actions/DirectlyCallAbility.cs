using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/DirectlyCallAbility")]
public class DirectlyCallAbility : Actions
{
    public int directCallIndex;

    public override void Act(EnemyStateMachine stateMachine)
    {
        UseAbility(stateMachine);
    }

    private void UseAbility(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.Enemy.AbilityManager.abilityAlreadyActivated)
        {
            stateMachine.Enemy.AbilityManager.abilityAlreadyActivated = true;
            stateMachine.Enemy.AbilityManager.UseAbility(stateMachine.Enemy.AbilityManager.directCallAbilityList[directCallIndex]);
        }
    }
}
