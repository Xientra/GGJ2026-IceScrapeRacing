using System;
using UnityEngine;

public class LightsOutManager : MonoBehaviour
{

    public static LightsOutManager instancce;
    
    public LightSwitch[] lights;

    public TimeUI timer;

    public MoveMatGlow[] materials;

    private void Awake()
    {
        instancce = this;
    }

    private void Start()
    {
        TurnTheLightsWhatever(false);
        timer.SwitchOnOff(true);
    }

    public void TurnTheLightsWhatever(bool on)
    {
        FindAnyObjectByType<Radio>().GetComponent<AudioSource>().volume = on ? 1.0f : 0.4f;
        
        foreach (LightSwitch l in lights)
        {
            l.SwitchLightOnOff(on);
        }

        foreach (MoveMatGlow moveMatGlow in materials)
        {
            moveMatGlow.SwitchOnOff(on);
        }
        
        timer.SwitchOnOff(on);
    }

}
