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
        if (!_enemy.abilityManager.abilityAlreadyActivated)
        {
            EnemyTriggerable abilityToUse = _enemy.abilityManager.abilityToUse;
            _enemy.abilityManager.abilityAlreadyActivated = true;            
            _enemy.abilityManager.UseAbility(abilityToUse);
        }
    }
}
