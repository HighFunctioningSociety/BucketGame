using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheck : MonoBehaviour
{
    public Collider2D enemyCollider;
    public LayerMask whatIsGround;
    public float extraLength;
    public float extraWidth;
    public bool wasGrounded;
    public bool grounded;
    private Animator animator;
    private EnemyContainer enemy;
    public bool edgeLeft = false;
    public bool edgeRight = false;
    public bool rightEdgeAlreadyFound = false;
    public bool leftEdgeAlreadyFound = false;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
        enemy = GetComponentInParent<EnemyContainer>();
        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    void LateUpdate()
    {
        CheckIfEdgeLeftAlreadyFound();
        CheckIfEdgeRightAlreadyFound();
        if (edgeLeft == false && edgeRight == false)
        {
            rightEdgeAlreadyFound = false;
            leftEdgeAlreadyFound = false;
        }
        wasGrounded = grounded;
        grounded = false;
        IsGrounded();
    }

    public void IsGrounded()
    {
        RaycastHit2D raycastLeft = Physics2D.Raycast(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) - new Vector3(enemyCollider.bounds.extents.x + extraWidth, 0), Vector2.down, extraLength, whatIsGround);
        RaycastHit2D raycastRight = Physics2D.Raycast(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) + new Vector3(enemyCollider.bounds.extents.x + extraWidth, 0), Vector2.down, extraLength, whatIsGround);
        Color rayColorLeft = Color.green;
        Color rayColorRight = Color.green;

        if ((raycastLeft.collider != null || raycastRight.collider != null) && (enemy.rb.velocity.y < 0.01))
        {
            grounded = true;
            animator.SetBool("Grounded", true);
            if (!wasGrounded)
            {
                HasBecomeGrounded();
            }
        }
        else
        {
            animator.ResetTrigger("Hurt");
            animator.SetBool("Grounded", false);
        }

        edgeLeft = CheckLeftRaycast(raycastLeft);
        edgeRight = CheckRightRaycast(raycastRight);
        SetRaycastColors(rayColorLeft, rayColorRight);
        DrawRaycasts(rayColorLeft, rayColorRight);
    }

    public void OnLanding()
    {
        enemy.rb.gravityScale = enemy.enemyStats.defaultGravity;
        enemy.StopMomentum();
    }

    private bool CheckLeftRaycast(RaycastHit2D _raycastLeft)
    {
        if (_raycastLeft.collider != null)
            return false;
        else
            return true;
    }

    private bool CheckRightRaycast(RaycastHit2D _raycastRight)
    {
        if (_raycastRight.collider != null)
            return false;
        else
            return true;
    }
    
    private void SetRaycastColors(Color leftColor, Color rightColor)
    {
        if (edgeLeft == false)
            leftColor = Color.red;

         if (edgeRight == true)
            rightColor = Color.red;
    }

    private void DrawRaycasts(Color leftColor, Color rightColor)
    {
        Debug.DrawRay(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) - new Vector3(enemyCollider.bounds.extents.x + extraWidth, 0), Vector2.down * (extraLength), leftColor);
        Debug.DrawRay(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) + new Vector3(enemyCollider.bounds.extents.x + extraWidth, 0), Vector2.down * (extraLength), rightColor);
    }

    private void HasBecomeGrounded()
    {
        OnLandEvent.Invoke();
        if (enemy.enemyStats.isHeavy)
        {
            SimpleCameraShake._CameraShake(0.3f, 0.2f);
        }
    }

    private void CheckIfEdgeLeftAlreadyFound()
    {
        if (edgeRight == true && !rightEdgeAlreadyFound)
        {
            rightEdgeAlreadyFound = true;
        }
    }

    private void CheckIfEdgeRightAlreadyFound()
    {
        if (edgeLeft == true && !leftEdgeAlreadyFound)
        {
            leftEdgeAlreadyFound = true;
        }
    }
}
