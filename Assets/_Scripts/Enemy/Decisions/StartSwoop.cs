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
        if (_enemy.CheckLineOfSight() == true && _enemy.LOSCount >= 25 && _enemy.abilityManager.globalCooldownComplete) 
        {
            _enemy.dir = Mathf.Sign(_enemy.transform.position.x - _enemy.targetObject.position.x);
            _enemy.speed = Mathf.Abs(_enemy.dir);
            _enemy.targetPosition = new Vector2(_enemy.targetObject.position.x - (30 * _enemy.dir), _enemy.targetObject.position.y + 2.5f);
            _enemy.startingPosition = _enemy.transform.position;
            _enemy.rb.velocity = Vector2.zero;
            _enemy.animator.SetTrigger("Dive");
            return true;
        }
        return false;
    }
}
