using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopMovement")]
public class StopMovementState : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool stopMovement = MovementTimeOver(enemy);
        return stopMovement;
    }

    private bool MovementTimeOver(EnemyContainer _enemy)
    {
        //if (_enemy.stateTimeElapsed > _enemy.movementTiming)
            //return true;
        //else
            return false;
    }
}
