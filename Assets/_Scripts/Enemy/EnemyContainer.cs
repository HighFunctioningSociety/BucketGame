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

    [Space]
    [Header ("State information")]
    public float StateTimeElapsed = 0;
    public int AttacksDoneInState = 0;

    [Space]
    [Header("Enemy Prefabs")]
    public Transform EnemyPrefabDead;
    public Transform EnemyParticlesDead;

    [Header("Other Values")]
    [Space]
    public bool UnfreezeConstraintsOnReset = false;
    public bool AINotActiveOnStart;
    public GroundCheck groundCheck;
    public Transform BoundLeft, BoundRight;
    public bool Invul = false;

    [HideInInspector] public LayerMask PlayerLayer = Constants.Layers.Player;
    [HideInInspector] public LayerMask ObstacleLayer = Constants.Layers.Obstacles;

    [HideInInspector] public float FullscreenAttackTiming;
    [HideInInspector] public float HurtTimeRemaining;
    [HideInInspector] public float AggroTimeRemaining;
    [HideInInspector] public bool WasHurt;
    [HideInInspector] public bool InIdle;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody2D RigidBody;
    [HideInInspector] public Collider2D EnemyCollider;

    [HideInInspector] public int PatrolDirection;
    [HideInInspector] public float Direction;
    [HideInInspector] public float Speed;
    [HideInInspector] public int CurrentPatrol;
    [HideInInspector] public Transform TargetObject;
    [HideInInspector] public Vector3 SpawnPosition;
    [HideInInspector] public Vector3 StartingPosition;
    
    [HideInInspector] public bool BlockPointAttacked = false;

    // Optional Components
    [HideInInspector] public EnemyAbilityManager AbilityManager;
    [HideInInspector] public LOSController LOS;
    [HideInInspector] public HurtController Hurt;
    [HideInInspector] public RepositionController Reposition;
    [HideInInspector] public BossTriggers Triggers;

    private readonly int _flashCount = 1;
    private readonly int _invulFrames = 5;
    private Material _matWhite;
    private Material _matDefault;
    private SpriteRenderer _spriteRenderer;

    private Vector3 _targetPosition;

    public Vector3 TargetPosition
    {
        get { return _targetPosition; }
        set {
            if (BoundLeft != null && BoundRight != null)
            {
                value.x = Mathf.Clamp(value.x, BoundLeft.position.x, BoundRight.position.x);
            }
            value.z = 0;
            _targetPosition = value; 
        }
    }

    private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, enemyStats.maxHealth); }
    }
    private float _currentArmor;
    public float CurrentArmor
    {
        get { return _currentArmor; }
        set { _currentArmor = Mathf.Clamp(value, 0, enemyStats.armor); }
    }

    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        EnemyCollider = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
        LOS = GetComponent<LOSController>();
        Triggers = GetComponent<BossTriggers>();
        Reposition = GetComponent<RepositionController>();
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
    }
    
    

    public void SetUpAI()
    {
        CurrentPatrol = 0;
        int[] patrolDirectionValues = new int[] { -1, 1 };
        PatrolDirection = patrolDirectionValues[Random.Range(0, 1)];
        Hurt = GetComponent<HurtController>();
        SpawnPosition = transform.position;

        currentState = defaultState;

        CurrentHealth = enemyStats.maxHealth;
        CurrentArmor = enemyStats.armor;
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
        if (nextState != currentState)
        {
            currentState = nextState;
            ResetStateTimers();
            ActivateStateEnterAction();
            CalculateFullscreenAttackTiming();
        }

        if (nextState.IsAggroState)
        {
            AggroTimeRemaining = currentState.TimeToRemainInState;
        }
    }

    private void ActivateStateEnterAction()
    {
        if (currentState.OnStateEnterAction != null)
            currentState.OnStateEnterAction.Act(this);
    }

    private void IncrementStateTime()
    {
        StateTimeElapsed += Time.fixedDeltaTime;
    }

    private void ResetStateTimers()
    {
        AttacksDoneInState = 0;
        StateTimeElapsed = 0;
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

    public void TakeDamage(int _damage, bool bypassInvul)
    {
        if (Invul != true || bypassInvul == true)
        {
            CurrentHealth -= _damage;
            StopAllCoroutines();
            StartCoroutine(Invulnerability());
            StartCoroutine(DamageFlash());
        }

        if (CurrentHealth <= 0)
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

    public void KnockBack(float _knockBackX, float _knockBackY, Vector3 _playerPosition)
    {
        // reduce current velocity based on armor
        RigidBody.velocity *= CurrentArmor;

        // Calculate Knockback
        float knockBackX = _knockBackX * RigidBody.mass * (1 - CurrentArmor) * Mathf.Sign(EnemyCollider.bounds.center.x - _playerPosition.x);
        float knockBackY = -_knockBackY * RigidBody.mass * (1 - CurrentArmor);

        // Apply knockback
        RigidBody.AddForce(new Vector2(knockBackX, -knockBackY), ForceMode2D.Impulse);
    }

    public void ActivateBlockReaction()
    {
        BlockPointAttacked = true;
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

    public void ResetAnimationTriggers()
    {
        foreach (var parameter in Animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                Animator.ResetTrigger(parameter.name);
        }
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
