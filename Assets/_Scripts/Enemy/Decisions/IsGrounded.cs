using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsGrounded")]
public class IsGrounded : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool grounded = CheckForGround(enemy);
        return grounded;
    }

    private bool CheckForGround(EnemyContainer enemy)
    {
        return enemy.groundCheck.grounded;
    }
}
