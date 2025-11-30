using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public sealed class SubmarineOxygen : MonoBehaviour
    {
        [SerializeField, Min(0)] private float oxygenOnStart;
        [SerializeField, Min(0)] private float passiveOxygenUsagePerSecond;

        private float _oxygen;

        public event Action OxygenOver;
        public event Action OxygenAvailable;

        public float Oxygen
        {
            get => _oxygen;
            set
            {
                _oxygen = Mathf.Max(0, value);  
                if (_oxygen == 0) OxygenOver?.Invoke();
                else OxygenAvailable?.Invoke();
            }
        }

        private void Awake()
        {
            ResetOxygen();
        }

        private void Update()
        {
            Oxygen -= passiveOxygenUsagePerSecond * Time.deltaTime;
        }

        public void ResetOxygen()
        {
            Oxygen = oxygenOnStart;
        }
    }
}