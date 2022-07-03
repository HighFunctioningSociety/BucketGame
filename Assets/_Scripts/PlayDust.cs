using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDust : MonoBehaviour
{
    public ParticleSystem dust;
    public GroundCheck gc;

    private void Update()
    {
        if (gc.Grounded && !gc.WasGrounded)
        {
            dust.Play();
        }
    }
}
