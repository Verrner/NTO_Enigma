using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace NTO
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody submarineRigidbody;
        
        [Header("Movement"), SerializeField] public bool canMove = true;
        [SerializeField, Min(0)] private float speed;
        [SerializeField] private new CharacterAudio audio;

        [Header("Camera Rotation"), SerializeField] private new Transform camera;
        [SerializeField] public bool canRotate = true;
        [SerializeField, Min(0)] private float smoothing;
        [SerializeField] private bool lockCursor = true;

        [Header("Camera Twitch"), SerializeField] private bool canTwitch = true;
        [SerializeField] private AnimationCurve twitchCurve;
        [SerializeField] private float twitchSpeed;
        [SerializeField] private float twitchAmplitude;
        
        private Rigidbody _rigidbody;
        
        private Vector2 _velocity;
        private Vector2 _frameVelocity;

        private float _twitchTime;
        private float _baseCameraHeight;

        private CharacterSaving _characterSaving;
        
        public bool Moving { get; private set; }

        private void Awake()
        {
            _characterSaving = FindFirstObjectByType<CharacterSaving>();
            
            _rigidbody = GetComponent<Rigidbody>();
            
            CheckCursorLocking();

            _baseCameraHeight = camera.localPosition.y;

            SettingsUI.SettingsOpened += () =>
            {
                canMove = false;
                canRotate = false;
            };
            SettingsUI.SettingsClosed += () =>
            {
                canMove = true;
                canRotate = true;
            };
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateCameraTwitch();
            UpdateCameraRotation();
        }
        
        private void UpdateMovement()
        {
            var targetVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized * speed;
            targetVelocity *= canMove ? 1 : 0;
            _rigidbody.linearVelocity = _rigidbody.rotation * new Vector3(targetVelocity.x, 0, targetVelocity.y) +
                                        AddVelocity();
            
            if (targetVelocity.magnitude != 0 == Moving) return;
            
            _twitchTime = 0;
            Moving = targetVelocity.magnitude != 0;
            audio.RefreshStepsSource(Moving);
            camera.localPosition = new Vector3(camera.localPosition.x, _baseCameraHeight, camera.localPosition.z);
        }

        protected virtual Vector3 AddVelocity() => submarineRigidbody.linearVelocity;
        
        private void UpdateCameraTwitch()
        {
            if (!Moving || !canTwitch || twitchCurve.length < 2) return;
            
            _twitchTime = (_twitchTime + twitchSpeed) % twitchCurve.keys[twitchCurve.length - 1].time;
            
            var twitch = twitchCurve.Evaluate(_twitchTime) * twitchAmplitude;

            camera.localPosition = new Vector3(camera.localPosition.x, _baseCameraHeight + twitch, camera.localPosition.z);
        }

        private void UpdateCameraRotation()
        {
            var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            var rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * _characterSaving.sensitivity);
            _frameVelocity = !canRotate ? Vector2.zero : Vector2.Lerp(_frameVelocity, rawFrameVelocity, 1 / smoothing);
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