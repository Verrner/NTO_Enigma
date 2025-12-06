using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class SettingsUI : MonoBehaviour
    {
        [Header("General"), SerializeField] private MainMenuUI mainMenuUI;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;
        
        [Header("Elements"), SerializeField] private string exitButtonName = "exit-button";
        [SerializeField] private string languageDropdownName = "language-dropdown";

        private static SettingsUI _instance;

        public static event Action SettingsOpened;
        public static event Action SettingsClosed;
        
        public static bool Opened { get; private set; }
        
        private VisualElement _root;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.Q<Button>(exitButtonName).clicked += Close;
            var languageDropdown = _root.Q<DropdownField>(languageDropdownName);
            languageDropdown.value = LocalizationManager.Language == SystemLanguage.English ? "English" : "Русский";
            languageDropdown.RegisterValueChangedCallback(callback =>
            {
                LocalizationManager.Language = callback.newValue switch
                {
                    "English" => SystemLanguage.English,
                    "Русский" => SystemLanguage.Russian,
                    _ => LocalizationManager.Language
                };
            });
            LocalizationManager.LocalizeUI(_root, this);
            _root.visible = false;
        }

        private void Update()
        {
            if (!Opened)
                return;
            
            if (Input.GetKeyDown(exitKey))
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