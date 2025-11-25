using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(SubmarineEnergy), typeof(SubmarineMovement), typeof(SubmarineRotation))]
    public sealed class Submarine : MonoBehaviour
    {
        [SerializeField] private SubmarineEnergy energy;
        [SerializeField] private SubmarineMovement movement;
        [SerializeField] private SubmarineRotation rotation;
        
        public SubmarineEnergy Energy => energy;
        public SubmarineMovement Movement => movement;
        public SubmarineRotation Rotation => rotation;
    }
}