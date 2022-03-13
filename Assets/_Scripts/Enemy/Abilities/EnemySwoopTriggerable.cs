using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwoopTriggerable : EnemyProximityTriggerable
{
    [HideInInspector] public EnemyContainer enemy;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        enemy = GetComponentInParent<EnemyContainer>();
    }

    private void Start()
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
        enemy.dir = Mathf.Sign(enemy.transform.position.x - enemy.targetObject.position.x);
        enemy.speed = Mathf.Abs(enemy.dir);
        enemy.startingPosition = enemy.transform.position;
        enemy.targetPosition = new Vector2(enemy.targetObject.position.x, enemy.targetObject.position.y);
        enemy.rb.velocity = Vector2.zero;
        animator.SetTrigger(triggerName);
    }
}
