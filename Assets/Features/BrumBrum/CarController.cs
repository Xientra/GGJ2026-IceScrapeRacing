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
        [SerializeField] private float carAccel = 1;
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
            this._input.Player.Move.canceled  += OnMove;
        }

        void OnDisable()
        {
            this._input.Player.Move.performed -= OnMove;
            this._input.Player.Move.canceled  -= OnMove;
            this._input.Player.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            this._move = context.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            // Forward Backward
            float forwardAccel = this._move.y * carAccel;

            // F = ma
            float forwardForce = this._rb.mass * forwardAccel;
            
            Vector3 forwardForceDir = forwardForce * this.transform.forward;

            this._rb.AddForce(forwardForceDir);
            
            
            // Turning
            float turn = this._move.y * this._move.x * carTurn;

            Vector3 turnDir = Vector3.up * turn;
            
            this._rb.AddTorque(turnDir);
        }
    }
}
