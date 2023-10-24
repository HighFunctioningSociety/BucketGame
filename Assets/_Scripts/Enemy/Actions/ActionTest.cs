using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Test")]
public class ActionTest : Action
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        DoThing(stateMachine);
    }

    private void DoThing(EnemyStateMachine stateMachine)
    {
        stateMachine.transform.position += new Vector3 (100,0);
    }
}
