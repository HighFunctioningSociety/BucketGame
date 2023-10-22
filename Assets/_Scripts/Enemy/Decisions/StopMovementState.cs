using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopMovement")]
public class StopMovementState : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool stopMovement = MovementTimeOver(stateMachine);
        return stopMovement;
    }

    private bool MovementTimeOver(EnemyStateMachine stateMachine)
    {
        //if (_enemy.stateTimeElapsed > _enemy.movementTiming)
            //return true;
        //else
            return false;
    }
}
