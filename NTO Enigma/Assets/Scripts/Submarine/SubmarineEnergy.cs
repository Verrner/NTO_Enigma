using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public sealed class SubmarineEnergy : MonoBehaviour
    {
        [SerializeField, Min(0)] private float energyFromStart;
        
        private float _energy;

        public event Action<float> EnergyChanged;
        public event Action EnergyEnded;

        private void Awake()
        {
            ResetEnergy();
        }

        public void SpendEnergy(float amount)
        {
            _energy = Mathf.Max(0, _energy - amount);
            EnergyChanged?.Invoke(_energy);
            if (_energy == 0)
                EnergyEnded?.Invoke();
        }

        public void ResetEnergy()
        {
            _energy = energyFromStart;
            EnergyChanged?.Invoke(_energy);
        }
    }
}