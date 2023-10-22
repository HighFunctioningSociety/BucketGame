using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


public class _GameManager : MonoBehaviour
{
    public static _GameManager gm;
    public static SceneDirectory.Scene CurrentScene = SceneDirectory.Scene.Station_1;
    public static SceneDirectory.Scene currentSpawnScene = SceneDirectory.Scene.Station_1;
    public static int currentSpawnIndex = 0;
    public static int DoorID = 0;
    public static bool respawningPlayer = false;
    public static bool FirstUpdate = true;
    public static bool waitForSceneManager = false;
    public static bool debugSceneChange = false;

    public GameState GameData;

    [Header ("Player References")]
    public static GameObject Player;
    public static PlayerContainer PlayerContainer;
    public static PlayerStats PlayerStats;

    public _SceneManager sceneManager;
    public DropManager dropManager;
    public Transform spawnPrefab;
    public Transform deathPrefab;
    public int SpawnDelay = 2;

    public bool isStopped = false;
    public float hitStopDuration = 0.1f;

    public UnityEvent SceneChangeEvent;
    public UnityEvent SceneLoadEvent;

    private void Awake()
    {
        if (gm == null)
		{
            gm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(SceneChangeEvent == null)
        {
            SceneChangeEvent = new UnityEvent();
        }

        if(SceneLoadEvent == null)
        {
            SceneLoadEvent = new UnityEvent();
        }

    }

    private void Update()
    {
        if (FirstUpdate && sceneManager != null)
        {
            if (sceneManager.entrancePoints.Length != 0)
            {
                LoadGameInfo();
                
                FirstUpdate = false;
            }
        }
    }


    public static void GetPlayer(GameObject player)
    {
        gm._GetPlayer(player);
    }

    private void _GetPlayer(GameObject player)
    {
        Player = player;
        PlayerContainer = Player.GetComponent<PlayerContainer>();
        PlayerStats = PlayerContainer.playerStats;
    }

    public static GameObject GivePlayer()
    {
        return gm._GivePlayer();
    }

    public GameObject _GivePlayer()
    {
        return Player;
    }

    public static void PrepForMenu()
    {
        gm._PrepForMenu();
    }

    private void _PrepForMenu()
    {
        Destroy(Player);
    }

    public static void GetSceneManager(_SceneManager sceneManager)
    {
        gm._GetSceneManager(sceneManager);
    }

    private void _GetSceneManager(_SceneManager _sceneManager)
    {
        sceneManager = _sceneManager;
        waitForSceneManager = false;
    }
    public static void MovePlayerToEntrance()
    {
        gm._MovePlayerToEntrance();
    }

    private void _MovePlayerToEntrance()
    {
        sceneManager._MoveToEntrance(Player.transform);
    }

    public static void ResetScene()
    {
        gm._ResetAllObjects();
        gm._ResetAllEnemies();
        gm._ResetFightTriggers();
    }

    public static void KillAllEnemies()
    {
        gm._KillAllEnemies();
    }

    private void _KillAllEnemies()
    {
        sceneManager._KillAllEnemies();
    }

    public static void ResetAllEnemies()
    {
        gm._ResetAllEnemies();
    }

    private void _ResetAllEnemies()
    {
        sceneManager._ResetAllEnemies();
    }

    public static void ResetFightTriggers()
    {
        gm._ResetFightTriggers();
    }

    private void _ResetFightTriggers()
    {
        sceneManager._ResetFightTriggers();
    }

    public static void ResetAllObjects()
    {
        gm._ResetAllObjects();
    }

    private void _ResetAllObjects()
    {
        sceneManager._ResetAllObjects();
    }

    public static void RelinquishPlayerInput(bool relinquish = true)
    {
            PlayerContainer.CurrentControlType = relinquish ? PlayerContainer.CONTROLSTATE.RELINQUISHED : PlayerContainer.CurrentControlType = PlayerContainer.CONTROLSTATE.ACCEPT_INPUT;
    }

    public IEnumerator _RespawnPlayer()
    {
        while (waitForSceneManager == true)
        {
            yield return null;
        }
        ResetScene();
        Player.transform.position = sceneManager.spawnPoints[currentSpawnIndex].position;
        yield return new WaitForSeconds(SpawnDelay);
        PlayerContainer.StopCoroutines();
        PlayerContainer.SetHealth(PlayerStats.maxHealth);
        PlayerContainer.CurrentControlType = PlayerContainer.CONTROLSTATE.ACCEPT_INPUT;
        PlayerContainer.CurrentState = PlayerContainer.PSTATE.NORMAL;
        respawningPlayer = false;

        Transform particles = Instantiate(spawnPrefab, sceneManager.spawnPoints[currentSpawnIndex].position, sceneManager.spawnPoints[currentSpawnIndex].rotation);
        Destroy(particles.gameObject, 4f);
    }

    private IEnumerator RespawnFadeToScene()
    {
        BlankerAnimator.blanker.FadeOut();
        while (BlankerAnimator.blanker.transitionInProgress)
        {
            yield return null;
        }
        Loader.Load(currentSpawnScene);
    }

    public static void KillPlayer()
    {
        gm._KillPlayer();
    }

    private void _KillPlayer()
    {
        PlayerContainer.rb.velocity = Vector2.zero;
        PlayerContainer.CurrentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
        PlayerContainer.playerStats.curHealth = PlayerStats.maxHealth;
        PlayerContainer.playerStats.curSpirit = 0;

        SaveSystem.SaveGame();

        if (CurrentScene != currentSpawnScene && respawningPlayer == false)
        {
            respawningPlayer = true;
            waitForSceneManager = true;
            CurrentScene = currentSpawnScene;
            StartCoroutine(RespawnFadeToScene());
        }
        gm.StartCoroutine(gm._RespawnPlayer());
    }

    public static void KillEnemy(EnemyContainer enemy)
    {
        SimpleCameraShake._CameraShake(0.2f, 0.2f);
        HitStopController._SimpleHitStop(0.1f);
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(EnemyContainer enemy)
    {
        dropManager.CalculateAndDropEnemyCoins(enemy);
        enemy.gameObject.SetActive(false);
        Transform deathPrefab = Instantiate(enemy.EnemyPrefabDead, enemy.transform.position, enemy.transform.rotation);
        Transform deathParticles = Instantiate(enemy.EnemyParticlesDead, enemy.transform.position, enemy.transform.rotation);
        deathPrefab.GetComponent<Rigidbody2D>().AddForce(enemy.RigidBody.velocity * 2, ForceMode2D.Impulse); // right now applying the enemies walk velocity instead of knockback (possibly fixed)
        Destroy(deathPrefab.gameObject, 50f);
        Destroy(deathParticles.gameObject, 10f);
    }

    public static void SpawnChestContents(ChestController _chest)
    {
        gm.dropManager.DropChestContents(_chest);
    }

    private void LoadGameInfo()
    {
        GameData = SaveSystem.LoadGame();
        currentSpawnIndex = GameData.CurrentSpawnIndex;
        PlayerStats.maxHealth = GameData.MaxHealth;
        PlayerStats.curHealth = GameData.CurHealth;
        PlayerStats.maxSpirit = GameData.MaxMeter;
        PlayerStats.curSpirit = GameData.CurMeter;
        PlayerStats.healthUpgrades = GameData.HealthUpgrades;
        Player.transform.position = sceneManager.spawnPoints[currentSpawnIndex].position;

        _UIManager.UIManager.MapController.LoadMapData(GameData);
        Inventory.instance.walletAmount = GameData.WalletAmount;

        // Parse the save data to get the current scene
        try 
        { 
            CurrentScene = (SceneDirectory.Scene)System.Enum.Parse(typeof(SceneDirectory.Scene), FirstUpdate ? GameData.CurrentSpawnScene : GameData.CurrentScene);
        }
        catch
        {
            CurrentScene = Constants.Scenes.OnErrorScene;
        }
    }

    public static string PrintSceneInfo()
    {
        return("(" + CurrentScene
            + ", " + currentSpawnScene 
            + " " + currentSpawnIndex + ")");
    }
}
