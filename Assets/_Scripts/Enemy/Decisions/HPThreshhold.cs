using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HealthThreshhold")]
public class HPThreshhold : Decision
{
    public float healthThreshhold;
    public int triggerIndex;

    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool TimeOver = CheckHealth(stateMachine);
        return TimeOver;
    }

    private bool CheckHealth(EnemyStateMachine stateMachine)
    {
        bool triggerUsed = stateMachine.Enemy.Triggers.GetTriggerValue(triggerIndex);
        bool hpThreshholdReached = ((float)stateMachine.Enemy.CurrentHealth / (float)stateMachine.Enemy.EnemyStats.maxHealth <= healthThreshhold);

        if (hpThreshholdReached && !triggerUsed) 
            stateMachine.Enemy.Triggers.FlipTrigger(triggerIndex);

        return hpThreshholdReached && !triggerUsed;
    }
}
