using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.BrumBrum
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        // system vars
        private InputSystem_Actions _input;
        private Rigidbody _rb;

        //[SerializeField] private float carMass;
        [Header("Driving")] [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private float maxCarAccel = 1;

        [SerializeField] private float maxCarVelocity = 30;
        
        [Header("Steering")] 
        [SerializeField] private float carTurn = 1;

        // gameplay vars
        private Vector2 _move = Vector2.zero;

        private void Awake()
        {
            this._rb = this.GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            this._input = new InputSystem_Actions();
            this._input.Enable();

            this._input.Player.Move.performed += OnMove;
            this._input.Player.Move.canceled += OnMove;
        }

        void OnDisable()
        {
            this._input.Player.Move.performed -= OnMove;
            this._input.Player.Move.canceled -= OnMove;
            this._input.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            this._move = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            // don't scrape and drive
            if (this._input.Player.Scrape.IsPressed()) return;
            
            float curVel = this._rb.linearVelocity.magnitude;
            float curVel01 = Mathf.Clamp01(curVel / maxCarVelocity);
            AddSteering(curVel01);
            AddDrivingForce(curVel01);
        }

        private void AddDrivingForce(float curVel01)
        {
            float accel = this.accelerationCurve.Evaluate(curVel01) * maxCarAccel;
            
            // Forward Backward
            float forwardAccel = this._move.y * accel; 
            //   Debug.Log($"forAccel: {forwardAccel}, VelRel: {curVel01} Accel: {accel}");

            Vector3 forwardForceDir = forwardAccel * this.transform.forward;
            forwardForceDir.y = 0;

            this._rb.AddForce(forwardForceDir, ForceMode.Acceleration);

        }

        private void AddSteering(float curVel01)
        {
            if (this._rb.linearVelocity.sqrMagnitude > 0.1)
            {
                int dirMul = this._move.y < 0 ? -1 : 1;
                
                float turn = this._move.x * carTurn * curVel01 * dirMul;
                Vector3 turnDir = Vector3.up * turn;
                this._rb.AddTorque(turnDir);   
            }
        }
    }
}
