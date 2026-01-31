using System;
using UnityEngine;

public class SteeringWeel : MonoBehaviour
{
   [SerializeField, Range(-1,1)] private float x;
   [SerializeField] private int maxAngle;
   [SerializeField] private Transform weel;

   private void Update()
   {
      weel.localRotation = Quaternion.Euler(0, 0, -x * maxAngle);
   }
}
