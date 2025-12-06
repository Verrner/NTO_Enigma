using System;
using System.Collections.Generic;
using UnityEngine;

namespace NTO
{
    public class SubmarineAlarmSound : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        
        private static SubmarineAlarmSound _instance;

        private static readonly List<string> reasonsList = new List<string>();

        private void Awake()
        {
            _instance = this;
        }

        public static void Play(string reason)
        {
            if (reasonsList.Contains(reason))
                return;
            if (!_instance.source.isPlaying)
                _instance.source.Play();
            reasonsList.Add(reason);
        }

        public static void Stop(string reason)
        {
            if (!reasonsList.Contains(reason))
                return;
            if (reasonsList.Count == 1)
                _instance.source.Stop();
            reasonsList.Remove(reason);
        }
    }
}