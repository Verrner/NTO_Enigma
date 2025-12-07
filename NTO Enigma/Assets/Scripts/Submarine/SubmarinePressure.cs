using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarinePressure : MonoBehaviour
    {
        [Header("General"), SerializeField, Min(0)] private float maxPressure;
        [SerializeField, Min(0)] private float timeUntilDeath;
        
        [HideInInspector, LocalizationDynamicVariable("death-cause")] public string deathCause;

        private float _pressure;
        private bool _died;

        private float _pressureTimer;
        
        public float Pressure
        {
            get => _pressure;
            private set
            {
                _pressure = Mathf.Clamp(value, 0, maxPressure);
                if (_pressure < maxPressure)
                {
                    _pressureTimer = 0;
                    SubmarineAlarmSound.Stop("pressure");
                }
                else
                    SubmarineAlarmSound.Play("pressure");
            }
        }

        private sealed class PressureChanger
        {
            public readonly string Key;
            public readonly float PressureAdding;
            public readonly string DeathSentenceKey;
            public readonly object Source;

            public PressureChanger(string key, string deathSentenceKey, float pressureAdding, object source)
            {
                Key = key;
                DeathSentenceKey = deathSentenceKey;
                PressureAdding = pressureAdding;
                Source = source;
            }
        }
        
        private List<PressureChanger> _pressureChangers = new List<PressureChanger>();

        public void AddPressure(float pressure, string sourceKey, string deathSentenceKey, object source)
        {
            var pressureChanger = new PressureChanger(sourceKey, deathSentenceKey, pressure, source);
            _pressureChangers.Add(pressureChanger);
            Pressure += pressure;
        }

        public void RemovePressure(string sourceKey)
        {
            var pressureChanger = _pressureChangers.Find(x => x.Key == sourceKey);
            if (pressureChanger == null)
                return;
            _pressureChangers.Remove(pressureChanger);
            Pressure -= pressureChanger.PressureAdding;
        }

        private void Update()
        {
            if (Pressure < maxPressure || _died)
                return;

            if (_pressureTimer >= timeUntilDeath)
            {
                Died();
                return;
            }

            _pressureTimer += Time.deltaTime;
        }

        private void Died()
        {
            _died = true;
            var pressureChanger = _pressureChangers.Last();
            deathCause = LocalizationManager.GetValue(pressureChanger.DeathSentenceKey, pressureChanger.Source);
            LocalizationManager.LanguageChanged += () =>
                deathCause = LocalizationManager.GetValue(pressureChanger.DeathSentenceKey, pressureChanger.Source);
            DeathUI.ShowDeath("submarine-pressure-sentence", this);
        }
    }
}