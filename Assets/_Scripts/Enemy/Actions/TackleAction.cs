using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Tackle")]
public class TackleAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Tackle(enemy);
    }

    public void Tackle(EnemyContainer _enemy)
    {
        _enemy.rb.velocity = new Vector2(70 * -1 * _enemy.dir, 0);
    }
}
