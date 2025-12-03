using System;
using System.Collections.Generic;
using UnityEngine;

namespace NTO
{
    [Serializable]
    public class LevelInstance
    {
        [SerializeField] private int instances = 1;

        public int Instances
        {
            get => instances;
            set => instances = Mathf.Max(1, value);
        }

        [SerializeField] private Vector3Int size = Vector3Int.one;

        public Vector3Int Size
        {
            get => size;
            set
            {
                size = new Vector3Int(Mathf.Max(1, value.x), Mathf.Max(1, value.y), Mathf.Max(1, value.z));
                level = new List<int>(size.x * size.y * size.z);
            }
        }

        [SerializeField] private List<int> level = new (){ 0 };

        public int this[int x, int y, int z]
        {
            get
            {
                try
                {
                    return level[y * size.x * size.z + z * size.x + x];
                }
                catch
                {
                    Debug.Log($"({x}, {y}, {z}) {y * size.x * size.z + z * size.x + x} > {level.Count}");
                    throw;
                }
            }
            set
            {
                var val = value < 0 ? instances - 1 : value >= instances ? 0 : value;
                level[y * size.x * size.z + z * size.x + x] = val;
            }
        }
    }
}