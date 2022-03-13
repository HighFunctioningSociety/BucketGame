using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneZero : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Awake()
    {
        string path = Application.persistentDataPath + "/gameState.bin";
        if (!File.Exists(path))
        {
            SaveSystem.NewGame(playerPrefab.GetComponent<PlayerContainer>());
            GameState data = SaveSystem.LoadGame();
            _GameManager.currentScene = (Loader.Scene)System.Enum.Parse(typeof(Loader.Scene), data.currentSpawnScene);
        }
        else
        {
            GameState data = SaveSystem.LoadGame();
            _GameManager.currentScene = (Loader.Scene)System.Enum.Parse(typeof(Loader.Scene), data.currentSpawnScene);
        }
    }

    private void Start()
    {
        _GameManager.GetPlayer(playerPrefab);
        Loader.Load(_GameManager.currentScene);
    }
}
