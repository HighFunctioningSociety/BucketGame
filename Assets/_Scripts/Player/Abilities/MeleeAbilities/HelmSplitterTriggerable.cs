using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmSplitterTriggerable : MeleeAttackTriggerable
{
    public HelmSplitterSubHitAbility subHit;
    public Collider2D subHitCollider;
    public TrailRenderer trailRenderer;
    public ParticleSystem particles;
    [HideInInspector] public float meterProgression = 0;
    [HideInInspector] public float knockBackY, knockBackX;
    [HideInInspector] public float moveForceY;
    [HideInInspector] public float hitStop;
    [HideInInspector] public int subHitDamage;
    [HideInInspector] public float subHitHitStop;
    public bool helmSplitterActive;


    private void Awake()
    {
        player = GetComponentInParent<PlayerContainer>();
        animator = GetComponentInParent<Animator>();
        Initialize(ability, this.gameObject);
        Initialize(subHit, this.gameObject);
    }

    private void Update()
    {
        if (helmSplitterActive)
        {
            ExecuteHelmSplitter();
        }
        else if(subHitCollider.enabled || attackCollider.enabled)
        {
            DisableColliders();
        }
    }


    public override void Initialize(Ability selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }

    public override void Trigger()
    {
        animator.SetTrigger(triggerName);
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        helmSplitterActive = true;
        subHitCollider.enabled = true;
    }

    public override void DrawHurtBox()
    {
        enNum = subHitCollider.OverlapCollider(enemyLayer, hitEnemies);

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
            if (_enemy.Hurt != null)
                _enemy.Hurt.EnterHurtState();
            if (_enemy.enemyStats.armor != 1)
                _enemy.RigidBody.velocity = Vector2.down * 100;
            if (_enemy.Invul == false)
                DrawHitEffect(enemy);
            _enemy.TakeDamage(subHitDamage, false);
        }

        ApplyHitstop(subHitHitStop, enNum);
    }

    private void DrawFinisher()
    {
        enNum = attackCollider.OverlapCollider(enemyLayer, hitEnemies);


        if (rumbleDurration > 0)
        {
            Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyContainer _enemy = enemy.GetComponent<EnemyContainer>();
            if (CheckForNull(_enemy))
                _enemy = enemy.GetComponentInParent<EnemyContainer>();
            if (CheckForNull(_enemy) == true)
                return;

            _enemy.Hurt.EnterHurtState();
            _enemy.KnockBack(knockBackX, knockBackY, player.rb.transform.position);
            _enemy.TakeDamage(damage, true);
            DrawHitEffect(enemy);
        }

        ApplyHitstop(hitStop, enNum);
    }

    private void ExecuteHelmSplitter()
    { 
        if (!player.controller.GetGrounded())
        {
            player.rb.velocity = new Vector2(player.controller.DirectionFaced, -80);
            if ((player.rb.constraints & RigidbodyConstraints2D.FreezePositionY) != RigidbodyConstraints2D.FreezePositionY)
            {
                trailRenderer.emitting = true;
                DrawHurtBox();
            }
        }
        else
        {
            attackCollider.enabled = true;
            helmSplitterActive = false;
            trailRenderer.emitting = false;
            player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
            SimpleCameraShake._CameraShake(0.2f, 0.5f);
            particles.Play();
            DrawFinisher();
            DisableColliders();
        }
    }
    private bool CheckForNull(EnemyContainer _enemy)
    {
        return _enemy == null;
    }

    private void DisableColliders()
    {
        subHitCollider.enabled = false;
        attackCollider.enabled = false;
    }

    private void ApplyHitstop(float _hitstop, int _enemiesHit)
    {
        if (hitStop > 0 && enNum != 0)
        {
            SimpleCameraShake._CameraShake(0.1f, 0.05f);
            HitStop._SimpleHitStop(_hitstop * _enemiesHit);
        }
    }
}
