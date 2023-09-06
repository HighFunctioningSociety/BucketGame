using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsGround; // A mask determining what is ground to the character
    [HideInInspector] public bool wasGrounded;
    [HideInInspector] public bool wasJumping;
    [HideInInspector] public PlayerContainer player;
    [HideInInspector] public Collider2D boxCollider;
    public bool isOnSlope;
    public bool canWalkOnSlope;
    public float DirectionFaced = 1;
    public float BoxcastHeight = 0.4f;
    public bool KillHorizontalInput;
    public bool _grounded;            // Whether or not the player is grounded.
    private bool _falling;               // Controls if the animation is falling is playing. True if rb.velocity.y < 0
    private bool _launched;
    private Rigidbody2D _rb;
    private Vector3 _velocity = Vector3.zero;

    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

    public float slopeDownAngle;
    public float slopeSideAngle;
    public float lastSlopeAngle;

    private Vector2 slopeNormalPerp;
    bool _xInputIsZero = false;

    [HideInInspector]
    public float MoveSpeed; // pass value to animatePlayer

    public delegate void OnLand();
    public delegate void OnFall();

    public OnLand LandEvent;
    public OnFall FallEvent;

    private void Awake()
    {
        player = GetComponent<PlayerContainer>();
        boxCollider = GetComponent<BoxCollider2D>();
        _rb = player.rb;
    }

    public void PlayerControllerLoop()
    {
        RaycastHit2D raycastHit = DrawBoxcast();
        SlopeCheck(raycastHit);
        IsGrounded(raycastHit);
        IsFalling();
        DrawDebugRays(raycastHit);
    }
    
    public bool GetGrounded()
    {
        return _grounded;
    }

    public bool GetFalling()
    {
        return _falling;
    }

    public void IsGrounded(RaycastHit2D raycastHit)
    {
        wasGrounded = _grounded;
        _grounded = false;

        if (raycastHit.collider != null && (_rb.velocity.y < 0.01f || isOnSlope))
        {
            SetGrounded();
        }
    }

    private void SetGrounded()
    {
        wasJumping = false;
        _grounded = true;
        _falling = false;
        player.RefreshMovement();
        if (!wasGrounded)
        {
            LandEvent.Invoke();

            if (isOnSlope)
            {
                _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
            }
        }
    }

    private void SlopeCheck(RaycastHit2D raycastHit)
    {
        SlopeCheckHorizontal();
        SlopeCheckVertical(raycastHit);
    }

    private void SlopeCheckHorizontal()
    {
        Vector3 colliderOrigin = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y + BoxcastHeight, boxCollider.bounds.center.z);
        RaycastHit2D slopeHitFront = Physics2D.Raycast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) + new Vector3(boxCollider.bounds.extents.x, 0), transform.right, slopeCheckDistance, _whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) - new Vector3(boxCollider.bounds.extents.x, 0), -transform.right, slopeCheckDistance, _whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(RaycastHit2D raycastHit)
    {
        if (raycastHit)
        {
            slopeNormalPerp = Vector2.Perpendicular(raycastHit.normal).normalized;

            slopeDownAngle = Vector2.Angle(raycastHit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && _xInputIsZero)
        {
            _rb.sharedMaterial = fullFriction;
        }
        else
        {
            _rb.sharedMaterial = noFriction;
        }
    }

    private RaycastHit2D DrawBoxcast()
    {
        Vector3 colliderOrigin = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y + BoxcastHeight, boxCollider.bounds.center.z);
        Vector3 boxcastSize = new Vector3(boxCollider.bounds.size.x, BoxcastHeight, boxCollider.bounds.size.z);
        return Physics2D.BoxCast(colliderOrigin, boxcastSize, 0f, Vector2.down, BoxcastHeight, _whatIsGround);
    }

    private void DrawDebugRays(RaycastHit2D raycastHit)
    {
        Color rayColor;
        if (raycastHit.collider != null) 
            rayColor = Color.green;
        else 
            rayColor = Color.red;

        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (BoxcastHeight), rayColor);
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (BoxcastHeight), rayColor);

        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y), slopeNormalPerp, Color.magenta);
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y), raycastHit.normal, Color.cyan);

        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) - new Vector3(boxCollider.bounds.extents.x, 0), Vector3.left * slopeCheckDistance, Color.white);
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) + new Vector3(boxCollider.bounds.extents.x, 0), Vector3.right * slopeCheckDistance, Color.white);

    }

    private void IsFalling()
    {
        if (_rb.velocity.y < 0.01 && !_grounded)
        {
            _falling = true;
            FallEvent.Invoke();
        }
    }

    public void Move(float xInput)
    {
        player.rb.gravityScale = player.playerStats.defaultGravity;
        MoveSpeed = xInput * player.playerStats.curSpeed * 10f;
        float movementSmoothing = player.playerStats.movementSmoothing;

        if (Inputs.Horizontal != 0)
        {
            DirectionFaced = Inputs.Horizontal;
            _xInputIsZero = false;
        }
        else
        {
            _xInputIsZero = true;
        }

        // And then smoothing it out and applying it to the character
        if (!KillHorizontalInput)
        {
            float targetXVelocity;
            float targetYVelocity;

            if (isOnSlope && canWalkOnSlope)
            {
                targetXVelocity = MoveSpeed * -slopeNormalPerp.x;
                targetYVelocity = MoveSpeed * -slopeNormalPerp.y;
            }
            else
            {
                targetXVelocity = MoveSpeed;
                targetYVelocity = _rb.velocity.y;
            }

            Vector3 targetVelocity = new Vector2(targetXVelocity, targetYVelocity);

            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, movementSmoothing);
        }
    }

    public void Jump(bool jump, bool jumpHeld)
    {
        float jumpforce = player.playerStats.jumpForce;
        float shorthopSubtract = player.playerStats.shortHopSubtraction;

        if (_grounded && jumpHeld && jump && slopeDownAngle <= maxSlopeAngle)
        {
            // Add a vertical force to the player.
            _grounded = false;
            _launched = false;
            _falling = false;
            wasJumping = true;
            _rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }

        // peak of jump achieved, add a gravity multiplier to falling
        if (_rb.velocity.y <= 0 && _grounded == false && player.dashAbility.dashTime <= 0)
        {
            player.rb.gravityScale = player.playerStats.defaultGravity * 1.3f;
            _launched = false;
        }

        // jump button released before the peak of the jump is achieved, immediately start descent 
        else if (_rb.velocity.y > 0.1 && (!jumpHeld || !wasJumping) && _grounded == false && _launched == false)
        {
            _rb.AddForce(Vector2.up * -shorthopSubtract, ForceMode2D.Impulse);
        }

        // used when an external object launches the player character
        if (_launched)
        {
            player.rb.gravityScale = player.playerStats.defaultGravity * 1.3f;
        }
    }

    public void LaunchPlayer(float launchForce)
    {
        player.PlayerAbilityController.CancelAbilities();
        player.PlayerAnimationController.SetAnimationToIdle();
        player.coolDownManager.SetNextCoolDown(0.3f);
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(new Vector2(0, launchForce), ForceMode2D.Impulse);
        _falling = false;
        wasJumping = true;
        _grounded = false;
        _launched = true;
    }
}

