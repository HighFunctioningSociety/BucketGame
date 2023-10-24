using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LingeringTriggerable : MeleeAttackTriggerable
{
    [HideInInspector] public float knockBackX, knockBackY;
    [HideInInspector] public float moveForceX, moveForceY;
    [HideInInspector] public float meterProgression = 0;
    [HideInInspector] public float hitStop;

    private void Awake()
    {
        player = GetComponentInParent<PlayerContainer>();
        animator = GetComponentInParent<Animator>();
        Initialize(ability, this.gameObject);
    }

    private void Update()
    {
        if (attackCollider.enabled == true)
        {
            DrawHurtBox();
        }
    }

    public override void Initialize(Ability selectedAbility, GameObject scriptObject)
    {
        selectedAbility.Initialize(scriptObject);
    }

    public override void Trigger()
    {
        EnableCollider();
        player.rb.AddForce(new Vector2(moveForceX * player.transform.localScale.x, moveForceY), ForceMode2D.Impulse);
        animator.SetTrigger(triggerName);
        audioSource.Play();
    }

    public override void DrawHurtBox()
    {
        enNum = attackCollider.OverlapCollider(enemyLayer, hitEnemies);
        obNum = attackCollider.OverlapCollider(obstacleLayer, hitObstacles);

        if (enNum != 0 || obNum != 0)
        {
            player.rb.AddForce(new Vector2(knockBackX * -player.transform.localScale.x, 0f), ForceMode2D.Impulse);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            // Check if the enemy hurt state is present, if so put the enemy in the hurt state
            EnemyStateMachine enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            
            if (enemyStateMachine == null && enemyStateMachine.Hurt != null)
            {
                enemyStateMachine.Hurt.EnterHurtState();
            }
                
            // Check for the enemy container and affect it accordingly
            EnemyContainer enemyContainer = enemy.GetComponent<EnemyContainer>();
            
            float knockBackTotalX = knockBackX, knockBackTotalY = knockBackY;
            if (enemyContainer.GroundCheck != null)
            {
                if (!enemyContainer.GroundCheck.Grounded == false)
                {
                    knockBackTotalY += 25;
                    knockBackTotalX -= 5;
                }
            }

            enemyContainer.KnockBack(knockBackTotalX, knockBackTotalY, player.rb.transform.position);
            enemyContainer.TakeDamage(damage, false);
            
            // Trigger hit stop
            if (hitStop > 0)
            {
                SimpleCameraShake._CameraShake(0.1f, 0.05f);
                HitStop._SimpleHitStop(hitStop);
            }

            // Trigger rumble
            if (rumbleDurration > 0)
            {
                Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
            }
            
            // Increase players meter
            player.playerStats.curSpiritProgression += meterProgression;
        }
    }
}
