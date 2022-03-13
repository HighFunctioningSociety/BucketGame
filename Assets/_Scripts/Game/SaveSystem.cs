using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void NewGame(PlayerContainer player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameState.bin";
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        GameState data = new NewState(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGame(PlayerContainer player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameState.bin";
        //Debug.Log(path);
        FileStream stream = new FileStream(path, FileMode.Create);

        GameState data = new GameState(player);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameState LoadGame()
    {
        string path = Application.persistentDataPath + "/gameState.bin";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameState data = formatter.Deserialize(stream) as GameState;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in directory " + path);
            return null;
        }
    }
}

[System.Serializable]
public class GameState
{
    //Scene Info
    public string currentScene;
    public int currentSpawnIndex;
    public string currentSpawnScene;
    public bool isRespawning = false;

    //Player stats
    public int MaxHealth = 5;
    public int HealthUpgrades = 0;
    public int CurHealth = 5;
    public int MaxMeter = 3;
    public int CurMeter = 0;
    public int WalletAmount = 0;

    //Player progression
    public bool ScampLord = false;
    public bool Boss2 = false;
    public bool Boss3 = false;

    //Environment States
    public bool[] SwitchStates;
    public bool[] UpgradeStates;

    public GameState(PlayerContainer player)
    {
        //Scene info
        currentScene = _GameManager.currentScene.ToString();
        currentSpawnScene = _GameManager.currentSpawnScene.ToString();
        currentSpawnIndex = _GameManager.currentSpawnIndex;

        //Player stats
        MaxHealth = player.playerStats.maxHealth;
        CurHealth = player.playerStats.curHealth;
        HealthUpgrades = player.playerStats.healthUpgrades;
        MaxMeter = player.playerStats.maxSpirit;
        CurMeter = player.playerStats.curSpirit;

        //Player inventory
        WalletAmount = Inventory.instance.walletAmount;

        //Player Progression
        ScampLord = player.gameProgress.ScampLord;

        //Environment States
        SwitchStates = new bool[player.gameProgress.switchStates.states.Length];
        for (int i = 0; i < SwitchStates.Length; i++)
        {
            SwitchStates[i] = player.gameProgress.switchStates.states[i].state;
        }

        UpgradeStates = new bool[player.gameProgress.upgradeStates.states.Length];
        for (int i = 0; i < SwitchStates.Length; i++)
        {
            UpgradeStates[i] = player.gameProgress.upgradeStates.states[i].state;
        }
    }
}

[System.Serializable]
public class NewState : GameState
{
    public NewState(PlayerContainer player) : base(player)
    {
        currentScene = Loader.Scene.Town_HubArea.ToString();
        currentSpawnScene = Loader.Scene.Town_HubArea.ToString();
        currentSpawnIndex = 0;

        MaxHealth = player.playerStats.defaultHealth;
        CurHealth = player.playerStats.defaultHealth;
        HealthUpgrades = 0;
        MaxMeter = 3;
        CurMeter = 0;

        WalletAmount = 0;

        ScampLord = false;

        SwitchStates = new bool[player.gameProgress.switchStates.states.Length];
        for (int i = 0; i < SwitchStates.Length; i++)
        {
            SwitchStates[i] = false;
        }

        UpgradeStates = new bool[player.gameProgress.upgradeStates.states.Length];
        for (int i = 0; i < SwitchStates.Length; i++)
        {
            UpgradeStates[i] = false;
        }
    }
}

