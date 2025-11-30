using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(CharacterMovement), typeof(CharacterInteraction), typeof(Rigidbody)),
    RequireComponent(typeof(CharacterLoading), typeof(CharacterOxygenLackScreenBlur))]
    public sealed class Character : MonoBehaviour
    {
        [SerializeField] private CharacterMovement movement;
        [SerializeField] private CharacterInteraction interaction;
        
        public CharacterMovement Movement => movement;
        public CharacterInteraction Interaction => interaction;
    }
}