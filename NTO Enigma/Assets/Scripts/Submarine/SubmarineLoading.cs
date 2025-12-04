using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine), typeof(Rigidbody))]
    public sealed class SubmarineLoading : MonoBehaviour
    {
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Submarine submarine;

        private void Awake()
        {
            var data = FindFirstObjectByType<SubmarineSaving>();
            if (data.Loaded)
            {
                rigidbody.position = data.position;
                rigidbody.rotation = data.rotation;
                submarine.Energy.Energy = data.energy;
                submarine.Oxygen.Oxygen = data.oxygen;
                submarine.Oxygen.DestroySpentTanks(data.oxygenTanksSpent);
                submarine.SpeedChanging.ChangeMode(data.speedMode);
                submarine.EngineBreak.engineBroken = data.engineBroken;
            }

            var saving = FindFirstObjectByType<SavingManager>();
            saving.SavingStarted += () =>
            {
                data.position = rigidbody.position;
                data.rotation = rigidbody.rotation;
                data.energy = submarine.Energy.Energy;
                data.oxygen = submarine.Oxygen.Oxygen;
                data.oxygenTanksSpent = submarine.Oxygen.GetSpentTanks();
                data.speedMode = submarine.Movement.speedMode;
                data.engineBroken = submarine.EngineBreak.engineBroken;
            };
        }
    }
}