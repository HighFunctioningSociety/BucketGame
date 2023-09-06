using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatform : MonoBehaviour
{
    private PlatformEffector2D _effector;
    private Collider2D _platformCollider;
    private float _platX;
    private float _platY;
    private Vector2 _platCenter;
    private float _defaultRotation;
    private Collider2D[] _colliders;

    public LayerMask playerLayer;

    private void Start()
    {
        _platformCollider = GetComponent<Collider2D>();
        _effector = GetComponent<PlatformEffector2D>();
        _defaultRotation = _effector.rotationalOffset;
        _platX = _platformCollider.bounds.size.x;
        _platY = _platformCollider.bounds.size.y;
        _platCenter = _platformCollider.bounds.center;
    }

    void Update()
    {
        CheckForPlayer();
    }

    private void DropPlayer()
    {
        if (Inputs.Vertical < 0)
        {
            Inputs.supressJump = true;
            if (Inputs.confirm)
            {   
                _effector.rotationalOffset = 180f;
            }
        }
        else
        {
            Inputs.supressJump = false;
        }
    }

    private void CheckForPlayer()
    {
        DropPlayer();
        
        _colliders = Physics2D.OverlapBoxAll(_platCenter, new Vector2(_platX, _platY), 0f, playerLayer);

        if (_colliders.Length == 0)
        {
            _effector.rotationalOffset = _defaultRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Inputs.supressJump = false;
    }
}
