using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    public class GameplaySettingsChanging : MonoBehaviour
    {
        [SerializeField] private string exitKey = "exit";

        private void Awake()
        {
            SettingsUI.SettingsOpened += () => ChangeOpeningState(true);
            SettingsUI.SettingsClosed += () => ChangeOpeningState(false);
            
            var root = FindFirstObjectByType<SettingsUI>().GetComponent<UIDocument>().rootVisualElement;
            var buttonsExists = root.Contains(root.Q<Button>("exit-button"));
            if (!buttonsExists)
                root.Add(GetExitButton());
        }

        private Button GetExitButton()
        {
            var button = new Button
            {
                name = "exit-button",
                text = LocalizationManager.GetValue(exitKey, this),
                style =
                {
                    position = Position.Absolute,
                    left = 0,
                    bottom = 0,
                    marginLeft = 20,
                    marginBottom = 20,
                }
            };
            LocalizationManager.LanguageChanged += () => button.text = LocalizationManager.GetValue(exitKey, this);
            button.clicked += () =>
            {
                SettingsUI.Close();
                var saving = FindFirstObjectByType<SavingManager>();
                saving.Save((_, _) =>
                {
                    Application.Quit();
                });
            };
            return button;
        }

        private void ChangeOpeningState(bool state)
        {
            Cursor.visible = state;
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}