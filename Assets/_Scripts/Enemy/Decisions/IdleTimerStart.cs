using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IdleTimerStart")]
public class IdleTimerStart : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool shouldIdle = ReadyToIdle(enemy);
        return shouldIdle;
    }

    private bool ReadyToIdle(EnemyContainer enemy)
    {
        //Vector2 patrolTarget = new Vector2(enemy.patrolPoints[enemy.currentPatrol].position.x, enemy.transform.position.y);

        //if (Vector2.Distance(enemy.transform.position, patrolTarget) < 1f)
        //{
        //    ChangePatrolPoint(enemy);
        //    return true;
        //}else
        //{
        //    return false;
        //}
        return false;
    }

    private void ChangePatrolPoint(EnemyContainer enemy)
    {
        //enemy.currentPatrol = (enemy.currentPatrol + 1) % (enemy.patrolPoints.Length);
    }
}

