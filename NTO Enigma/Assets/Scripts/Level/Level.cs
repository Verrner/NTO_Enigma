using System.Collections.Generic;
using UnityEngine;

namespace NTO
{
    public sealed class Level : MonoBehaviour
    {
        [SerializeField, Min(0)] private Vector3Int levelSize = Vector3Int.one;
        [SerializeField, Min(0)] private float chunkSize;
        [SerializeField] private Chunk chunkPrefab;

        private List<Chunk> _chunksInstances;

        public Chunk GetChunkByPos(Vector3Int pos) =>
            _chunksInstances[pos.y * levelSize.x * levelSize.z + pos.z * levelSize.x + pos.x];
        
        public Vector3Int LevelSize => levelSize;
        public float ChunkSize => chunkSize;
        
        private void Awake()
        {
            _chunksInstances = new List<Chunk>();
            GenerateChunks();
        }

        private void GenerateChunks()
        {
            for (var y = 0; y < levelSize.y; y++)
            {
                for (var z = 0; z < levelSize.z; z++)
                {
                    for (var x = 0; x < levelSize.x; x++)
                    {
                        var chunkInstance = Instantiate(chunkPrefab, new Vector3(x, y, z) * chunkSize,
                            Quaternion.identity, transform);
                        chunkInstance.name = $"Chunk ({x}, {y}, {z})";
                        _chunksInstances.Add(chunkInstance);
                    }
                }
            }
        }
    }
}