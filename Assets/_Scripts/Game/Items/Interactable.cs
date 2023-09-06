using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public abstract class Interactable : MonoBehaviour
{
    public LayerMask playerLayer;
    public bool playerInRange;
    public float rangeX, rangeY;
    public GameObject dpadIcon;
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

    private void DisplayIcon()
    {
        playerCollider = Physics2D.OverlapBox(transform.position, new Vector2(rangeX, rangeY), 0, playerLayer);
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
        if (Inputs.Vertical == 1 && !buttonDown && _player.PlayerController.GetGrounded() && _player.CurrentState == PlayerContainer.PSTATE.NORMAL && _player.CurrentControlType == PlayerContainer.CONTROLSTATE.ACCEPT_INPUT)
        {
            buttonDown = true;
            Interact();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(rangeX, rangeY));
    }

    public abstract void Interact();
}
