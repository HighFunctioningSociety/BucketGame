using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="PluggableAI/Actions/Land")]
public class LandAction : Actions
{
    public GameObject fallPrefab;
    private GameObject _fallPrefab;

    public override void Act(EnemyStateMachine stateMachine)
    {
        Land(stateMachine);
    }

    public void Land(EnemyStateMachine stateMachine)
    {
        if (_fallPrefab == null && stateMachine.StateTimeElapsed < 1f)
        {
            _fallPrefab = Instantiate(fallPrefab, new Vector2(stateMachine.Enemy.TargetObject.position.x, -9.097368f), fallPrefab.transform.rotation);
        }

        if(stateMachine.StateTimeElapsed < 3f)
        {
            _fallPrefab.transform.position = new Vector2(Mathf.Clamp(stateMachine.Enemy.TargetObject.position.x, stateMachine.Enemy.BoundLeft.transform.position.x, stateMachine.Enemy.BoundRight.transform.position.x), stateMachine.Enemy.BoundLeft.position.y);
        }

        if(stateMachine.StateTimeElapsed > 2.7f)
        {
            if (_fallPrefab.gameObject != null)
                stateMachine.transform.position = new Vector3(_fallPrefab.transform.position.x, stateMachine.transform.position.y, stateMachine.transform.position.z);
            stateMachine.Enemy.RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            stateMachine.Enemy.RigidBody.velocity = Vector2.down * 180;
            stateMachine.Enemy.AbilityManager.nextReadyTime = Time.time + 1f;
        }
    }
}
