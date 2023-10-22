using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StartTackle")]
public class StartSwoop : Decision
{
    public override bool Decide(EnemyStateMachine stateMachine)
    {
        bool shouldTackle = TargetInLineOfSight(stateMachine);
        return shouldTackle;
    }

    private bool TargetInLineOfSight(EnemyStateMachine stateMachine)
    {
        if (stateMachine.Enemy.LOS.CheckLineOfSight() == true && stateMachine.Enemy.LOS.LOSHitCount >= 25 && stateMachine.Enemy.AbilityManager.globalCooldownComplete) 
        {
            stateMachine.Enemy.Direction = Mathf.Sign(stateMachine.transform.position.x - stateMachine.Enemy.TargetObject.position.x);
            stateMachine.Enemy.Speed = Mathf.Abs(stateMachine.Enemy.Direction);
            stateMachine.Enemy.TargetPosition = new Vector2(stateMachine.Enemy.TargetObject.position.x - (30 * stateMachine.Enemy.Direction), stateMachine.Enemy.TargetObject.position.y + 2.5f);
            stateMachine.Enemy.StartingPosition = stateMachine.transform.position;
            stateMachine.Enemy.RigidBody.velocity = Vector2.zero;

            //TODO: why am I calling this here???
            stateMachine.Enemy.Animator.SetTrigger("Dive");

            return true;
        }

        return false;
    }
}
