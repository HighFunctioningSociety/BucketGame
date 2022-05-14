using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopAttacking")]
public class StopAttacking : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool stopAttacking = EnteredIdleAnimation(enemy);
        return stopAttacking;
    }

    private bool EnteredIdleAnimation(EnemyContainer _enemy)
    {
        return _enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _enemy.InIdle == false && _enemy.StateTimeElapsed > 0.1f;
    }
}

