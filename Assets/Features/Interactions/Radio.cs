using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Radio : MonoBehaviour, IInteracttable
{
    [SerializeField] private List<AudioClip> audioClips;
    [SerializeField] private Transform redButton;
    [SerializeField] private AudioSource radioAudioSource;
    [SerializeField] private float pressOffset = 0.1f;
    private int _currentIndex = 0;

    private float[] _times;
    
    private void Start()
    {
        radioAudioSource.clip = audioClips[_currentIndex];
        radioAudioSource.Play();

        _times = new float[audioClips.Count];
    }

    public void OnInteract()
    {
        redButton.DOLocalMove(redButton.localPosition + (redButton.forward * pressOffset), 0.2f).SetLoops(2, LoopType.Yoyo)
            .SetAutoKill(true)
            .SetEase(Ease.OutExpo).Restart();

        _times[_currentIndex] = radioAudioSource.time;

        _currentIndex = (_currentIndex + 1) % audioClips.Count;
        
        radioAudioSource.clip = audioClips[_currentIndex];
        radioAudioSource.Play();
        radioAudioSource.time = _times[_currentIndex];
    }

    public void OnEndInteract()
    {
       
    }

    public void OnHover()
    {
       
    }

    public void EndOnHover()
    {
        
    }
}