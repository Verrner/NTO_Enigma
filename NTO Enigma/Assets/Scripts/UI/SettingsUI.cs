using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class SettingsUI : MonoBehaviour
    {
        [Header("General"), SerializeField] private MainMenuUI mainMenuUI;
        
        [Header("Elements"), SerializeField] private string exitButtonName = "exit-button";
        [SerializeField] private string languageDropdownName = "language-dropdown";
        
        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>(exitButtonName).clicked += ClosedSettings;
            var languageDropdown = root.Q<DropdownField>(languageDropdownName);
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
            LocalizationManager.LocalizeUI(root, this);
        }

        private void ClosedSettings()
        {
            gameObject.SetActive(false);
        }
    }
}