using System;
using System.Collections.Generic;
using System.Linq;
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

        private sealed class OxygenTankData
        {
            public bool Spent;
            public GameObject Instance;

            public OxygenTankData(bool spent, GameObject instance)
            {
                Spent = spent;
                Instance = instance;
            }
        }
        
        private readonly List<OxygenTankData> _oxygenTankData = new List<OxygenTankData>();

        public event Action OxygenOver;
        public event Action OxygenAvailable;

        public float Oxygen
        {
            get => _oxygen;
            set
            {
                _oxygen = Mathf.Max(0, value);
                if (_oxygen == 0)
                {
                    SubmarineAlarmSound.Play("oxygen-lack");
                    SubmarineAlarmLightning.Play("oxygen-lack");
                    OxygenOver?.Invoke();
                }
                else
                {
                    SubmarineAlarmSound.Stop("oxygen-lack");
                    SubmarineAlarmLightning.Stop("oxygen-lack");
                    OxygenAvailable?.Invoke();
                }
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
                _oxygenTankData.Add(new OxygenTankData(false, tank.gameObject));
            }
        }

        private void OxygenTankInteracted(GameObject tank)
        {
            if (characterOxygenTank.TankGrabbed)
                return;

            var index = _oxygenTankData.FindIndex(d => d.Instance == tank);
            _oxygenTankData[index].Spent = true;
            Destroy(tank);
            characterOxygenTank.GrabTank();
        }

        public void DestroySpentTanks(bool[] spentTanks)
        {
            var length = Mathf.Min(oxygenTanks.Length, spentTanks.Length);
            for (var i = 0; i < length; i++)
            {
                if (!spentTanks[i])
                    continue;
                _oxygenTankData[i].Spent = true;
                Destroy(_oxygenTankData[i].Instance);
            }
        }

        public bool[] GetSpentTanks() => _oxygenTankData.Select(d => d.Spent).ToArray();
    }
}