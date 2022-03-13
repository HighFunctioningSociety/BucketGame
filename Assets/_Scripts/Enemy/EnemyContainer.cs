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
    public State hurtState;
    public State remainState;

    [Space]
    [Header ("State information")]
    public float stateTimeElapsed = 0;
    public float idleTimeElapsed = 0;
    public int attacksDoneInState = 0;
    public Decision[] hurtDecisions;
    [HideInInspector] public float hurtTimeRemaining;
    [HideInInspector] public float aggroTimeRemaining;
    public float fullscreenAttackTiming;
    [HideInInspector] public bool wasHurt;
    
    [Space]
    [Header ("Enemy Components/Variables")]
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public Collider2D enemyCollider;
    public EnemyAbilityManager abilityManager;
    public Rigidbody2D rb;
    public Transform enemyPrefabDead;
    public Transform enemyParticlesDead;
    private Material matWhite;
    private Material matDefault;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public bool inIdle;
    [HideInInspector] public Animator animator;

    [Header("Other Values")]
    [Space]
    public bool unfreezeConstraintsOnReset = false;
    public bool aiNotActiveOnStart;
    private int flashCount = 1;
    public bool blockPointAttacked = false;
    public float LOSLength;
    public float LOSCount;
    public int patrolDirection;
    public GroundCheck groundCheck;
    public BossTriggers triggers;
    public Transform boundLeft, boundRight;
    public bool invul = false;
    private int invulFrames = 5;

    [HideInInspector] public float dir;
    [HideInInspector] public float speed;

    [HideInInspector] public int currentPatrol;
    public Transform targetObject;
    [HideInInspector] public Vector3 spawnPosition;
    public Vector3 startingPosition;
    [HideInInspector] public Vector3 _targetPosition;

    public Vector3 targetPosition
    {
        get { return _targetPosition; }
        set {
            if (boundLeft != null && boundRight != null)
                value.x = Mathf.Clamp(value.x, boundLeft.position.x, boundRight.position.x);
            value.z = 0;
            _targetPosition = value; }
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
        spawnPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        if (spriteRenderer != null)
        {
            matWhite = Resources.Load("WhiteFlash", typeof(Material)) as Material;
            matDefault = spriteRenderer.material;
        }
        SetUpAI();
    }
    private void FixedUpdate()
    {
        if (!aiActive)
            return;
        if (targetObject == null)
        {
            FindPlayer();
        }
        currentState.UpdateState(this);
        IncrementStateTime();
        IncrementIdleTime();
    }
    
    private void IncrementStateTime()
    {
        stateTimeElapsed += Time.fixedDeltaTime;
    }

    private void IncrementIdleTime()
    {
        if (currentState.isNeutralState)
        {
            inIdle = true;
            idleTimeElapsed += Time.fixedDeltaTime;
        }
        else
        {
            inIdle = false;
        }
    }

    public void SetUpAI()
    {
        currentPatrol = 0;
        int[] patrolDirectionValues = new int[] { -1, 1 };
        patrolDirection = patrolDirectionValues[Random.Range(0, 1)];
        currentState = defaultState;
        curHealth = enemyStats.maxHealth;
        curArmor = enemyStats.armor;
        invul = false;
        rb.velocity = Vector2.zero;
        if (spriteRenderer != null)
            spriteRenderer.material = matDefault;
        if (triggers != null)
            triggers.ResetTriggers();

        aiActive = (aiNotActiveOnStart ?  false : true);
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
            aggroTimeRemaining = enemyStats.aggroTime;
        }
    }

    private void ResetStateTimers()
    {
        if (!currentState.isNeutralState)
            idleTimeElapsed = 0;
        attacksDoneInState = 0;
        stateTimeElapsed = 0;
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
            fullscreenAttackTiming = 0;
        }
        else 
        {
            fullscreenAttackTiming = Random.Range(enemyStats.fullscreenTimingMin, enemyStats.fullscreenTimingMax); 
        }
    }

    public void InitiateReposition()
    {
        abilityManager.nextRepositionTime = enemyStats.repositionCooldown + Time.time;
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

    public bool CheckForObstacleContacts()
    {
        return enemyCollider.IsTouchingLayers(obstacleLayer);
    }

    public bool CheckLineOfSight()
    {
        RaycastHit2D raycastLOS = GenerateLOSRaycast();
        if (raycastLOS.collider != null)
        {
            LOSCount++;
            return true;
        }
        else
        {
            LOSCount = 0;
            return false;
        }
    }

    public bool CheckRaycastObstacleCollision()
    {
        RaycastHit2D raycastLOS = GenerateLOSRaycast();
        return raycastLOS.collider != null;
    }

    public RaycastHit2D GenerateLOSRaycast()
    {
        Vector3 xBoundsExtents = (dir == 1 ? -new Vector3(enemyCollider.bounds.extents.x, 0) : new Vector3(enemyCollider.bounds.extents.x, 0));
        Vector2 directionalVector = (dir == 1 ? Vector2.left : Vector2.right);
        return Physics2D.Raycast(enemyCollider.bounds.center + xBoundsExtents, directionalVector, 1f, obstacleLayer);
    }

    public void DrawLineOfSight()
    {
        if (LOSLength != 0)
        {
            Color raycastColor = Color.cyan;
            Vector3 xBoundsExtents = (dir == 1 ? -new Vector3(enemyCollider.bounds.extents.x, 0) : new Vector3(enemyCollider.bounds.extents.x, 0));
            Vector2 directionalVector = (dir == 1 ? Vector2.left : Vector2.right);
            Debug.DrawRay(enemyCollider.bounds.center + xBoundsExtents,  directionalVector * LOSLength, raycastColor);
        }
    }

    public void EnterHurtState()
    {
        if (CheckHurtConditions() && !currentState.uninterruptable)
        {
            currentState = hurtState;
            StopMomentum();
            LOSCount = 0;
            ResetAnimationTriggers();
            UnfreezeConstraints();
            SetHurtAnimation();
        }
    }

    public void TakeDamage(int _damage, bool bypassInvul)
    {
        if (invul != true || bypassInvul == true)
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

    private bool CheckHurtConditions()
    {
        if (hurtDecisions.Length == 0)
            return false;
        
        bool meetsConditions = true;
        foreach (Decision condition in hurtDecisions)
        {
            if (condition.Decide(this) == false)
                meetsConditions = false;
        }
        return meetsConditions;
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.material = matWhite;
        for (int i = 0; i < flashCount; i++)
        {
            yield return new WaitForEndOfFrame();   
        }
        if (spriteRenderer != null)
            spriteRenderer.material = matDefault;
    }

    private void SetHurtAnimation()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
        {
            animator.SetTrigger("Hurt");
        }
    }

    private void ResetAnimationTriggers()
    {
        foreach (var parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(parameter.name);
        }
    }

    public void KnockBack(float _knockBackX, float _knockBackY, Vector3 _playerPosition)
    {
        // reduce current velocity based on armor
        rb.velocity *= curArmor;

        // Calculate Knockback
        float knockBackX = _knockBackX * rb.mass * (1 - curArmor) * Mathf.Sign(this.transform.position.x - _playerPosition.x);
        float knockBackY = -_knockBackY * rb.mass * (1 - curArmor);

        // Apply knockback
        rb.AddForce(new Vector2(knockBackX, -knockBackY), ForceMode2D.Impulse);
    }

    public void ActivateBlockReaction()
    {
        blockPointAttacked = true;
    }

    IEnumerator Invulnerability()
    {
        invul = true;
        for (int i = 0; i < invulFrames; i++)
            yield return new WaitForEndOfFrame();
        invul = false;
    }

    public void StopMomentum()
    {
        rb.velocity = Vector3.zero;
    }

    public void FreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FreezeXConstraint()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }
    public void FreezeYConstraint()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void UnfreezeConstraints()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FindPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult != null)
        {
            targetObject = searchResult.transform;
        }
    }

    public void _RespawnEnemy()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Reset");
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        SetUpAI();
        transform.position = spawnPosition;
    }

    private void OnDrawGizmos()
    {
        if (currentState != null)
        {
            //State Color
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(this.transform.position, enemyStats.stateSphereRadius);
        }

        if (LOSLength != 0)
        {
            Color raycastColor = Color.cyan;
            Debug.DrawRay(enemyCollider.bounds.center - new Vector3(enemyCollider.bounds.extents.x, 0), Vector2.left * LOSLength, raycastColor);
        }

        //Aggro Range
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(enemyStats.aggroRangeX, enemyStats.aggroRangeY, 0));
    }
}
