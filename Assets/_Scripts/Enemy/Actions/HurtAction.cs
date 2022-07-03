using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Hurt")]
public class HurtAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Hurt(enemy);
    }

    private void Hurt(EnemyContainer _enemy)
    {
        _enemy.Direction = 0;
        _enemy.Speed = 0;
    }
}
