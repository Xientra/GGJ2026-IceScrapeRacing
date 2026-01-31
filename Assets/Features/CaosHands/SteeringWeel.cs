using System;
using DG.Tweening;
using UnityEngine;

public class SteeringWeel : MonoBehaviour
{
   [SerializeField, Range(-1,1)] private float x;
   [SerializeField] private int maxAngle;
   [SerializeField] private Transform weel;
   [SerializeField] private Ease rotateEase;
   [SerializeField] private float rotateDuration = 1;
   
   private InputSystem_Actions _input;

   private void Start()
   {
      _input = new InputSystem_Actions();
      _input.Enable();
   }

   private void Update()
   {
      var move = _input.Player.Move.ReadValue<Vector2>();

      weel.DOKill();
      weel.DOLocalRotate(new Vector3(0, 0, -move.x * maxAngle), rotateDuration).SetAutoKill(true).SetEase(rotateEase);
   }
}
