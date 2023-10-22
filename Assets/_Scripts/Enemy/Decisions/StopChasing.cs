using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/StopChasing")]
public class StopChasing : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool stillChasing = AggroTimer(stateMachine);
        return stillChasing;
    }

    private bool AggroTimer(EnemyStateMachine stateMachine) 
    {
        float aggroX = stateMachine.Enemy.EnemyStats.aggroRangeX;
        float aggroY = stateMachine.Enemy.EnemyStats.aggroRangeY;
        Vector2 center = stateMachine.transform.position;
        Vector2 aggroRange = new Vector2(aggroX, aggroY);

        Collider2D playerObj = Physics2D.OverlapBox(center, aggroRange, 0, Constants.Layers.Player);

        if (stateMachine.Enemy.AggroTimeRemaining <= 0)
        {
            return true;
        }

        if (playerObj == null)
        {
            stateMachine.Enemy.AggroTimeRemaining -= Time.deltaTime;
        }
        else
        {
            stateMachine.Enemy.AggroTimeRemaining = stateMachine.CurrentState.TimeToRemainInState;
        }

        return false;
        
    }
}
