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

    [Space]
    [Header("State information")]
    public float StateTimeElapsed = 0;
    public int AttacksDoneInState = 0;

    [Space]
    [Header("State Count Down")]
    public float StateCountDown = 0;

    [Space]
    [Header("Gizmos")]
    public float StateSphereRadius = 5;

    [Space]
    [Header("Optional States")]
    public HurtController Hurt;


    private void Start()
    {
        Hurt = GetComponent<HurtController>();
    }

    public void FixedUpdate()
    {
        CurrentState.UpdateState(this);
        IncrementStateTime();   
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != CurrentState)
        {
            CurrentState.OnStateExit(this);
            CurrentState = nextState;
            ResetStateTimers();
            CalculateFullscreenAttackTiming();
            CurrentState.OnStateEnter(this);
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


    private void OnDrawGizmos()
    {
        if (CurrentState != null)
        {
            //State Color
            Gizmos.color = CurrentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(GetComponent<Collider2D>().bounds.center, StateSphereRadius);
        }

        //Aggro Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, new Vector3(Enemy.EnemyStats.aggroRangeX, Enemy.EnemyStats.aggroRangeY, 0));
    }
}
