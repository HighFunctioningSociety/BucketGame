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
        return _enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && _enemy.inIdle == false && _enemy.stateTimeElapsed > 0.1f;
    }
}

