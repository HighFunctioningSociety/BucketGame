using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Move")]
public class MoveAction : Action
{
    public override void Act(EnemyStateMachine stateMachine)
    {
        Move(stateMachine);
    }

    private void Move(EnemyStateMachine stateMachine)
    {
        stateMachine.Enemy.Direction = stateMachine.Enemy.PatrolDirection * -1;
        stateMachine.Enemy.Speed = Mathf.Abs(stateMachine.Enemy.Direction);
        stateMachine.transform.position = Vector2.MoveTowards(stateMachine.transform.position, new Vector2 (stateMachine.transform.position.x + (1000 * stateMachine.Enemy.PatrolDirection), stateMachine.transform.position.y), stateMachine.Enemy.EnemyStats.speed * Time.fixedDeltaTime);
    }
}
