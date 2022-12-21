using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCollider : MonoBehaviour
{
    public int launchForce;
    public bool Colliding = false;
    public TriggerEffect Effect;

    private void OnTriggerStay2D(Collider2D _colInfo)
    {
        PlayerContainer player;

        if ((player = _colInfo.GetComponent<PlayerContainer>()))
        {
            player.PlayerController.LaunchPlayer(launchForce);
            Effect.PlayEffect();
            Colliding = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Colliding = false;
    }
}
