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
    public float DirectionFaced = 1;
    public float BoxcastHeight = 0.4f;
    public bool KillHorizontalInput;
    private bool _grounded;            // Whether or not the player is grounded.
    private bool _falling;               // Controls if the animation is falling is playing. True if rb.velocity.y < 0
    private Rigidbody2D _rb;
    private Vector3 _velocity = Vector3.zero;


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

    private void FixedUpdate()
    {
        RaycastHit2D raycastHit = DrawBoxcast();
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

        if (raycastHit.collider != null && _rb.velocity.y < 0.01f)
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
        MoveSpeed = xInput * player.playerStats.curSpeed;
        float movementSmoothing = player.playerStats.movementSmoothing;

        if (Inputs.Horizontal != 0)
            DirectionFaced = Inputs.Horizontal;
        
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(MoveSpeed * 10f, _rb.velocity.y);

        // And then smoothing it out and applying it to the character
        if (!KillHorizontalInput)
            _rb.velocity = Vector3.SmoothDamp(_rb.velocity, targetVelocity, ref _velocity, movementSmoothing);
    }

    public void Jump(bool jump, bool jumpHeld)
    {
        float jumpforce = player.playerStats.jumpForce;
        float shorthopSubtract = player.playerStats.shortHopSubtraction;

        if (_grounded && jumpHeld && jump)
        {
            // Add a vertical force to the player.
            _grounded = false;
            _falling = false;
            wasJumping = true;
            _rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }

        // peak of jump achieved, add a gravity multiplier to falling
        if (_rb.velocity.y <= 0)
        {
            player.rb.gravityScale = player.playerStats.defaultGravity * 1.3f;
        }

        // jump button released before the peak of the jump is achieved, immediately start descent 
        else if ((_rb.velocity.y > 0.1 && !jumpHeld) && _grounded == false)
        {
            _rb.AddForce(Vector2.up * -shorthopSubtract, ForceMode2D.Impulse);
        }
    }
}

