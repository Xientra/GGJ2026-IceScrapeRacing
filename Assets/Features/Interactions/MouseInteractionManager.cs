using System;
using UnityEngine;

public class MouseInteractionManager : MonoBehaviour
{
    private  Camera _camera;
    private InputSystem_Actions _input;
    
    private void Start()
    {
        _camera =  Camera.main;
        
        _input = new InputSystem_Actions();
        _input.Enable();
    }

    private void Update()
    {
        Vector2 mousePos = _input.Player.MousePosition.ReadValue<Vector2>();
        bool scraping = _input.Player.Scrape.IsPressed();
        bool interacting = _input.Player.Interact.WasPressedThisFrame();

        if(scraping) return;
        
        
        if (Physics.Raycast(_camera.ScreenPointToRay(mousePos), out RaycastHit hit, 100))
        {
            if (interacting)
            {
                // Interact
                var hitt = hit.collider.gameObject.GetComponent<IInteracttable>();
                
                if(hitt != null)
                {
                    hitt.OnInteract();
                }
                
                // Heilight Intteractable
                
            }
        }
    }
}

public interface IInteracttable
{
    public void OnInteract();
}
