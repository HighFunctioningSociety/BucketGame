﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/FlyingChase")]
public class FlyingChaseAction : Actions
{
    public override void Act(EnemyContainer enemy)
    {
        Chase(enemy);
    }

    private void Chase(EnemyContainer _enemy)
    {
        if (_enemy.targetObject == null)
            return;
        _enemy.dir = Mathf.Sign(_enemy.transform.position.x - _enemy.targetObject.position.x);
        _enemy.speed = Mathf.Abs(_enemy.dir);
        Vector2 newTarget = new Vector2(_enemy.targetObject.position.x, _enemy.targetObject.position.y + 5);
        Vector2 direction = new Vector2((newTarget.x - _enemy.transform.position.x), (newTarget.y - _enemy.transform.position.y));
        float directionMagnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
        Vector2 directionUnit = direction / directionMagnitude;
        _enemy.rb.AddForce(directionUnit * _enemy.enemyStats.speed, ForceMode2D.Impulse);
    }
}