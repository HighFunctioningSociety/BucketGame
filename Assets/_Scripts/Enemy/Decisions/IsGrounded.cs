using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/IsGrounded")]
public class IsGrounded : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool grounded = CheckForGround(stateMachine);
        return grounded;
    }

    private bool CheckForGround(EnemyStateMachine stateMachine)
    {
        return stateMachine.Enemy.GroundCheck.Grounded;
    }
}
