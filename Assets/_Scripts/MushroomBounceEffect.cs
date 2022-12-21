using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBounceEffect : TriggerEffect
{
    public ParticleSystem SporeParticles;
    public Animator MushroomAnimator;
    public string AnimationName;

    public override void PlayEffect()
    {
        if (MushroomAnimator != null)
            MushroomAnimator.Play(AnimationName);
        if (SporeParticles != null)
            SporeParticles.Play();
    }
}
