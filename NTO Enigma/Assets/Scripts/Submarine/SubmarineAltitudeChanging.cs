using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineAltitudeChanging : MonoBehaviour
    {
        [Header("General"), SerializeField] private float altitudeChangingMultiplier;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;
        
        [Header("Objects"), SerializeField] private EventInteractable altitudeChangingInteractable;
        [SerializeField] private Transform altitudeChangingTransform;
        [SerializeField] private Character character;
        [SerializeField] private Rigidbody submarine;

        [Header("Animation"), SerializeField] private float animationSensitivity;
        [SerializeField] private float animationSpeed;
        [SerializeField] private float maxAnimationPosition;
        [SerializeField] private float minAnimationPosition;
        [SerializeField] private float defaultAnimationPosition;

        private bool _interacted;
        private float _targetAnimationPosition;

        private float _altitudeChangingActual = 0;
        
        private void Awake()
        {
            altitudeChangingInteractable.Interacted += _ => ChangeInteraction(true);
        }

        private void Update()
        {
            altitudeChangingTransform.localPosition = new Vector3(altitudeChangingTransform.localPosition.x,
                Mathf.Lerp(altitudeChangingTransform.localPosition.y, _targetAnimationPosition,
                    animationSpeed * Time.deltaTime), altitudeChangingTransform.localPosition.z);
            
            if (!_interacted)
                return;

            var mouseButtonClicked = Input.GetMouseButton(0);
            var altitudeChangingRaw = mouseButtonClicked ? Input.GetAxis("Mouse Y") : 0;
            var altitudeChangingAnimation = altitudeChangingRaw * animationSensitivity * Time.deltaTime;

            _altitudeChangingActual = mouseButtonClicked
                ? _altitudeChangingActual + altitudeChangingRaw * altitudeChangingMultiplier * Time.deltaTime
                : 0;
                

            submarine.linearVelocity = new Vector3(submarine.linearVelocity.x, _altitudeChangingActual, submarine.linearVelocity.z);
            _targetAnimationPosition = mouseButtonClicked
                ? Mathf.Clamp(_targetAnimationPosition + altitudeChangingAnimation, minAnimationPosition,
                    maxAnimationPosition)
                : defaultAnimationPosition;
            
            if (Input.GetKeyDown(exitKey))
                ChangeInteraction(false);
        }

        private void ChangeInteraction(bool interaction)
        {
            if (_interacted == interaction)
                return;
            
            _interacted = interaction;
            character.Movement.canMove = !interaction;
            character.Movement.canRotate = !interaction;
            character.Interaction.canInteract = !interaction;

            Cursor.visible = interaction;
            Cursor.lockState = interaction ? CursorLockMode.None : CursorLockMode.Locked;

            _targetAnimationPosition = 0;
        }
    }
}