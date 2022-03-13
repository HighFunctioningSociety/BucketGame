using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CheckForPlayer")]
public class CheckForPlayer : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool playerFound = ScanForPlayer(enemy);
        return playerFound;
    }

    private bool ScanForPlayer(EnemyContainer enemy)
    {
        float aggroX = enemy.enemyStats.aggroRangeX;
        float aggroY = enemy.enemyStats.aggroRangeY;
        Vector2 center = enemy.transform.position;
        Vector2 aggroRange = new Vector2(aggroX, aggroY);

        Collider2D playerObj = Physics2D.OverlapBox(center, aggroRange, 0, enemy.playerLayer);

        if (playerObj != null)
        {
            enemy.targetObject = playerObj.GetComponent<Transform>();
            return true;
        }
        else
        {
            enemy.targetObject = null;
            return false;
        }
    }
}
