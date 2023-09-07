using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/StopChasing")]
public class StopChasing : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool stillChasing = AggroTimer(enemy);
        return stillChasing;
    }

    private bool AggroTimer(EnemyContainer enemy) 
    {
        float aggroX = enemy.enemyStats.aggroRangeX;
        float aggroY = enemy.enemyStats.aggroRangeY;
        Vector2 center = enemy.transform.position;
        Vector2 aggroRange = new Vector2(aggroX, aggroY);

        Collider2D playerObj = Physics2D.OverlapBox(center, aggroRange, 0, enemy.PlayerLayer);

        if (enemy.AggroTimeRemaining <= 0)
        {
            return true;
        }

        if (playerObj == null)
        {
            enemy.AggroTimeRemaining -= Time.deltaTime;
        }
        else
        {
            enemy.AggroTimeRemaining = enemy.currentState.TimeToRemainInState;
        }

        return false;
        
    }
}
