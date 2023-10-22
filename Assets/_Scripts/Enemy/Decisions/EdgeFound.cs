using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/EdgeFound")]
public class EdgeFound : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool edgeFound = CheckForEdge(stateMachine);
        return edgeFound;
    }

    private bool CheckForEdge(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.GroundCheck.EdgeLeft  && stateMachine.Enemy.PatrolDirection == -1)
        {
            stateMachine.Enemy.GroundCheck.LeftEdgeAlreadyFound = true;
            stateMachine.Enemy.PatrolDirection *= -1;
            return true;
        }
        else if (stateMachine.Enemy.GroundCheck.EdgeRight && stateMachine.Enemy.PatrolDirection == 1)
        {
            stateMachine.Enemy.GroundCheck.RightEdgeAlreadyFound = true;
            stateMachine.Enemy.PatrolDirection *= -1;
            return true;
        }
        return false;
    }
}
