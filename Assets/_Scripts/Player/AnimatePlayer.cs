using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlayer : MonoBehaviour
{
    public ParticleSystem dust;
    public CoolDownManager coolDown;
    private Animator animator;
    private CharacterController2D controller;
    private PlayerContainer player;
    private DashAbility dashAbility;
    private bool wasDashing;
    private bool facingRight = true;
    public int speed;
    private float directionToUse;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
        player = GetComponent<PlayerContainer>();
        dashAbility = player.dashAbility;
    }

    private void Update()
    {
        if (CheckIfIdleOrWalking())
        {
            InNeutralState();
        }

        if (SpriteIsFlippable())
        {
            SpriteFlipManager();
        }
        animator.SetFloat("VelocityY", player.rb.velocity.y);
        DashAnimationManager();
        JumpAnimationManager();
        HurtAnimationManager();
        RelinquishedSpeedManager();
    }

    #region Functions

    private bool SpriteIsFlippable()
    {
        return ((player.coolDownManager.coolDownComplete || 
            CheckIfIdleOrWalking()) 
            && player.currentState != PlayerContainer.PSTATE.HURT 
            && !player.dashAbility.stinging 
            && player.currentControlType != PlayerContainer.CONTROLSTATE.RELINQUISHED);
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
        animator.SetTrigger("JumpCancelTrigger");
    }

    private void JumpAnimationManager()
    {
        if (controller.GetGrounded() == false && controller.GetFalling() == false)
        {
            animator.SetBool("IsJumping", true);
            if (controller.wasGrounded)
                CreateDust();
        }

        animator.SetBool("WasJumpCanceled", player.abilityManager.wasJumpCanceled);
        animator.SetBool("WasJumping", controller.wasJumping);
        animator.SetBool("IsGrounded", controller.GetGrounded());
        animator.SetBool("WasGrounded", controller.wasGrounded);
    }

    private void DashAnimationManager()
    {
        CheckForDashDirection();

        if (dashAbility.dashTime > 0 && wasDashing == false && player.currentState != PlayerContainer.PSTATE.HURT)
        {
            Dash();
        }
        else if (dashAbility.dashTime <= 0 && dashAbility.stinging == false)
        {
            wasDashing = false;
        }

        animator.SetFloat("DashTimer", dashAbility.dashTime);
        animator.SetBool("Stinging", dashAbility.stinging);
    }

    private void RelinquishedSpeedManager()
    {
        if (player.currentControlType != PlayerContainer.CONTROLSTATE.RELINQUISHED && !PauseMenu.GamePaused)
        {
            animator.SetFloat("Speed", Mathf.Abs(Inputs.Horizontal));
        }
        else if (player.currentControlType == PlayerContainer.CONTROLSTATE.RELINQUISHED && !PauseMenu.GamePaused)
        {
            if (!player.transitioningLeft && !player.transitioningRight)
            {
                animator.SetFloat("Speed", 0);
            }
            else
            {
                animator.SetFloat("Speed", 1);
            }
        }
    }

    private void HurtAnimationManager()
    {
        if (player.currentState == PlayerContainer.PSTATE.HURT)
        {
            Hurt();
        }
        else
        {
            animator.SetBool("Hurt", false);
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
            directionToUse = player.controller.dir;
        }
    }

    public void InNeutralState()
    {
        ResetAnimationTriggers();
        player.abilityManager._StopStinging();
        coolDown.coolDownFlag = true;
    }

    public void Dash()
    {
        wasDashing = true;
        animator.SetTrigger("Dash");
        CreateDust();
    }

    public void Hurt()
    {
        ResetAnimationTriggers();
        animator.SetFloat("Speed", 0);
        animator.SetBool("Hurt", true);
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", false);
        animator.SetTrigger("Landing");
    }

    public void OnFalling()
    {
        animator.SetBool("IsJumping", false);
        animator.SetBool("IsFalling", true);
    }

    public void CreateDust()
    {
        dust.Play();
    }

    public bool CheckIfIdleOrWalking()
    {
        bool playingIdle = animator.GetCurrentAnimatorStateInfo(0).IsName("player_idle");
        bool playingWalk = animator.GetCurrentAnimatorStateInfo(0).IsName("player_walk");
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
        animator.ResetTrigger("Slash_1");
        animator.ResetTrigger("Slash_2");
        animator.ResetTrigger("Slash_3");
        animator.ResetTrigger("SlashUp");
        animator.ResetTrigger("Dash");
        animator.ResetTrigger("Vault");
        animator.ResetTrigger("StingerEnd");
        animator.ResetTrigger("StingerSpecialEnd");
        animator.ResetTrigger("HelmSplitterStart");
        animator.ResetTrigger("Plunge");
        animator.ResetTrigger("PlungeEnd");
        animator.ResetTrigger("PlungeLand");
        animator.ResetTrigger("Shoot");
        animator.ResetTrigger("Landing");
        animator.ResetTrigger("JumpCancelTrigger");
    }
    #endregion
}
