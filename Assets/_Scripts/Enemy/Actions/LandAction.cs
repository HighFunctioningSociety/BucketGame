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
        if (_fallPrefab == null && _enemy.stateTimeElapsed < 1f)
        {
            _fallPrefab = Instantiate(fallPrefab, new Vector2(_enemy.targetObject.position.x, -9.097368f), fallPrefab.transform.rotation);
        }

        if(_enemy.stateTimeElapsed < 3f)
        {
            _fallPrefab.transform.position = new Vector2(Mathf.Clamp(_enemy.targetObject.position.x, _enemy.boundLeft.transform.position.x, _enemy.boundRight.transform.position.x), _enemy.boundLeft.position.y);
        }

        if(_enemy.stateTimeElapsed > 2.7f)
        {
            if (_fallPrefab.gameObject != null)
                _enemy.transform.position = new Vector3(_fallPrefab.transform.position.x, _enemy.transform.position.y, _enemy.transform.position.z);
            _enemy.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _enemy.rb.velocity = Vector2.down * 180;
            _enemy.abilityManager.nextReadyTime = Time.time + 1f;
        }
    }
}
