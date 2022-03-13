using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


public class _GameManager : MonoBehaviour
{
    public static _GameManager gm;
    public static Loader.Scene currentScene = Loader.Scene.Station_1;
    public static Loader.Scene currentSpawnScene = Loader.Scene.Station_1;
    public static int currentSpawnIndex = 0;
    public static int DoorID = 0;
    public static bool respawningPlayer = false;
    public static bool firstUpdate = true;
    public static bool waitForSceneManager = false;
    public static bool debugSceneChange = false;

    [HideInInspector] public GameObject player;
    public _SceneManager sceneManager;
    public DropManager dropManager;
    public Transform spawnPrefab;
    public Transform deathPrefab;
    public int spawnDelay = 2;

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

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
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
        if (firstUpdate && sceneManager != null)
        {
            if (sceneManager.entrancePoints.Length != 0)
            {
                GameState data = SaveSystem.LoadGame();
                currentSpawnScene = (Loader.Scene)System.Enum.Parse(typeof(Loader.Scene), data.currentSpawnScene);
                currentScene = (Loader.Scene)System.Enum.Parse(typeof(Loader.Scene), data.currentScene);
                currentSpawnIndex = data.currentSpawnIndex;
        
                player.GetComponent<PlayerContainer>().transform.position = sceneManager.spawnPoints[currentSpawnIndex].position;
                
                firstUpdate = false;
            }
        }
    }


    public static void GetPlayer(GameObject player)
    {
        gm._GetPlayer(player);
    }

    private void _GetPlayer(GameObject _player)
    {
        player = _player;
    }

    public static GameObject GivePlayer()
    {
        return gm._GivePlayer();
    }

    public GameObject _GivePlayer()
    {
        return player;
    }

    public static void PrepForMenu()
    {
        gm._PrepForMenu();
    }

    private void _PrepForMenu()
    {
        Destroy(player);
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
        sceneManager._MoveToEntrance(player.transform);
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

    public static void RelinquishPlayerInput()
    {
        gm._RelinquishPlayerInput();
    }

    private void _RelinquishPlayerInput()
    {
        PlayerContainer _player = player.GetComponent<PlayerContainer>();
        _player.currentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
    }

    public static void AcceptPlayerInput()
    {
        gm._AcceptPlayerInput();
    }

    private void _AcceptPlayerInput()
    {
        PlayerContainer _player = player.GetComponent<PlayerContainer>();
        _player.currentControlType = PlayerContainer.CONTROLSTATE.ACCEPT_INPUT;
    }

    public IEnumerator _RespawnPlayer(PlayerContainer _player)
    {
        while (waitForSceneManager == true)
        {
            yield return null;
        }
        ResetScene();
        _player.transform.position = sceneManager.spawnPoints[currentSpawnIndex].position;
        yield return new WaitForSeconds(spawnDelay);
        PlayerContainer.StopCoroutines();
        _player.SetHealth(_player.playerStats.maxHealth);
        _player.currentControlType = PlayerContainer.CONTROLSTATE.ACCEPT_INPUT;
        _player.currentState = PlayerContainer.PSTATE.NORMAL;
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

    public static void KillPlayer(PlayerContainer player)
    {
        gm._KillPlayer(player);
    }

    private void _KillPlayer(PlayerContainer _player)
    {
        _player.rb.velocity = Vector2.zero;
        _player.currentControlType = PlayerContainer.CONTROLSTATE.RELINQUISHED;
        _player.playerStats.curHealth = _player.playerStats.maxHealth;
        _player.playerStats.curSpirit = 0;

        SaveSystem.SaveGame(_player);

        if (currentScene != currentSpawnScene && respawningPlayer == false)
        {
            respawningPlayer = true;
            waitForSceneManager = true;
            currentScene = currentSpawnScene;
            StartCoroutine(RespawnFadeToScene());
        }
        gm.StartCoroutine(gm._RespawnPlayer(_player));
    }

    public static void KillEnemy(EnemyContainer enemy)
    {
        SimpleCameraShake._CameraShake(0.2f, 0.2f);
        HitStop._SimpleHitStop(0.1f);
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(EnemyContainer _enemy)
    {
        dropManager.CalculateAndDropEnemyCoins(_enemy);
        _enemy.gameObject.SetActive(false);
        Transform deathPrefab = Instantiate(_enemy.enemyPrefabDead, _enemy.transform.position, _enemy.transform.rotation);
        Transform deathParticles = Instantiate(_enemy.enemyParticlesDead, _enemy.transform.position, _enemy.transform.rotation);
        deathPrefab.GetComponent<Rigidbody2D>().AddForce(_enemy.rb.velocity * 2, ForceMode2D.Impulse); // right now applying the enemies walk velocity instead of knockback (possibly fixed)
        GameObject.Destroy(deathPrefab.gameObject, 50f);
        GameObject.Destroy(deathParticles.gameObject, 10f);
    }

    public static void SpawnChestContents(ChestController _chest)
    {
        gm._SpawnChestContents(_chest);
    }

    public void _SpawnChestContents(ChestController _chest)
    {
        dropManager.DropChestContents(_chest);
    }

    public static void SaveGameState(PlayerContainer player)
    {
        gm._SaveGameState(player);
    }

    private void _SaveGameState(PlayerContainer _player)
    {
        SaveSystem.SaveGame(_player);
    }

    public static void LoadGameState(PlayerContainer player)
    {
        gm._LoadGameState(player);
    }

    private void _LoadGameState(PlayerContainer _player)
    {
        GameState data = SaveSystem.LoadGame();
        _player.playerStats.maxHealth = data.MaxHealth;
        _player.playerStats.curHealth = data.CurHealth;
        _player.playerStats.maxSpirit = data.MaxMeter;
        _player.playerStats.curSpirit = data.CurMeter;
        _player.playerStats.healthUpgrades = data.HealthUpgrades;
        _player.gameProgress.switchStates.SetBools(data.SwitchStates);
        _player.gameProgress.upgradeStates.SetBools(data.UpgradeStates);
        Inventory.instance.walletAmount = data.WalletAmount;

        if (firstUpdate)
        {
            currentScene = (Loader.Scene)System.Enum.Parse(typeof(Loader.Scene), data.currentSpawnScene);
        }
        else
        {
            currentScene = (Loader.Scene)System.Enum.Parse(typeof(Loader.Scene), data.currentScene);
        }
    }

    public static string PrintSceneInfo()
    {
        return("(" + currentScene
            + ", " + currentSpawnScene 
            + " " + currentSpawnIndex + ")");
    }
}
