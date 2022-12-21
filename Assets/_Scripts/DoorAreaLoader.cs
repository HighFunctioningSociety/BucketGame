using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAreaLoader : AreaLoader
{
    public ContactFilter2D playerLayer;
    public AudioSource audioSource;
    public GameObject dpadIcon;
    public float rangeX, rangeY;
    public bool playerInRange;
    private bool buttonDown;
    private Collider2D playerCollider;

    private void Start()
    {
        doorDirection = Direction.CENTER;
    }

    public void Update()
    {
        if (!PauseMenu.GamePaused && playerInRange)
        {
            InteractWithDoor();
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

    private void OnTriggerEnter2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            playerInRange = true;
            dpadIcon.SetActive(true);
            playerCollider = _colInfo;
        }
    }

    private void OnTriggerExit2D(Collider2D _colInfo)
    {
        if (_colInfo.GetComponent<PlayerContainer>() != null)
        {
            playerInRange = false;
            dpadIcon.SetActive(false);
        }
    }

    private void InteractWithDoor()
    {
        PlayerContainer _player = playerCollider.GetComponent<PlayerContainer>();
        if (Inputs.Vertical == 1 && !buttonDown && _player.PlayerController.GetGrounded())
        {
            buttonDown = true;
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
        _GameManager.RelinquishPlayerInput();
        Inputs.DisableHorizontal();
        PlayerContainer.KillVelocity();
        StartCoroutine(FadeToNextScene());
    }
}