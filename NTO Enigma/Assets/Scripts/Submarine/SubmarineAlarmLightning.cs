using System;
using System.Collections.Generic;
using UnityEngine;

namespace NTO
{
    public class SubmarineAlarmLightning : MonoBehaviour
    {
        [SerializeField] private Light[] lights;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color alarmColor;
        [SerializeField] private AnimationCurve intensityMultipliersCurve;

        private static SubmarineAlarmLightning _instance;
        private static bool _started;
        
        private float _alarmTime;

        private static List<string> _reasons = new List<string>();

        private void Awake()
        {
            _instance = this;
            foreach (var light in lights)
                light.color = defaultColor;
        }

        private void Update()
        {
            if (!_started)
                return;
            
            var multiplier = intensityMultipliersCurve.Evaluate(_alarmTime);
            foreach (var light in lights)
                light.intensity *= multiplier;
            
            _alarmTime += Time.deltaTime;
        }

        public static void Play(string reason)
        {
            if (_reasons.Contains(reason))
                return;
            _reasons.Add(reason);
            if (_started)
                return;
            foreach (var light in _instance.lights)
                light.color = _instance.alarmColor;
            _started = true;
            _instance._alarmTime = 0;
        }

        public static void Stop(string reason)
        {
            if (!_reasons.Contains(reason))
                return;
            _reasons.Remove(reason);
            if (_reasons.Count != 0 || !_started)
                return;
            var lastMultiplier =
                _instance.intensityMultipliersCurve.Evaluate(_instance.intensityMultipliersCurve.length);
            foreach (var light in _instance.lights)
            {
                light.color = _instance.defaultColor;
                light.intensity /= lastMultiplier;
            }
            _started = false;
        }
    }
}