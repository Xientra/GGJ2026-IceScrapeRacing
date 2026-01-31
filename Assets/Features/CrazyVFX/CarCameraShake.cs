using UnityEngine;

public class CarCameraShake : MonoBehaviour
{
    public static CarCameraShake Instance { get; private set; }
    
    [Header("Shake Settings")]
    public float baseShakeMagnitude = 0.05f;
    public float shakeSpeed = 20f;  

    private Vector3 initialPosition;
    private bool isShaking = false;
    private float currentMagnitude = 0f;

    void Awake()
    {
        initialPosition = transform.localPosition;
        Instance = this;
    }


    void Update()
    {
        if (isShaking)
        {
            // Looping shake using Perlin noise
            float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2 * currentMagnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2 * currentMagnitude;
            transform.localPosition = initialPosition + new Vector3(x, y, 0);
        }
        else
        {
            // Reset camera to initial position when not shaking
            transform.localPosition = initialPosition;
        }
    }
    
    [ContextMenu("StartShake")]
    public void StartLoopShake(float magnitude)
    {
        currentMagnitude = magnitude;
        isShaking = true;
    }

    [ContextMenu("StopShake")]
    public void StopLoopShake()
    {
        isShaking = false;
    }
}

// using System;
// using UnityEngine;
//
// public class CarCameraShake : MonoBehaviour
// {
//     [Header("Shake Settings")]
//     public float baseShakeMagnitude = 0.03f;   // Normal driving shake
//     public float startShakeMagnitude = 0.2f;   // Strong shake when starting
//     public float shakeSpeed = 5f;              // Lower = smoother movement
//     public float smoothing = 8f;               // Higher = smoother interpolation
//
//     private Vector3 targetOffset;
//     private float currentMagnitude = 0f;
//     private bool isShaking = false;
//     
//     public static CarCameraShake Instance { get; private set; }
//
//     private void Awake()
//     {
//         Instance = this;
//     }
//
//     void Update()
//     {
//         if (isShaking)
//         {
//             // Smooth Perlin noise shake
//             float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2 * currentMagnitude;
//             float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2 * currentMagnitude;
//             targetOffset = new Vector3(x, y, 0);
//
//             // Apply shake on top of current position (base can be changed by DOTween)
//             transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + targetOffset, Time.deltaTime * smoothing);
//         }
//     }
//     
//     [ContextMenu("Start Shake")]
//     public void StartLoopShake()
//     {
//         currentMagnitude = startShakeMagnitude;
//         isShaking = true;
//     }
//
//     public void StartLoopShake(float magnitude)
//     {
//         currentMagnitude = magnitude;
//         isShaking = true;
//     }
//
//     public void StopLoopShake()
//     {
//         isShaking = false;
//     }
//
//     public void TriggerStartShake(float duration = 0.5f)
//     {
//         StartCoroutine(StartShakeCoroutine(duration));
//     }
//
//     private System.Collections.IEnumerator StartShakeCoroutine(float duration)
//     {
//         float elapsed = 0f;
//         float originalMagnitude = currentMagnitude;
//         currentMagnitude = startShakeMagnitude;
//         isShaking = true;
//
//         while (elapsed < duration)
//         {
//             elapsed += Time.deltaTime;
//             yield return null;
//         }
//
//         currentMagnitude = originalMagnitude;
//     }
//
//     public void SetShakeStrength(float magnitude)
//     {
//         currentMagnitude = magnitude;
//     }
// }