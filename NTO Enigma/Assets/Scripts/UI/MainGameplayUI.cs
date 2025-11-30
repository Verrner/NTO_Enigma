using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainGameplayUI : MonoBehaviour
    {
        private static MainGameplayUI _instance;

        [Header("Tooltip"), SerializeField] private string tooltipLabelName = "tooltip-label";
        
        private VisualElement _root;
        
        private Label _tooltipLabel;
        private string _currentTooltipKey;
        private object _currentTooltipSource;

        private void Awake()
        {
            _instance = this;
        }

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _tooltipLabel = _root.Q<Label>(tooltipLabelName);

            LocalizationManager.LanguageChanged += () =>
            {
                if (_currentTooltipKey == "")
                    return;
                _tooltipLabel.text = LocalizationManager.GetValue(_currentTooltipKey, _currentTooltipSource);
            };
        }

        public static void SetTooltip(string localizationKey) => SetTooltip(localizationKey, _instance);

        public static void SetTooltip(string localizationKey, object source)
        {
            var value = LocalizationManager.GetValue(localizationKey, source);
            _instance._tooltipLabel.text = value;
            _instance._currentTooltipKey = localizationKey;
            _instance._currentTooltipSource = source;
        }

        public static void ResetTooltip()
        {
            _instance._tooltipLabel.text = "";
            _instance._currentTooltipKey = "";
            _instance._currentTooltipSource = null;
        }
    }
}