﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [HideInInspector] public EnemyContainer enemy;

    private void Start()
    {
        enemy = GetComponent<EnemyContainer>();
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

    public void _SpawnProjectileEvent()
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
            enemy.attacksDoneInState++;
            enemy.dir = 0;
            enemy.speed = 0;
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

    private void OnDrawGizmos()
    {
        foreach (EnemyProximityTriggerable proximityAbility in proximityAbilityList)
        {
            Gizmos.color = Color.red;
            EnemyProximityAbility enemyAbility = (EnemyProximityAbility)proximityAbility.scriptableAbility;
            Gizmos.DrawWireCube(proximityAbility.transform.position, new Vector2(enemyAbility.rangeX, enemyAbility.rangeY));
        }
    }

    public void _ZeroOutDirection()
    {
        enemy.dir = 0;
    }

    public void _ZeroOutSpeed()
    {
        enemy.speed = 0;
    }
}
