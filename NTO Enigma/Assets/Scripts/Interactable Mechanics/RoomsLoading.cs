using UnityEngine;

namespace NTO
{
    public class RoomsLoading : MonoBehaviour
    {
        [SerializeField] private OxygenSupplyRoomUI oxygenSupplyRoomUI;
        
        private void Awake()
        {
            var data = FindFirstObjectByType<RoomsSaving>();
            if (data.Loaded)
            {
                oxygenSupplyRoomUI.hasTank = data.oxygenTankInserted;
            }
            
            oxygenSupplyRoomUI.gameObject.SetActive(false);
            
            var saving = FindFirstObjectByType<SavingManager>();
            saving.SavingStarted += () =>
            {
                data.oxygenTankInserted = oxygenSupplyRoomUI.hasTank;
            };
        }
    }
}