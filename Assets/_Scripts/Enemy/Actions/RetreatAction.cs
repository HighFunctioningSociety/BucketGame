using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Retreat")]
public class RetreatAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Retreat(enemy);
    }

    private void Retreat(EnemyContainer _enemy)
    {
        if (!_enemy.triggers.retreated )
        {
            Debug.Log("Retreat");
            _enemy.abilityManager.globalCooldownComplete = false;
            _enemy.abilityManager.nextReadyTime = Time.time + 1.5f;
            _enemy.triggers.retreated = true;
            _enemy.animator.SetTrigger("Retreat");
        }
    }
}
