using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityController : MonoBehaviour
{

    [Header ("Ability List")]
    [SerializeField] private SingleDrawTriggerable slash_1;
    [SerializeField] private SingleDrawTriggerable slash_2;
    [SerializeField] private SingleDrawTriggerable slash_3;
    [SerializeField] private UpSlashTriggerable slashUp;
    [SerializeField] private SingleDrawTriggerable stinger;
    [SerializeField] public SingleDrawTriggerable stingerSpecial;
    [SerializeField] private HelmSplitterTriggerable helmSplitter;
    [SerializeField] private PlungingMeleeTriggerable plunge;
    [SerializeField] private ProjectileAttackTriggerable spiritBall;

    [Space]
    [Header("Attack Modifiers")]
    public bool slowMovement;
    public bool resetCoolDownOnLanding;
    public bool mustCoolDown = false;
    public bool canJumpCancelAttack = false;
    public bool wasJumpCanceled = false;
    public bool freezeMovement = false;

    [Space]
    [Header("Combo Variables")]
    public float comboTime = 0.6f;
    public float comboTimeLeft = 0f;
    public int comboState = 0;

    [Space]
    public EquipmentTriggerable currentEquipmentAbility;

    private PlayerContainer player;
    private SpiritManager spiritManager;
    private PlayerAnimator animatePlayer;
    private DashAbility dashAbility;
    private MeleeAttackTriggerable stingerAttack;
    private CoolDownManager coolDown;
    private EquipmentManager equipmentManager;
    private Animator animator;
    [HideInInspector] public MeleeAttackTriggerable currentMeleeAttack;
    [HideInInspector] public ProjectileAttackTriggerable currentProjectileAttack;

    private void Start()
    {
        coolDown = GetComponent<CoolDownManager>();
        equipmentManager = GetComponent<EquipmentManager>();
        player = GetComponent<PlayerContainer>();
        spiritManager = GetComponent<SpiritManager>();
        dashAbility = player.dashAbility;
        stingerAttack = stinger.GetComponent<MeleeAttackTriggerable>();
        animatePlayer = GetComponent<PlayerAnimator>();
        animator = GetComponent<Animator>();

        player.PlayerController.LandEvent += OnLanding;
    }

    public void AbilityUpdateLoop()
    {
        if (animatePlayer.CheckIfIdleOrWalking()) freezeMovement = false;
        if (CanReturnToNormalState())
        {
            ResetMovementBools();
            UnfreezeConstraints();

            if (player.CurrentState == PlayerContainer.PSTATE.ATTACK)
            {
                player.CurrentState = PlayerContainer.PSTATE.NORMAL;
            }
            if (dashAbility.dashTime <= 0)
            {
                SpiritBall();
                GetAttack();
                GetEquipmentInput();
                if (!player.PlayerController.GetGrounded() && coolDown.coolDownComplete)
                {
                    CallHelmSplitter();
                }
            }
            else
            {
                CallStinger();
            }
        }
        ComboTimer();
    }

    #region Functions

    private bool CanReturnToNormalState()
    {
        return coolDown.coolDownComplete && !PauseMenu.GamePaused && player.CurrentControlType == PlayerContainer.CONTROLSTATE.ACCEPT_INPUT;
    }

    private void ResetMovementBools()
    {
        slowMovement = false;
        freezeMovement = false;
        mustCoolDown = false;
        resetCoolDownOnLanding = false;
        canJumpCancelAttack = false; 
        if (player.PlayerController.GetGrounded())
        {
            wasJumpCanceled = false;
        }
    }

    private void ComboTimer()
    {
        comboTimeLeft -= Time.fixedDeltaTime;
        comboTimeLeft = Mathf.Clamp(comboTimeLeft, 0, comboTime);

        if (comboTimeLeft <= 0)
            comboState = 0;
    }

    public void FreezeMovement(bool freeze = true)
    {
        freezeMovement = freeze;
    }

    public void _ActivateJumpCancel()
    {
        wasJumpCanceled = true;
        resetCoolDownOnLanding = false;
    }

    public void _HurtboxEvent()
    {
        currentMeleeAttack.DrawHurtBox();
    }

    public void _HurtboxEnable()
    {
        currentMeleeAttack.EnableCollider();
    }

    public void _HurtBoxDisable()
    {
        currentMeleeAttack.DisableCollider();
    }

    public void _SpawnProjectileEvent()
    {
        currentProjectileAttack.SpawnProjectileEvent();
    }

    public void _StartHelmSplitter()
    {
        helmSplitter.helmSplitterActive = true;
    }

    public void _StopStinging()
    {
        dashAbility.stinging = false;
    }

    public void _TurnOffMovementModifier()
    {
        slowMovement = false;
    }

    private void UseMeleeAttack(MeleeAttackTriggerable abilityObject)
    {
        if(player.CurrentState == PlayerContainer.PSTATE.HURT)
        {
            return;
        }

        player.PlayerController.wasJumping = false;
        coolDown.SetNextCoolDown(abilityObject.ability.aBaseCoolDown);
        player.CurrentState = PlayerContainer.PSTATE.ATTACK;
        currentMeleeAttack = abilityObject;
        comboState += 1 % 3;
        player.playerStats.curMovementDivider--;
        abilityObject.ability.TriggerAbility();
    }

    private void UseProjectileAttack(ProjectileAttackTriggerable abilityObject)
    {
        if (player.CurrentState == PlayerContainer.PSTATE.HURT)
        {
            return;
        }

        coolDown.SetNextCoolDown(abilityObject.ability.aBaseCoolDown);
        player.CurrentState = PlayerContainer.PSTATE.ATTACK;
        currentProjectileAttack = abilityObject;
        abilityObject.ability.TriggerAbility();
    }

    private void UseEquipment(EquipmentTriggerable equipmentObject)
    {
        equipmentManager.SetNextReadyTime(equipmentObject.equipment.cooldown);
        equipmentObject.TriggerEquipmentAbility();
    }
    private void UseEquipmentSecondary(EquipmentTriggerable equipmentObject)
    {
        equipmentManager.SetSecondaryNextReadyTime(equipmentObject.equipment.secondaryCooldown);
        equipmentObject.TriggerSecondaryFunction();
    }

    private void GetAttack()
    {
        // First tier combo attack, comboState = 0
        if (comboState == 0)
        {
            if (Inputs.attack && !(Inputs.Vertical > 0) && !(Inputs.Vertical < 0))
            {
                UseMeleeAttack(slash_1);
                comboTimeLeft = comboTime;
                slowMovement = true;
                if (player.PlayerController.GetGrounded())
                    canJumpCancelAttack = true;
            }
        }

        // Second tier combo attack, comboState = 1
        else if (comboState == 1)
        {
            if (Inputs.attack && !(Inputs.Vertical > 0) && !(Inputs.Vertical < 0))
            {
                UseMeleeAttack(slash_2); 
                comboTimeLeft = comboTime;
                slowMovement = true;
                if (player.PlayerController.GetGrounded())
                    canJumpCancelAttack = true;
            }
        }

        // Combo finisher, comboState = 2
        else if (comboState == 2)
        {
            if (Inputs.attack && !(Inputs.Vertical > 0) && !(Inputs.Vertical < 0))
            {
                UseMeleeAttack(slash_3);
                slowMovement = true;
                if (player.PlayerController.GetGrounded())
                    canJumpCancelAttack = true;
            }
        }

        if (Inputs.attack && (Inputs.Vertical < 0) && !player.PlayerController.GetGrounded())
        {
            UseMeleeAttack(plunge);
        }

        // Up attack, doesn't effect comboState
        if (Inputs.attack && (Inputs.Vertical > 0))
        {
            UseMeleeAttack(slashUp);
            freezeMovement = true;
            if (player.PlayerController.GetGrounded())
                canJumpCancelAttack = true;
            
            if (!player.PlayerController.GetGrounded())
                resetCoolDownOnLanding = true;
        }
    }

    private void GetEquipmentInput()
    {
        if (Inputs.equipment && currentEquipmentAbility != null)
        {
            if (equipmentManager.equipmentCoolDownComplete)
            {
                UseEquipment(currentEquipmentAbility);
                Inputs.equipment = false;
            }
            else if (equipmentManager.secondaryCoolDownComplete)
                UseEquipmentSecondary(currentEquipmentAbility);
        }
    }

    private void SpiritBall()
    {
        if (Inputs.specialAttack && !(Inputs.Vertical > 0) && !(Inputs.Vertical < 0) && spiritManager.currentSpirit >= 1)
        {
            spiritManager.SubtractSpirit(1);
            slowMovement = false;
            UseProjectileAttack(spiritBall);
        }
    }

    private void CallStinger()
    {
        if (Inputs.specialAttack && !(Inputs.Vertical < 0) && dashAbility.coolDown == dashAbility.dashCoolDown)
        {   
            if (spiritManager.currentSpirit >= 1)
            {
                coolDown.CoolDownFlagStart();
                spiritManager.SubtractSpirit(1);
                dashAbility.dashTime += 0.4f;
                dashAbility.startStingerSpecial = true;
                dashAbility.stinging = true;
            }
        }
        else if (Inputs.attack && dashAbility.coolDown == dashAbility.dashCoolDown)
        {
            coolDown.CoolDownFlagStart();
            dashAbility.dashTime += 0.2f;
            dashAbility.startStinger = true;
            dashAbility.stinging = true;
        }
    }

    public void ExecuteStinger()
    {
        dashAbility.enemyFound = false;
        dashAbility.startStinger = false;
        FreezeConstraints();
        UseMeleeAttack(stinger);
    }

    public void Vault()
    {
        if (Inputs.attackHeld && stingerAttack.enNum != 0)
        {
            UnfreezeConstraints();
            coolDown.ResetCoolDown();
            animator.SetTrigger("Vault");
            player.rb.AddForce(Vector2.down * 12, ForceMode2D.Impulse);
            player.rb.AddForce(new Vector2(70 * player.PlayerController.DirectionFaced, 170), ForceMode2D.Impulse);
        }
    }

    private IEnumerator VaultGravity()
    {
        player.rb.gravityScale = player.playerStats.defaultGravity * 1.5f;
        yield return new WaitForSeconds(0.5f);
        player.rb.gravityScale = player.playerStats.defaultGravity;
    }

    public void ExecuteStingerSpecial()
    {
        dashAbility.enemyFound = false;
        dashAbility.startStingerSpecial = false;
        FreezeConstraints();
        UseMeleeAttack(stingerSpecial);
    }


    private void CallHelmSplitter()
    {
        if (Inputs.specialAttack && Inputs.Vertical < 0)
        {
            if (spiritManager.currentSpirit >= 1)
            {
                player.coolDownManager.CoolDownFlagStart();
                spiritManager.SubtractSpirit(1);
                FreezeConstraints();
                UseMeleeAttack(helmSplitter);
            }
        }
    }

    public void CancelAbilities()
    {
        coolDown.ResetCoolDown();

        player.ActivateHorizontalInput();
        player.dashAbility.RefreshDash();

        helmSplitter.DisableCollider();
        helmSplitter.helmSplitterActive = false;
        helmSplitter.trailRenderer.emitting = false;

        comboTimeLeft = 0;
        comboState = 0;

        UnfreezeConstraints();
    }

    public void OnLanding()
    {
        if (resetCoolDownOnLanding)
        {
            resetCoolDownOnLanding = false;
            coolDown.ResetCoolDown();
            Inputs.SetAttackInputBufferTime(0.1f);
        }
    }

    public void _PlayAttackAudioEvent()
    {
        currentMeleeAttack.audioSource.Play();
    }

    public void UnfreezeXConstraint()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    public void UnfreezeYConstraint()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    public void UnfreezeConstraints()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void FreezeConstraints()
    {
        player.rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    #endregion
}

