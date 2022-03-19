using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceCollider : MonoBehaviour
{
    public int launchForce;
    private void OnTriggerStay2D(Collider2D _colInfo)
    {
        PlayerContainer player;
        if (player = _colInfo.GetComponent<PlayerContainer>())
        {
            LaunchPlayer(launchForce, player);
        }
    }

    private void LaunchPlayer(float _launchForce, PlayerContainer _player)
    {
        _player.rb.AddForce(new Vector2(0, _launchForce), ForceMode2D.Impulse);
    }
}
