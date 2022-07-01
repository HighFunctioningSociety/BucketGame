using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
    [HideInInspector] public bool wasGrounded;
    [HideInInspector] public bool wasJumping;
    [HideInInspector] public PlayerContainer player;
    [HideInInspector] public Collider2D boxCollider;
    public float dir = 1;
    public float extraHeight;
    public bool killHorizontalInput;
    public bool grounded;            // Whether or not the player is grounded.
    public bool falling;               // Controls if the animation is falling is playing. True if rb.velocity.y < 0
    public bool isOnSlope;
    private Rigidbody2D _rb;
    private Vector3 _velocity = Vector3.zero;

    private Vector2 _slopeNormalPerp;
    [SerializeField]
    private float _slopeDownAngle;

    private float _activeGravityScale;




    [HideInInspector]
    public float _move; // pass value to animatePlayer

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;
    public UnityEvent OnFallingEvent;

    private void Awake()
    {
        player = GetComponent<PlayerContainer>();
        boxCollider = GetComponent<BoxCollider2D>();
        _rb = player.rb;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnFallingEvent == null)
            OnFallingEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        RaycastHit2D raycastHit = DrawBoxCast();
        IsGrounded(raycastHit);
        CheckSlope(raycastHit);
        IsFalling();
    }
    
    public bool GetGrounded()
    {
        return grounded;
    }

    public bool GetFalling()
    {
        return falling;
    }

    public void IsGrounded(RaycastHit2D raycastHit)
    {
        wasGrounded = grounded;
        grounded = false;
        isOnSlope = false;

        if (raycastHit.collider != null)// && (_rb.velocity.y < 0.01f))
        {
            SetGrounded();
        }

        DrawDebugRayLines(raycastHit);
    }

    private void SetGrounded()
    {
        wasJumping = false;
        grounded = true;
        falling = false;
        player.RefreshMovement();
        if (!wasGrounded)
        {
            OnLandEvent.Invoke();
        }
    }

    private void CheckSlope(RaycastHit2D raycastHit)
    {
        SlopeCheckVertical(raycastHit);
        SlopeCheckHorizontal(raycastHit);
    }

    private void SlopeCheckHorizontal(RaycastHit2D raycastHit)
    {

    }

    public void SetGravityScale()
    {

    }

    private RaycastHit2D DrawBoxCast()
    {
        Vector3 origin = new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y + extraHeight, boxCollider.bounds.center.z);
        Vector3 size = new Vector3(boxCollider.bounds.size.x, extraHeight, boxCollider.bounds.size.z);
        return Physics2D.BoxCast(origin, size, 0f, Vector2.down, extraHeight, whatIsGround);
    }
    private void SlopeCheckVertical(RaycastHit2D raycastHit)
    {
        if (raycastHit)
        {
            _slopeNormalPerp = Vector2.Perpendicular(raycastHit.normal).normalized;
            _slopeDownAngle = grounded ? Vector2.Angle(raycastHit.normal, Vector2.up) : 0;
            isOnSlope = (_slopeDownAngle > 0.1f || _slopeDownAngle < -0.1f);
        }
    }

    private void DrawDebugRayLines(RaycastHit2D raycastHit)
    {
        Color rayColor;
        if (raycastHit.collider != null) 
            rayColor = Color.green;
        else 
            rayColor = Color.red;
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (extraHeight), rayColor);
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (extraHeight), rayColor);
        Debug.DrawRay(raycastHit.point, _slopeNormalPerp, Color.red);
        Debug.DrawRay(raycastHit.point, raycastHit.normal, Color.green);
    }

    private void IsFalling()
    {
        if (_rb.velocity.y < 0.01 && !grounded)
        {
            falling = true;
            OnFallingEvent.Invoke();
        }
    }

    public void Move(float xInput, bool jump, bool jumpHeld)
    {
        _move = xInput * player.playerStats.curSpeed;
        float _jumpForce = player.playerStats.jumpForce;
        float _movementSmoothing = player.playerStats.movementSmoothing;
        float _shortHopSubtraction = player.playerStats.shortHopSubtraction;
        _rb.gravityScale = player.playerStats.defaultGravity;

        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector3(); 

        if (Inputs.Horizontal != 0)
            dir = Inputs.Horizontal;

        // Character is on a slope
        if (isOnSlope && grounded)
        {
            targetVelocity.Set(-_move * 10f * _slopeNormalPerp.x, -_move * _rb.velocity.y * _slopeNormalPerp.y, 0);
            _rb.gravityScale = 0;
        }

        // Character is on the ground or in the air
        else
        {
            targetVelocity.Set(_move * 10f, _rb.velocity.y, 0);
        }

        // And then smoothing it out and applying it to the character
        if (!killHorizontalInput)
        {
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, _movementSmoothing);
        }

        //____________JUMP______________
        if (grounded && jumpHeld && jump)
        {
            // Add a vertical force to the player.
            grounded = false;
            falling = false;
            wasJumping = true;
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        // peak of jump achieved, add a gravity multiplier to falling
        if (falling)
        {
            _rb.gravityScale = player.playerStats.defaultGravity * 1.3f;
        }

        // jump button released before the peak of the jump is achieved, immediately start descent 
        else if ((_rb.velocity.y > 0.1 && !jumpHeld || !wasJumping) && grounded == false)
        {
            _rb.AddForce(Vector2.up * -_shortHopSubtraction, ForceMode2D.Impulse);
        }
    }
}

