using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/StorePosition")]
public class StorePositionAction : Action
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        StorePosition(stateMachine);
    }

    private void StorePosition(EnemyStateMachine stateMachine)
    {
        //TODO: Establish a good method for storing and retrieving positions. such as having established channels for certain tasks or something. maybe use a dictionary
        stateMachine.Enemy.StoredPositions = stateMachine.transform.position;
    }
}
