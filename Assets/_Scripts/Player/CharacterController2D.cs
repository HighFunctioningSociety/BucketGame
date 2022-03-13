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
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;


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
        rb = player.rb;

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnFallingEvent == null)
            OnFallingEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        wasGrounded = grounded;
        grounded = false;
        IsGrounded();
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

    public void IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y + extraHeight, boxCollider.bounds.center.z), new Vector3(boxCollider.bounds.size.x, extraHeight, boxCollider.bounds.size.z), 0f, Vector2.down, extraHeight, whatIsGround);
        if (raycastHit.collider != null && rb.velocity.y < 0.01f)
        {
            SetGrounded();
        }
        DrawRayLines(raycastHit);
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

    private void DrawRayLines(RaycastHit2D raycastHit)
    {
        Color rayColor;
        if (raycastHit.collider != null) 
            rayColor = Color.green;
        else 
            rayColor = Color.red;
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) + new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (extraHeight), rayColor);
        Debug.DrawRay(new Vector3(boxCollider.bounds.center.x, boxCollider.bounds.min.y) - new Vector3(boxCollider.bounds.extents.x, 0), Vector2.down * (extraHeight), rayColor);
    }

    private void IsFalling()
    {
        if (rb.velocity.y < 0.01 && !grounded)
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
        float _fallMultiplier = player.playerStats.fallMultiplier;
        float _shortHopSubtraction = player.playerStats.shortHopSubtraction;

        if (Inputs.Horizontal != 0)
            dir = Inputs.Horizontal;
        
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(_move * 10f, rb.velocity.y);

        // And then smoothing it out and applying it to the character
        if (!killHorizontalInput)
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, _movementSmoothing);

        //____________JUMP______________
        if (grounded && jumpHeld && jump)
        {
            // Add a vertical force to the player.
            grounded = false;
            falling = false;
            wasJumping = true;
            rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        // peak of jump achieved, add a gravity multiplier to falling
        if (rb.velocity.y <= 0)
        {
            rb.AddForce(Vector2.up * Physics2D.gravity * (_fallMultiplier - 1));
        }

        // jump button released before the peak of the jump is achieved, immediately start descent 
        else if ((rb.velocity.y > 0.1 && !jumpHeld || !wasJumping) && grounded == false)
        {
            rb.AddForce(Vector2.up * -_shortHopSubtraction, ForceMode2D.Impulse);
        }
    }
}

