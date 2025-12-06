using UnityEngine;

namespace NTO
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterInteraction interaction;
        
        public CharacterMovement Movement => movement;
        public CharacterInteraction Interaction => interaction;
    }
}