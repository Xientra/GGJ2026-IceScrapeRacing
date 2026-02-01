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

    private void Start()
    {
        radioAudioSource.clip = audioClips[_currentIndex];
        radioAudioSource.Play();
    }

    public void OnInteract()
    {
        redButton.DOLocalMove(redButton.localPosition + (redButton.forward * pressOffset), 0.2f).SetLoops(2, LoopType.Yoyo)
            .SetAutoKill(true)
            .SetEase(Ease.OutExpo);

        radioAudioSource.clip = audioClips[_currentIndex++];
        radioAudioSource.Play();
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