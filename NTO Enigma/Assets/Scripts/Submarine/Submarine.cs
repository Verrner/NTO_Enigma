using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(SubmarineEnergy), typeof(SubmarineMovement), typeof(SubmarineRotation)),
     RequireComponent(typeof(SubmarineLoading), typeof(SubmarineOxygen), typeof(SubmarinePressure)),
     RequireComponent(typeof(SubmarineLevelBounds), typeof(SubmarineSpeedChanging))]
    public sealed class Submarine : MonoBehaviour
    {
        [SerializeField] private SubmarineMovement movement;
        [SerializeField] private SubmarineEnergy energy;
        [SerializeField] private SubmarineOxygen oxygen;
        [SerializeField] private SubmarinePressure pressure;
        [SerializeField] private SubmarineSpeedChanging speedChanging;
        
        public SubmarineMovement Movement => movement;
        public SubmarineEnergy Energy => energy;
        public SubmarineOxygen Oxygen => oxygen;
        public SubmarinePressure Pressure => pressure;
        public SubmarineSpeedChanging SpeedChanging => speedChanging;
    }
}