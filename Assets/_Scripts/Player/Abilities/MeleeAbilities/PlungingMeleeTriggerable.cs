using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlungingMeleeTriggerable : MeleeAttackTriggerable
{
    [HideInInspector] public float meterProgression = 0;
    [HideInInspector] public float knockBackY;
    [HideInInspector] public float moveForceY;
    [HideInInspector] public float bounce;
    [HideInInspector] public float hitStop;


    private void Awake()
    {
        player = GetComponentInParent<PlayerContainer>();
        animator = GetComponentInParent<Animator>();
        Initialize(ability, this.gameObject);
    }

    private void Update()
    {
        if (attackCollider.enabled)
        {
            DrawHurtBox();
        }
    }

    public override void Initialize(Ability selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }

    public override void Trigger()
    {
        player.coolDownManager.CoolDownFlagStart();
        player.rb.velocity = Vector3.zero;
        player.rb.AddForce(Vector2.down * moveForceY, ForceMode2D.Impulse);
        animator.SetTrigger(triggerName);
        attackCollider.enabled = true;
    }

    public override void DrawHurtBox()
    {
        if (player.PlayerController.GetGrounded()){
            PlungingLandReaction();
        }

        blockNum = attackCollider.OverlapCollider(blockerLayer, hitBlockers); //If a blocker is hit, negate the rest of the attack

        if (blockNum != 0)
        {
            PlungingHitReaction();
            BlockerHitCalculation();
            return;
        }

        enNum = attackCollider.OverlapCollider(enemyLayer, hitEnemies);
        obNum = attackCollider.OverlapCollider(obstacleLayer, hitObstacles);

        if (enNum != 0)
        {
            PlungingHitReaction();
            EnemyHitCalculation();
            impactAudioScource.Play();
        }
        if (obNum != 0)
        {
            //PlungingHitReaction();
        }
    }

    private void EnemyHitCalculation()
    {
        foreach (Collider2D enemy in hitEnemies)
        {
            DrawHitEffect(enemy);

            
            HitReactor hitReactor = enemy.GetComponent<HitReactor>();
            if (hitReactor != null)
                hitReactor.ReactToHit();

            EnemyContainer enemyContainer = enemy.GetComponent<EnemyContainer>();
            if (enemyContainer == null)
            {
                enemyContainer = enemy.GetComponentInParent<EnemyContainer>();
                if (enemyContainer == null)
                {
                    return;
                }
            }

            if (hitStop > 0)
            {
                SimpleCameraShake._CameraShake(0.1f, 0.05f);
                HitStop._SimpleHitStop(hitStop);
            }

            if (rumbleDurration > 0)
            {
                Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
            }
            
            enemyContainer.KnockBack(0f, -knockBackY, player.rb.transform.position);
            enemyContainer.TakeDamage(damage, false);
            
            EnemyStateMachine enemyStateMachine = enemy.GetComponent<EnemyStateMachine>();
            
            if (enemyStateMachine.Hurt != null)
            {
                enemyStateMachine.Hurt.EnterHurtState();
            }
            
            player.playerStats.curSpiritProgression += meterProgression;
        }
    }

    private void PlungingHitReaction()
    {
        player.rb.velocity = new Vector2(player.rb.velocity.x, 0f);
        player.rb.AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
        player.playerStats.curMovementDivider = player.playerStats.movementDivider;
        DisableCollider();
        player.coolDownManager.nextReadyTime = Time.time + 0.4f;
        player.coolDownManager.waitForFlag = false;
        player.dashAbility.RefreshDash();
        animator.SetTrigger("PlungeEnd");
    }

    private void PlungingLandReaction()
    {
        DisableCollider();
        animator.SetTrigger("PlungeLand");
        player.PlayerAbilityController.freezeMovement = true;
        player.coolDownManager.waitForFlag = true;
    }

    private void BlockerHitCalculation()
    {
        foreach (Collider2D blocker in hitBlockers)
        {
            EnemyContainer _enemy = blocker.GetComponentInParent<EnemyContainer>();
            if (blocker.GetComponent<AudioSource>() != null)
                blocker.GetComponent<AudioSource>().Play();
            _enemy.ActivateBlockReaction();
            DrawHitEffect(blocker);
        }
        return;
    }
}
