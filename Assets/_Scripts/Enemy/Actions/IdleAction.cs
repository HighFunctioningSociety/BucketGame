using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Idle(enemy);
    }

    private void Idle(EnemyContainer _enemy)
    {
        _enemy.Direction = 0;
        _enemy.Speed = 0;
    }
}
