using UnityEngine;

namespace NTO
{
    public sealed class Level : MonoBehaviour
    {
        [Header("General"), SerializeField] private Chunk[] chunksPrefabs;
        
        [Header("Map"), SerializeField, Min(0)] private float chunkSize;
        [SerializeField] private LevelInstance levelInstance;
        
        public Vector3Int LevelSize => levelInstance.Size;
        public float ChunkSize => chunkSize;

        private Chunk[] _chunks;

        private void Awake()
        {
            GenerateWorld();
        }

        private void GenerateWorld()
        {
            _chunks = new Chunk[LevelSize.x * LevelSize.y * LevelSize.z];
            for (var y = 0; y < LevelSize.y; y++)
            {
                for (var z = 0; z < LevelSize.z; z++)
                {
                    for (var x = 0; x < LevelSize.x; x++)
                    {
                        var index = Mathf.Min(chunksPrefabs.Length - 1, levelInstance[x, y, z]);
                        InstantiateChunk(index, x, y, z);
                    }
                }
            }
        }

        private Chunk InstantiateChunk(Chunk prefab, int x, int y, int z)
        {
            var instance = Instantiate(prefab, new Vector3(x, y, z) * chunkSize, Quaternion.identity, transform);
            instance.name = $"Chunk ({x}, {y}, {z}) | {prefab.GetType().Name}";
            instance.index = new Vector3Int(x, y, z);
            instance.level = this;
            _chunks[y * LevelSize.x * LevelSize.z + z * LevelSize.x + x] = instance;
            return instance;
        }
        
        private Chunk InstantiateChunk(int index, int x, int y, int z) => InstantiateChunk(chunksPrefabs[index], x, y, z);

        public Chunk this[int x, int y, int z] => _chunks[y * LevelSize.x * LevelSize.z + z * LevelSize.x + x];

#if UNITY_EDITOR

        [Header("Drawing"), SerializeField] private bool draw = true;
        [SerializeField] private bool drawWire = true;
        [SerializeField] private Gradient drawGradient = new Gradient();
        private bool _selected;
        private void OnDrawGizmos()
        {
            if (draw)
            {
                for (var y = 0; y < LevelSize.y; y++)
                {
                    for (var z = 0; z < LevelSize.z; z++)
                    {
                        for (var x = 0; x < LevelSize.x; x++)
                        {
                            DrawChunkGizmos(x, y, z);
                        }
                    }
                }
            }

            _selected = false;
        }

        private void OnDrawGizmosSelected()
        {
            _selected = true;
        }

        private void DrawChunkGizmos(int x, int y, int z)
        {
            var color = !_selected
                ? Color.white
                : drawGradient.Evaluate((float)levelInstance[x, y, z] / levelInstance.Instances);

            Gizmos.color = color;
            var bounds = Chunk.GetBounds(x, y, z, chunkSize);
            if (drawWire)
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            else
                Gizmos.DrawCube(bounds.center, bounds.size);
        }
        #endif
    }
}