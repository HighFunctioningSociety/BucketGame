using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Patrol(enemy);
    }

    private void Patrol(EnemyContainer enemy)
    {
        //if (enemy.patrolPoints.Length == 0)
        //return;

        //Vector2 patrolTarget = new Vector2(enemy.patrolPoints[enemy.currentPatrol].position.x, enemy.transform.position.y);

        //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, patrolTarget, enemy.enemyStats.speed * Time.fixedDeltaTime);
        //enemy.dir = Mathf.Sign(enemy.transform.position.x - enemy.patrolPoints[enemy.currentPatrol].position.x);

        //if (enemy.groundCheck == null)
        //{
        //    return;
        //}
        //if (enemy.groundCheck.edgeLeft)
        //{
        //    enemy.patrolDirection = 1;
        //}
        //if (enemy.groundCheck.edgeRight)
        //{
        //    enemy.patrolDirection = -1;
        //}

        //float direction = enemy.patrolDirection;

        //enemy.transform.
    }
}
