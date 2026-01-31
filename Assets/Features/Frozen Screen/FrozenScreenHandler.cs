using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrozenScreenHandler : MonoBehaviour
{
    public LayerMask windowLayer;
    public ComputeShader computeShader;
    public RenderTexture renderTexture;

    public Texture scrapeMask;
    
    [Range(0.0f, 1.0f)]
    public float scrapeSize = 0.01f;

    public float clearStrength = 0.001f;
    
    [Header("Ice regrowth stuff")]
    public float iceRegrowth = 0.0001f;
    public float iceRegrowthNeighbourFactor = 2;
    public float maxIceRegrowthNeighbourFactor;
    public float iceRegrowthNeighbourTreshold = 0.5f;
    
    private InputSystem_Actions _input;
    private Camera _camera;
    private int _kernel;
    private Vector2 _mousePostLastFrame;

    private void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();
        
        _camera = Camera.main;
        _kernel = computeShader.FindKernel("CSMain");
        computeShader.SetTexture(_kernel, "Result", renderTexture);

        SetShaderVariables();
        ClearRenderTexture();
    }

    private void OnDestroy()
    {
        ClearRenderTexture();
    }

    private void ClearRenderTexture()
    {
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;
    }

    private void SetShaderVariables()
    {
        computeShader.SetInts("resolution", renderTexture.width, renderTexture.height);
        computeShader.SetFloat("scrapeSize", scrapeSize);
        computeShader.SetFloat("clearStrength", clearStrength);
        computeShader.SetFloat("iceRegrowth", iceRegrowth);
        computeShader.SetFloat("iceRegrowthNeighbourFactor", iceRegrowthNeighbourFactor);
        computeShader.SetFloat("iceRegrowthNeighbourThreshold", iceRegrowthNeighbourTreshold);
        computeShader.SetFloat("maxIceRegrowthNeighbourFactor", maxIceRegrowthNeighbourFactor);
        computeShader.SetTexture(_kernel, "ScrapeMask", scrapeMask);
    }

    private Vector2 GetWindowUV(Vector2 mousePos)
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(mousePos), out RaycastHit hit, 100, windowLayer))
        {
            Debug.DrawRay(_camera.ScreenPointToRay(mousePos).origin, _camera.ScreenPointToRay(mousePos).direction * 12, Color.blue);
            return hit.textureCoord;
        }

        return Vector2.zero;
    }

    private void Update()
    {
        Vector2 mousePos = _input.Player.MousePosition.ReadValue<Vector2>();
        bool scraping = _input.Player.Scrape.IsPressed();

        computeShader.SetBool("scraping", scraping);
        Vector2 windowUV = GetWindowUV(mousePos);

        //if (!scraping || _mousePostLastFrame == mousePos)
        //    return;
        //_mousePostLastFrame = mousePos;

        SetShaderVariables();
        computeShader.SetFloats("scrapeUV", windowUV.x, windowUV.y);
        computeShader.SetFloat("deltaTime", Time.deltaTime);

        computeShader.Dispatch(_kernel, renderTexture.width, renderTexture.height, 1);
    }
}