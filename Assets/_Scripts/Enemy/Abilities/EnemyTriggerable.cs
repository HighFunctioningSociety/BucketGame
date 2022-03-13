using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyTriggerable : MonoBehaviour
{
    public AudioSource audioSource;
    public EnemyAbility scriptableAbility;
    public bool abilityCooldown;
    public float nextReadyTime;
    [HideInInspector] public EnemyAbility enemyAbilityClone;

    public abstract void Trigger();
    public abstract void Initialize(EnemyAbility selectedAbility, GameObject abilityObject);

    private void LateUpdate()
    {
        CheckCooldown();
    }

    protected void CheckCooldown()
    {
        abilityCooldown = (Time.time > nextReadyTime);
    }
}
