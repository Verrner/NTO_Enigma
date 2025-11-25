using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NTO
{
    [RequireComponent(typeof(Submarine), typeof(Rigidbody))]
    public sealed class SubmarineRotation : MonoBehaviour
    {
        [SerializeField, Min(0)] private float sensitivity;
        [SerializeField] private EventInteractable wheelInteractable;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;
        
        private Rigidbody _rigidbody;

        private bool _turning;
        private float _rotation;

        private Character _character;

        private Vector3 _localCharacterPosition;
        private Quaternion _localCharacterRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rotation = _rigidbody.rotation.eulerAngles.y;
            wheelInteractable.Interacted += WheelInteracted;
        }

        private void Update()
        {
            if (!_turning)
                return;
            
            _rotation += Input.GetAxis("Horizontal") * sensitivity * Time.deltaTime;
            _rigidbody.rotation = Quaternion.Euler(0, _rotation, 0);
            _character.transform.localPosition = _localCharacterPosition;
            _character.transform.localRotation = _localCharacterRotation;

            if (!Input.GetKeyDown(exitKey))
                return;
            
            _character.Movement.canMove = true;
            _character.Movement.canRotate = true;
            _character.Interaction.canInteract = true;
            _turning = false;
        }

        private void WheelInteracted(CharacterInteraction character)
        {
            if (_turning)
                return;

            _turning = true;

            character.canInteract = false;
            character.Character.Movement.canMove = false;
            character.Character.Movement.canRotate = false;

            _localCharacterPosition = character.transform.localPosition;
            _localCharacterRotation = character.transform.localRotation;

            _character = character.Character;
        }
    }
}