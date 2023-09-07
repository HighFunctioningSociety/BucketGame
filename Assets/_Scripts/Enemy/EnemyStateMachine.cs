using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Essential Enemy Information")]
    public EnemyContainer Enemy;

    [Space]
    [Header("Enemy State Objects")]
    public State CurrentState;
    public State DefaultState;
    public State AggroState;
    public State RemainState;

    [Space]
    [Header("State information")]
    public float StateTimeElapsed = 0;
    public int AttacksDoneInState = 0;

    [Space]
    [Header("State Count Down")]
    public float StateCountDown = 0;


    public void FixedUpdate()
    {
        CurrentState.UpdateState(Enemy);
        IncrementStateTime();   
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != RemainState)
        {
            CurrentState = nextState;
            ResetStateTimers();
            ActivateStateEnterAction();
            CalculateFullscreenAttackTiming();
        }
    }

    private void IncrementStateTime()
    {
        StateTimeElapsed += Time.fixedDeltaTime;
    }


    private void ResetStateTimers()
    {
        AttacksDoneInState = 0;
        StateTimeElapsed = 0;
    }

    private void ActivateStateEnterAction()
    {
        if (CurrentState.OnStateEnterAction != null)
            CurrentState.OnStateEnterAction.Act(Enemy);
    }

    private void CalculateFullscreenAttackTiming()
    {
        //if (enemyStats.fullscreenTimingMax == 0)
        //{
        //    FullscreenAttackTiming = 0;
        //}
        //else
        //{
        //    FullscreenAttackTiming = Random.Range(enemyStats.fullscreenTimingMin, enemyStats.fullscreenTimingMax);
        //}
    }
}
