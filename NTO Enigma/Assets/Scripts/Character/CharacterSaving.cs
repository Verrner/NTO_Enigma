using System;
using UnityEngine;

namespace NTO
{
    public sealed class CharacterSaving : MonoBehaviour, ISavingReceiver
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
        public bool canMove;
        public bool canRotate;
        public bool canInteract;
        public bool tankGrabbed;
        public float blurringTimeSinceOxygenOver;
        public float actualTimeSinceOxygenOver;
        public bool Loaded { get; private set; }

        [Serializable]
        private sealed class CharacterSavingModel
        {
            public Vector3 localPosition;
            public Quaternion localRotation;
            public bool canMove;
            public bool canRotate;
            public bool canInteract;
            public bool tankGrabbed;
            public float blurringTimeSinceOxygenOver;
            public float actualTimeSinceOxygenOver;

            public CharacterSavingModel(Vector3 localPosition, Quaternion localRotation, bool canMove, bool canRotate, bool canInteract,
                bool tankGrabbed, float blurringTimeSinceOxygenOver, float actualTimeSinceOxygenOver)
            {
                this.localPosition = localPosition;
                this.localRotation = localRotation;
                this.canMove = canMove;
                this.canRotate = canRotate;
                this.canInteract = canInteract;
                this.tankGrabbed = tankGrabbed;
                this.blurringTimeSinceOxygenOver = blurringTimeSinceOxygenOver;
                this.actualTimeSinceOxygenOver = actualTimeSinceOxygenOver;
            }
        }
        
        public string GetSavedData()
        {
            var model = new CharacterSavingModel
                (localPosition, localRotation, canMove, canRotate,
                canInteract, tankGrabbed, blurringTimeSinceOxygenOver, actualTimeSinceOxygenOver);
            return JsonUtility.ToJson(model);
        }

        public void LoadData(string data)
        {
            var model = JsonUtility.FromJson<CharacterSavingModel>(data);
            localPosition = model.localPosition;
            localRotation = model.localRotation;
            canMove = model.canMove;
            canRotate = model.canRotate;
            canInteract = model.canInteract;
            tankGrabbed = model.tankGrabbed;
            blurringTimeSinceOxygenOver = model.blurringTimeSinceOxygenOver;
            actualTimeSinceOxygenOver = model.actualTimeSinceOxygenOver;
            Loaded = true;
        }

        public string Id => "character";
    }
}