using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : Interactable
{
    public AudioSource audioSource;
    public int spawnIndex;
    public SceneDirectory.Scene spawnScene;
    public Transform particlePrefab;

    public override void Interact()
    {
        InteractWithSpawnPoint();
    }

    private void InteractWithSpawnPoint()
    {
        PlayerContainer _player = _GameManager.GivePlayer().GetComponent<PlayerContainer>();
        if (_player.dashAbility.dashTime <= 0)
            Rest(_player);
    }

    private void Rest(PlayerContainer _player)
    {
        if (PauseMenu.GamePaused || MapController.MapActive)
            return;

        _GameManager.ResetScene();
        audioSource.Play();
        EnableRestingMenu(_player);
        RestoreHealth(_player);
        RecordGame();
        SpawnParticles();
    }

    private void EnableRestingMenu(PlayerContainer _player)
    {
        _player.rb.velocity = Vector2.zero;
        _player.CurrentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
        _UIManager.UIManager._EnableRestingMenu();
    }

    private void RecordGame()
    {
        _GameManager.currentSpawnIndex = spawnIndex;
        _GameManager.currentSpawnScene = spawnScene;
        SaveSystem.SaveGame();
        _GameManager.PrintSceneInfo();
    }

    private void RestoreHealth(PlayerContainer _player)
    {
        _player.SetHealth(_player.playerStats.maxHealth);
    }

    private void SpawnParticles()
    {
        Transform particles = Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(particles.gameObject, 3f);
    }
}
