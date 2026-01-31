using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class UIButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float minTime = 2f;
    public float maxTime = 5f;
    private float randomTime;
    
    private bool isHolding = false;
    private float holdTimer = 0f;

    private void Start()
    { 
        randomTime = Random.Range(minTime, maxTime);
    }

    public UnityEvent OnEngineToggle;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        holdTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        holdTimer = 0f;
    }

    private void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= randomTime)
            {
                isHolding = false;
                holdTimer = 0f;
                OnEngineToggle?.Invoke();
                randomTime = Random.Range(minTime, maxTime);
                Debug.Log("Button held for " + randomTime + " seconds!");
            }
        }
    }
}