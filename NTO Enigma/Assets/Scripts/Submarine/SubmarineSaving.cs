using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NTO
{
    public sealed class SubmarineSaving : MonoBehaviour, ISavingReceiver
    {
        public Vector3 position;
        public Quaternion rotation;
        public float energy;
        public float oxygen;
        public bool Loaded { get; private set; }

        [Serializable]
        private sealed class SubmarineSavingModel
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;
            public readonly float Energy;
            public readonly float Oxygen;

            public SubmarineSavingModel(Vector3 position, Quaternion rotation, float energy, float oxygen)
            {
                Position = position;
                Rotation = rotation;
                Energy = energy;
                Oxygen = oxygen;
            }
        }
        
        public string GetSavedData()
        {
            var model = new SubmarineSavingModel(position, rotation, energy, oxygen);
            return JsonUtility.ToJson(model);
        }

        public void LoadData(string data)
        {
            var model = JsonUtility.FromJson<SubmarineSavingModel>(data);
            position = model.Position;
            rotation = model.Rotation;
            energy = model.Energy;
            oxygen = model.Oxygen;
            Loaded = true;
        }

        public string Id => "submarine";
    }
}