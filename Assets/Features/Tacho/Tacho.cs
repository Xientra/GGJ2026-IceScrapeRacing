using System;
using UnityEngine;

public class Tacho : MonoBehaviour
{
    [SerializeField,Range(0,1)] private float tachoMeter;
    [SerializeField] private int maxAngle;
    [SerializeField] private Transform tachoNeedle;
    [SerializeField] private int startAngle = 90;
    [SerializeField] private Rigidbody carRigidBody;
    private void Update()
    {
        tachoMeter = carRigidBody.linearVelocity.magnitude;
        
        tachoNeedle.localRotation = Quaternion.Euler(0, 0, (-tachoMeter * maxAngle) - startAngle);
    }
}
