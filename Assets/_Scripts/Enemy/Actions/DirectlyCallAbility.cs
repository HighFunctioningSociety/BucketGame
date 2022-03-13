using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/DirectlyCallAbility")]
public class DirectlyCallAbility : Actions
{
    public int directCallIndex;

    public override void Act(EnemyContainer enemy)
    {
        UseAbility(enemy);
    }

    private void UseAbility(EnemyContainer _enemy)
    {
        if (!_enemy.abilityManager.abilityAlreadyActivated)
        {
            _enemy.abilityManager.abilityAlreadyActivated = true;
            _enemy.abilityManager.UseAbility(_enemy.abilityManager.directCallAbilityList[directCallIndex]);
        }
    }
}
