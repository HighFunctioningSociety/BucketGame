using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager psm;
    [SerializeField] private AudioSource slash_1AudioScource;
    [SerializeField] private AudioSource slash_2AudioScource;
    [SerializeField] private AudioSource slash_3AudioScource;
    [SerializeField] private AudioSource slashUpAudioScource;
    [SerializeField] private AudioSource stingerAudioScource;
    [SerializeField] private AudioSource stingerSpecialAudioScource;
    [SerializeField] private AudioSource plungeAudioScource;
    [SerializeField] private AudioSource helmSplitterEndAudioScource;
    [SerializeField] private AudioSource helmSplitterSubHitAudioScource;
    [SerializeField] private AudioSource dashAudioScource;
    [SerializeField] private AudioSource LandAudioScource;
    [SerializeField] private AudioSource footStepsAudioScource;

    private void Awake()
    {
        if (psm == null)
        {
            psm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void SilenceSlash_1()
    {
        slash_1AudioScource.Stop();
    }

    public void SilenceSlash_2()
    {
        slash_2AudioScource.Stop();
    }

    public void SilenceSlash_3()
    {
        slash_3AudioScource.Stop();
    }

    public void SilenceSlashUp()
    {
        slashUpAudioScource.Stop();
    }

    public void SilenceFootSteps()
    {
        footStepsAudioScource.Stop();
    }

    public void SilenceLand()
    {
        LandAudioScource.Stop();
    }

    public void SilenceDash()
    {
        dashAudioScource.Stop();
    }
}
