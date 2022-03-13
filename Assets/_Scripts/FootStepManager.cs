using UnityEngine;

public class FootStepManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] footSteps;

    private bool clipSwapped = false;

    private void Update()
    {
        if (!audioSource.isPlaying & clipSwapped == false)
        {
            int randomStep = Random.Range(0, footSteps.Length);
            audioSource.clip = footSteps[randomStep];
            clipSwapped = true;
        }

        if (audioSource.isPlaying)
        {
            clipSwapped = false;
        }
    }

    public void PlayFootStepEvent()
    {
        audioSource.Play();
    }
}
