using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundCheck : MonoBehaviour
{
    public Collider2D enemyCollider;
    public LayerMask whatIsGround;
    public LayerMask whatIsSlope;
    public bool WasGrounded;
    public bool Grounded;
    public bool EdgeLeft = false;
    public bool EdgeRight = false;
    [HideInInspector] public bool RightEdgeAlreadyFound = false;
    [HideInInspector] public bool LeftEdgeAlreadyFound = false;

    public float _extraLength = 0.4f;
    public float _extraWidth = 0.05f;

    private Animator _animator;
    private EnemyContainer _enemy;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    void Start()
    {
        _animator = GetComponentInParent<Animator>();
        _enemy = GetComponentInParent<EnemyContainer>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }
    void LateUpdate()
    {
        CheckIfEdgeLeftAlreadyFound();
        CheckIfEdgeRightAlreadyFound();
        if (EdgeLeft == false && EdgeRight == false)
        {
            RightEdgeAlreadyFound = false;
            LeftEdgeAlreadyFound = false;
        }
        WasGrounded = Grounded;
        Grounded = false;
        IsGrounded();
    }

    public void IsGrounded()
    {
        RaycastHit2D raycastLeft = Physics2D.Raycast(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) - new Vector3(enemyCollider.bounds.extents.x + _extraWidth, 0), Vector2.down, _extraLength, whatIsGround);
        RaycastHit2D raycastRight = Physics2D.Raycast(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) + new Vector3(enemyCollider.bounds.extents.x + _extraWidth, 0), Vector2.down, _extraLength, whatIsGround);

        if ((raycastLeft.collider != null || raycastRight.collider != null) && (_enemy.RigidBody.velocity.y < 0.01))
        {
            Grounded = true;
            _animator.SetBool("Grounded", true);
            if (!WasGrounded)
            {
                HasBecomeGrounded();
            }
        }
        else
        {
            _animator.ResetTrigger("Hurt");
            _animator.SetBool("Grounded", false);
        }

        EdgeLeft = CheckLeftRaycast(raycastLeft);
        EdgeRight = CheckRightRaycast(raycastRight);
        DrawRaycasts(GetRaycastColor(EdgeLeft), GetRaycastColor(EdgeRight));
    }

    public void IsOnSlope()
    {
        Vector2 checkPos = transform.position - new Vector3(0.0f, enemyCollider.bounds.size.y /2);
    }

    public void OnLanding()
    {
        _enemy.RigidBody.gravityScale = _enemy.EnemyStats.defaultGravity;
        _enemy.StopMomentum();
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
    
    private Color GetRaycastColor(bool edgeFound)
    {
        Color color = Color.green;

        if (edgeFound == true)
        {
            color = Color.red;
        }

        return color;
    }

    private void DrawRaycasts(Color leftColor, Color rightColor)
    {
        Debug.DrawRay(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) - new Vector3(enemyCollider.bounds.extents.x + _extraWidth, 0), Vector2.down * (_extraLength), leftColor);
        Debug.DrawRay(new Vector3(enemyCollider.bounds.center.x, enemyCollider.bounds.min.y) + new Vector3(enemyCollider.bounds.extents.x + _extraWidth, 0), Vector2.down * (_extraLength), rightColor);
    }

    private void HasBecomeGrounded()
    {
        OnLandEvent.Invoke();
        if (_enemy.EnemyStats.isHeavy)
        {
            SimpleCameraShake._CameraShake(0.3f, 0.2f);
        }
    }

    private void CheckIfEdgeLeftAlreadyFound()
    {
        if (EdgeRight == true && !RightEdgeAlreadyFound)
        {
            RightEdgeAlreadyFound = true;
        }
    }

    private void CheckIfEdgeRightAlreadyFound()
    {
        if (EdgeLeft == true && !LeftEdgeAlreadyFound)
        {
            LeftEdgeAlreadyFound = true;
        }
    }
}
