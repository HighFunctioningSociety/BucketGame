using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (menuName = "PluggableAI/Decisions/InIdleAnimation")]
public class InIdleAnimation : Decision
{
    public override bool Decide(EnemyContainer enemy)
    {
        bool isIdlePlaying = IsIdlePlaying(enemy);
        return isIdlePlaying;
    }
    private bool IsIdlePlaying(EnemyContainer _enemy)
    {
        return _enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }
}
