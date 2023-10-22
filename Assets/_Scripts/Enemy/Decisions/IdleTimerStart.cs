using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IdleTimerStart")]
public class IdleTimerStart : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool shouldIdle = ReadyToIdle(stateMachine);
        return shouldIdle;
    }

    private bool ReadyToIdle(EnemyStateMachine stateMachine)
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

    private void ChangePatrolPoint(EnemyStateMachine stateMachine)
    {
        //enemy.currentPatrol = (enemy.currentPatrol + 1) % (enemy.patrolPoints.Length);
    }
}

