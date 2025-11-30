using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Character))]
    public sealed class CharacterOxygenTank : MonoBehaviour
    {
        [SerializeField] private Transform tankPrefab;
        [SerializeField] private Transform tankRoot;
        
        public bool TankGrabbed { get; private set; }

        public void GrabTank()
        {
            if (TankGrabbed)
                return;
            InstantiateTank();
            TankGrabbed = true;
        }

        public void DestroyTank()
        {
            if (!TankGrabbed)
                return;
            Destroy(tankRoot.GetChild(0).gameObject);
            TankGrabbed = false;
        }

        private void InstantiateTank() => Instantiate(tankPrefab, tankRoot);
    }
}