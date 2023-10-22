using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Ability")]
public class AbilityAction : Actions
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        UseAbility(stateMachine);
    }

    private void UseAbility(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.Enemy.AbilityManager.abilityAlreadyActivated)
        {
            EnemyTriggerable abilityToUse = stateMachine.Enemy.AbilityManager.abilityToUse;
            stateMachine.Enemy.AbilityManager.abilityAlreadyActivated = true;            
            stateMachine.Enemy.AbilityManager.UseAbility(abilityToUse);
        }
    }
}
