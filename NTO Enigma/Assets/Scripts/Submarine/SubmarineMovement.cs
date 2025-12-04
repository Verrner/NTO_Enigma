using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine), typeof(SubmarineEnergy))]
    public sealed class SubmarineMovement : MonoBehaviour
    {
        public enum SpeedMode
        {
            Default,
            Fast,
            Silent
        }
        
        [Header("General"), SerializeField, Min(0)] private float speed;
        [SerializeField, Min(0)] private float energyPerSecond;
        [SerializeField] private Level level;
        [SerializeField] private SubmarineEngineBreak engineBreak;
        
        [Header("Swift Mode"), SerializeField, Min(0)] private float swiftModeSpeedMultiplier;
        [SerializeField, Min(0)] private float swiftModeEnergyUsageMultiplier;
        
        [Header("Silent Mode"), SerializeField, Min(0)] private float silentModeSpeedMultiplier;
        [SerializeField, Min(0)] private float silentModeEnergyUsageMultiplier;

        public SpeedMode speedMode;

        private SubmarineEnergy _energy;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _energy = GetComponent<SubmarineEnergy>();
            _rigidbody = GetComponent<Rigidbody>();
            SetStartPosition();
        }

        private void SetStartPosition()
        {
            transform.position = new Vector3(level.LevelSize.x / 2, level.LevelSize.y - 1, 0) * level.ChunkSize +
                                 Vector3.one * level.ChunkSize / 2;
        }
        
        private void Update()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var curSpeed = speed * (engineBreak.engineBroken ? silentModeSpeedMultiplier : speedMode switch
            {
                SpeedMode.Default => 1,
                SpeedMode.Fast => swiftModeSpeedMultiplier,
                _ => silentModeSpeedMultiplier
            }) * Time.deltaTime;

            _rigidbody.linearVelocity = _rigidbody.rotation * new Vector3(0, 0, curSpeed);
            
            _energy.SpendEnergy(energyPerSecond * Time.deltaTime * (engineBreak.engineBroken ? 1 : speedMode switch
            {
                SpeedMode.Default => 1,
                SpeedMode.Fast => swiftModeEnergyUsageMultiplier,
                _ => silentModeEnergyUsageMultiplier
            }));
        }
    }
}