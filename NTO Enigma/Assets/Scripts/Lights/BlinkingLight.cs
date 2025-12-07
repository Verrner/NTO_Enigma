using UnityEngine;
using Random = UnityEngine.Random;

namespace NTO
{
    [RequireComponent(typeof(Light))]
    public class BlinkingLight : MonoBehaviour
    {
        [SerializeField, Min(0)] private float dimmingOffset;
        [SerializeField, Min(0)] private float dimmingOffsetRandom;
        [SerializeField, Min(0)] private float dimmingDuration;
        [SerializeField, Min(0)] private float dimmingDurationRandom;

        private Light _light;
        private float _startIntensity;

        private float _timeUntilDimmingEnd;
        private float _timeUntilNextDimming;
        private float _currentTime;

        private bool _dimming;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _startIntensity = _light.intensity;
        }

        private void Update()
        {
            if (_currentTime >= _timeUntilDimmingEnd && _dimming)
            {
                _dimming = false;
                _light.intensity = _startIntensity;
            }

            if (_currentTime >= _timeUntilNextDimming + _timeUntilDimmingEnd)
                SetDimming();
            
            _currentTime += Time.deltaTime;
        }

        private void SetDimming()
        {
            _timeUntilDimmingEnd = dimmingDuration + Random.Range(-dimmingDurationRandom / 2, dimmingDurationRandom / 2);
            _timeUntilNextDimming = dimmingOffset + Random.Range(-dimmingOffsetRandom / 2, dimmingOffsetRandom / 2);

            _light.intensity = 0;
            _dimming = true;
            _currentTime = 0;
        }
    }
}