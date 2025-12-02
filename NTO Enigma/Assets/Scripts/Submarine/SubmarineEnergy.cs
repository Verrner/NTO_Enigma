using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public sealed class SubmarineEnergy : MonoBehaviour
    {
        [Header("General"), SerializeField, Min(0)] private float energyOnStart;
        [SerializeField] private Submarine submarine;

        [Header("Pressure"), SerializeField, Min(0)] private float pressureAdding = 1000;
        [SerializeField] private string deathSentenceKey = "energy-lack";
        [SerializeField] private string sourceKey = "energy";
        
        private float _energy;

        public float Energy
        {
            get => _energy;
            set
            {
                _energy = Mathf.Max(0, value);
                if (_energy == 0)
                    submarine.Pressure.AddPressure(pressureAdding, sourceKey, deathSentenceKey, this);
                else
                    submarine.Pressure.RemovePressure(sourceKey);
            }
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