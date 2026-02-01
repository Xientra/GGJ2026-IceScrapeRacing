using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Features.BrumBrum
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        public static CarController Instance { get; private set; }

        public UnityEvent<bool> OnEngineToggle;
        public UnityEvent<bool> OnEngineBroken;
        
        // system vars
        private InputSystem_Actions _input;
        private Rigidbody _rb;

        //[SerializeField] private float carMass;
        [Header("Driving")] [SerializeField] private AnimationCurve accelerationCurve;
        [SerializeField] private float maxCarAccel = 1;

        [SerializeField] private float maxCarVelocity = 30;

        [Header("Steering")] [SerializeField] private AnimationCurve steeringCurve;
        [SerializeField] private float maxSteering = 20;

        [Header("Collision")] [SerializeField] private float crashSqrImpulseThreshold = 100;
        [SerializeField] private float crashDuration = 2.5f;
        [SerializeField] private int crashesTilSmoke = 2;
        
        // Sound
        [Header("Audio")]
        [SerializeField] private AudioSource engineAudio;
        [SerializeField] private AudioSource crashAudio;
        [SerializeField] private AnimationCurve enginePitchCurve;
        [SerializeField] private float engineOverPitch = 0.5f;
        [SerializeField] private float baseEnginePitch = 0.25f;
        [SerializeField] private float pitchDecelerationLerp = 2f;

        [Header("Other")] 
        [SerializeField] private float ignitionShakeMagnitude = 0.01f;
        [SerializeField] private float crashShakeMagnitude = 0.1f;

        // gameplay vars
        private Vector2 _move = Vector2.zero;
        private bool _engineOn = false;
        public bool EngineOn => _engineOn;
        private bool _engineBroken = false;
        private int _currentCrashes = 0;

        private void Awake()
        {
            Instance = this;
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
            SaveCar();
            // don't scrape and drive
            if (this._input.Player.Scrape.IsPressed() || !this._engineOn) return;
            
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
            if(Mathf.Abs(this._move.y) > 0)
            {
                float relAccel = Mathf.Abs(forwardAccel) / maxCarAccel;
                this.engineAudio.pitch = Mathf.Max(baseEnginePitch, enginePitchCurve.Evaluate(relAccel));
            } else if (this.engineAudio.pitch > baseEnginePitch + 0.1f)
            {
                this.engineAudio.pitch = Mathf.Lerp(this.engineAudio.pitch, baseEnginePitch, pitchDecelerationLerp);
            }
            else
            {
                this.engineAudio.pitch = baseEnginePitch;
            }

            Vector3 forwardForceDir = forwardAccel * this.transform.forward;
            forwardForceDir.y = 0;

            this._rb.AddForce(forwardForceDir, ForceMode.Acceleration);

        }

        private void AddSteering(float curVel01)
        {
            if (this._rb.linearVelocity.sqrMagnitude > 0.1)
            {
                int dirMul = this._move.y < 0 ? -1 : 1;

                float carTurn = this.steeringCurve.Evaluate(curVel01) * maxSteering;
                
                float turn = this._move.x * carTurn * dirMul;
                Vector3 turnDir = Vector3.up * turn;
                this._rb.AddTorque(turnDir);   
            }
        }

        private void SaveCar()
        {
            // save rotation
            Vector3 euler = transform.eulerAngles;
            float tiltX = Mathf.Abs(Mathf.DeltaAngle(euler.x, 0f));
            float tiltZ = Mathf.Abs(Mathf.DeltaAngle(euler.z, 0f));
            
            Quaternion targetRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            if (tiltX > 5f || tiltZ > 5f)
            {
                this.transform.rotation = Quaternion.Slerp(_rb.rotation, targetRot, 0.5f);
            } else if (tiltX is <= 5f and > 0|| tiltZ is <= 5f and > 0)
            {
                this.transform.rotation = targetRot;
            }

            // save position
            
            Vector3 targetPos = this.transform.position;
            targetPos.y = 0.25f;
            if (Mathf.Abs(this.transform.position.y) > 0.5f)
            {
                this.transform.position = Vector3.Lerp(this.transform.position,
                    targetPos, 0.5f);
            } else if (this.transform.position.y is <= 0.5f and > 0)
            {
                this.transform.position = targetPos;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.impulse.sqrMagnitude > this.crashSqrImpulseThreshold)
            {
                //sound
                float randPitch = Random.Range(0.5f, 1.5f);
                this.crashAudio.pitch = randPitch;
                this.crashAudio.Play();
                
                // stop engine
                this._engineOn = false;
                OnEngineToggle?.Invoke(_engineOn);
                
                // life handling
                this._currentCrashes++;
                if (this._currentCrashes == this.crashesTilSmoke)
                {
                    this._engineBroken = true;
                    OnEngineBroken?.Invoke(_engineBroken);
                }

            }
        }

        public void OnToggleEngine(bool on)
        {
            this._engineOn = on;
            if (on)
            {
                this.engineAudio.Play();
                CarCameraShake.Instance.StartLoopShake(ignitionShakeMagnitude);
            }
            else
            {
                this.engineAudio.Stop();
                StartCoroutine(CrashShake());
            }
        }
        
        private IEnumerator CrashShake()
        {
            CarCameraShake.Instance.StartLoopShake(crashShakeMagnitude);
            yield return new WaitForSeconds(0.1f);
            CarCameraShake.Instance.StopLoopShake();
        }
    }
}
