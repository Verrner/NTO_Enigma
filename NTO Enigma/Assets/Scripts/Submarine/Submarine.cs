using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(SubmarineEnergy), typeof(SubmarineMovement), typeof(SubmarineRotation)),
     RequireComponent(typeof(SubmarineLoading), typeof(SubmarineOxygen))]
    public sealed class Submarine : MonoBehaviour
    {
        [SerializeField] private SubmarineEnergy energy;
        [SerializeField] private SubmarineMovement movement;
        [SerializeField] private SubmarineRotation rotation;
        [SerializeField] private SubmarineOxygen oxygen;
        
        public SubmarineEnergy Energy => energy;
        public SubmarineMovement Movement => movement;
        public SubmarineRotation Rotation => rotation;
        public SubmarineOxygen Oxygen => oxygen;
    }
}