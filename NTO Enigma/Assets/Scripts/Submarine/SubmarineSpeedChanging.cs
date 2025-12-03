using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineSpeedChanging : MonoBehaviour
    {
        [Header("Objects"), SerializeField] private SubmarineMovement submarineMovement;
        [SerializeField] private Character character;
        [SerializeField] private EventInteractable interactable;
        [SerializeField] private Transform leverRootTransform;
        [SerializeField] private float leverRotationSpeed;
        
        [Header("Keys"), SerializeField] private KeyCode fastModeKey = KeyCode.F;
        [SerializeField] private KeyCode silentModeKey = KeyCode.S;
        [SerializeField] private KeyCode defaultModeKey = KeyCode.D;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;

        private bool _interacted;
        private float _targetRotation;

        private void Awake()
        {
            interactable.Interacted += _ => SetInteraction(true);
        }

        private void SetInteraction(bool interaction)
        {
            if (_interacted == interaction)
                return;
            
            _interacted = interaction;
            character.Movement.canMove = !interaction;
            character.Movement.canRotate = !interaction;
            character.Interaction.canInteract = !interaction;
        }

        private void Update()
        {
            leverRootTransform.localRotation = Quaternion.Euler(
                Mathf.LerpAngle(leverRootTransform.localRotation.eulerAngles.x, _targetRotation,
                    leverRotationSpeed * Time.deltaTime), 0, 0);
            
            if (!_interacted)
                return;
            
            if (Input.GetKeyDown(fastModeKey))
                ChangeMode(SubmarineMovement.SpeedMode.Fast);
            else if (Input.GetKeyDown(silentModeKey))
                ChangeMode(SubmarineMovement.SpeedMode.Silent);
            else if (Input.GetKeyDown(defaultModeKey))
                ChangeMode(SubmarineMovement.SpeedMode.Default);
            else if (Input.GetKeyDown(exitKey))
                SetInteraction(false);
        }

        public void ChangeMode(SubmarineMovement.SpeedMode mode)
        {
            submarineMovement.speedMode = mode;
            _targetRotation = mode switch
            {
                SubmarineMovement.SpeedMode.Default => 0,
                SubmarineMovement.SpeedMode.Silent => -45,
                SubmarineMovement.SpeedMode.Fast => 45,
                _ => 0
            };
        }
    }
}