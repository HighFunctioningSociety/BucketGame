using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/GCDIncomplete")]
public class GDCIncomplete : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool stillOnCooldown = CheckGCD(enemy);
        return stillOnCooldown;
    }

    private bool CheckGCD(EnemyContainer _enemy)
    {
        return !_enemy.AbilityManager.globalCooldownComplete;
    }
}
