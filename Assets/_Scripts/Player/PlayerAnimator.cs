using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public ParticleSystem dust;
    public CoolDownManager coolDown;
    public Animator Animator;
    private CharacterController2D controller;
    private PlayerContainer player;
    private DashAbility dashAbility;
    public int speed;
    private float directionToUse;
    private bool wasDashing;
    private bool facingRight = true;
    private bool _forceLeft;
    private bool _forceRight;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        player = GetComponent<PlayerContainer>();
        dashAbility = player.dashAbility;

        player.PlayerController.LandEvent += OnLanding;
        player.PlayerController.FallEvent += OnFalling;
    }

    public void PlayerAnimationLoop()
    {
        if (CheckIfIdleOrWalking())
        {
            InNeutralState();
        }

        if (SpriteIsFlippable())
        {
            SpriteFlipManager();
        }

        Animator.SetFloat("VelocityY", player.rb.velocity.y);
        DashAnimationManager();
        JumpAnimationManager();
        HurtAnimationManager();
        SetSpeed();
        ForceMovement();
    }

    #region Functions

    private bool SpriteIsFlippable()
    {
        return ((player.coolDownManager.coolDownComplete || 
            CheckIfIdleOrWalking()) 
            && player.CurrentState != PlayerContainer.PSTATE.HURT 
            && !player.dashAbility.stinging 
            && player.CurrentControlType != PlayerContainer.CONTROLSTATE.RELINQUISHED);
    }

    private void ForceMovement()
    {
        if (_forceLeft)
        {
            controller.Move(0.7f * Time.fixedDeltaTime);
        }
        else if (_forceRight)
        {
            controller.Move(-0.7f * Time.fixedDeltaTime);
        }
    }

    public void SetForcedRightMovement(bool value)
    {
        _forceLeft = value;
    }

    public void SetForcedLeftMovement(bool value)
    {
        _forceRight = value;
    }

    private void SpriteFlipManager()
    {
        if (directionToUse > 0 && !facingRight)
        {
            Flip();
            if (controller.wasGrounded)
                CreateDust();
        }
        else if (directionToUse < 0 && facingRight)
        {
            Flip();
            if (controller.wasGrounded)
                CreateDust();
        }
    }

    public void JumpCancelTrigger()
    {
        Animator.SetTrigger("JumpCancelTrigger");
    }

    private void JumpAnimationManager()
    {
        if (controller.GetGrounded() == false && controller.GetFalling() == false)
        {
            Animator.SetBool("IsJumping", true);
            if (controller.wasGrounded)
                CreateDust();
        }

        Animator.SetBool("WasJumpCanceled", player.PlayerAbilityController.wasJumpCanceled);
        Animator.SetBool("WasJumping", controller.wasJumping);
        Animator.SetBool("IsGrounded", controller.GetGrounded());
        Animator.SetBool("WasGrounded", controller.wasGrounded);
    }

    private void DashAnimationManager()
    {
        CheckForDashDirection();

        if (dashAbility.dashTime > 0 && wasDashing == false && player.CurrentState != PlayerContainer.PSTATE.HURT)
        {
            Dash();
        }
        else if (dashAbility.dashTime <= 0 && dashAbility.stinging == false)
        {
            wasDashing = false;
        }

        Animator.SetFloat("DashTimer", dashAbility.dashTime);
        Animator.SetBool("Stinging", dashAbility.stinging);
    }

    private void SetSpeed()
    {
        if (player.CurrentControlType != PlayerContainer.CONTROLSTATE.RELINQUISHED && !PauseMenu.GamePaused)
        {
            Animator.SetFloat("Speed", Mathf.Abs(Inputs.Horizontal));
        }
        else if (player.CurrentControlType == PlayerContainer.CONTROLSTATE.RELINQUISHED && !PauseMenu.GamePaused)
        {
            bool forcedMovement = player.PlayerAnimationController._forceLeft || player.PlayerAnimationController._forceRight;
            Animator.SetFloat("Speed", forcedMovement ? 1 : 0);
        }
    }

    private void HurtAnimationManager()
    {
        if (player.CurrentState == PlayerContainer.PSTATE.HURT)
        {
            Hurt();
        }
        else
        {
            Animator.SetBool("Hurt", false);
        }
    }

    private void CheckForDashDirection()
    {
        if (dashAbility.dashTime > 0)
        {
            directionToUse = dashAbility.direction;
        }
        else
        {
            directionToUse = player.PlayerController.DirectionFaced;
        }
    }

    public void InNeutralState()
    {
        ResetAnimationTriggers();
        player.PlayerAbilityController._StopStinging();
        coolDown.coolDownFlag = true;
    }

    public void Dash()
    {
        wasDashing = true;
        Animator.SetTrigger("Dash");
        CreateDust();
    }

    public void Hurt()
    {
        ResetAnimationTriggers();
        Animator.SetFloat("Speed", 0);
        Animator.SetBool("Hurt", true);
    }

    public void OnLanding()
    {
        Animator.SetBool("IsJumping", false);
        Animator.SetBool("IsFalling", false);
        Animator.SetTrigger("Landing");
    }

    public void OnFalling()
    {
        Animator.SetBool("IsJumping", false);
        Animator.SetBool("IsFalling", true); //Might be able to remove this
    }

    public void SetAnimationToIdle()
    {
        Animator.Play("player_idle");
    }

    public void CreateDust()
    {
        dust.Play();
    }

    public bool CheckIfIdleOrWalking()
    {
        bool playingIdle = Animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle");
        bool playingWalk = Animator.GetCurrentAnimatorStateInfo(0).IsName("player_walk");
        return playingIdle || playingWalk;
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ResetAnimationTriggers()
    {
        Animator.ResetTrigger("Slash_1");
        Animator.ResetTrigger("Slash_2");
        Animator.ResetTrigger("Slash_3");
        Animator.ResetTrigger("SlashUp");
        Animator.ResetTrigger("Dash");
        Animator.ResetTrigger("Vault");
        Animator.ResetTrigger("StingerEnd");
        Animator.ResetTrigger("StingerSpecialEnd");
        Animator.ResetTrigger("HelmSplitterStart");
        Animator.ResetTrigger("Plunge");
        Animator.ResetTrigger("PlungeEnd");
        Animator.ResetTrigger("PlungeLand");
        Animator.ResetTrigger("Shoot");
        Animator.ResetTrigger("Landing");
        Animator.ResetTrigger("JumpCancelTrigger");
    }
    #endregion
}
