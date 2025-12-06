using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NTO
{
    public class AmbientAudio : MonoBehaviour
    {
        [Serializable]
        private sealed class AmbientSoundInfo
        {
            public AudioClip audio;
            [Min(0)] public float appearanceProbability;
        }
        
        [SerializeField] private AmbientSoundInfo[] ambientSounds;
        [SerializeField, Min(0)] private float thresholdFromStart;
        [SerializeField, Min(0)] private float ambientSoundsThreshold;
        [SerializeField] private float ambientSoundsThresholdRandomOffset;
        [SerializeField] private AudioSource ambientAudioSource;
        [SerializeField] private Bounds ambientAudioSourceRandomPositionBounds;

        private float _timeSinceAudioPlayed;
        private float _timeForNextAudioToChose;
        private float _currentAudioDuration;

        private void Awake()
        {
            _timeForNextAudioToChose = thresholdFromStart;
        }

        private void Update()
        {
            if (_timeSinceAudioPlayed >= _timeForNextAudioToChose + _currentAudioDuration)
                SetAmbient();
            
            _timeSinceAudioPlayed += Time.deltaTime;
        }

        private void SetAmbient()
        {
            var info = GetRandomInfo();
            var audio = info.audio;
            
            _currentAudioDuration = audio.length;
            _timeSinceAudioPlayed = 0;
            _timeForNextAudioToChose = Random.Range(ambientSoundsThreshold - ambientSoundsThresholdRandomOffset / 2,
                ambientSoundsThreshold + ambientSoundsThresholdRandomOffset / 2);
            
            ambientAudioSource.clip = audio;
            var minSourceLocalPosition = ambientAudioSourceRandomPositionBounds.center -
                                    ambientAudioSourceRandomPositionBounds.size / 2;
            var maxSourceLocalPosition = ambientAudioSourceRandomPositionBounds.center +
                                    ambientAudioSourceRandomPositionBounds.size / 2;
            var localPosition = new Vector3(Random.Range(minSourceLocalPosition.x, maxSourceLocalPosition.x),
                                            Random.Range(minSourceLocalPosition.y, maxSourceLocalPosition.y),
                                            Random.Range(minSourceLocalPosition.z, maxSourceLocalPosition.z));
            ambientAudioSource.transform.localPosition = localPosition;
            
            ambientAudioSource.Play();
        }

        private AmbientSoundInfo GetRandomInfo()
        {
            var sum = ambientSounds.Sum(i => i.appearanceProbability);
            var previousChances = 0f;

            foreach (var info in ambientSounds)
            {
                if (info.appearanceProbability <= Random.Range(0, sum - previousChances))
                    return info;
                previousChances += info.appearanceProbability;
            }

            return ambientSounds[0];
        }
    }
}