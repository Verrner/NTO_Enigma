using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceVolume : MonoBehaviour
    {
        [SerializeField, Min(0)] private float multiplier = 1;
        
        private AudioSource _audioSource;

        private void OnEnable()
        {
            _audioSource = GetComponent<AudioSource>();
            RefreshVolume();
            AudioSettings.VolumeChanged += RefreshVolume;
        }

        private void OnDisable()
        {
            AudioSettings.VolumeChanged -= RefreshVolume;
        }

        private void RefreshVolume()
        {
            _audioSource.volume = AudioSettings.Volume * multiplier;
        }
    }
}