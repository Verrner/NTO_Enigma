using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Character))]
    public sealed class CharacterLoading : MonoBehaviour
    {
        [SerializeField] private CharacterMovement characterMovement;
        [SerializeField] private CharacterInteraction characterInteraction;
        [SerializeField] private CharacterOxygenLackScreenBlur oxygenBlurring;
        [SerializeField] private CharacterOxygenTank oxygenTank;
        
        private void Awake()
        {
            var data = FindFirstObjectByType<CharacterSaving>();
            if (data.Loaded)
            {
                transform.localPosition = data.localPosition;
                transform.localRotation = data.localRotation;
                characterMovement.canMove = data.canMove;
                characterMovement.canRotate = data.canRotate;
                characterInteraction.canInteract = data.canInteract;
                oxygenBlurring.SetTimes(data.blurringTimeSinceOxygenOver, data.actualTimeSinceOxygenOver);
                oxygenTank.SetTankGrabbed(data.tankGrabbed);
            }

            var saving = FindFirstObjectByType<SavingManager>();
            saving.SavingStarted += () =>
            {
                data.localPosition = transform.localPosition;
                data.localRotation = transform.localRotation;
                data.canMove = characterMovement.canMove;
                data.canRotate = characterMovement.canRotate;
                data.canInteract = characterInteraction.canInteract;
                (data.blurringTimeSinceOxygenOver, data.actualTimeSinceOxygenOver) = oxygenBlurring.GetTimes();
                data.tankGrabbed = oxygenTank.TankGrabbed;
            };
        }
    }
}