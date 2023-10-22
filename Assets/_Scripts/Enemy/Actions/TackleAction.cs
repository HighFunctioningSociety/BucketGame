using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Tackle")]
public class TackleAction : Actions
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        Tackle(stateMachine);
    }

    public void Tackle(EnemyStateMachine stateMachine)
    {
        stateMachine.Enemy.RigidBody.velocity = new Vector2(70 * -1 * stateMachine.Enemy.Direction, 0);
    }
}
