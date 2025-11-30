using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public sealed class SubmarineOxygen : MonoBehaviour
    {
        [Header("General"), SerializeField, Min(0)] private float oxygenOnStart;
        [SerializeField, Min(0)] private float passiveOxygenUsagePerSecond;

        [Header("Tanks"), SerializeField] private EventInteractable[] oxygenTanks;
        [SerializeField] private CharacterOxygenTank characterOxygenTank;
        
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
            SetOxygenTanks();
        }

        private void Update()
        {
            Oxygen -= passiveOxygenUsagePerSecond * Time.deltaTime;
        }

        public void ResetOxygen()
        {
            Oxygen = oxygenOnStart;
        }

        private void SetOxygenTanks()
        {
            for (var i = 0; i < oxygenTanks.Length; i++)
            {
                var tank = oxygenTanks[i];
                tank.Interacted += _ => OxygenTankInteracted(tank.gameObject);
            }
        }

        private void OxygenTankInteracted(GameObject tank)
        {
            if (characterOxygenTank.TankGrabbed)
                return;

            Destroy(tank);
            characterOxygenTank.GrabTank();
        }
    }
}