using System.Collections;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public bool aiActive;

    public EnemyStats enemyStats;

    [Space]
    [Header("Enemy State Objects")]
    public State currentState;
    public State defaultState;
    public State aggroState;
    public State remainState;

    [Space]
    [Header ("State information")]
    public float StateTimeElapsed = 0;
    public float IdleTimeElapsed = 0;
    public int AttacksDoneInState = 0;
    
    [Space]
    [Header ("Enemy Components/Variables")]
    public LayerMask PlayerLayer;
    public LayerMask ObstacleLayer;
    public Transform enemyPrefabDead;
    public Transform enemyParticlesDead;


    [Header("Other Values")]
    [Space]
    public bool UnfreezeConstraintsOnReset = false;
    public bool AINotActiveOnStart;
    public bool blockPointAttacked = false;
    public int patrolDirection;
    public GroundCheck groundCheck;
    public Transform boundLeft, boundRight;
    public bool Invul = false;

    public BossTriggers Triggers;

    [HideInInspector] public float FullscreenAttackTiming;
    [HideInInspector] public float HurtTimeRemaining;
    [HideInInspector] public float AggroTimeRemaining;
    [HideInInspector] public bool WasHurt;
    [HideInInspector] public bool InIdle;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody2D RigidBody;
    [HideInInspector] public Collider2D EnemyCollider;

    [HideInInspector] public EnemyAbilityManager AbilityManager;
    [HideInInspector] public LOSComponenet LOSCaster;
    [HideInInspector] public HurtComponent HurtController;

    [HideInInspector] public float Direction;
    [HideInInspector] public float Speed;
    [HideInInspector] public int CurrentPatrol;
    [HideInInspector] public Transform TargetObject;
    [HideInInspector] public Vector3 SpawnPosition;
    [HideInInspector] public Vector3 StartingPosition;
    [HideInInspector] public Vector3 TargetPosition;
    

    private readonly int _flashCount = 1;
    private readonly int _invulFrames = 5;
    private Material _matWhite;
    private Material _matDefault;
    private SpriteRenderer _spriteRenderer;

    public Vector3 targetPosition
    {
        get { return TargetPosition; }
        set {
            if (boundLeft != null && boundRight != null)
            {
                value.x = Mathf.Clamp(value.x, boundLeft.position.x, boundRight.position.x);
            }
            value.z = 0;
            TargetPosition = value; 
        }
    }

    private int _curHealth;
    public int curHealth
    {
        get { return _curHealth; }
        set { _curHealth = Mathf.Clamp(value, 0, enemyStats.maxHealth); }
    }
    private float _curArmor;
    public float curArmor
    {
        get { return _curArmor; }
        set { _curArmor = Mathf.Clamp(value, 0, enemyStats.armor); }
    }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        EnemyCollider = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
        LOSCaster = GetComponent<LOSComponenet>();
        AbilityManager = GetComponent<EnemyAbilityManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null)
        {
            _matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
            _matDefault = _spriteRenderer.material;
        }

        SetUpAI();
    }

    private void FixedUpdate()
    {
        if (!aiActive)
            return;
        if (TargetObject == null)
        {
            FindPlayer();
        }
        currentState.UpdateState(this);
        IncrementStateTime();
        IncrementIdleTime();
    }
    
    private void IncrementStateTime()
    {
        StateTimeElapsed += Time.fixedDeltaTime;
    }

    private void IncrementIdleTime()
    {
        if (currentState.isNeutralState)
        {
            InIdle = true;
            IdleTimeElapsed += Time.fixedDeltaTime;
        }
        else
        {
            InIdle = false;
        }
    }

    public void SetUpAI()
    {
        CurrentPatrol = 0;
        int[] patrolDirectionValues = new int[] { -1, 1 };
        patrolDirection = patrolDirectionValues[Random.Range(0, 1)];
        HurtController = GetComponent<HurtComponent>();
        SpawnPosition = transform.position;
        currentState = defaultState;
        curHealth = enemyStats.maxHealth;
        curArmor = enemyStats.armor;
        Invul = false;
        RigidBody.velocity = Vector2.zero;
        if (_spriteRenderer != null)
            _spriteRenderer.material = _matDefault;
        if (Triggers != null)
            Triggers.ResetTriggers();

        aiActive = (AINotActiveOnStart ?  false : true);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            ResetStateTimers();
            ActivateStateEnterAction();
            CalculateFullscreenAttackTiming();
        }

        if (nextState == aggroState)
        {
            AggroTimeRemaining = enemyStats.aggroTime;
        }
    }

    private void ResetStateTimers()
    {
        if (!currentState.isNeutralState)
            IdleTimeElapsed = 0;
        AttacksDoneInState = 0;
        StateTimeElapsed = 0;
    }

    private void ActivateStateEnterAction()
    {
        if (currentState.OnStateEnterAction != null)
            currentState.OnStateEnterAction.Act(this);
    }

    private void CalculateFullscreenAttackTiming()
    {
        if (enemyStats.fullscreenTimingMax == 0) 
        {
            FullscreenAttackTiming = 0;
        }
        else 
        {
            FullscreenAttackTiming = Random.Range(enemyStats.fullscreenTimingMin, enemyStats.fullscreenTimingMax); 
        }
    }

    public void InitiateReposition()
    {
        AbilityManager.nextRepositionTime = enemyStats.repositionCooldown + Time.time;
        targetPosition = GenerateTargetPosition();
    }

    private Vector2 GenerateTargetPosition()
    {
        float randRangeX = Random.Range(enemyStats.repositionMinDistance, enemyStats.repositionMaxDistance);
        int randSign = RandomSignGenerator();
        Debug.Log(randSign + "reposition sign");
        randRangeX *= randSign;
        randRangeX += transform.position.x;
        Vector2 newTargetPosition = new Vector2(randRangeX, transform.position.y);
        return newTargetPosition;
    }

    private int RandomSignGenerator()
    {
        int randNum = Random.Range(0, 2);
        return (randNum == 0 ? -1 : 0);
    }

    public void TakeDamage(int _damage, bool bypassInvul)
    {
        if (Invul != true || bypassInvul == true)
        {
            curHealth -= _damage;
            StopAllCoroutines();
            StartCoroutine(Invulnerability());
            StartCoroutine(DamageFlash());
        }

        if (curHealth <= 0)
        {
            _GameManager.KillEnemy(this);
            Rumbler.RumbleConstant(30, 30, 0.1f);
        }
    }

    private IEnumerator DamageFlash()
    {
        _spriteRenderer.material = _matWhite;
        for (int i = 0; i < _flashCount; i++)
        {
            yield return new WaitForEndOfFrame();   
        }
        if (_spriteRenderer != null)
            _spriteRenderer.material = _matDefault;
    }

    public void ResetAnimationTriggers()
    {
        foreach (var parameter in Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                Animator.ResetTrigger(parameter.name);
        }
    }

    public void KnockBack(float _knockBackX, float _knockBackY, Vector3 _playerPosition)
    {
        // reduce current velocity based on armor
        RigidBody.velocity *= curArmor;

        // Calculate Knockback
        float knockBackX = _knockBackX * RigidBody.mass * (1 - curArmor) * Mathf.Sign(EnemyCollider.bounds.center.x - _playerPosition.x);
        float knockBackY = -_knockBackY * RigidBody.mass * (1 - curArmor);

        // Apply knockback
        RigidBody.AddForce(new Vector2(knockBackX, -knockBackY), ForceMode2D.Impulse);
    }

    public void ActivateBlockReaction()
    {
        blockPointAttacked = true;
    }

    IEnumerator Invulnerability()
    {
        Invul = true;

        for (int i = 0; i < _invulFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        Invul = false;
    }

    public void StopMomentum()
    {
        RigidBody.velocity = Vector3.zero;
    }

    public void FreezeConstraints()
    {
        RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FreezeXConstraint()
    {
        RigidBody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void FreezeYConstraint()
    {
        RigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void UnfreezeConstraints()
    {
        RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FindPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult != null)
        {
            TargetObject = searchResult.transform;
        }
    }

    public void RespawnEnemy()
    {
        gameObject.SetActive(true);
        Animator.SetTrigger("Reset");
        RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetUpAI();
        transform.position = SpawnPosition;
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            //State Color
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(GetComponent<Collider2D>().bounds.center, enemyStats.stateSphereRadius);
        }

        //Aggro Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, new Vector3(enemyStats.aggroRangeX, enemyStats.aggroRangeY, 0));
    }
}
