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
            EnemyContainer _enemy = enemy.GetComponent<EnemyContainer>();
            if (_enemy == null) return;

            if (hitStop > 0)
            {
                SimpleCameraShake._CameraShake(0.1f, 0.05f);
                HitStop._SimpleHitStop(hitStop);
            }

            if (rumbleDurration > 0)
            {
                Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
            }

            float knockBackTotalX = knockBackX, knockBackTotalY = knockBackY;
            if (_enemy.groundCheck != null)
            {
                if (!_enemy.groundCheck.Grounded == false)
                {
                    knockBackTotalY += 25;
                    knockBackTotalX -= 5;
                }
            }

            if (_enemy.Hurt != null)
            {
                _enemy.Hurt.EnterHurtState();
            }

            _enemy.KnockBack(knockBackTotalX, knockBackTotalY, player.rb.transform.position);
            _enemy.TakeDamage(damage, false);
            player.playerStats.curSpiritProgression += meterProgression;
        }
    }
}
