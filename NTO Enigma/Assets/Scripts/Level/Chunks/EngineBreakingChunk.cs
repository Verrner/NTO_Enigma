using UnityEngine;
using Random = UnityEngine.Random;

namespace NTO
{
    public class EngineBreakingChunk : Chunk
    {
        [SerializeField, Range(0, 100)] private float chanceOfBreaking;

        private SubmarineEngineBreak _engineBreak;
        
        private void Awake()
        {
            _engineBreak = FindFirstObjectByType<SubmarineEngineBreak>();
        }

        protected override void SubmarineEntered(Submarine submarine)
        {
            _engineBreak.EngineBroken = true;
        }

        protected override void SubmarineLeave(Submarine submarine){}

        public override void SubmarineInside(Submarine submarine){}
    }
}