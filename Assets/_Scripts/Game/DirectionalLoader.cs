using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLoader : AreaLoader
{

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() == null)
        {
            return;
        }
        else
        {
            if (VerifyNextScene())
            {
                SetScene();
            }
            else
            {
                return;
            }
            PlayerContainer player = _colInfo.GetComponent<PlayerContainer>();
            LoadScene(player);
        }
    }

    protected override void LoadScene(PlayerContainer player)
    {
        if (player.controller.grounded)
        {
            SetWalking(player);
        }
        player.currentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
        StartCoroutine(FadeToNextScene(player));
    }


    private void SetWalking(PlayerContainer player)
    {
        player.abilityManager.CancelAbilities();
        if (doorDirection == Direction.LEFT)
        {
            player.transitioningLeft = true;
        }
        else if (doorDirection == Direction.RIGHT)
        {
            player.transitioningRight = true;
        }
    }
}
