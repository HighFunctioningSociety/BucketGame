using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Swoop")]
public class SwoopAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Swoop(enemy);
    }

    private void Swoop(EnemyContainer _enemy)
    {
        float horizontalSwoopSpeed = 50f;
        float virticalOffset = 5f;
        float yDifference = Mathf.Abs(_enemy.TargetPosition.y - _enemy.transform.position.y);
        float xDifference = Mathf.Sign(_enemy.transform.position.x - _enemy.TargetPosition.x);
        float yForceCalculation = xDifference * _enemy.Direction * - (virticalOffset + Mathf.Pow(yDifference/3f, 2));
        float xForceCalculation = _enemy.Speed * _enemy.Direction * -horizontalSwoopSpeed;

        _enemy.RigidBody.velocity = new Vector2(xForceCalculation, yForceCalculation); 
    }
}
