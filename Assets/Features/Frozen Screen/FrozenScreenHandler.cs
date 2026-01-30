using UnityEngine;

public class FrozenScreenHandler : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexture;
    public int circleSize = 64;

    int kernel;

    private void Start()
    {
        kernel = computeShader.FindKernel("CSMain");

        computeShader.SetTexture(kernel, "Result", renderTexture);
        computeShader.SetInt("resolution", renderTexture.width);
        computeShader.SetInt("circleSize", -1);

        // pass floats explicitly (use height if needed)
        computeShader.SetFloats("mousePosition", renderTexture.width * 0.5f, renderTexture.width * 0.5f);
    }

    [ContextMenu("DoStep")]
    private void Update()
    //private void Update()
    {
        if (circleSize <= 0 || circleSize > renderTexture.width)
        {
            circleSize = 1;
        }

        Debug.Log("Hi");

        computeShader.SetInt("circleSize", circleSize);
        computeShader.Dispatch(kernel, renderTexture.width, renderTexture.height, 1);
    }
}