using System;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineSpeedChanging : MonoBehaviour
    {
        [Header("Objects"), SerializeField] private SubmarineMovement submarineMovement;
        [SerializeField] private Character character;
        [SerializeField] private EventInteractable interactable;
        [SerializeField] private Tooltip tooltip;
        
        [Header("Keys"), SerializeField] private KeyCode fastModeKey = KeyCode.F;
        [SerializeField] private KeyCode silentModeKey = KeyCode.S;
        [SerializeField] private KeyCode defaultModeKey = KeyCode.D;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;

        [Header("Tooltips"), SerializeField] private string fastModeTooltipLocalizationKey = "fast-mode-tooltip";
        [SerializeField] private string silentModeTooltipLocalizationKey = "silent-mode-tooltip";
        [SerializeField] private string defaultModeTooltipLocalizationKey = "default-mode-tooltip";

        [HideInInspector, LocalizationDynamicVariable("mode-tooltip")] public string modeTooltip = "";

        private bool _interacted;

        private void Awake()
        {
            SetModeTooltip();
            LocalizationManager.LanguageChanged += SetModeTooltip;
            tooltip.LocalizationSource = this;
            interactable.Interacted += _ => SetInteraction(true);
        }

        private void SetInteraction(bool interaction)
        {
            if (_interacted == interaction)
                return;
            
            _interacted = interaction;
            character.Movement.canMove = !interaction;
            character.Movement.canRotate = !interaction;
            character.Interaction.canInteract = !interaction;
        }

        private void Update()
        {
            if (!_interacted)
                return;
            
            if (Input.GetKeyDown(fastModeKey))
                SetMode(SubmarineMovement.SpeedMode.Fast);
            else if (Input.GetKeyDown(silentModeKey))
                SetMode(SubmarineMovement.SpeedMode.Silent);
            else if (Input.GetKeyDown(defaultModeKey))
                SetMode(SubmarineMovement.SpeedMode.Default);
            else if (Input.GetKeyDown(exitKey))
                SetInteraction(false);
        }

        private void SetMode(SubmarineMovement.SpeedMode mode)
        {
            submarineMovement.speedMode = mode;
            SetModeTooltip();
            tooltip.RefreshTooltipText();
        }

        private void SetModeTooltip()
        {
            var mode = submarineMovement.speedMode;
            var key = mode switch
            {
                SubmarineMovement.SpeedMode.Default => defaultModeTooltipLocalizationKey,
                SubmarineMovement.SpeedMode.Silent => silentModeTooltipLocalizationKey,
                _ => fastModeTooltipLocalizationKey
            };
            modeTooltip = LocalizationManager.GetValue(key, this);
        }
    }
}