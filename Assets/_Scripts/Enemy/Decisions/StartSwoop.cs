using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StartTackle")]
public class StartSwoop : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool shouldTackle = TargetInLineOfSight(enemy);
        return shouldTackle;
    }

    private bool TargetInLineOfSight(EnemyContainer _enemy)
    {
        if (_enemy.LOS.CheckLineOfSight() == true && _enemy.LOS.LOSCount >= 25 && _enemy.AbilityManager.globalCooldownComplete) 
        {
            _enemy.Direction = Mathf.Sign(_enemy.transform.position.x - _enemy.TargetObject.position.x);
            _enemy.Speed = Mathf.Abs(_enemy.Direction);
            _enemy.TargetPosition = new Vector2(_enemy.TargetObject.position.x - (30 * _enemy.Direction), _enemy.TargetObject.position.y + 2.5f);
            _enemy.StartingPosition = _enemy.transform.position;
            _enemy.RigidBody.velocity = Vector2.zero;
            _enemy.Animator.SetTrigger("Dive");
            return true;
        }
        return false;
    }
}
