using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Collider))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private string tooltipKey;
        [SerializeField] private float radius = 3;

        private object _localizationSource;
        public object LocalizationSource
        {
            get => _localizationSource ?? this;
            set => _localizationSource = value;
        }
        
        private Transform _characterTransform;

        private bool _mouseEnter;
        private bool _tooltipShown;
        
        private void Awake()
        {
            _characterTransform = FindFirstObjectByType<Character>().transform;
        }

        private void Update()
        {
            switch (_tooltipShown)
            {
                case false when _mouseEnter && Vector3.Distance(_characterTransform.position, transform.position) <= radius:
                    MainGameplayUI.SetTooltip(tooltipKey, LocalizationSource);
                    _tooltipShown = true;
                    break;
                case true when !_mouseEnter || Vector3.Distance(_characterTransform.position, transform.position) > radius:
                    MainGameplayUI.ResetTooltip();
                    _tooltipShown = false;
                    break;
            }
        }

        public void RefreshTooltipText()
        {
            if (!_tooltipShown)
                MainGameplayUI.ResetTooltip();
            else
                MainGameplayUI.SetTooltip(tooltipKey, LocalizationSource);
        }

        private void OnMouseEnter() => _mouseEnter = true;

        private void OnMouseExit() => _mouseEnter = false;
    }
}