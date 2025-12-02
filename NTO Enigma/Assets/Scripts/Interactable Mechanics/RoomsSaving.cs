using System;
using UnityEngine;

namespace NTO
{
    public sealed class RoomsSaving : MonoBehaviour, ISavingReceiver
    {
        public bool oxygenTankInserted;
        public bool Loaded { get; private set; }

        [Serializable]
        private sealed class RoomsSavingModel
        {
            public bool oxygenTankInserted;

            public RoomsSavingModel(bool oxygenTankInserted)
            {
                this.oxygenTankInserted = oxygenTankInserted;
            }
        }
        
        public string GetSavedData()
        {
            var model = new RoomsSavingModel(oxygenTankInserted);
            return JsonUtility.ToJson(model);
        }

        public void LoadData(string data)
        {
            var model = JsonUtility.FromJson<RoomsSavingModel>(data);
            oxygenTankInserted = model.oxygenTankInserted;
            Loaded = true;
        }

        public string Id => "rooms";
    }
}