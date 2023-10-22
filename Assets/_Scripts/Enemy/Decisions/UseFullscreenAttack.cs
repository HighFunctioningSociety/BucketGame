using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/UseFullscreenAttack")]
public class UseFullscreenAttack: Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool timerDone = CheckTimer(stateMachine);
        return timerDone;
    }

    public bool CheckTimer(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.AbilityManager.fullscreenAbilityList.Length == 0)
        {
            return false;
        }

        if (stateMachine.Enemy.AbilityManager.globalCooldownComplete)
        {
            EnemyTriggerable[] fullscreenAbilityList = stateMachine.Enemy.AbilityManager.fullscreenAbilityList;
            EnemyTriggerable[] useableAbilities = new EnemyTriggerable[fullscreenAbilityList.Length];
            int i = 0;

            if ((stateMachine.StateTimeElapsed > stateMachine.Enemy.FullscreenAttackTiming) && stateMachine.CurrentState.IsIdleState)
            {
                //abilities that can be used regardless of player distance
                foreach (EnemyTriggerable ability in fullscreenAbilityList)
                {
                    useableAbilities[i] = ability;
                    i++;
                }

            
                int j = Random.Range(0, i);

                stateMachine.Enemy.AbilityManager.abilityToUse = useableAbilities[j];
                return true;
            }
        }
        return false;
    }
}
