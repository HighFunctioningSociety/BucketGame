﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Move")]
public class MoveAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Move(enemy);
    }

    private void Move(EnemyContainer _enemy)
    {
        _enemy.Direction = _enemy.PatrolDirection * -1;
        _enemy.Speed = Mathf.Abs(_enemy.Direction);
        _enemy.transform.position = Vector2.MoveTowards(_enemy.transform.position, new Vector2 (_enemy.transform.position.x + (1000 * _enemy.PatrolDirection), _enemy.transform.position.y), _enemy.enemyStats.speed * Time.fixedDeltaTime);
    }
}
