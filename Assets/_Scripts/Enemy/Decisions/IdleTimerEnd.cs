using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IdleTimerEnd")]
public class IdleTimerEnd : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool TimeOver = CheckTimer(enemy);
        return TimeOver;
    }
    
    private bool CheckTimer(EnemyContainer enemy)
    {
        return enemy.idleTimeElapsed >= enemy.enemyStats.idleTime;
    }
}
