using System;
using UnityEngine;

namespace NTO
{
    public sealed class Ladder : MonoBehaviour
    {
        [Header("Up"), SerializeField] private EventInteractable climbUpInteractable;
        [SerializeField] private Vector3 localPositionAfterClimbingUp;
        [SerializeField] private Vector3 localRotationAfterClimbingUp;
        
        [Header("Down"), SerializeField] private EventInteractable climbDownInteractable;
        [SerializeField] private Vector3 localPositionAfterClimbingDown;
        [SerializeField] private Vector3 localRotationAfterClimbingDown;

        private void Awake()
        {
            climbUpInteractable.Interacted += character =>
                SetClimbingPosition(character.transform, localPositionAfterClimbingUp, localRotationAfterClimbingUp);
            climbDownInteractable.Interacted += character =>
                SetClimbingPosition(character.transform, localPositionAfterClimbingDown, localRotationAfterClimbingDown);
        }

        private void SetClimbingPosition(Transform character, Vector3 position, Vector3 rotation)
        {
            character.localPosition = position;
            character.localRotation = Quaternion.Euler(rotation);
        }
    }
}