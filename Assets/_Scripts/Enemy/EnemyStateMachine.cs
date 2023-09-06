using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [Header("Enemy State Objects")]
    public State CurrentState;
    public State DefaultState;
    public State AggroState;
    public State RemainState;

    [Space]
    [Header("State information")]
    public float StateTimeElapsed = 0;
    public float IdleTimeElapsed = 0;
    public int AttacksDoneInState = 0;

    [HideInInspector] public float FullscreenAttackTiming;
    [HideInInspector] public float HurtTimeRemaining;
    [HideInInspector] public float AggroTimeRemaining;


    public void TransitionToState(State nextState)
    {
        if (nextState != RemainState)
        {
            CurrentState = nextState;
            ResetStateTimers();
            //ActivateStateEnterAction();
            //CalculateFullscreenAttackTiming();
        }

        if (nextState.IsAggroState)
        {
            //AggroTimeRemaining = enemyStats.aggroTime;
        }
    }

    private void IncrementStateTime()
    {
        StateTimeElapsed += Time.fixedDeltaTime;
    }

    private void IncrementIdleTime()
    {
        if (CurrentState.IsIdleState)
        {
            IdleTimeElapsed += Time.fixedDeltaTime;
        }
    }

    private void ResetStateTimers()
    {
        if (!CurrentState.IsIdleState)
            IdleTimeElapsed = 0;
        AttacksDoneInState = 0;
        StateTimeElapsed = 0;
    }

}
