using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = ("SaveFileData"))]
public class GameProgress : ScriptableObject
{
    [Header("Secrets Found")]
    [Space]
    public bool station = false;

    [Header ("Boss Progression")]
    [Space]
    public bool ScampLord = false;


    [Header ("Abilities Aquired")]
    [Space]

    [Header ("Inventory")]
    [Space]
    public int money = 0;

    [Header("States")]
    [Space]
    public StateList upgradeStates;
    public StateList switchStates;
    public StateList chestStates;
}
