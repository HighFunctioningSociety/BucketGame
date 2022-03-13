using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageAnimator : MonoBehaviour
{
    public BoxCollider2D boxCollider;
    public Animator animator;
    public string defaultState;

    private void OnTriggerStay2D(Collider2D _colInfo)
    {
        if (_colInfo.tag == "Player" || _colInfo.GetComponent<EnemyContainer>() != null)
        {
            if (Mathf.Round(_colInfo.GetComponent<Rigidbody2D>().velocity.x) != 0){
                if (Mathf.Sign(_colInfo.transform.localScale.x) < 0 && animator.GetCurrentAnimatorStateInfo(0).IsName(defaultState) && !animator.IsInTransition(0))
                {
                    animator.SetTrigger("SwayLeft");
                }
                if (Mathf.Sign(_colInfo.transform.localScale.x) > 0 && animator.GetCurrentAnimatorStateInfo(0).IsName(defaultState) && !animator.IsInTransition(0))
                {
                    animator.SetTrigger("SwayRight");
                    
                }
            }
        }
    }
}
