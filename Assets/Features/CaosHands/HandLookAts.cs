using System;
using UnityEngine;

public class HandLookAts : MonoBehaviour
{
   [SerializeField] private Transform handPos;
   [SerializeField] private Transform hand;
   [SerializeField] private Transform elbow;
   [SerializeField] private Transform sholder;


   private void Update()
   {
      hand.position = handPos.position;
      elbow.up = (hand.position - elbow.position).normalized;
      sholder.up = (hand.position - sholder.position).normalized;

      hand.rotation = handPos.rotation;
   }
}
