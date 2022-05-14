using System.Collections.Generic;
using UnityEngine;

public class SingleDrawTriggerable : MeleeAttackTriggerable
{
    [SerializeField] private ContactFilter2D switchLayer;
    public int switchNum = 0;
    public List<Collider2D> hitSwitches;

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
    }

    public override void DrawHurtBox()
    {
        player.rb.AddForce(new Vector2(moveForceX * player.transform.localScale.x, moveForceY), ForceMode2D.Impulse);

        EnableCollider();
        blockNum = attackCollider.OverlapCollider(blockerLayer, hitBlockers); //If a blocker is hit, negate the rest of the attack

        if (blockNum != 0)
        {
            BlockerHitCalculation();
            return;
        }

        enNum = attackCollider.OverlapCollider(enemyLayer, hitEnemies);
        obNum = attackCollider.OverlapCollider(obstacleLayer, hitObstacles);
        switchNum = attackCollider.OverlapCollider(switchLayer, hitSwitches);
        DisableCollider();

        if (enNum != 0 || obNum != 0 || blockNum != 0)
        {
            player.rb.AddForce(new Vector2(knockBackX * -Mathf.Sign(player.transform.localScale.x) * 0.5f, 0f), ForceMode2D.Impulse);
        }

        if (obNum != 0)
            ObstacleHitCalculation();
        if (switchNum != 0)
            SwitchHitCalculation();
        if (enNum != 0)
        {
            impactAudioScource.Play();
            EnemyHitCalculations();
        }
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

    private void ObstacleHitCalculation()
    {
        foreach (Collider2D obstacle in hitObstacles)
        {
            if (obstacle.GetComponent<BreakableObject>() != null)
            {
                BreakableObject _obstacle = obstacle.GetComponent<BreakableObject>();
                _obstacle.DamageObject(player.transform);
                DrawHitEffect(obstacle);
            }
        }
    }

    private void SwitchHitCalculation()
    {
        foreach (Collider2D switchCollider in hitSwitches)
        {
            if (switchCollider.GetComponent<SwitchManager>() != null)
            {
                SwitchManager _switch = switchCollider.GetComponent<SwitchManager>();
                _switch.FlipSwitch();
                if (_switch.multiUse || _switch.scriptableBool.state == false)
                {
                    DrawHitEffect(switchCollider);
                }
            }
        }
    }

    private void EnemyHitCalculations()
    {
        foreach (Collider2D enemy in hitEnemies)
        {
            DrawHitEffect(enemy);
            HitReactor hitReactor = enemy.GetComponent<HitReactor>();
            if (hitReactor != null)
                hitReactor.ReactToHit();


            EnemyContainer _enemy = enemy.GetComponent<EnemyContainer>();
            if (CheckForNull(_enemy) == true)
                _enemy = enemy.GetComponentInParent<EnemyContainer>();
            if (CheckForNull(_enemy) == true)
                return;

            ApplyHitstop(hitStop, enNum);
            ApplyControllerRumble();

            if (_enemy.HurtController != null)
            {
                _enemy.HurtController.EnterHurtState();
            }

            ApplyKnockBack(_enemy);
            _enemy.TakeDamage(damage, false);
            player.playerStats.curSpiritProgression += meterProgression;
        }
    }

    private bool CheckForNull(EnemyContainer _enemy)
    {
        return _enemy == null;
    }

    private void ApplyControllerRumble()
    {
        if (rumbleDurration > 0)
        {
            Rumbler.RumbleConstant(rumbleLow, rumbleHigh, rumbleDurration);
        }
    }

    private void ApplyKnockBack(EnemyContainer _enemy)
    {
        float knockBackTotalX = knockBackX, knockBackTotalY = knockBackY;
        if (_enemy.groundCheck != null)
        {
            if (!_enemy.groundCheck.Grounded)
            {
                knockBackTotalY += 42;
                knockBackTotalX -= 10;
            }
        }
        _enemy.KnockBack(knockBackTotalX, knockBackTotalY, player.rb.transform.position);
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
