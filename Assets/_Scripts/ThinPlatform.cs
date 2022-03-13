using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private Collider2D platformCol;
    private float platX;
    private float platY;
    private Vector2 platCenter;
    private float defaultRotation;
    public Collider2D[] colliders;

    public LayerMask playerLayer;

    private void Start()
    {
        platformCol = GetComponent<Collider2D>();
        effector = GetComponent<PlatformEffector2D>();
        defaultRotation = effector.rotationalOffset;
        platX = platformCol.bounds.size.x;
        platY = platformCol.bounds.size.y;
        platCenter = platformCol.bounds.center;
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
                effector.rotationalOffset = 180f;
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
        
        colliders = Physics2D.OverlapBoxAll(platCenter, new Vector2(platX, platY), 0f, playerLayer);

        if (colliders.Length == 0)
        {
            effector.rotationalOffset = defaultRotation;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Inputs.supressJump = false;
    }
}
