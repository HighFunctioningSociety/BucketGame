using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/TransitionToAbilityState")]
public class TransitionToAbilityState : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool abilityIsAvailable = CanUseAbility(stateMachine);
        return abilityIsAvailable;
    }

    private bool CanUseAbility(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.AbilityManager.globalCooldownComplete)
        {  
            CheckProximityAbilities(stateMachine);
            CheckFullScreenAbilities(stateMachine);
            CheckMovementAbilities(stateMachine);
            int abilityCount = stateMachine.Enemy.AbilityManager.useableAbilities.Count;
            if (abilityCount != 0)
            {
                int j = Random.Range(0, abilityCount);
                stateMachine.Enemy.AbilityManager.abilityToUse = stateMachine.Enemy.AbilityManager.useableAbilities[j];
                return true;
            }
        }
        return false;
    }

    private void CheckProximityAbilities(EnemyStateMachine stateMachine)
    {
        EnemyProximityTriggerable[] proximityAbilityList = stateMachine.Enemy.AbilityManager.proximityAbilityList;
        if (proximityAbilityList.Length == 0)
            return;

        foreach (EnemyProximityTriggerable ability in proximityAbilityList)
        {
            Vector2 range = new Vector2(ability.rangeX, ability.rangeY);
            Collider2D checkForPlayer = Physics2D.OverlapBox(ability.transform.position, range, 0, Constants.Layers.Player);
            if (checkForPlayer != null && ability.abilityCooldown)
            {
                stateMachine.Enemy.AbilityManager.useableAbilities.Add(ability);
            }
        }
    }

    private void CheckFullScreenAbilities(EnemyStateMachine stateMachine)
    {
        EnemyTriggerable[] fullscreenAbilityList = stateMachine.Enemy.AbilityManager.fullscreenAbilityList;
        if (fullscreenAbilityList.Length == 0)
            return;

        foreach (EnemyTriggerable ability in fullscreenAbilityList)
        {
            if (ability.abilityCooldown)
            {
                stateMachine.Enemy.AbilityManager.useableAbilities.Add(ability);
            }
        }
    }

    private void CheckMovementAbilities(EnemyStateMachine stateMachine)
    {
        EnemyMoveTriggerable[] movementAbilityList = stateMachine.Enemy.AbilityManager.movementAbilityList;
        if (movementAbilityList.Length == 0)
            return;

        foreach (EnemyMoveTriggerable ability in movementAbilityList)
        {
            if (ability.abilityCooldown)
            {
                stateMachine.Enemy.AbilityManager.useableAbilities.Add(ability);
            }
        }
    }
}
