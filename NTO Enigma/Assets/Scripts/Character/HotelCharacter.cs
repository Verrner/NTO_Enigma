using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(CharacterInteraction))]
    public class HotelCharacter : CharacterMovement
    {
        protected override Vector3 AddVelocity() => Vector3.zero;
    }
}