using UnityEngine;
using System.Collections;

public class SimpleCameraShake : MonoBehaviour
{
    public Camera mainCamera;
    public static SimpleCameraShake simpleCameraShake;
    private float shakeAmplitude;
    public float timeLeft = 0.0f;

    private void Start()
    {
        if (simpleCameraShake == null)
        {
            simpleCameraShake = this;
        }
    }

    private void Update()
    {
        Vector3 originalPos = transform.localPosition;

        if (timeLeft > 0)
        {
            float x = Random.Range(-1f, 1f) * shakeAmplitude;
            float y = Random.Range(-1f, 1f) * shakeAmplitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
        }
        else
        {
            transform.localPosition = originalPos;
        }

        timeLeft -= Time.deltaTime;
    }

    public static void _CameraShake(float _shakeLength, float _shakeAmplitude)
    {
        simpleCameraShake.CameraShake(_shakeLength, _shakeAmplitude);
    }

    public void CameraShake(float _shakeLength, float _shakeAmplitude)
    {
        shakeAmplitude = _shakeAmplitude;
        timeLeft = _shakeLength;
    }
}
