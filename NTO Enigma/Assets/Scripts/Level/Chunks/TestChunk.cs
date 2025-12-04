using UnityEngine;

namespace NTO
{
    public sealed class TestChunk : Chunk
    {
        private bool _inside;
        
        public override void SubmarineEntered(Submarine submarine)
        {
            _inside = true;
        }

        public override void SubmarineLeave(Submarine submarine)
        {
            _inside = false;
        }

        public override void SubmarineInside(Submarine submarine)
        {
            
        }

        private void OnDrawGizmos()
        {
            if (!_inside)
                return;
            Gizmos.color = Color.blue;
            var bounds = GetBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}