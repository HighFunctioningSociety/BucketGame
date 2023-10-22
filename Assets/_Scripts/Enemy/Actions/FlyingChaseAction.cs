using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FlyingChase")]
public class FlyingChaseAction : Actions
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        Chase(stateMachine);
    }

    private void Chase(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.TargetObject == null)
            return;
        stateMachine.Enemy.Direction = Mathf.Sign(stateMachine.transform.position.x - stateMachine.Enemy.TargetObject.position.x);
        stateMachine.Enemy.Speed = Mathf.Abs(stateMachine.Enemy.Direction);
        Vector2 newTarget = new Vector2(stateMachine.Enemy.TargetObject.position.x, stateMachine.Enemy.TargetObject.position.y + 5);
        Vector2 direction = new Vector2((newTarget.x - stateMachine.transform.position.x), (newTarget.y - stateMachine.transform.position.y));
        float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
        Vector2 directionUnit = direction / directionMagnitude;
        stateMachine.Enemy.RigidBody.AddForce(directionUnit * stateMachine.Enemy.EnemyStats.speed, ForceMode2D.Impulse);
    }
}
