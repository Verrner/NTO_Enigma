using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine), typeof(Rigidbody))]
    public sealed class SubmarineLevelBounds : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidbody;
        [SerializeField] private Bounds bounds;
        [SerializeField] private Level level;
        [SerializeField, Min(1)] private int intersectionsChecksPerSecond = 1;
        
        private Submarine _submarine;
        
        private List<Chunk> _intersectedChunks = new List<Chunk>();
        public IReadOnlyList<Chunk> IntersectedChunks => _intersectedChunks;
        
        private float _timeSinceLastCheck;

        private void Awake()
        {
            _submarine = GetComponent<Submarine>();
        }

        private void Update()
        {
            for (var i = 0; i < _timeSinceLastCheck / intersectionsChecksPerSecond; i++)
            {
                CheckIntersections();
                _timeSinceLastCheck %= intersectionsChecksPerSecond;
            }
            _timeSinceLastCheck += Time.deltaTime;
        }

        private void CheckIntersections()
        {
            List<Chunk> newIntersectedChunks = new List<Chunk>();

            foreach (var chunk in from chunk in GetApproximateAdjacentChunks()
                     where chunk.GetBounds().Intersects(new Bounds(rigidbody.position + bounds.center, bounds.size)) select chunk)
            {
                if (_intersectedChunks.Contains(chunk))
                    chunk.SubmarineInside(_submarine);
                else
                    chunk.SubmarineEntered(_submarine);
                newIntersectedChunks.Add(chunk);
            }

            foreach (var oldChunk in _intersectedChunks.Where(oldChunk => !newIntersectedChunks.Contains(oldChunk)))
            {
                oldChunk.SubmarineLeave(_submarine);
            }

            _intersectedChunks = newIntersectedChunks;
        }

        private List<Chunk> GetApproximateAdjacentChunks()
        {
            var approximatePosition = GetApproximateChunkPosition();
            var res = new List<Chunk>();
            
            for (var y = Mathf.Max(0, approximatePosition.y - 1);
                 y <= Mathf.Min(level.LevelSize.y - 1, approximatePosition.y + 1);
                 y++)
            {
                for (var z = Mathf.Max(0, approximatePosition.z - 1);
                     z <= Mathf.Min(level.LevelSize.z - 1, approximatePosition.z + 1);
                     z++)
                {
                    for (var x = Mathf.Max(0, approximatePosition.x - 1);
                         x <= Mathf.Min(level.LevelSize.x - 1, approximatePosition.x + 1);
                         x++)
                    {
                        res.Add(level[x, y, z]);
                    }
                }
            }
            return res;
        }

        private Vector3Int GetApproximateChunkPosition()
        {
            var pos = rigidbody.position / level.ChunkSize;
            var res = new Vector3Int(Mathf.CeilToInt(pos.x), Mathf.CeilToInt(pos.y), Mathf.CeilToInt(pos.z));
            return res;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(rigidbody.position + bounds.center, bounds.size);
        }
    }
}