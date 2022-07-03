using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Ability")]
public class AbilityAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        UseAbility(enemy);
    }

    private void UseAbility(EnemyContainer _enemy)
    {
        if (!_enemy.AbilityManager.abilityAlreadyActivated)
        {
            EnemyTriggerable abilityToUse = _enemy.AbilityManager.abilityToUse;
            _enemy.AbilityManager.abilityAlreadyActivated = true;            
            _enemy.AbilityManager.UseAbility(abilityToUse);
        }
    }
}
