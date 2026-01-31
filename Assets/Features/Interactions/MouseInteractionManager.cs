using System;
using EPOOutline;
using UnityEngine;

public class MouseInteractionManager : MonoBehaviour
{
    private  Camera _camera;
    private InputSystem_Actions _input;
    
    private Outlinable _lastOutlinable;
    private IInteracttable _lastInteracttable;
    
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
        bool endInteracting = _input.Player.Interact.WasReleasedThisFrame();

        if(scraping) return;
        
        if (Physics.Raycast(_camera.ScreenPointToRay(mousePos), out RaycastHit hit, 100))
        {
            var interacttable = hit.collider.gameObject.GetComponent<IInteracttable>();
            if (interacting)
            {
                // Interact
                if(interacttable != null)
                {
                    interacttable.OnInteract();
                    _lastInteracttable = interacttable;
                }
            }
            
            if(endInteracting)
            {
                if(_lastInteracttable != null)
                {
                    _lastInteracttable.OnEndInteract();
                    _lastInteracttable = null;
                }
            }
            
            if(interacttable != null)
            {
                interacttable.OnHover();
            }
            
            var outlinable = hit.collider.gameObject.GetComponent<Outlinable>();
                
            if(outlinable != null)
            {
                outlinable.enabled = true;
                _lastOutlinable = outlinable;
            }
            else
            {
                if (_lastOutlinable != null)
                {
                    _lastOutlinable.enabled = false;
                    _lastOutlinable = null;
                } 
            }
        }
        else
        {
            
            if(endInteracting)
            {
                if(_lastInteracttable != null)
                {
                    _lastInteracttable.OnEndInteract();
                    _lastInteracttable = null;
                }
            }
            
            if (_lastOutlinable != null)
            {
                _lastOutlinable.enabled = false;
                _lastOutlinable = null;
            }
        }
    }
}

public interface IInteracttable
{
    public void OnInteract();
    public void OnEndInteract();
    
    public void OnHover();
}
