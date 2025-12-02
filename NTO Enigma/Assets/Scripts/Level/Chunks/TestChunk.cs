using UnityEngine;

namespace NTO
{
    public sealed class TestChunk : Chunk
    {
        private bool _inside;
        
        public override void SubmarineEntered(Submarine submarine)
        {
            _inside = true;
            Debug.Log($"Entered {index}");
        }

        public override void SubmarineLeave(Submarine submarine)
        {
            _inside = false;
            Debug.Log($"Leave {index}");
        }

        public override void SubmarineInside(Submarine submarine)
        {
            Debug.Log($"Inside {index}");
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