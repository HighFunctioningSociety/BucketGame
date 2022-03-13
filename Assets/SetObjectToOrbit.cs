using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectToOrbit : MonoBehaviour
{
    public Orbit[] orbitingObjects;

    public void SetOrbitTargets(Transform objectToOrbit)
    {
        foreach (Orbit orbiter in orbitingObjects)
        {
            orbiter.objectToOrbit = objectToOrbit;
        }
    }
}
