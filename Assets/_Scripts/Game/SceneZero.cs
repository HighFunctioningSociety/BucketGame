using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneZero : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Awake()
    {
        if (!File.Exists(Constants.Paths.SavePath))
        {
            SaveSystem.NewGame();
        }

        GameState data = SaveSystem.LoadGame();
        _GameManager.CurrentScene = (SceneDirectory.Scene)System.Enum.Parse(typeof(SceneDirectory.Scene), data.currentSpawnScene);
    }

    private void Start()
    {
        _GameManager.GetPlayer(playerPrefab);
        Loader.Load(_GameManager.CurrentScene);
    }
}
