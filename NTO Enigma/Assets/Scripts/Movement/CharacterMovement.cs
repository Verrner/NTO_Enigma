using System;
using UnityEngine;

namespace NTO.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class CharacterMovement : MonoBehaviour
    {
        [Header("Movement"), SerializeField] private bool canMove = true;
        [SerializeField, Min(0)] private float speed;
        
        [Header("Running"), SerializeField] private bool canRun = true;
        [SerializeField] private KeyCode runningKey = KeyCode.LeftShift;
        [SerializeField, Min(1)] private float runningSpeedMultiplier;

        [Header("Camera Rotation"), SerializeField] private new Transform camera;
        [SerializeField] private bool canRotate = true;
        [SerializeField, Min(0)] private float sensitivity;
        [SerializeField, Min(0)] private float smoothing;
        [SerializeField] private bool lockCursor = true;
        
        private Rigidbody _rigidbody;
        private Vector2 _velocity;
        private Vector2 _frameVelocity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            CheckCursorLocking();
        }

        private void Update()
        {
            UpdateMovement();
            UpdateCameraRotation();
        }

        private void UpdateMovement()
        {
            var currentSpeed = canMove ? speed * (canRun && Input.GetKey(runningKey) ? runningSpeedMultiplier : 1) : 0;
            var targetVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * currentSpeed * Time.deltaTime;
            _rigidbody.linearVelocity = _rigidbody.rotation * new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.y);
        }

        private void UpdateCameraRotation()
        {
            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            var rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity * Time.deltaTime);
            _frameVelocity = Vector2.Lerp(_frameVelocity, rawFrameVelocity, 1 / smoothing);
            _velocity += _frameVelocity;
            _velocity.y = Mathf.Clamp(_velocity.y, -90, 90);
            
            camera.localRotation = Quaternion.AngleAxis(-_velocity.y, Vector3.right);
            transform.localRotation = Quaternion.AngleAxis(_velocity.x, Vector3.up);
        }

        private void CheckCursorLocking()
        {
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
        }
    }
}