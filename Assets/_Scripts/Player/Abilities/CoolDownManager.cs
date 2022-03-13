using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{
    public float nextReadyTime;
    public bool coolDownComplete = false;
    public bool waitForFlag = false;
    public bool coolDownFlag = false;

    private void Update()
    {
        if (waitForFlag == false)
        {
            coolDownComplete = (Time.time > nextReadyTime);
            coolDownFlag = false;
        }
        else if (coolDownFlag == true)
        {
            coolDownComplete = coolDownFlag;
            coolDownFlag = false;
            waitForFlag = false;
        }
    }

    public void ResetCoolDown()
    {
        coolDownFlag = false;
        waitForFlag = false;
        nextReadyTime = Time.time;
    }

    public void CoolDownFlagStart()
    {
        waitForFlag = true;
        coolDownComplete = false;
    }

    public void CoolDownFinished()
    {
        coolDownFlag = true;
    }

    public void SetNextCoolDown(float _nextReadyTime)
    {
        nextReadyTime = Time.time + _nextReadyTime;
    }
}
