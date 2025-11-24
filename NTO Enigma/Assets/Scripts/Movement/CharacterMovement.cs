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

        public bool CanMove
        {
            get => canMove;
            set => canMove = value;
        }
        
        public bool CanRun
        {
            get => canRun;
            set => canRun = value;
        }
        
        public KeyCode RunningKey
        {
            get => runningKey;
            set => runningKey = value;
        }
        
        public float Speed
        {
            get => speed;
            set => speed = Mathf.Max(0, value);
        }
        
        public float RunningSpeedMultiplier
        {
            get => runningSpeedMultiplier;
            set => runningSpeedMultiplier = Mathf.Max(1, value);
        }
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var currentSpeed = CanMove ? speed * (CanRun && Input.GetKey(RunningKey) ? runningSpeedMultiplier : 1) : 0;
            var targetVelocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * currentSpeed * Time.deltaTime;
            _rigidbody.linearVelocity = _rigidbody.rotation * new Vector3(targetVelocity.x, _rigidbody.linearVelocity.y, targetVelocity.y);
        }
    }
}