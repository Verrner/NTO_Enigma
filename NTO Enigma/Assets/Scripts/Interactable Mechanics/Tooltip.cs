using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Collider))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private string tooltipKey;
        [SerializeField] private float radius = 3;
        
        private Transform _characterTransform;

        private object _localizationSource;

        public object LocalizationSource
        {
            get => _localizationSource ?? this;
            set => _localizationSource = value;
        }
        
        private bool _mouseEnter;
        private bool _tooltipShown;
        
        private void Awake()
        {
            var character = FindFirstObjectByType<Character>() ?? (MonoBehaviour)FindFirstObjectByType<HotelCharacter>();
            _characterTransform = character.transform;
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

        private void OnMouseEnter() => _mouseEnter = true;

        private void OnMouseExit() => _mouseEnter = false;
    }
}