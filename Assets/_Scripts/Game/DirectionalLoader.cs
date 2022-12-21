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
            LoadScene();
        }
    }

    protected override void LoadScene()
    {
        _UIManager.UIManager.MapController.UpdateMap(sceneToLoad);
        ForcePlayerMovement(_GameManager.PlayerContainer);
        _GameManager.PlayerContainer.CurrentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
        StartCoroutine(FadeToNextScene());
    }

    private void ForcePlayerMovement(PlayerContainer player)
    {
        player.PlayerAbilityController.CancelAbilities();
        player.PlayerAnimationController.SetForcedLeftMovement(doorDirection == Direction.LEFT ?  true : false);
        player.PlayerAnimationController.SetForcedRightMovement(doorDirection == Direction.RIGHT ? true : false);
    }
}
