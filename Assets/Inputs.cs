using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inputs : MonoBehaviour
{
    public static Inputs _inputs;
    public PlayerControls controls;

    public static bool supressJump;

    public static bool jump = false;
    public static bool jumpHeld = false;
    public static bool confirm = false;
    public static bool dash = false;
    public static bool attack = false;
    public static bool equipment = false;
    public static bool attackHeld = false;
    public static bool specialAttack = false;
    public static bool killXInput = false;
    public static bool map = false;
    public static bool leftTrigger = false;
    public static bool rightTrigger = false;
    public static float Horizontal { get; set; }
    public static float Vertical { get; set; }

    private float attackBuffer = 0;
    private float dashBuffer = 0;
    private float attackInputCooldownTime = 0;


    public delegate void InventoryCallback();
    public InventoryCallback onInventoryCallback;

    public delegate void OptionsCallback();
    public OptionsCallback onOptionsCallback;

    public delegate void MapCallback();
    public MapCallback OnMapCallback;

    private void Awake()
    {
        if (_inputs == null)
        {
            _inputs = this; 
        }
        else
        {
            Destroy(gameObject);
        }

        controls = new PlayerControls();

        controls.Default.Right.performed += context => Horizontal = 1;
        controls.Default.Right.canceled += context => Horizontal = 0;

        controls.Default.Left.performed += context => Horizontal = -1;
        controls.Default.Left.canceled += context => Horizontal = 0;

        controls.Default.Up.performed += context => Vertical= 1;
        controls.Default.Up.canceled += context => Vertical = 0;

        controls.Default.Down.performed += context => Vertical = -1;
        controls.Default.Down.canceled += context => Vertical = 0;

        controls.Default.Jump.performed += context => Jumping();
        controls.Default.Jump.canceled += context => JumpOff();

        controls.Default.Dash.performed += context => Dashing();

        controls.Default.Attack.performed += context => Attacking();
        controls.Default.Attack.canceled += context => attackHeld = false;

        controls.Default.SpecialAttack.performed += context => specialAttack = true;
        controls.Default.SpecialAttack.canceled += context => specialAttack = false;

        controls.Default.Equipment.performed += context => equipment = true;
        controls.Default.Equipment.canceled += context => equipment = false;

        controls.Default.Options.performed += context => OptionsToggle();

        controls.Default.Inventory.performed += context => InventoryToggle();
        controls.Default.Map.performed += context => OnMapCallback.Invoke();

        controls.Default.Horizontal.performed += context => Horizontal = MapController.MapActive ? context.ReadValue<float>() : HorizontalRaw(context.ReadValue<float>());
        controls.Default.Horizontal.canceled += context => Horizontal = 0;

        controls.Default.Vertical.performed += context => Vertical = context.ReadValue<float>();
        controls.Default.Vertical.canceled += context => Vertical = 0;

        controls.Default.LeftTrigger.performed += context => leftTrigger = true;
        controls.Default.LeftTrigger.canceled += context => leftTrigger = false;

        controls.Default.RightTrigger.performed += context => rightTrigger = true;
        controls.Default.RightTrigger.canceled += context => rightTrigger = false;
    }

    private void Update()
    {
        attackBuffer -= Time.deltaTime;
        dashBuffer -= Time.deltaTime;
        bool attackInputCooldownFinished = attackInputCooldownTime < Time.time;

        if (attackBuffer <= 0 || !attackInputCooldownFinished)
        {
            attack = false;
        }
        if(dashBuffer <= 0)
        {
            dash = false;
        }
        if (killXInput)
        {
            Horizontal = 0;
        }
    }

    public static void SetAttackInputBufferTime(float _bufferTime)
    {
        _inputs._SetAttackInputBufferTime(_bufferTime);
    }

    private void _SetAttackInputBufferTime(float _bufferTime)
    {
        attackInputCooldownTime = Time.time + _bufferTime;
    }

    private float HorizontalRaw(float value)
    {
        if (value > 0)
        {
            return 1;
        }
        else if (value < 0)
        {
            return -1;
        }
        else return 0;
    }

    private void Jumping()
    {
        if (!supressJump)
        {
            jump = true;    
            jumpHeld = true;
        }
        confirm = true;
    }

    private void JumpOff()
    {
        jump = false;
        jumpHeld = false;
        confirm = false;
    }

    private void Attacking()
    {
        attack = true;
        attackHeld = true;
        attackBuffer = 0.2f;
    }

    private void Dashing()
    {
        dash = true;
        dashBuffer = 0.15f;
    }


    private void InventoryToggle()
    {
        if (onInventoryCallback != null)
        {
            onInventoryCallback.Invoke();
        }
    }

    private void OptionsToggle()
    {
        if (onOptionsCallback != null)
        {
            onOptionsCallback.Invoke();
        }
    }

    public static void DisableAttack()
    {
        attack = false;
    }

    public static void DisableInput()
    {
        dash = false;
        attack = false;
        specialAttack = false;
        jump = false;
        jumpHeld = false;
    }

    public static void DisableHorizontal()
    {
        Horizontal = 0;
    }

    private void OnEnable()
    {
        if (controls != null)
            controls.Default.Enable();
    }

    private void OnDisable()
    {
        if (controls != null)
            controls.Default.Disable();
    }
}
