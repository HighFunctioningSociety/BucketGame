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
        ForcePlayerMovement(player);
        player.currentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
        StartCoroutine(FadeToNextScene(player));
    }


    private void ForcePlayerMovement(PlayerContainer player)
    {
        player.PlayerAbilityController.CancelAbilities();
        player.PlayerAnimationController.SetForcedLeftMovement(doorDirection == Direction.LEFT ?  true : false);
        player.PlayerAnimationController.SetForcedRightMovement(doorDirection == Direction.RIGHT ? true : false);
    }
}
