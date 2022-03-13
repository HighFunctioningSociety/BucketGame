using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/ResetBellBossTriggers")]
public class ResetBellBossTriggers : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        ResetAnimationTriggers(enemy);
    }

    private void ResetAnimationTriggers(EnemyContainer _enemy)
    {
        if (_enemy.stateTimeElapsed < 0.05f)
        {
            _enemy.animator.ResetTrigger("Blast");
            _enemy.animator.ResetTrigger("Jump");
            _enemy.animator.ResetTrigger("LowPunch");
            _enemy.animator.ResetTrigger("HighPunch");
            _enemy.animator.ResetTrigger("Hurt");
        }
    }
}
