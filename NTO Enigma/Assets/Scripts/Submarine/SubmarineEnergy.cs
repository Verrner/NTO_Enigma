using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public sealed class SubmarineEnergy : MonoBehaviour
    {
        [SerializeField, Min(0)] private float energyOnStart;
        
        private float _energy;

        public float Energy
        {
            get => _energy;
            set => _energy = Mathf.Max(0, value);
        }

        private void Awake()
        {
            ResetEnergy();
        }

        public void SpendEnergy(float amount)
        {
            Energy -= amount;
        }

        public void ResetEnergy()
        {
            Energy = energyOnStart;
        }
    }
}