using System.Collections;
using DG.Tweening;
using EPOOutline;
using Features.BrumBrum;
using UnityEngine;

public class Ignishen : MonoBehaviour, IInteracttable
{
    [SerializeField] private Transform key;
    [SerializeField] private Transform keyHole;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration;
    
    public float ignitionShakeMagnitude = 0.05f;

    public float ignitionTime;

    public ParticleSystem smokeVFX;

    public Outlinable outline;

    [Header("Audio")] 
    public AudioClip ignitionClip;
    public AudioSource ignitionAudio;
    public float ignitionVolume = 1f;

    private bool isHolding = false;
    private float holdTimer = 0f;
    

    public void OnInteract()
    {
        isHolding = true;
        holdTimer = 0f;

        key.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, -90), duration).SetAutoKill(true).SetEase(ease);
        keyHole.DOLocalRotateQuaternion(Quaternion.Euler(-180, 0, 90), duration).SetAutoKill(true).SetEase(ease);
        CarCameraShake.Instance.StartLoopShake(ignitionShakeMagnitude);
        
        ignitionAudio.PlayOneShot(ignitionClip,  ignitionVolume);
    }

    public void OnEndInteract()
    {
        isHolding = false;
        holdTimer = 0f;

        key.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.5f).SetAutoKill(true).SetEase(ease);
        keyHole.DOLocalRotateQuaternion(Quaternion.Euler(-180, 0, 0), 0.5f).SetAutoKill(true).SetEase(ease);

        if (!CarController.Instance.EngineOn)
        {
            CarCameraShake.Instance.StopLoopShake();
        }
        
        ignitionAudio.Stop();
    }

    public void OnHover()
    {
    }


    private void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= ignitionTime)
            {
                isHolding = false;
                holdTimer = 0f;
                OnIgnitionStarted();
                OnEndInteract();
            }
        }
    }

    private void OnIgnitionStarted()
    {
        IntroCutscene.Instance.EndIntro();
        CarController.Instance.OnToggleEngine(true);
        smokeVFX.Stop();
        outline.enabled = false;
        Debug.Log("Ignition started");
    }
    
    public void StartSmoke(bool value)
    {
        if(!value)
            smokeVFX.Play();
    }

    public void ToggleOutline(bool value)
    {
        Debug.Log("Toogle outline : " + value);
        outline.enabled = !value;
    }
}