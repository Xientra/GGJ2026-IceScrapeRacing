// using UnityEngine;
//
// public class CarCameraShake : MonoBehaviour
// {
//     public static CarCameraShake Instance { get; private set; }
//     
//     [Header("Shake Settings")]
//     public float baseShakeMagnitude = 0.05f;
//     public float shakeSpeed = 20f;  
//
//     private Vector3 initialPosition;
//     private bool isShaking = false;
//     private float currentMagnitude = 0f;
//
//     void Awake()
//     {
//         initialPosition = transform.localPosition;
//         Instance = this;
//     }
//
//
//     void Update()
//     {
//         if (isShaking)
//         {
//             // Looping shake using Perlin noise
//             float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2 * currentMagnitude;
//             float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2 * currentMagnitude;
//             transform.localPosition = initialPosition + new Vector3(x, y, 0);
//         }
//         else
//         {
//             // Reset camera to initial position when not shaking
//             transform.localPosition = initialPosition;
//         }
//     }
//     
//     [ContextMenu("StartShake")]
//     public void StartLoopShake(float magnitude)
//     {
//         currentMagnitude = magnitude;
//         isShaking = true;
//     }
//
//     [ContextMenu("StopShake")]
//     public void StopLoopShake()
//     {
//         isShaking = false;
//     }
// }


using UnityEngine;

public class CarCameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeSpeed = 5f; 
    public float smoothing = 8f; 

    private Vector3 initialPosition;
    private Vector3 targetOffset;
    private float currentMagnitude = 0f;
    private bool isShaking = false;
    
    public static CarCameraShake Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            // Smooth perlin noise shake
            float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2 * currentMagnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2 * currentMagnitude;
            targetOffset = new Vector3(x, y, 0);

            // Smoothly interpolate towards the target offset
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + targetOffset, Time.deltaTime * smoothing);
        }
        else
        {
            // Reset to initial position
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * smoothing);
        }
    }

    public void StartLoopShake(float magnitude)
    {
        currentMagnitude = magnitude;
        isShaking = true;
    }

    public void StopLoopShake()
    {
        isShaking = false;
    }
}
