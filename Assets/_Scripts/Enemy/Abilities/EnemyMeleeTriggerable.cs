using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeTriggerable : EnemyProximityTriggerable
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public EnemyContainer enemy;
    [HideInInspector] public Animator animator;
    [HideInInspector] public int damage;
    [HideInInspector] public float knockBackX;
    [HideInInspector] public float knockBackY;
    [HideInInspector] public float moveForceX;
    [HideInInspector] public float moveForceY;


    public Collider2D attackCollider;
    public Transform knockBackPoint;
    public List<Collider2D> hitPlayer;
    public ContactFilter2D playerLayer;
    public float playerNum;
    private bool hurtBoxFlag = false;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
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

    private void Update()
    {
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName(enemyAbilityClone.aName))
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
        animator.SetTrigger(triggerName);
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
            _player._KnockBack(knockBackX, knockBackY, knockBackPoint.position);
            _player._DamagePlayer(damage, false);
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
        bool stopLeftMovement = enemy.groundCheck.edgeLeft && -enemy.transform.localScale.x < 0;
        bool stopRightMovement = enemy.groundCheck.edgeRight && -enemy.transform.localScale.x > 0;
        bool dontMove = stopLeftMovement || stopRightMovement;
        if (!dontMove)
            rb.AddForce(new Vector2(moveForceX * -enemy.transform.localScale.x, moveForceY) * rb.mass, ForceMode2D.Impulse);
    }
}
