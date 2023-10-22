using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeTriggerable : EnemyProximityTriggerable
{
    [HideInInspector] public Rigidbody2D RB;
    [HideInInspector] public EnemyContainer Enemy;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public int Damage;
    [HideInInspector] public float KnockBackX;
    [HideInInspector] public float KnockBackY;
    [HideInInspector] public float MoveForceX;
    [HideInInspector] public float moveForceY;


    public Collider2D attackCollider;
    public List<Collider2D> hitPlayer;
    public ContactFilter2D playerLayer;
    public float playerNum;
    private bool hurtBoxFlag = false;

    private void Start()
    {
        enemyAbilityClone = Object.Instantiate(scriptableAbility);
        Enemy = GetComponentInParent<EnemyContainer>();
        Initialize(enemyAbilityClone, this.gameObject);
        Animator = GetComponentInParent<Animator>();
        RB = GetComponentInParent<Rigidbody2D>();
    }

    public override void Initialize(EnemyAbility selectedAbility, GameObject abilityObject)
    {
        selectedAbility.Initialize(abilityObject);
    }

    private void Update()
    {
        if (!Enemy.Animator.GetCurrentAnimatorStateInfo(0).IsName(enemyAbilityClone.aName))
        {
            HurtBoxFlagOff();
        }
        if (hurtBoxFlag)
        {
            HurtBoxEvent();
        }
    }

    public override void Trigger()
    {
        Animator.Play(AnimationName);
    }

    //Gets called during attack animation, used only for attacks that need one frame of a hurtbox
    public void HurtBoxEvent()
    {
        attackCollider.enabled = true;
        playerNum = attackCollider.OverlapCollider(playerLayer, hitPlayer);
        attackCollider.enabled = false;

        foreach (Collider2D player in hitPlayer)
        {
            PlayerContainer _player = player.GetComponentInParent<PlayerContainer>();
            _player._KnockBack(KnockBackX, KnockBackY, Enemy.EnemyCollider.bounds.center);
            _player._DamagePlayer(Damage, false);
        }
    }

    public void HurtBoxFlagOn()
    {
        hurtBoxFlag = true;
    }

    public void HurtBoxFlagOff()
    {
        hurtBoxFlag = false;
    }

    public void AddForceEvent()
    {
        bool stopLeftMovement = Enemy.GroundCheck.EdgeLeft && -Enemy.transform.localScale.x < 0;
        bool stopRightMovement = Enemy.GroundCheck.EdgeRight && -Enemy.transform.localScale.x > 0;
        bool dontMove = stopLeftMovement || stopRightMovement;
        if (!dontMove)
            RB.AddForce(new Vector2(MoveForceX * -Enemy.transform.localScale.x, moveForceY) * RB.mass, ForceMode2D.Impulse);
    }
}
