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
        enemy.Direction = Mathf.Sign(enemy.transform.position.x - enemy.TargetObject.position.x);
        enemy.Speed = Mathf.Abs(enemy.Direction);
        enemy.StartingPosition = enemy.transform.position;
        enemy.targetPosition = new Vector2(enemy.TargetObject.position.x, enemy.TargetObject.position.y);
        enemy.RigidBody.velocity = Vector2.zero;
        animator.SetTrigger(AnimationName);
    }
}
