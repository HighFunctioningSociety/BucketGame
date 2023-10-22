using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAbilityManager : MonoBehaviour
{
    public EnemyProximityTriggerable[] proximityAbilityList;
    public EnemyTriggerable[] fullscreenAbilityList;
    public EnemyMoveTriggerable[] movementAbilityList;
    public EnemyTriggerable[] directCallAbilityList;
    public bool abilityAlreadyActivated;

    public EnemyTriggerable abilityToUse;
    public bool globalCooldownComplete;
    public bool repositionCooldownComplete;
    [HideInInspector] public List<EnemyTriggerable> useableAbilities = new List<EnemyTriggerable>();
    [HideInInspector] public float nextReadyTime;
    [HideInInspector] public float nextRepositionTime;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyContainer Enemy;
    [HideInInspector] public EnemyStateMachine StateMachine;

    private void Start()
    {
        Enemy = GetComponent<EnemyContainer>();
        StateMachine = GetComponent<EnemyStateMachine>();
    }

    private void Update()
    {
        globalCooldownComplete = (Time.time > nextReadyTime);
        repositionCooldownComplete = (Time.time > nextRepositionTime);
        if (globalCooldownComplete)
        {
            abilityAlreadyActivated = false;
        }  
    }

    public void _HurtboxFlagOn()
    {
        EnemyMeleeTriggerable meleeToUse;
        if (abilityToUse is EnemyMeleeTriggerable)
        {
            meleeToUse = (EnemyMeleeTriggerable)abilityToUse;
            meleeToUse.HurtBoxFlagOn();
        }
    }

    public void _HurtboxFlagOff()
    {
        EnemyMeleeTriggerable meleeToUse;
        if (abilityToUse is EnemyMeleeTriggerable)
        {
            meleeToUse = (EnemyMeleeTriggerable)abilityToUse;
            meleeToUse.HurtBoxFlagOff();
        }
    }

    public void _HurtboxEvent()
    {
        EnemyMeleeTriggerable meleeToUse;
        if (abilityToUse is EnemyMeleeTriggerable)
        {
            meleeToUse = (EnemyMeleeTriggerable)abilityToUse;
            meleeToUse.HurtBoxEvent();
        }
    }

    public void _AddForceEvent()
    {
        EnemyMeleeTriggerable meleeToUse;
        if (abilityToUse is EnemyMeleeTriggerable)
        {
            meleeToUse = (EnemyMeleeTriggerable)abilityToUse;
            meleeToUse.AddForceEvent();
        }
    }

    public void _MovementEvent()
    {
        EnemyMoveTriggerable movementToUse;
        if (abilityToUse is EnemyMoveTriggerable)
        {
            movementToUse = (EnemyMoveTriggerable)abilityToUse;
            movementToUse.ApplyForceEvent();
        }
    }

    public void SpawnProjectileEvent()
    {
        EnemyProjectileTriggerable projectileToUse;
        if (abilityToUse is EnemyProjectileTriggerable)
        {
            projectileToUse = (EnemyProjectileTriggerable)abilityToUse;
            projectileToUse.SpawnProjectileEvent();
        }
    }

    public void UseAbility(EnemyTriggerable ability)
    {
        if (globalCooldownComplete)
        {
            StateMachine.AttacksDoneInState++;
            Enemy.Direction = 0;
            Enemy.Speed = 0;
            ApplyCooldown(ability);
            Debug.Log(ability.scriptableAbility.aName);
            ability.enemyAbilityClone.TriggerAbility();
            useableAbilities.Clear();
        }
    }

    private void ApplyCooldown(EnemyTriggerable ability)
    {
        ability.nextReadyTime = ability.enemyAbilityClone.abilityCooldownTime + Time.time;
        nextReadyTime = ability.enemyAbilityClone.globalCooldown + Time.time;
    }
    
    public void SetCooldown(float cooldownValue)
    {
        nextReadyTime = Time.time + cooldownValue;
    }

    public void ZeroOutDirection()
    {
        Enemy.Direction = 0;
    }

    public void ZeroOutSpeed()
    {
        Enemy.Speed = 0;
    }

    private void OnDrawGizmos()
    {
        foreach (EnemyProximityTriggerable proximityAbility in proximityAbilityList)
        {
            Gizmos.color = Color.red;
            EnemyProximityAbility enemyAbility = (EnemyProximityAbility)proximityAbility.scriptableAbility;
            Gizmos.DrawWireCube(proximityAbility.transform.position, new Vector2(enemyAbility.rangeX, enemyAbility.rangeY));
        }
    }
}

