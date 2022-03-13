using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileTriggerable : EnemyTriggerable
{
    [HideInInspector] public int damage;
    [HideInInspector] public float speed;
    [HideInInspector] public float knockBack;
    [HideInInspector] public string triggerName;
    [HideInInspector] public Animator animator;
    [HideInInspector] public EnemyContainer enemy;
    [HideInInspector] public GameObject projectilePrefab;
    [HideInInspector] public bool isTracking = false;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
        enemy = GetComponentInParent<EnemyContainer>();
        enemyAbilityClone = Object.Instantiate(scriptableAbility);
        Initialize(enemyAbilityClone, this.gameObject);
    }

    public void Update()
    {
        CheckCooldown();
    }

    public override void Initialize(EnemyAbility selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }

    public override void Trigger()
    {
        enemy.dir = 0;
        enemy.speed = 0;
        animator.SetTrigger(triggerName);
    }

    public void SpawnProjectileEvent()
    {
        float projectileDir = -enemy.transform.localScale.x;
        GameObject projectile = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = projectile.GetComponentInChildren<Rigidbody2D>();

        rb.velocity = new Vector2(projectileDir * speed, 0);
        if (isTracking)
        {
            TrackingProjectile tracker = projectile.GetComponentInChildren<TrackingProjectile>();
            tracker.target = enemy.targetObject;
            tracker.speed = speed;
            tracker.projectileDir = projectileDir;
        }
    }
}
