using System;
using DG.Tweening;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{

    public float duration = 1.0f;

    public float onIntenstiy = 100f;

    private Light _light;
    
    private void Start()
    {
        _light = GetComponent<Light>();
    }

    public void SwitchLightOnOff(bool on)
    {
        if (on == false)
            DOVirtual.Float(onIntenstiy, 1.0f, duration / 2f, f => _light.intensity = f);

        if (on)
            DOVirtual.Float(0.0f, onIntenstiy, duration, f => _light.intensity = f);
    }
}
