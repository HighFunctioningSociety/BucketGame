using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWith : MonoBehaviour
{
    public LayerMask playerLayer;
    public bool playerInRange;
    public float rangeX, rangeY;
    public GameObject dpadIcon;
    public TriggerableEvent interactable;
    private Collider2D playerCollider;
    private bool buttonDown;


    public void Update()
    {
        DisplayIcon();
        if (!PauseMenu.GamePaused && playerCollider != null)
        {
            CheckForUpInput();
        }
        if (Inputs.Vertical == 0)
        {
            buttonDown = false;
        }
        if (Inputs.Vertical == 1 && !playerInRange)
        {
            buttonDown = true;
        }
    }
    private void InteractWithInteractable()
    {
        interactable.TriggerEvent();
    }

    private void DisplayIcon()
    {
        playerCollider = Physics2D.OverlapBox(this.transform.position, new Vector2(rangeX, rangeY), 0, playerLayer);
        if (playerCollider != null)
        {
            playerInRange = true;
            dpadIcon.SetActive(true);
        }
        else
        {
            playerInRange = false;
            dpadIcon.SetActive(false);
        }
    }


    private void CheckForUpInput()
    {
        if (playerCollider.GetComponent<PlayerContainer>() == null)
            return;

        PlayerContainer _player = playerCollider.GetComponent<PlayerContainer>();
        if (Inputs.Vertical == 1 && !buttonDown && _player.controller.grounded && _player.currentState == PlayerContainer.PSTATE.NORMAL && _player.currentControlType == PlayerContainer.CONTROLSTATE.ACCEPT_INPUT)
        {
            buttonDown = true;
            InteractWithInteractable();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(rangeX, rangeY));
    }
}
