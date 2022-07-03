using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/SetCooldown")]
public class SetCooldownAction : Actions
{
    public float cooldownValue;
    public override void Act(EnemyContainer enemy)
    {
        ApplyCooldown(enemy);
    }

    private void ApplyCooldown(EnemyContainer _enemy)
    {
        _enemy.AbilityManager.SetCooldown(cooldownValue);
    }
}
