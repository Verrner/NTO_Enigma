using System;
using System.Globalization;
using UnityEngine;

namespace NTO
{
    public class AudioSettings : MonoBehaviour, ISavingReceiver
    {
        private static float _volume = 1;
        
        public static float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                VolumeChanged?.Invoke();
            }
        }

        public string GetSavedData() => Volume.ToString(CultureInfo.CurrentCulture);

        public void LoadData(string data) => Volume = float.Parse(data);

        public string Id => "audio";

        public static event Action VolumeChanged;
    }
}