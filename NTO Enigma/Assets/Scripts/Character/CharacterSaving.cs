using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace NTO
{
    public sealed class CharacterSaving : MonoBehaviour, ISavingReceiver
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public bool Loaded { get; private set; }

        [Serializable]
        private sealed class CharacterSavingModel
        {
            public readonly Vector3 LocalPosition;
            public readonly Quaternion LocalRotation;

            public CharacterSavingModel(Vector3 localPosition, Quaternion localRotation)
            {
                LocalPosition = localPosition;
                LocalRotation = localRotation;
            }
        }
        
        public string GetSavedData()
        {
            var model = new CharacterSavingModel(localPosition, localRotation);
            return JsonUtility.ToJson(model);
        }

        public void LoadData(string data)
        {
            var model = JsonUtility.FromJson<CharacterSavingModel>(data);
            localPosition = model.LocalPosition;
            localRotation = model.LocalRotation;
            Loaded = true;
        }

        public string Id => "character";
    }
}