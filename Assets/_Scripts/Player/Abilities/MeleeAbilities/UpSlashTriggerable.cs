using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpSlashTriggerable : MeleeAttackTriggerable
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

    public override void Initialize(Ability selectedAbility, GameObject scriptObject)
    {
        selectedAbility.Initialize(scriptObject);
    }

    public override void Trigger()
    {
        animator.SetTrigger(triggerName);
        //audioSource.Play();
        if (!player.controller.grounded)
        {
            player.rb.AddForce(Vector2.up * moveForceY, ForceMode2D.Impulse);
        }
    }

    public override void DrawHurtBox()
    {
        EnableCollider();
        enNum = attackCollider.OverlapCollider(enemyLayer, hitEnemies);
        obNum = attackCollider.OverlapCollider(obstacleLayer, hitObstacles);
        DisableCollider();
        if (enNum != 0)
            impactAudioScource.Play();
        
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyContainer _enemy = enemy.GetComponent<EnemyContainer>();
            if (_enemy == null)
            {
                _enemy = enemy.GetComponentInParent<EnemyContainer>();
                if (_enemy == null)
                {
                    return;
                }
            }

            ApplyHitstop(hitStop, enNum);

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

            if (_enemy.HurtController != null)
            {
                _enemy.HurtController.EnterHurtState();
            }

            _enemy.KnockBack(knockBackTotalX, knockBackTotalY, player.rb.transform.position);
            _enemy.TakeDamage(damage, false);
            DrawHitEffect(enemy);
            player.playerStats.curSpiritProgression += meterProgression;
        }
    }

    private void ApplyHitstop(float _hitstop, int _enemiesHit)
    {
        if (hitStop > 0)
        {
            SimpleCameraShake._CameraShake(0.1f, 0.05f);
            HitStop._SimpleHitStop(_hitstop * _enemiesHit);
        }
    }
}
