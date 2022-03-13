using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private float speed;
    private bool restoreTime;
    public static HitStop hitStop;
    public static bool hitStopActive = false;

    private void Start()
    {
        if (hitStop == null)
        {
            hitStop = this;
        }
        restoreTime = false;
    }

    private void Update()
    {
        if (restoreTime)
        {
            if (Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                hitStopActive = false;
                restoreTime = false;
            }
        }
    }

    public static void _SimpleHitStop(float _delay)
    {
        hitStopActive = true;
        hitStop.SimpleHitStop(_delay);
    }

    public void SimpleHitStop(float delay)
    {
        hitStopActive = true;
        StartCoroutine(SimpleStop(delay));
    }

    public static void _StopTime(float _changeTime, int _restoreSpeed, float _delay)
    {
        hitStop.StopTime(_changeTime, _restoreSpeed, _delay);
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if (delay > 0)
        {
            StopCoroutine(StartTime(delay));
            StartCoroutine(StartTime(delay));
        }
        else
        {
            restoreTime = true;
        }

        Time.timeScale = changeTime;
    }

    IEnumerator StartTime(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        restoreTime = true;
    }

    IEnumerator SimpleStop(float delay)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        hitStopActive = false;
    }
}
