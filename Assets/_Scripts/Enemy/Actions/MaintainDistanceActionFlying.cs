using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/MaintainDistanceFlying")]
public class MaintainDistanceActionFlying : Actions
{
    public float distanceX;
    public float distanceY;
    public override void Act(EnemyContainer enemy)
    {
        MaintainDistance(enemy);
    }
    
    private void MaintainDistance(EnemyContainer _enemy)
    {
        if (_enemy.TargetObject == null)
            return;

        _enemy.Direction = Mathf.Sign(_enemy.transform.position.x - _enemy.TargetObject.position.x);
        _enemy.Speed = Mathf.Abs(_enemy.Direction);
        Vector2 newTarget = new Vector2(_enemy.TargetObject.position.x + (distanceX * _enemy.Direction), _enemy.TargetObject.position.y + 5 + (distanceY));

        if (Mathf.Abs(_enemy.transform.position.x - newTarget.x) < 1.5f && Mathf.Abs(_enemy.transform.position.y - newTarget.y) < 1.5f)
            return;

        Vector2 direction = new Vector2((newTarget.x - _enemy.transform.position.x), (newTarget.y - _enemy.transform.position.y));
        float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
        Vector2 directionUnit = direction / directionMagnitude;
        _enemy.RigidBody.AddForce(directionUnit * _enemy.enemyStats.speed, ForceMode2D.Impulse);
    }
}
