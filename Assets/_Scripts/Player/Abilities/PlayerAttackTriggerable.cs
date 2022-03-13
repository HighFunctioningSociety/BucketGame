using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackTriggerable : MonoBehaviour
{
    [HideInInspector] public PlayerContainer player;
    [HideInInspector] public Animator animator;
    [HideInInspector] public string triggerName;
    [HideInInspector] public int damage;

    public Ability ability;
    public AudioSource audioSource;

    public abstract void Initialize(Ability selectedAbility, GameObject abilityObject);

    public abstract void Trigger();
}
