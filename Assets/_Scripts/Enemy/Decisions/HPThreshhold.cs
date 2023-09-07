using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HealthThreshhold")]
public class HPThreshhold : Decision
{
    public float healthThreshhold;
    public int triggerIndex;

    public override bool Decide(EnemyContainer enemy)
    {
        bool TimeOver = CheckHealth(enemy);
        return TimeOver;
    }

    private bool CheckHealth(EnemyContainer _enemy)
    {
        bool triggerUsed = _enemy.Triggers.GetTriggerValue(triggerIndex);
        bool hpThreshholdReached = ((float)_enemy.CurrentHealth / (float)_enemy.enemyStats.maxHealth <= healthThreshhold);

        if (hpThreshholdReached && !triggerUsed) 
            _enemy.Triggers.FlipTrigger(triggerIndex);

        return hpThreshholdReached && !triggerUsed;
    }
}
