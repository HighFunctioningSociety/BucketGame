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
    private AbilityManager abilityManager;
    public EnemyScan Stinger;
    
    public void Start()
    {
        player = GetComponentInParent<PlayerContainer>();
        coolDownManager = GetComponentInParent<CoolDownManager>();
        abilityManager = GetComponentInParent<AbilityManager>();
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
            StartDashing(direction);
            StingerScan();
        }
        else
        {
            DecideStingerExecution();
        }
    }

    private void StartDashing(float direction)
    {
        abilityManager.UnfreezeXConstraint();
        player.rb.velocity = new Vector2 (direction, 0) * dashSpeed;
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

    public void CallDash(bool dash)
    {
        if (dash == true && coolDown < 0 && coolDownManager.coolDownComplete && canDash)
        {
            audioSource.Play();
            canDash = false;
            direction = player.controller.dir;
            dashTime = startDashTime;
            coolDown = dashCoolDown;
            player.playerStats.curMovementDivider++;
        }
        Inputs.dash = false;
    }


    public void RefreshDash()
    {
        canDash = true;
    }
}
