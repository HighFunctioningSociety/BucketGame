using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/TransitionToAbilityState")]
public class TransitionToAbilityState : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool abilityIsAvailable = CanUseAbility(enemy);
        return abilityIsAvailable;
    }

    private bool CanUseAbility(EnemyContainer _enemy)
    {
        if (_enemy.AbilityManager.globalCooldownComplete)
        {  
            CheckProximityAbilities(_enemy);
            CheckFullScreenAbilities(_enemy);
            CheckMovementAbilities(_enemy);
            int abilityCount = _enemy.AbilityManager.useableAbilities.Count;
            if (abilityCount != 0)
            {
                int j = Random.Range(0, abilityCount);
                _enemy.AbilityManager.abilityToUse = _enemy.AbilityManager.useableAbilities[j];
                return true;
            }
        }
        return false;
    }

    private void CheckProximityAbilities(EnemyContainer _enemy)
    {
        EnemyProximityTriggerable[] proximityAbilityList = _enemy.AbilityManager.proximityAbilityList;
        if (proximityAbilityList.Length == 0)
            return;

        foreach (EnemyProximityTriggerable ability in proximityAbilityList)
        {
            Vector2 range = new Vector2(ability.rangeX, ability.rangeY);
            Collider2D checkForPlayer = Physics2D.OverlapBox(ability.transform.position, range, 0, _enemy.PlayerLayer);
            if (checkForPlayer != null && ability.abilityCooldown)
            {
                _enemy.AbilityManager.useableAbilities.Add(ability);
            }
        }
    }

    private void CheckFullScreenAbilities(EnemyContainer _enemy)
    {
        EnemyTriggerable[] fullscreenAbilityList = _enemy.AbilityManager.fullscreenAbilityList;
        if (fullscreenAbilityList.Length == 0)
            return;

        foreach (EnemyTriggerable ability in fullscreenAbilityList)
        {
            if (ability.abilityCooldown)
            {
                _enemy.AbilityManager.useableAbilities.Add(ability);
            }
        }
    }

    private void CheckMovementAbilities(EnemyContainer _enemy)
    {
        EnemyMoveTriggerable[] movementAbilityList = _enemy.AbilityManager.movementAbilityList;
        if (movementAbilityList.Length == 0)
            return;

        foreach (EnemyMoveTriggerable ability in movementAbilityList)
        {
            if (ability.abilityCooldown)
            {
                _enemy.AbilityManager.useableAbilities.Add(ability);
            }
        }
    }
}
