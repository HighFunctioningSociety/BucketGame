using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopLanding")]
public class StopLanding : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool grounded = CheckGroundAndIdle(enemy);
        return grounded;
    }

    private bool CheckGroundAndIdle(EnemyContainer _enemy)
    {
        return _enemy.groundCheck.Grounded && _enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}
