using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/GCDComplete")]
public class GlobalCooldownComplete : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool cooldownComplete = CheckGlobalCooldown(enemy);
        return cooldownComplete;
    }

    private bool CheckGlobalCooldown(EnemyContainer _enemy)
    {
        return _enemy.AbilityManager.globalCooldownComplete;
    }
}
