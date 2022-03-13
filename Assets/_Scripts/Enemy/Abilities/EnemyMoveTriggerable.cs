using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveTriggerable : EnemyTriggerable
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public EnemyContainer enemy;
    [HideInInspector] public Animator animator;
    [HideInInspector] public string triggerName;
    [HideInInspector] public float verticalForce;
    [HideInInspector] public float horizontalForce;
    [HideInInspector] public ForceMode2D forceMode;

    public void Awake()
    {
        animator = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
        enemy = GetComponentInParent<EnemyContainer>();
    }

    public void Start()
    {
        enemyAbilityClone = Object.Instantiate(scriptableAbility);
        Initialize(enemyAbilityClone, this.gameObject);
    }

    public override void Initialize(EnemyAbility selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }

    public override void Trigger()
    {
        animator.SetTrigger(triggerName);
    }

    public void ApplyForceEvent()
    {
        rb.AddForce(new Vector2(horizontalForce * -enemy.transform.localScale.x, verticalForce) * rb.mass, forceMode);
    }
}
