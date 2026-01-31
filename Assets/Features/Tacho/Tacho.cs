using System;
using UnityEngine;

public class Tacho : MonoBehaviour
{
    [SerializeField,Range(0,1)] private float tachoMeter;
    [SerializeField] private int maxAngle;
    [SerializeField] private Transform tachoNeedle;
    

    private void Update()
    {
        tachoNeedle.localRotation = Quaternion.Euler(0, 0, -tachoMeter * maxAngle);
    }
}
