using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    public AudioSource audioSource;
    public float dashSpeed;
    public float startDashTime;
    public float dashCoolDown;
    public float dashTime;
    [HideInInspector] public float coolDown;
    [HideInInspector] public float direction;
    [HideInInspector] public bool stinging = false;
    [HideInInspector] public bool startStinger = false;
    [HideInInspector] public bool startStingerSpecial = false;
    [HideInInspector] public bool enemyFound = false;
    private bool canDash = false;

    private PlayerContainer player;
    private CoolDownManager coolDownManager;
    private AbilityController abilityManager;
    public EnemyScan Stinger;
    
    public void Start()
    {
        player = GetComponentInParent<PlayerContainer>();
        coolDownManager = GetComponentInParent<CoolDownManager>();
        abilityManager = GetComponentInParent<AbilityController>();
    }

    public void FixedUpdate()
    {
        dashTime -= Time.fixedDeltaTime;
        DashExecute();
        if (dashTime <= 0)
        {
            coolDown -= Time.fixedDeltaTime;
        }
    }

    public void Update()
    {
        if (player.controller.GetGrounded())
        {
            canDash = true;
        }
    }

    private void DashExecute()
    {
        if (dashTime > 0)
        {
            dashTime -= Time.fixedDeltaTime;
            abilityManager.UnfreezeXConstraint();
            player.rb.velocity = new Vector2(direction, 0) * dashSpeed;
            StingerScan();
        }
        else
        {
            DecideStingerExecution();
        }
    }

    private void StingerScan()
    {
        if (!enemyFound)
        {
            DecideOnScan();
        }
        else
        {
            dashTime = 0f;
        }    
    }

    private void DecideOnScan()
    {
        if (startStinger)
        {
            enemyFound = Stinger.EnemyCheckStinger();
        }
        else if (startStingerSpecial)
        {
            enemyFound = Stinger.EnemyCheckStingerSpecial();
        }
    }

    private void DecideStingerExecution()
    {
        if (startStinger)
        {
            abilityManager.ExecuteStinger();
            coolDown = 1f;
        }
        else if (startStingerSpecial)
        {
            abilityManager.ExecuteStingerSpecial();
            coolDown = 1f;
        }
    }

    public void StartDashing(bool dash)
    {
        if (dash == true && coolDown < 0 && coolDownManager.coolDownComplete && canDash)
        {
            //player.playerStats.curMovementDivider++;
            direction = player.controller.DirectionFaced;
            dashTime = startDashTime;
            coolDown = dashCoolDown;
            audioSource.Play();
            canDash = false;
        }

        Inputs.dash = false;
    }


    public void RefreshDash()
    {
        canDash = true;
    }
}
