using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/UseFullscreenAttack")]
public class UseFullscreenAttack: Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool timerDone = CheckTimer(enemy);
        return timerDone;
    }

    public bool CheckTimer(EnemyContainer _enemy)
    {
        if (_enemy.AbilityManager.fullscreenAbilityList.Length == 0)
        {
            return false;
        }

        if (_enemy.AbilityManager.globalCooldownComplete)
        {
            EnemyTriggerable[] fullscreenAbilityList = _enemy.AbilityManager.fullscreenAbilityList;
            EnemyTriggerable[] useableAbilities = new EnemyTriggerable[fullscreenAbilityList.Length];
            int i = 0;

            if (_enemy.IdleTimeElapsed > _enemy.FullscreenAttackTiming)
            {
                //abilities that can be used regardless of player distance
                foreach (EnemyTriggerable ability in fullscreenAbilityList)
                {
                    useableAbilities[i] = ability;
                    i++;
                }

            
                int j = Random.Range(0, i);

                _enemy.AbilityManager.abilityToUse = useableAbilities[j];
                return true;
            }
        }
        return false;
    }
}
