using System;
using UnityEngine;

namespace NTO
{
    public abstract class Chunk : MonoBehaviour
    {
        public Level level;
        public Vector3Int index;
        public bool Inside { get; private set; }

        public void Enter(Submarine submarine)
        {
            Inside = true;
            SubmarineEntered(submarine);
        }

        public void Leave(Submarine submarine)
        {
            Inside = false;
            SubmarineLeave(submarine);
        }
        
        protected abstract void SubmarineEntered(Submarine submarine);
        protected abstract void SubmarineLeave(Submarine submarine);
        public abstract void SubmarineInside(Submarine submarine);

        public static Bounds GetBounds(Vector3Int position, float size) =>
            new Bounds((position + Vector3.one / 2) * size, Vector3.one * size);
        public static Bounds GetBounds(int x, int y, int z, float size) => GetBounds(new Vector3Int(x, y, z), size);

        public Bounds GetBounds() => GetBounds(index, level.ChunkSize);

        private void OnDrawGizmos()
        {
            if (!Inside)
                return;
            Gizmos.color = Color.blue;
            var bounds = GetBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}