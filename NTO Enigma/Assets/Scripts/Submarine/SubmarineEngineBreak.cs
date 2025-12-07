using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineEngineBreak : MonoBehaviour
    {
        [SerializeField] private bool engineBroken;

        public bool EngineBroken
        {
            get => engineBroken;
            set
            {
                engineBroken = value;
                if (value)
                {
                    SubmarineAlarmSound.Play("engine-broken");
                    SubmarineAlarmLightning.Play("engine-broken");
                }
                else
                {
                    SubmarineAlarmSound.Stop("engine-broken");
                    SubmarineAlarmLightning.Stop("engine-broken");
                }

                EngineBreakingStateChanged?.Invoke(value);
            }
        }

        public event Action<bool> EngineBreakingStateChanged;
    }
}