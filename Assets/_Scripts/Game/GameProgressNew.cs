using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class GameProgressNew
{

    public int currentSpawnIndex = 0;
    public int DoorID = 0;

    [Header("Secrets Found")]
    [Space]
    public  bool Station = false;

    [Header ("Boss Progression")]
    [Space]
    public bool ScampLord = false;

    [Header ("Abilities Aquired")]
    [Space]

    [Header ("Inventory")]
    [Space]
    public int Money = 0;

    [Header("Health Upgrades")]
    [Space]
    public int HealthUpgrades;

    [Header("Switch States")]
    [Space]
    public bool MineSwitch;

    [Header("Chest States")]
    [Space]
    public bool chestStates;
}
