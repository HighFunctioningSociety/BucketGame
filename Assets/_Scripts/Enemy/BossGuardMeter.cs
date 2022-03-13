using UnityEngine;

public class BossGuardMeter : MonoBehaviour
{
    public float maxGuard;
    private float _currentGuard;
    public float regen;
    public float brokenRegen;
    public bool guardBroken = false;
    //private bool wasGuardBroken = false;
    public GuardUI guardUI;
    private EnemyContainer enemy;

    //public float currentGuard
    //{
    //    get { return _currentGuard; }
    //    set { _currentGuard = Mathf.Clamp(value, 0, maxGuard); }
    //}

    //public void Start()
    //{
    //    currentGuard = maxGuard;
    //    enemy = GetComponentInParent<EnemyContainer>();
    //}

    //public void Update()
    //{
    //    if (!guardBroken)
    //    {
    //        if (wasGuardBroken)
    //        {
    //            enemy.curArmor = enemy.enemyStats.armor;
    //            wasGuardBroken = false;
    //        }
    //        currentGuard += regen * Time.deltaTime;
    //    }
    //    else
    //    {
    //        wasGuardBroken = true;
    //        if (enemy.groundCheck.grounded)
    //        {
    //            currentGuard += brokenRegen * 2 * Time.deltaTime;
    //        }
    //        else
    //        {
    //            currentGuard += brokenRegen * Time.deltaTime;
    //        }

    //        if (currentGuard == maxGuard)
    //        {
    //            guardBroken = false;
    //        }
    //    }

    //    guardUI.SetGuard(currentGuard, maxGuard);
    //}

    //public void LowerGuard(int _damage)
    //{
    //    if (!guardBroken)
    //    {
    //        currentGuard -= (float)_damage / 4;
    //        if (currentGuard <= 0)
    //        {
    //            guardBroken = true;
    //            enemy.curArmor = 0;
    //        }
    //    }
    //}
}
