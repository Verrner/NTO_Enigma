using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Rigidbody), typeof(Character))]
    public sealed class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody submarineRigidbody;
        
        [Header("Movement"), SerializeField] public bool canMove = true;
        [SerializeField, Min(0)] private float speed;

        [Header("Camera Rotation"), SerializeField] private new Transform camera;
        [SerializeField] public bool canRotate = true;
        [SerializeField, Min(0)] private float sensitivity;
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
        
        public bool Moving { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            CheckCursorLocking();

            _baseCameraHeight = camera.localPosition.y;
        }

        private void Update()
        {
            UpdateMovement();
            UpdateCameraTwitch();
            UpdateCameraRotation();
        }

        private void UpdateMovement()
        {
            var targetVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime * (canMove ? 1 : 0);
            _rigidbody.linearVelocity = _rigidbody.rotation * new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.y) +
                                        submarineRigidbody.linearVelocity;

            if (targetVelocity.magnitude != 0 == Moving) return;
            
            _twitchTime = 0;
            Moving = targetVelocity.magnitude != 0;
            camera.localPosition = new Vector3(camera.localPosition.x, _baseCameraHeight, camera.localPosition.z);
        }

        private void UpdateCameraTwitch()
        {
            if (!Moving || !canTwitch || twitchCurve.length < 2) return;
            
            _twitchTime = (_twitchTime + Time.deltaTime * twitchSpeed) % twitchCurve.keys[twitchCurve.length - 1].time;
            
            var twitch = twitchCurve.Evaluate(_twitchTime) * twitchAmplitude;

            camera.localPosition = new Vector3(camera.localPosition.x, _baseCameraHeight + twitch, camera.localPosition.z);
        }

        private void UpdateCameraRotation()
        {
            if (!canRotate) return;
            
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