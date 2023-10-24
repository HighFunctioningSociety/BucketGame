using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Swoop")]
public class SwoopAction : Action
{
    
    [Tooltip("Position we want to hit")]
    public Vector3 targetPos;
	
    [Tooltip("Horizontal speed, in units/sec")]
    public float speed = 50;
	
    [Tooltip("How high the arc should be, in units")]
    public float arcHeight = 20;
    
    
    public override void Act(EnemyStateMachine stateMachine)
    {
        Swoop(stateMachine);
    }

    private void Swoop(EnemyStateMachine stateMachine)
    {
        float startingPositionX = stateMachine.transform.position.x;
        float targetPositionX = targetPos.x + (5 * Mathf.Sign(targetPos.x - stateMachine.transform.position.x ));
        float dist = targetPositionX - startingPositionX;
        float nextX = Mathf.MoveTowards(stateMachine.transform.position.x, targetPositionX, stateMachine.Enemy.Direction * -speed * Time.deltaTime);
        float baseY = Mathf.Lerp(stateMachine.Enemy.StoredPositions.y, targetPos.y, (nextX - startingPositionX) / dist);
        float arc = arcHeight * (nextX - startingPositionX) * (nextX - targetPositionX) / (-0.25f * dist * dist);

        stateMachine.Enemy.TargetPosition = new Vector3(targetPositionX, stateMachine.transform.position.y);
        
        Vector3 nextPos = new Vector3(nextX, baseY - arc, stateMachine.transform.position.z);

        stateMachine.transform.position = nextPos;
    }
}
