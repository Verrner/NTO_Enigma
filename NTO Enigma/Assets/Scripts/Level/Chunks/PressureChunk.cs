using UnityEngine;

namespace NTO
{
    public class PressureChunk : Chunk
    {
        [SerializeField] private float pressureAdding;
        [SerializeField] private string pressureSourceKeyPrefix = "pressure-chunk";
        [SerializeField] private string deathSentenceKey = "fast-currents";

        private SubmarinePressure _pressure;
        private string _pressureSourceKey;

        private void Awake()
        {
            _pressure = FindFirstObjectByType<SubmarinePressure>();
            _pressureSourceKey = $"{pressureSourceKeyPrefix}-{index.x}-{index.y}-{index.z}";
        }

        protected override void SubmarineEntered(Submarine submarine)
        {
            _pressure.AddPressure(pressureAdding, _pressureSourceKey, deathSentenceKey, this);
        }

        protected override void SubmarineLeave(Submarine submarine)
        {
            _pressure.RemovePressure(_pressureSourceKey);
        }

        public override void SubmarineInside(Submarine submarine){}
    }
}