using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveFileData/StateList")]
public class StateList : ScriptableObject
{
    public ScriptableBool[] states;

    public void SetBools(bool[] serializedStates)
    {
        for(int i = 0; i < serializedStates.Length; i++)
        {
            states[i].state = serializedStates[i];
        }
    }
}
