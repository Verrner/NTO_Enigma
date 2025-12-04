using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    public sealed class OxygenSupplyRoomUI : RoomUI
    {
        [Header("Oxygen Room"), SerializeField] private Texture2D withoutTankBackground; 
        [SerializeField] private Texture2D withTankBackground;
        [SerializeField] private string insertTankButtonName = "insert-tank-button";
        [SerializeField] private string hasTankLabelName = "has-tank-label";
        [SerializeField] private SubmarineOxygen submarineOxygen;
        [SerializeField] private CharacterOxygenTank characterOxygenTank;

        [HideInInspector, LocalizationDynamicVariable("has-tank")] public bool hasTank;

        private bool HasTank
        {
            get => hasTank;
            set
            {
                hasTank = value;
                RefreshHasTank();
            }
        }
        
        [HideInInspector, LocalizationDynamicVariable("tank-inserted")] public bool tankInsertedDynamic = true;
        private bool _tankInserted = true;
        private bool TankInserted
        {
            get => _tankInserted;
            set
            {
                tankInsertedDynamic = value;
                _tankInserted = value;
                RefreshTankInserted();
            }
        }

        private Button _insertTankButton;
        private Label _hasTankLabel;
        
        protected override void Enable()
        {
            _insertTankButton = Root.Q<Button>(insertTankButtonName);
            _insertTankButton.clicked += InsertTankButtonClicked;
            _hasTankLabel = Root.Q<Label>(hasTankLabelName);
            HasTank = characterOxygenTank.TankGrabbed;
            RefreshTankInserted();
        }
        
        private void RefreshTankInserted()
        {
            Background.style.backgroundImage = TankInserted
                ? new StyleBackground(withTankBackground)
                : new StyleBackground(withoutTankBackground);
            RefreshTooltip();
        }

        private void RefreshHasTank()
        {
            _hasTankLabel.visible = HasTank;
        }
        
        private void InsertTankButtonClicked()
        {
            if (!OpenedOneFrame)
                return;
            
            if (TankInserted)
            {
                TankInserted = false;
                submarineOxygen.Oxygen = 0;
                return;
            }

            if (!HasTank)
                return;

            TankInserted = true;
            HasTank = false;
            characterOxygenTank.DestroyTank();
            submarineOxygen.ResetOxygen();
        }

        protected override void RoomOpened(Character character)
        {
            
        }

        protected override void RoomClosed(Character character)
        {
            
        }
    }
}