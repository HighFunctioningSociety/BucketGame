using System.Collections;
using UnityEngine;

public class PlayerContainer : MonoBehaviour
{
    public static PlayerContainer _playerContainer;
    public PlayerStats playerStats;

    public PSTATE CurrentState;
    public CONTROLSTATE CurrentControlType;
    public bool invul;

    public AbilityController PlayerAbilityController;
    public EquipmentManager equipmentManager;
    public CoolDownManager coolDownManager;
    public DashAbility dashAbility;
    public PlayerAnimator PlayerAnimationController;
    public Rigidbody2D rb;
    public CharacterController2D PlayerController;
    public PlayerRenderer pr;
    public GameObject damageEffect;
    public float JumpCancelMonitor;
    public float GameTime;
    public bool JumpCancelBoolMonitor;
    private Color stateColor = Color.grey;


    public enum PSTATE
    {
        NORMAL,
        DASH,
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
        //LoadGame();
        playerStats.Init();
        UpdateHealthUI();

        if (dashAbility == null)
        {
            Debug.LogError("dashAbility not plugged in");
            dashAbility = GetComponent<DashAbility>();
        }

        if (PlayerAbilityController == null)
        {
            Debug.LogError("abilityManager not plugged in");
            PlayerAbilityController = GetComponent<AbilityController>();
        }

        invul = false;
    }

    private void FixedUpdate()
    {
        PlayerUpdateLoop();
    }

    /// <summary>
    /// The singular update loop used for the player character
    /// </summary>
    public void PlayerUpdateLoop()
    {
        // Handles all of the players collision
        PlayerController.PlayerControllerLoop();

        // Handles all of the players animation, in desperate need of refactoring
        PlayerAnimationController.PlayerAnimationLoop();

        // Handles all attacks and abilities
        PlayerAbilityController.AbilityUpdateLoop();

        // Handles player input
        PlayerStateLoop();

        // Handles dashing, needs to be refactored to not be stupid
        dashAbility.DashLoop();
    }

    public void PlayerStateLoop()
    {
        switch (CurrentControlType)
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
        switch (CurrentState)
        {
            case PSTATE.NORMAL:
                PStateNormal();
                break;

            case PSTATE.DASH:
                PStateDash();
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
        Inputs.DisableInput();  
    }

    private void PStateNormal()
    {
        float horizontalMove = Inputs.Horizontal;
        bool jump = Inputs.jump;
        bool jumpHeld = Inputs.jumpHeld;

        if (Inputs.dash == true)
        {
            CurrentState = PSTATE.DASH;
            dashAbility.StartDashing(Inputs.dash);
        }

        PlayerController.Move(horizontalMove * Time.fixedDeltaTime);
        PlayerController.Jump(jump, jumpHeld);

        Inputs.jump = false;
    }

    private void PStateDash()
    {
        bool jump = Inputs.jump;
        bool jumpHeld = Inputs.jumpHeld;
     
        if (dashAbility.dashTime < dashAbility.startDashTime * 0.4f && Inputs.jump && PlayerController.GetGrounded())
        {
            CurrentState = PSTATE.NORMAL;
            PlayerAbilityController.UnfreezeConstraints();
            dashAbility.dashTime = 0;
        }
        else if (dashAbility.dashTime > 0)
        {
            jump = false;
            jumpHeld = false;
        }
        else if (dashAbility.dashTime <= 0)
        {
            CurrentState = PSTATE.NORMAL;
        }

        PlayerController.Jump(jump, jumpHeld);
        Inputs.jump = false;
    }


    private void PStateAttack()
    {
        if (Inputs.jump && PlayerController.GetGrounded() && JumpCancelTime() && !PlayerAbilityController.wasJumpCanceled && PlayerAbilityController.canJumpCancelAttack)
        {
            Inputs.SetAttackInputBufferTime(0.15f);
            PlayerAbilityController._TurnOffMovementModifier();
            PlayerAbilityController._ActivateJumpCancel();
            coolDownManager.ResetCoolDown();
            PlayerAnimationController.JumpCancelTrigger();
            Inputs.DisableAttack();
            PlayerController.Move(Inputs.Horizontal * Time.fixedDeltaTime);
            PlayerController.Jump(Inputs.jump, Inputs.jumpHeld);
        } 
        else if (!PlayerAbilityController.freezeMovement)
        {
            UnfrozenJumpCancelManager();
        }
        else if (PlayerAbilityController.freezeMovement)
        {
            FrozenJumpCancelManager();
        }
        else
        {
            PlayerController.Move(Inputs.Horizontal * Time.fixedDeltaTime);
        }
    }

    private bool JumpCancelTime()
    {
        if (PlayerAbilityController.currentMeleeAttack == null)
            return false;
        float jumpCancelBuffer = 0.15f;
        return Time.time > ((coolDownManager.nextReadyTime - PlayerAbilityController.currentMeleeAttack.ability.aBaseCoolDown) + jumpCancelBuffer);
    }

    private void FrozenJumpCancelManager()
    {
        float horizontalMove = 0;
        bool jumpHeld = false;
        bool jump = false;

        if (PlayerAbilityController.wasJumpCanceled)
        {
            jump = Inputs.jump;
            jumpHeld = Inputs.jumpHeld;
        }

        if (!PlayerController.GetGrounded())
        {
            horizontalMove = Inputs.Horizontal;
        }

        PlayerController.Move(horizontalMove * Time.fixedDeltaTime);
        PlayerController.Jump(jump, jumpHeld);
    }

    private void UnfrozenJumpCancelManager()
    {
        float horizontalMove = Inputs.Horizontal;
        bool jumpHeld = false;
        bool jump = false;

        if (PlayerAbilityController.slowMovement)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y / playerStats.curMovementDivider, 0);
            horizontalMove /= 8;
            jump = false;
            jumpHeld = false;
        }

        if (PlayerAbilityController.wasJumpCanceled)
        {
            jump = Inputs.jump; 
            jumpHeld = Inputs.jumpHeld;
        }

        PlayerController.Move(horizontalMove * Time.fixedDeltaTime);
        PlayerController.Jump(jump, jumpHeld);
    }


    private void PStateHurt()
    {
        PlayerAbilityController.UnfreezeConstraints();
        PlayerAbilityController.CancelAbilities();
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
            CurrentState = PSTATE.HURT;
            SubtractFromHealth(_damage);
            PlayerAbilityController.CancelAbilities();
            StartCoroutine(PlayDamageEffect(0.025f));
            StartCoroutine(Invulnerability());
        }

        // If player runs out of health call the GameMaster to kill the player object
        if (playerStats.curHealth <= 0)
        {
            CurrentState = PSTATE.RESPAWNING;
            PlayerAbilityController.CancelAbilities();
            _GameManager.KillPlayer();
        }
    }

    public void _KnockBack(float _knockBackX, float _knockBackY, Vector2 _enemyPosition)
    {
        if (invul != true)
        {
            PlayerAbilityController.CancelAbilities();
            PlayerAbilityController.UnfreezeConstraints();

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
            CurrentState = PSTATE.NORMAL;
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
        CurrentState = PSTATE.NORMAL;
    }

    public void KillHorizontalInput()
    {
        PlayerController.KillHorizontalInput = true;
    }

    public void ActivateHorizontalInput()
    {
        PlayerController.KillHorizontalInput = false;
    }

    public static void RelinquishControl()
    {
        _playerContainer._RelinquishControl();
    }

    private void _RelinquishControl()
    {
        CurrentControlType = CONTROLSTATE.RELINQUISHED;
    }

    public static void EnableControl()
    {
        _playerContainer._EnableControl();
    }

    private void _EnableControl()
    {
        CurrentControlType = CONTROLSTATE.ACCEPT_INPUT;
    }

    public void SetDrag(float dragVal)
    {
        rb.drag = dragVal;
    }

    private void OnDrawGizmos()
    {
        switch (CurrentState)
        {
            case PSTATE.NORMAL:
                stateColor = Color.green;
                break;
            case PSTATE.DASH:
                stateColor = Color.magenta;
                break;
            case PSTATE.ATTACK:
                stateColor = Color.red;
                break;
            case PSTATE.HURT:
                stateColor = Color.cyan;
                break;
            case PSTATE.REST:
                stateColor = Color.blue;
                break;
        }

        Gizmos.color = stateColor;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + 4, 60), 5);
    }
}
