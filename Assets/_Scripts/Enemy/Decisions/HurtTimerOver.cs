using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HurtTimeOver")]
public class HurtTimerOver : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool hurtOver = CheckHurtTime(enemy);
        return hurtOver;
    }

    private bool CheckHurtTime(EnemyContainer _enemy)
    {
        return (_enemy.HurtTimeRemaining <= 0);
    }
}
