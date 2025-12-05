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
                EngineBreakingStateChanged?.Invoke(value);
            }
        }

        public event Action<bool> EngineBreakingStateChanged;
    }
}