using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CheckForPlayer")]
public class CheckForPlayer : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool playerFound = ScanForPlayer(stateMachine);
        return playerFound;
    }

    private bool ScanForPlayer(EnemyStateMachine stateMachine)
    {
        float aggroX = stateMachine.Enemy.EnemyStats.aggroRangeX;
        float aggroY = stateMachine.Enemy.EnemyStats.aggroRangeY;
        Vector2 center = stateMachine.transform.position;
        Vector2 aggroRange = new Vector2(aggroX, aggroY);

        Collider2D playerObj = Physics2D.OverlapBox(center, aggroRange, 0, Constants.Layers.Player);
        
        if (playerObj != null)
        {
            stateMachine.Enemy.TargetObject = playerObj.GetComponent<Transform>();
            Debug.LogWarning("shit sucks");
            return true;
        }
        else
        {
            stateMachine.Enemy.TargetObject = null;
            Debug.LogWarning("shit confusing");
            return false;
        }
    }
}
