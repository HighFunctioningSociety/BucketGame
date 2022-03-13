using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbility : ScriptableObject
{
    public string aName = "New Ability";
    public AudioClip aSound;
    public float globalCooldown = 1f;
    public float abilityCooldownTime = 0f;

    public abstract void Initialize(GameObject obj);
    public abstract void TriggerAbility();
}
