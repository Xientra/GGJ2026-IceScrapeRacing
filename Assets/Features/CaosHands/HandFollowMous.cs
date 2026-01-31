using System;
using UnityEngine;

public class HandFollowMous : MonoBehaviour
{
    [SerializeField] private Transform handTagetTransform;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform elbow;
    [SerializeField] private Transform sholder;
    [SerializeField] private Transform basee;
    [SerializeField] private float maxDistance;
    [SerializeField] private float offsetMultiplier = 1f;
    [SerializeField] private float offsetMultiplierR = 1f;
    [SerializeField] private AnimationCurve animationCurve;
    private InputSystem_Actions _input;
    private Camera _camera;
    private Vector3 _startBasePos;

    private void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();

        _camera = Camera.main;
        
        _startBasePos = basee.localPosition;
    }

    private void Update()
    {
        Vector2 mousePos = _input.Player.MousePosition.ReadValue<Vector2>();
        var ray = _camera.ScreenPointToRay(mousePos);
        
        handTagetTransform.position = ray.GetPoint(maxDistance);
       // (handTagetTransform.position - _startBasePos) * animationCurve.Evaluate(ray.direction.x) * offsetMultiplier
        basee.localPosition = _startBasePos + (_camera.transform.right + _camera.transform.up).normalized * animationCurve.Evaluate(ray.direction.x) * offsetMultiplierR ;
        
        hand.forward = (handTagetTransform.position - hand.position).normalized;
        //hand.position = handTagetTransform.position;
        
        elbow.up = (handTagetTransform.position - elbow.position).normalized;
        sholder.up = (handTagetTransform.position - sholder.position).normalized;

       

        // hand.rotation = handTagetTransform.rotation;

    }
}
