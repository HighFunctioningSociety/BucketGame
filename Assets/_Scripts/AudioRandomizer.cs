using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRandomizer : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private bool clipSwapped = false;

    void Update()
    {
        if (!audioSource.isPlaying & clipSwapped == false)
        {
            int randomSound = Random.Range(0, audioClips.Length);
            audioSource.clip = audioClips[randomSound];
            clipSwapped = true;
        }

        if (audioSource.isPlaying)
        {
            clipSwapped = false;
        }
    }
}
