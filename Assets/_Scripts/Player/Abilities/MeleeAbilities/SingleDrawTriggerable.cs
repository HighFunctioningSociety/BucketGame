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
    private float airKnockBackX = -10;
    private float airKnockBackY = 42;

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
        foreach (Collider2D _colInfo in hitEnemies)
        {
            DrawHitEffect(_colInfo);
            HitReactor hitReactor = _colInfo.GetComponent<HitReactor>();
            if (hitReactor != null)
                hitReactor.ReactToHit();

            EnemyContainer enemyContainer = _colInfo.GetComponent<EnemyContainer>();
            if (CheckForNull(enemyContainer) == true)
                enemyContainer = _colInfo.GetComponentInParent<EnemyContainer>();
            if (CheckForNull(enemyContainer) == true)
                return;

            ApplyHitstop(hitStop, enNum);
            ApplyControllerRumble();

            EnemyStateMachine enemyStateMachine = _colInfo.GetComponent<EnemyStateMachine>();
            
            if (enemyStateMachine.Hurt != null)
            {
                enemyStateMachine.Hurt.EnterHurtState();
            }

            ApplyKnockBack(enemyContainer);
            enemyContainer.TakeDamage(damage, false);
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

    private void ApplyKnockBack(EnemyContainer enemyContainer)
    {
        float knockBackTotalX = knockBackX;
        float knockBackTotalY = knockBackY;

        if (enemyContainer.GroundCheck != null && !enemyContainer.GroundCheck.Grounded)
        {
            knockBackTotalX -= airKnockBackX;
            knockBackTotalY += airKnockBackY;
        }

        enemyContainer.KnockBack(knockBackTotalX, knockBackTotalY, player.rb.transform.position);
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
