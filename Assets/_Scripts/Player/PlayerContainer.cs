using System.Collections;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public static PlayerContainer _playerContainer;
    public GameProgress gameProgress;
    public PlayerStats playerStats;

    public PSTATE currentState;
    public CONTROLSTATE currentControlType;
    public bool invul;

    [HideInInspector] public bool transitioningLeft;
    [HideInInspector] public bool transitioningRight;
    public AbilityManager abilityManager;
    public EquipmentManager equipmentManager;
    public CoolDownManager coolDownManager;
    public DashAbility dashAbility;
    public AnimatePlayer animatePlayer;
    public Rigidbody2D rb;
    public CharacterController2D controller;
    public PlayerRenderer pr;
    public GameObject damageEffect;
    public float JumpCancelMonitor;
    public float GameTime;
    public bool JumpCancelBoolMonitor;
    private Color stateColor = Color.grey;
    private bool isYVelocityFrozen = false;


    public enum PSTATE
    {
        NORMAL,
        ATTACK,
        HURT,
        REST,
        RESPAWNING,
    }

    public enum CONTROLSTATE
    {
        ACCEPT_INPUT,
        RELINQUISHED,
        RESTING,
    }

    public float stunTimeLeft;

    public void Awake()
    {
        if (_playerContainer == null)
        {
            _playerContainer = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadGame();
        playerStats.Init();
        UpdateHealthUI();

        if (dashAbility == null)
        {
            Debug.LogError("dashAbility not plugged in");
            dashAbility = GetComponent<DashAbility>();
        }

        if (abilityManager == null)
        {
            Debug.LogError("abilityManager not plugged in");
            abilityManager = GetComponent<AbilityManager>();
        }

        invul = false;
    }

    private void FixedUpdate()
    {
        switch (currentControlType)
        {
            case CONTROLSTATE.ACCEPT_INPUT:
                AcceptInput();
                break;

            case CONTROLSTATE.RELINQUISHED:
                RelinquishInput();
                break;
        }

    }

    private void AcceptInput()
    {
        switch (currentState)
        {
            case PSTATE.NORMAL:
                PStateNormal();
                break;

            case PSTATE.ATTACK:
                PStateAttack();
                break;

            case PSTATE.HURT:
                PStateHurt();
                break;

            case PSTATE.REST:
                PStateRest();
                break;
        }
    }

    private void RelinquishInput()
    {
        if (transitioningLeft)
        {
            controller.Move(-0.7f * Time.fixedDeltaTime, false, false);
        }
        else if (transitioningRight)
        {
            controller.Move(0.7f * Time.fixedDeltaTime, false, false);
        }
        Inputs.DisableInput();  
    }

    private void PStateNormal()
    {
        if (Inputs.dash == true)
        {
            dashAbility.CallDash(Inputs.dash);
        }
        else if (dashAbility.dashTime <= 0)
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, Inputs.jump, Inputs.jumpHeld);
        }
        else if (dashAbility.dashTime < dashAbility.startDashTime * 0.4 && Inputs.jump && controller.GetGrounded())
        {
            abilityManager.UnfreezeConstraints();
            dashAbility.dashTime = 0;
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, Inputs.jump, Inputs.jumpHeld);
        }
        Inputs.jump = false;
    }


    private void PStateAttack()
    {
        if (Inputs.jump && controller.grounded && JumpCancelTime() && !abilityManager.wasJumpCanceled && abilityManager.canJumpCancelAttack)
        {
            Inputs.SetAttackInputBufferTime(0.15f);
            abilityManager._TurnOffMovementModifier();
            abilityManager._ActivateJumpCancel();
            coolDownManager.ResetCoolDown();
            animatePlayer.JumpCancelTrigger();
            Inputs.DisableAttack();
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, Inputs.jump, Inputs.jumpHeld);
        } 
        else if (!abilityManager.freezeMovement)
        {
            UnfrozenJumpCancelManager();
        }
        else if (abilityManager.freezeMovement)
        {
            FrozenJumpCancelManager();
        }
        else
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, false, false);
        }

        if (isYVelocityFrozen)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }

    private bool JumpCancelTime()
    {
        if (abilityManager.currentMeleeAttack == null)
            return false;
        float jumpCancelBuffer = 0.15f;
        return Time.time > ((coolDownManager.nextReadyTime - abilityManager.currentMeleeAttack.ability.aBaseCoolDown) + jumpCancelBuffer);
    }

    private void FrozenJumpCancelManager()
    {
        if (abilityManager.canJumpCancelAttack && controller.GetGrounded() == false)
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, Inputs.jump, Inputs.jumpHeld);
        }
        else if (abilityManager.canJumpCancelAttack && controller.GetGrounded() && abilityManager.wasJumpCanceled)
        {
            controller.Move(0, Inputs.jump, Inputs.jumpHeld);
        }
        else if (abilityManager.canJumpCancelAttack && controller.GetGrounded() && !abilityManager.wasJumpCanceled)
        {
            controller.Move(0, false, false);
        }
        else if (controller.GetGrounded())
        {
            controller.Move(0, false, false);
        }
        else
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, false, false);
        }
    }

    private void UnfrozenJumpCancelManager()
    {
        if (abilityManager.slowMovement)
        {
            controller.Move(Inputs.Horizontal / 8 * Time.fixedDeltaTime, false, false);
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / playerStats.curMovementDivider, 0);
        }
        else if (abilityManager.canJumpCancelAttack && abilityManager.wasJumpCanceled)
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, Inputs.jump, Inputs.jumpHeld);
        }
        else if (abilityManager.canJumpCancelAttack && !abilityManager.wasJumpCanceled)
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, false, false);
        }
        else
        {
            controller.Move(Inputs.Horizontal * Time.fixedDeltaTime, false, false);
        }
    }


    private void PStateHurt()
    {
        abilityManager.UnfreezeConstraints();
        abilityManager.CancelAbilities();
        Inputs.attack = false;
        Inputs.specialAttack = false;
        StunTimeCountdown();
    }

    private void PStateRest()
    {
        playerStats.curHealth = playerStats.maxHealth;
    }

    public void _DamagePlayer(int _damage, bool bypassInvul)
    {
        // Damage current health and apply invulnerability
        if (invul != true || bypassInvul == true)
        {
            HitStop._SimpleHitStop(0.3f);
            Rumbler.RumbleConstant(30, 30, 0.3f);
            stunTimeLeft = playerStats.stunTime;
            currentState = PSTATE.HURT;
            isYVelocityFrozen = false;
            SubtractFromHealth(_damage);
            abilityManager.CancelAbilities();
            StartCoroutine(PlayDamageEffect(0.025f));
            StartCoroutine(Invulnerability());
        }

        // If player health is equal to zero call the GameMaster to kill the player object
        if (playerStats.curHealth <= 0)
        {
            currentState = PSTATE.RESPAWNING;
            abilityManager.CancelAbilities();
            _GameManager.KillPlayer(this);
        }
    }

    public void _KnockBack(float _knockBackX, float _knockBackY, Vector2 _enemyPosition)
    {
        if (invul != true)
        {
            abilityManager.CancelAbilities();
            abilityManager.UnfreezeConstraints();

            // Set velocity equal to zero so it doesnt mess with knockback
            rb.velocity = new Vector2(0f, 0f);

            // Calculate knockback using enemy location and enemy knockback value
            float knockBackX = rb.mass * (_knockBackX) * (Mathf.Sign(rb.transform.position.x - _enemyPosition.x));
            float knockBackY = rb.mass * (_knockBackY);

            // Apply knockback
            rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Impulse);
            StartCoroutine(LowG());
        }
    }

    public static void KillVelocity()
    {
        _playerContainer._KillVelocity();
    }

    private void _KillVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    private void StunTimeCountdown()
    {
        stunTimeLeft -= Time.fixedDeltaTime;

        if (stunTimeLeft <= 0)
            currentState = PSTATE.NORMAL;
    }

    public void RefreshMovement()
    {
        playerStats.curMovementDivider = playerStats.movementDivider;
    }

    public void SubtractFromHealth(int subtractByAmount)
    {
        playerStats.curHealth -= subtractByAmount;
        UpdateHealthUI();
    }

    public void AddHealth(int addAmount)
    {
        playerStats.curHealth += addAmount;
        UpdateHealthUI();
    }

    public void SetHealth(int value)
    {
        playerStats.curHealth = value;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        _UIManager.UIManager._SetHealth(playerStats.curHealth, playerStats.maxHealth);
    }

    public void FreezeYVelocity()
    {
        isYVelocityFrozen = true;
    }
    public void UnfreezeYVelocity()
    {
        isYVelocityFrozen = false;
    }


    IEnumerator LowG()
    {
        rb.drag = 15f;
        rb.gravityScale = playerStats.defaultGravity * (2/3);
        yield return new WaitForSeconds(playerStats.stunTime);
        rb.drag = playerStats.defaultDrag;
        rb.gravityScale = playerStats.defaultGravity;
    }

    IEnumerator Invulnerability()
    {
        invul = true;
        pr._flickerEffect();
        yield return new WaitForSeconds(playerStats.invulTime);
        invul = false;
    }

    IEnumerator PlayDamageEffect(float _delay)
    {
        Quaternion originalRotation = damageEffect.transform.rotation;
        damageEffect.SetActive(true);
        damageEffect.transform.rotation = new Quaternion(damageEffect.transform.rotation.x, damageEffect.transform.rotation.y, damageEffect.transform.rotation.z + Random.Range(-25, 25), damageEffect.transform.rotation.w);

        yield return new WaitForSeconds(_delay);

        damageEffect.transform.rotation = originalRotation;
        damageEffect.SetActive(false);
    }

    public static void StopCoroutines()
    {
        _playerContainer._StopCoroutines();
    }

    private void _StopCoroutines()
    {
        StopAllCoroutines();
        invul = false;
        rb.drag = playerStats.defaultDrag;
        rb.gravityScale = playerStats.defaultGravity;
        currentState = PSTATE.NORMAL;
    }

    public void KillHorizontalInput()
    {
        controller.killHorizontalInput = true;
    }

    public void ActivateHorizontalInput()
    {
        controller.killHorizontalInput = false;
    }

    public static void RelinquishControl()
    {
        _playerContainer._RelinquishControl();
    }

    private void _RelinquishControl()
    {
        currentControlType = CONTROLSTATE.RELINQUISHED;
    }

    public static void EnableControl()
    {
        _playerContainer._EnableControl();
    }

    private void _EnableControl()
    {
        currentControlType = CONTROLSTATE.ACCEPT_INPUT;
    }

    public void SetDrag(float dragVal)
    {
        rb.drag = dragVal;
    }

    public void SaveGame()
    {
        _GameManager.SaveGameState(this);
    }

    public void LoadGame()
    {
        _GameManager.LoadGameState(this);
    }

    private void OnDrawGizmos()
    {
        if (currentState == PSTATE.NORMAL)
            stateColor = Color.green;
        if (currentState == PSTATE.ATTACK)
            stateColor = Color.red;
        if (currentState == PSTATE.HURT || invul == true)
            stateColor = Color.cyan;
        if (currentState == PSTATE.REST)
            stateColor = Color.blue;
        Gizmos.color = stateColor;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 4, 60), 5);
    }
}
