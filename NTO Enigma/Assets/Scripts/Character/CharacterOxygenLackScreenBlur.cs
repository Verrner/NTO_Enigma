using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(Character))]
    public sealed class CharacterOxygenLackScreenBlur : MonoBehaviour
    {
        [SerializeField] private SubmarineOxygen submarineOxygen;
        [SerializeField] private AnimationCurve blurringTimeMultiplier;
        [SerializeField] private AnimationCurve blurring;
        [SerializeField] private UIDocument blurUIDocument;
        [SerializeField] private string blurBackgroundName = "blur-background";

        private float _timeUntilDeath;
        private float _blurringCycleLength;

        private float _blurringTimeSinceOxygenOver;
        private float _actualTimeSinceOxygenOver;

        private VisualElement _blurBackground;
        
        private void OnEnable()
        {
            _blurBackground = blurUIDocument.rootVisualElement.Q(blurBackgroundName);
            _blurBackground.style.opacity = 0;
            
            submarineOxygen.OxygenOver += OxygenOver;
            submarineOxygen.OxygenAvailable += () =>
            {
                _blurringTimeSinceOxygenOver = 0;
                _actualTimeSinceOxygenOver = 0;
            };
            
            if (blurringTimeMultiplier.length <= 1) throw new Exception("blurringTimeMultiplier must contain at least 2 key points");
            if (blurring.length <= 1) throw new Exception("blurring must contain at least 2 key points");
            
            _timeUntilDeath = blurringTimeMultiplier.keys[blurringTimeMultiplier.length - 1].time;
            _blurringCycleLength = blurring.keys[blurring.length - 1].time;
        }

        public void SetTimes(float blurringTime, float actualTime)
        {
            _blurringTimeSinceOxygenOver = blurringTime;
            _actualTimeSinceOxygenOver = actualTime;
        }

        public (float, float) GetTimes() => (_blurringTimeSinceOxygenOver, _actualTimeSinceOxygenOver);

        private void OxygenOver()
        {
            if (_actualTimeSinceOxygenOver >= _timeUntilDeath)
            {
                Died();
                return;
            }

            var opacity = blurring.Evaluate(_blurringTimeSinceOxygenOver % _blurringCycleLength);
            _blurBackground.style.opacity = opacity;
            _actualTimeSinceOxygenOver += Time.deltaTime;
            _blurringTimeSinceOxygenOver +=
                Time.deltaTime * blurringTimeMultiplier.Evaluate(_actualTimeSinceOxygenOver);
        }

        private void Died()
        {
            Death.ShowDeath("oxygen-lack-sentence", this);
        }
    }
}