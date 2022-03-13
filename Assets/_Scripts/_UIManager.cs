using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class _UIManager : MonoBehaviour
{
    public static _UIManager UIManager;
    public EventSystem eventSystem;
    public GameObject menus;
    public GameObject healthUIObject;
    public GameObject spiritUIObject;
    public GameObject equipmentUIObject;
    public GameObject debugUI;
    public GameObject inventoryUI;
    public GameObject playerHUD;
    public GameObject dialogueUIParent;
    public GameObject restingMenu;
    public GameObject restingMenuFirstButton;
    public GameObject equipmentMenu;
    public GameObject equipmentMenuFirstButton;

    public Animator animator;
    private bool menusActivated = false;
    private HealthUI healthUI;
    private SpiritUI spiritUI;
    private EquipmentUI equipmentUI;

    public void Awake()
    {
        if (UIManager == null)
        {
            UIManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        healthUI = healthUIObject.GetComponent<HealthUI>();
        spiritUI = spiritUIObject.GetComponent<SpiritUI>();
        equipmentUI = equipmentUIObject.GetComponent<EquipmentUI>();
    }

    public void FixedUpdate()
    {
        if (!_GameManager.firstUpdate && !menusActivated)
        {
            menus.SetActive(true);
            playerHUD.SetActive(true);
            inventoryUI.SetActive(true);
            dialogueUIParent.SetActive(false);
            animator.SetBool("FadeIn", true);
            EnableEditorUI();

            menusActivated = true;
        }
    }

    public void ResetMenus()
    {
        menus.SetActive(false);
        playerHUD.SetActive(false);
        inventoryUI.SetActive(false);
        dialogueUIParent.SetActive(false);
        restingMenu.SetActive(false);
        menusActivated = false;
    }

    public void DisableHUD()
    {
        playerHUD.SetActive(false);
    }

    public void EnableHUD()
    {
        playerHUD.SetActive(true);
    }

    public void DisableDialogueParent()
    {
        dialogueUIParent.SetActive(false);
    }
    public void EnableDialogueParent()
    {
        dialogueUIParent.SetActive(true);
    }

    public void MenuFadeOut()
    {
        animator.SetBool("FadeIn", false);
    }

    public void MenuFadeIn()
    {
        animator.SetBool("FadeIn", true);
    }

    public void SetSelectedButton(GameObject _button) 
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(_button);
    }

    public void _SetHealth(int _cur, int _max)
    {
        healthUI.SetHealth(_cur, _max);
    }

    public void _SetSpiritProgress(float _curProgress, float _maxProgress, int _curSpirit, int _maxSpirit)
    {
        spiritUI.SetProgression(_curProgress, _maxProgress, _curSpirit, _maxSpirit);
    }

    public void _SetSpirit(int _cur, int _max)
    {
        spiritUI.SetSpirit(_cur, _max);
    }

    public void _SetSpiritAnimation(int _cur)
    {
        spiritUI.SetSpiritAnimation(_cur);
    }

    public void _SetEquipmentCoolDown(float _nextReadyTime)
    {
        equipmentUI.SetEquipmentCooldown(_nextReadyTime);
    }

    public void _SetEquipmentUIStartTime(float _startTime)
    {
        equipmentUI.startTime = _startTime;
    }

    public void _SetEquipmentIconColor(bool _coolDownComplete)
    {
        equipmentUI.SetEquipmentIconColor(_coolDownComplete);
    }

    public void _ChangeEquipmentIcon(Sprite _iconToUse)
    {
        equipmentUI._ChangeEquipmentIcon(_iconToUse);
    }

    public void _EnableRestingMenu()
    {
        restingMenu.SetActive(true);
        SetSelectedButton(restingMenuFirstButton);
        Inputs.supressJump = true;
    }
    public void _DisableRestingMenu()
    {
        restingMenu.SetActive(false);
        Inputs.supressJump = false;
    }

    public void _SwitchToEquipmentMenu()
    {
        restingMenu.SetActive(false);
        equipmentMenu.SetActive(true);
        SetSelectedButton(equipmentMenuFirstButton);
    }
    public void _SwitchToRestingMenu()
    {
        restingMenu.SetActive(true);
        equipmentMenu.SetActive(false);
        SetSelectedButton(restingMenuFirstButton);
    }

    public void _ActivatePlayerControl()
    {
        PlayerContainer _player = _GameManager.GivePlayer().GetComponent<PlayerContainer>();
        _player.currentControlType = PlayerContainer.CONTROLSTATE.ACCEPT_INPUT;
    }

    public void _SelectEquipment(string equipmentName)
    {
        Debug.Log("you pushed me!");
        PlayerContainer _player = _GameManager.GivePlayer().GetComponent<PlayerContainer>();
        _player.equipmentManager.SelectEquipment(equipmentName);
    }

    private void EnableEditorUI()
    {
 #if UNITY_EDITOR
        {
            debugUI.SetActive(true);
        }
#else   
        {
            debugUI.SetActive(false);
        } 
#endif
    }
}



