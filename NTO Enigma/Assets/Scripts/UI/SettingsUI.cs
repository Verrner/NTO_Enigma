using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class SettingsUI : MonoBehaviour
    {
        [Header("General"), SerializeField] private MainMenuUI mainMenuUI;
        [SerializeField] private KeyCode changingKey = KeyCode.Tab;
        
        [Header("Elements"), SerializeField] private string languageDropdownName = "language-dropdown";
        [SerializeField] private string volumeSliderName = "volume-slider";

        private static SettingsUI _instance;

        public static event Action SettingsOpened;
        public static event Action SettingsClosed;
        
        public static bool Opened { get; private set; }
        
        private VisualElement _root;
        private Slider _volumeSlider;
        private DropdownField _languageDropdown;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                FindFirstObjectByType<SavingManager>().LoadingEnded += (_, _) =>
                {
                    _volumeSlider.value = AudioSettings.Volume;
                    _languageDropdown.value = LocalizationManager.Language == SystemLanguage.English ? "English" : "Русский";
                };
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            _languageDropdown = _root.Q<DropdownField>(languageDropdownName);
            _languageDropdown.value = LocalizationManager.Language == SystemLanguage.English ? "English" : "Русский";
            _languageDropdown.RegisterValueChangedCallback(callback =>
            {
                LocalizationManager.Language = callback.newValue switch
                {
                    "English" => SystemLanguage.English,
                    "Русский" => SystemLanguage.Russian,
                    _ => LocalizationManager.Language
                };
            });
            
            _volumeSlider = _root.Q<Slider>(volumeSliderName);
            _volumeSlider.value = AudioSettings.Volume;
            _volumeSlider.RegisterValueChangedCallback(callback =>
            {
                AudioSettings.Volume = callback.newValue;
            });
            
            LocalizationManager.LocalizeUI(_root, this);
            
            _root.visible = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(changingKey))
                SetOpened(!Opened);
            else if (Input.GetKeyDown(KeyCode.Escape))
                Close();
        }

        public static void Open() => SetOpened(true);

        public static void Close() => SetOpened(false);

        private static void SetOpened(bool opened)
        {
            _instance._root.visible = opened;
            Opened = opened;
            
            if (opened)
                SettingsOpened?.Invoke();
            else
                SettingsClosed?.Invoke();
        }
    }
}