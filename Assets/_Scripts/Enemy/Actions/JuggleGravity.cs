using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/JuggleGravity")]
public class JuggleGravity : Action
{
    public override void Act(EnemyStateMachine enemy)
    {
        ChangeGravity(enemy);
    }

    private void ChangeGravity(EnemyStateMachine stateMachine)
    {   if (stateMachine.Enemy.RigidBody.velocity.y < 0)
        {
            float newGravity = Mathf.Clamp(stateMachine.Enemy.EnemyStats.defaultGravity - 7 + stateMachine.StateTimeElapsed, 1, stateMachine.Enemy.EnemyStats.defaultGravity);
            stateMachine.Enemy.RigidBody.gravityScale = newGravity;
        }
        else
        {
            stateMachine.Enemy.RigidBody.gravityScale = stateMachine.Enemy.EnemyStats.defaultGravity;
        }
    }
}
