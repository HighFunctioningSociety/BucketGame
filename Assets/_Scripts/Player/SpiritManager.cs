using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritManager : MonoBehaviour
{
    private PlayerContainer player;
    private PlayerStats stats;
    public int currentSpirit;

    private void Awake()
    {
        player = GetComponent<PlayerContainer>();
        stats = player.playerStats;
    }

    private void Update()
    {
        if (!_GameManager.firstUpdate)
        {
            currentSpirit = stats.curSpirit;
            UpdateSpiritUI();
        }

        if (stats.curSpiritProgression == stats.maxSpiritProgression && stats.curSpirit != stats.maxSpirit)
        {
            stats.curSpiritProgression = 0;
            AddSpirit(1);
        }

        if (stats.curSpirit == stats.maxSpirit)
        {
            stats.curSpiritProgression = 0;
        }

        stats.curSpiritProgression -= 10 * Time.deltaTime;
        currentSpirit = stats.curSpirit;
        UpdateSpiritProgressUI();
    }    

    public void SubtractSpirit(int subtractByAmount)
    {
        stats.curSpirit -= subtractByAmount;
        UpdateSpiritUI();
    }

    public void AddSpirit(int increaseByAmount)
    {
        stats.curSpirit += increaseByAmount;
        UpdateSpiritUI();
    }

    private void UpdateSpiritUI()
    {
        _UIManager.UIManager._SetSpirit(stats.curSpirit, stats.maxSpirit);
        _UIManager.UIManager._SetSpiritAnimation(stats.curSpirit);
    }

    private void UpdateSpiritProgressUI()
    {
        _UIManager.UIManager._SetSpiritProgress(stats.curSpiritProgression, stats.maxSpiritProgression, stats.curSpirit, stats.maxSpirit);
    }
}
