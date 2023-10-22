using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/MaintainDistanceFlying")]
public class MaintainDistanceActionFlying : Actions
{
    public float distanceX;
    public float distanceY;
    public override void Act(EnemyStateMachine stateMachine)
    {
        MaintainDistance(stateMachine);
    }
    
    private void MaintainDistance(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.TargetObject == null)
            return;

        stateMachine.Enemy.Direction = Mathf.Sign(stateMachine.transform.position.x - stateMachine.Enemy.TargetObject.position.x);
        stateMachine.Enemy.Speed = Mathf.Abs(stateMachine.Enemy.Direction);
        Vector2 newTarget = new Vector2(stateMachine.Enemy.TargetObject.position.x + (distanceX * stateMachine.Enemy.Direction), stateMachine.Enemy.TargetObject.position.y + 5 + (distanceY));

        if (Mathf.Abs(stateMachine.transform.position.x - newTarget.x) < 1.5f && Mathf.Abs(stateMachine.transform.position.y - newTarget.y) < 1.5f)
            return;

        Vector2 direction = new Vector2((newTarget.x - stateMachine.transform.position.x), (newTarget.y - stateMachine.transform.position.y));
        float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
        Vector2 directionUnit = direction / directionMagnitude;
        stateMachine.Enemy.RigidBody.AddForce(directionUnit * stateMachine.Enemy.EnemyStats.speed, ForceMode2D.Impulse);
    }
}
