using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem 
{
    public static void NewGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Constants.Paths.SavePath, FileMode.Create);
        GameState data = new NewState();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(Constants.Paths.SavePath, FileMode.Create);
        GameState data = new GameState();
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameState LoadGame()
    {
        string path = Constants.Paths.SavePath;

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
    [Header("Scene Info")]
    [Space]
    public string currentScene;
    public int CurrentSpawnIndex;
    public string currentSpawnScene;
    public bool isRespawning = false;

    [Header("Player Stats")]
    [Space]
    public int MaxHealth = 5;
    public int HealthUpgrades = 0;
    public int CurHealth = 5;
    public int MaxMeter = 3;
    public int CurMeter = 0;
    public int WalletAmount = 0;

    [Header("Bosses Defeated")]
    [Space]
    public bool ScampLord = false;
    public bool Boss2 = false;
    public bool Boss3 = false;

    [Header("Switch States")]
    [Space]
    public bool[] SwitchStates;

    [Header("Upgrade States")]
    [Space]
    public bool[] UpgradeStates;

    [Header("Map Completion")]
    [Space]
    public bool Mine_0_Discovered = false;
    public bool Mine_1_Discovered = false;
    public bool Mine_2_Discovered = false;
    public bool Mine_3_Discovered = false;
    public bool Mine_4_Discovered = false;
    public bool Mine_5_Discovered = false;
    public bool ScampLordArena_Discovered = false;

    public GameState()
    {
        if (_GameManager.PlayerStats == null)
            return;

        //Scene info
        currentScene = _GameManager.CurrentScene.ToString();
        currentSpawnScene = _GameManager.currentSpawnScene.ToString();
        CurrentSpawnIndex = _GameManager.currentSpawnIndex;

        //Player stats
        MaxHealth = _GameManager.PlayerStats.maxHealth;
        CurHealth = _GameManager.PlayerStats.curHealth;
        HealthUpgrades = _GameManager.PlayerStats.healthUpgrades;
        MaxMeter = _GameManager.PlayerStats.maxSpirit;
        CurMeter = _GameManager.PlayerStats.curSpirit;

        //Player inventory
        WalletAmount = Inventory.instance.walletAmount;

        //Player Progression
        ScampLord = _GameManager.gm.GameData.ScampLord;

        //Environment States

        // Map Completion
        Mine_0_Discovered = _GameManager.gm.GameData.Mine_0_Discovered;
        Mine_1_Discovered = _GameManager.gm.GameData.Mine_1_Discovered;
        Mine_2_Discovered = _GameManager.gm.GameData.Mine_2_Discovered;
        Mine_3_Discovered = _GameManager.gm.GameData.Mine_3_Discovered;
        Mine_4_Discovered = _GameManager.gm.GameData.Mine_4_Discovered;
        Mine_5_Discovered = _GameManager.gm.GameData.Mine_5_Discovered;
        ScampLordArena_Discovered = _GameManager.gm.GameData.ScampLordArena_Discovered;
    }
}

[System.Serializable]
public class NewState : GameState
{
    public NewState()
    {
        Debug.LogWarning("NEW SAVE CREATED"); 
        currentScene = SceneDirectory.Scene.Town_HubArea.ToString();
        currentSpawnScene = SceneDirectory.Scene.Town_HubArea.ToString();
        CurrentSpawnIndex = 0;

        MaxHealth = Constants.Values.DefaultPlayerHealth;
        CurHealth = Constants.Values.DefaultPlayerHealth;
        HealthUpgrades = 0;
        MaxMeter = Constants.Values.DefaultMaxMeter;
        CurMeter = 0;

        WalletAmount = 0;

        ScampLord = false;

        //MAP DATA
        Mine_0_Discovered = false;
        Mine_1_Discovered = false;
        Mine_2_Discovered = false;
        Mine_3_Discovered = false;
        Mine_4_Discovered = false;
        Mine_5_Discovered = false;
        ScampLordArena_Discovered = false;
    }
}

