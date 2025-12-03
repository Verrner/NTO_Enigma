using System;
using System.Linq;
using UnityEngine;

namespace NTO
{
    public sealed class SubmarineSaving : MonoBehaviour, ISavingReceiver
    {
        public Vector3 position;
        public Quaternion rotation;
        public float energy;
        public float oxygen;
        public bool[] oxygenTanksSpent;
        public SubmarineMovement.SpeedMode speedMode;
        public bool Loaded { get; private set; }

        [Serializable]
        private sealed class SubmarineSavingModel
        {
            public Vector3 position;
            public Quaternion rotation;
            public float energy;
            public float oxygen;
            
            [SerializeField] private string oxygenTanksSpentBitmask;
            public bool[] OxygenTanksSpent
            {
                get => oxygenTanksSpentBitmask.ToCharArray().Select(c => c == '1').ToArray();
                set => oxygenTanksSpentBitmask = new string(value.Select(x => x ? '1' : '0').ToArray());
            }
            public SubmarineMovement.SpeedMode speedMode;
            
            public SubmarineSavingModel(Vector3 position, Quaternion rotation, float energy, float oxygen, bool[] oxygenTanksSpent,
                SubmarineMovement.SpeedMode speedMode)
            {
                this.position = position;
                this.rotation = rotation;
                this.energy = energy;
                this.oxygen = oxygen;
                OxygenTanksSpent = oxygenTanksSpent;
                this.speedMode = speedMode;
            }
        }
        
        public string GetSavedData()
        {
            var model = new SubmarineSavingModel(position, rotation, energy, oxygen, oxygenTanksSpent, speedMode);
            return JsonUtility.ToJson(model);
        }

        public void LoadData(string data)
        {
            var model = JsonUtility.FromJson<SubmarineSavingModel>(data);
            position = model.position;
            rotation = model.rotation;
            energy = model.energy;
            oxygen = model.oxygen;
            oxygenTanksSpent = model.OxygenTanksSpent;
            speedMode = model.speedMode;
            Loaded = true;
        }

        public string Id => "submarine";
    }
}