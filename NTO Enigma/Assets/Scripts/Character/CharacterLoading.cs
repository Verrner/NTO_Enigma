using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Character))]
    public sealed class CharacterLoading : MonoBehaviour
    {
        private void Awake()
        {
            var data = FindFirstObjectByType<CharacterSaving>();
            if (data.Loaded)
            {
                transform.localPosition = data.localPosition;
                transform.localRotation = data.localRotation;
            }

            var saving = FindFirstObjectByType<SavingManager>();
            saving.SavingStarted += () =>
            {
                data.localPosition = transform.localPosition;
                data.localRotation = transform.localRotation;
            };
        }
    }
}