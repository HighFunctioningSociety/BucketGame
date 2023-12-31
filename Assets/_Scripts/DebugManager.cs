﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour
{
    public PlayerStats playerStats;
    public GameObject HeroInfo;
    public GameObject DebugCommands;
    public EnemyListManager EnemyList;
    public SceneDirectory.Scene[] bannedScenes;

    [Header("Text")]
    public Text frameRate;
    private float deltaTime = 0.0f;

    public Text heroState;
    public Text velocity;
    public Text health;
    public Text meter;
    public Text cooldown;
    public Text input;
    public Text currentScene;
    public Text canJump;
    public Text jumpWasCanceled;
    public Text freezeMovement;
    public Text slowMovement;
    public Text resetOnLanding;

    [Header("Debug On/Off")]
    public GameObject showMenuCanvas;
    public GameObject debugCanvases;

    [Header("Cheat Buttons")]
    public Toggle invulToggle;
    public Toggle spiritToggle;

    [Header("Scene Dropdown")]
    public Dropdown sceneDropdown;

    private PlayerContainer _player;
    private bool sceneActive = false;
    private bool cheatsActive = false;
    private bool invulnerable = false;
    private bool infiniteSpirit = false;
    private List<string> sceneOptions;
    private bool firstValueChange = false;

    private void Start()
    {
        int currentSceneIndex = 0;
        List<string> sceneNames = System.Enum.GetNames(typeof(SceneDirectory.Scene)).ToList();
        List<string> bannedList = bannedScenes.Select(scene => scene.ToString()).ToList();
        sceneDropdown.ClearOptions();

        sceneOptions = sceneNames.Except(bannedList).ToList();
        currentSceneIndex = sceneOptions.FindIndex(scene => scene == _GameManager.CurrentScene.ToString());

        sceneDropdown.AddOptions(sceneOptions);
        sceneDropdown.value = currentSceneIndex;
        sceneDropdown.RefreshShownValue();
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void FixedUpdate()
    {
        if (!_GameManager.FirstUpdate)
        {
            SetEnemyDebugUI();
            if (_player == null)
                _player = _GameManager.GivePlayer().GetComponent<PlayerContainer>();
        }

        frameRate.text = (1f / deltaTime).ToString();

        if (_player != null)
        {
            PrintHeroInfo();
        }

        DebugPlayerInvul();
        DebugSpirit();
    }

    private void SetEnemyDebugUI()
    {
        if (_GameManager.gm.sceneManager.enemyUISet == false)
        {
            foreach (EnemyContainer enemy in _GameManager.gm.sceneManager.enemyList)
            {
                EnemyList.InstantiateEnemyUIData(enemy);
            }
            _GameManager.gm.sceneManager.enemyUISet = true;
        }
    }

    private void DebugPlayerInvul()
    {
        if (invulnerable)
        {
            _player.invul = true;
        }
    }

    private void DebugSpirit()
    {
        if (infiniteSpirit && playerStats.curSpirit != playerStats.maxSpirit)
        {
            _player.playerStats.curSpirit = _player.playerStats.maxSpirit;
        }
    }

    private void PrintHeroInfo()
    {
        heroState.text = _player.CurrentState.ToString();
        velocity.text = _player.rb.velocity.ToString();
        health.text = "(" + playerStats.curHealth.ToString() + "/" + playerStats.maxHealth.ToString() + ")";
        meter.text = "(" + playerStats.curSpirit.ToString() + "/" + playerStats.maxSpirit.ToString() + ")";
        cooldown.text = _player.coolDownManager.coolDownComplete.ToString();
        input.text = _player.CurrentControlType.ToString();
        canJump.text = _player.PlayerAbilityController.canJumpCancelAttack.ToString();
        jumpWasCanceled.text = _player.PlayerAbilityController.wasJumpCanceled.ToString();
        freezeMovement.text = _player.PlayerAbilityController.freezeMovement.ToString();
        slowMovement.text = _player.PlayerAbilityController.slowMovement.ToString();
        resetOnLanding.text = _player.PlayerAbilityController.resetCoolDownOnLanding.ToString();
        
        currentScene.text = _GameManager.PrintSceneInfo();
    }

    public void RespawnPlayer()
    {
        _GameManager.KillPlayer();
    }

    public void SetSpawn()
    {

    }

    public void HideMenu()
    {
        debugCanvases.SetActive(false);
        showMenuCanvas.SetActive(true);
    }

    public void ShowMenu()
    {
        debugCanvases.SetActive(true);
        showMenuCanvas.SetActive(false);
    }


    public void ChangeScene()
    {
        sceneActive = !sceneActive;
        sceneDropdown.gameObject.SetActive(sceneActive);
    }

    public void LoadScene(int index)
    {
        if (firstValueChange == false)
        {
            firstValueChange = true;
            return;
        }

        string sceneString = sceneOptions[index];
        _GameManager.debugSceneChange = true;
        SceneDirectory.Scene sceneToLoad = (SceneDirectory.Scene)System.Enum.Parse(typeof(SceneDirectory.Scene), sceneString);
        _GameManager.CurrentScene = sceneToLoad;
        StartCoroutine(FadeOut(sceneToLoad));
    }

    public void KillAll()
    {
        _GameManager.KillAllEnemies();
    }

    public void FreePlayer()
    {
        _player.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Cheats()
    {
        cheatsActive = !cheatsActive;

        spiritToggle.gameObject.SetActive(cheatsActive);
        invulToggle.gameObject.SetActive(cheatsActive);
    }

    public void Invunerable(bool toggleValue)
    {
        _player.invul = toggleValue;
        invulnerable = toggleValue;
    }

    public void InfiniteSpirit(bool toggleValue)
    {
        infiniteSpirit = toggleValue;
    }

    public IEnumerator FadeOut(SceneDirectory.Scene sceneToLoad)
    {
        BlankerAnimator.blanker.FadeOut();
        while (BlankerAnimator.blanker.transitionInProgress)
        {
            yield return null;
        }

        Loader.Load(sceneToLoad);
    }
}
