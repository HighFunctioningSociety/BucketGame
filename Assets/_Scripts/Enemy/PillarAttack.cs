using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarAttack : MonoBehaviour
{
    public Collider2D hurtBox;
    public SpriteRenderer sr;
    public float knockBackX, knockBackY;
    public int damage;

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        PlayerContainer player = _colInfo.GetComponentInParent<PlayerContainer>();
        if (player != null)
        {
            player._KnockBack(knockBackX, knockBackY, new Vector2(hurtBox.bounds.center.x, hurtBox.bounds.min.y));
            player._DamagePlayer(damage, false);
        }
    }

    public void ActivateCollider()
    {
        hurtBox.enabled = true;
    }

    public void AnimationFinished()
    {
        //hurtBox.enabled = false;
        Destroy(this.gameObject);
    }
}
