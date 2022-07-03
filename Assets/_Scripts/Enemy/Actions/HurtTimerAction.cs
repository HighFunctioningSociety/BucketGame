using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/HurtTimer")]
public class HurtTimerAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        HurtTimeDecrement(enemy);
    }

    private void HurtTimeDecrement(EnemyContainer _enemy)
    {
        _enemy.HurtTimeRemaining -= Time.fixedDeltaTime;
    }
}
