using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="PluggableAI/Actions/Land")]
public class LandAction : Actions
{
    public GameObject fallPrefab;
    private GameObject _fallPrefab;

    public override void Act(EnemyContainer enemy)
    {
        Land(enemy);
    }

    public void Land(EnemyContainer _enemy)
    {
        if (_fallPrefab == null && _enemy.StateTimeElapsed < 1f)
        {
            _fallPrefab = Instantiate(fallPrefab, new Vector2(_enemy.TargetObject.position.x, -9.097368f), fallPrefab.transform.rotation);
        }

        if(_enemy.StateTimeElapsed < 3f)
        {
            _fallPrefab.transform.position = new Vector2(Mathf.Clamp(_enemy.TargetObject.position.x, _enemy.BoundLeft.transform.position.x, _enemy.BoundRight.transform.position.x), _enemy.BoundLeft.position.y);
        }

        if(_enemy.StateTimeElapsed > 2.7f)
        {
            if (_fallPrefab.gameObject != null)
                _enemy.transform.position = new Vector3(_fallPrefab.transform.position.x, _enemy.transform.position.y, _enemy.transform.position.z);
            _enemy.RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            _enemy.RigidBody.velocity = Vector2.down * 180;
            _enemy.AbilityManager.nextReadyTime = Time.time + 1f;
        }
    }
}
