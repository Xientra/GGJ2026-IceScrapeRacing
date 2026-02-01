using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MoveMatGlow : MonoBehaviour
{
    public float offIntensity = -2;

    public float onIntensity = 2;

    public float duration = 1.0f;
    
    public int materialIndex = 0;

    private Material _mat;
    
    private void Start()
    {
        
        Renderer r = GetComponent<Renderer>();
        _mat = r.materials[materialIndex];
    }

    public void SwitchOnOff(bool on)
    {
        if (on == false)
        {
            Color c = _mat.GetColor("_EmissionColor");
            c.a = Mathf.Lerp(offIntensity, onIntensity, 0.0f);
            _mat.SetColor("_EmissionColor", c);
        }

        if (on)
            DOVirtual.Float(0.0f, 1.0f, duration, f =>
            {
                Color c = _mat.GetColor("_EmissionColor");
                c.a = Mathf.Lerp(offIntensity, onIntensity, f);
                _mat.SetColor("_EmissionColor", c);
            });
    }

}
