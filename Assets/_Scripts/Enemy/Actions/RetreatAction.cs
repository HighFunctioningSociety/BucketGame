using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Retreat")]
public class RetreatAction : Actions
{
    public string AnimationName;
    public override void Act(EnemyContainer enemy)
    {
        Retreat(enemy);
    }

    private void Retreat(EnemyContainer _enemy)
    {
        if (!_enemy.Triggers.retreated )
        {
            Debug.Log("Retreat");
            _enemy.AbilityManager.globalCooldownComplete = false;
            _enemy.AbilityManager.nextReadyTime = Time.time + 1.5f;
            _enemy.Triggers.retreated = true;
            _enemy.Animator.Play(AnimationName);
        }
    }
}
