using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(SubmarineEnergy), typeof(SubmarineMovement), typeof(SubmarineRotation)),
     RequireComponent(typeof(SubmarineLoading), typeof(SubmarineOxygen), typeof(SubmarinePressure)),
     RequireComponent(typeof(SubmarineLevelBounds))]
    public sealed class Submarine : MonoBehaviour
    {
        [SerializeField] private SubmarineEnergy energy;
        [SerializeField] private SubmarineOxygen oxygen;
        [SerializeField] private SubmarinePressure pressure;
        
        public SubmarineEnergy Energy => energy;
        public SubmarineOxygen Oxygen => oxygen;
        public SubmarinePressure Pressure => pressure;
    }
}