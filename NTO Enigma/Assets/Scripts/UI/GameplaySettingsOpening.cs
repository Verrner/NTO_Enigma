using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    public class GameplaySettingsOpening : MonoBehaviour
    {
        [SerializeField] private KeyCode settingsOpeningKey = KeyCode.Escape;
        [SerializeField] private string exitKey = "exit";
        [SerializeField] private Character character;

        private void Awake()
        {
            SettingsUI.SettingsOpened += () => ChangeOpeningState(true);
            SettingsUI.SettingsClosed += () => ChangeOpeningState(false);

            var root = FindFirstObjectByType<SettingsUI>().GetComponent<UIDocument>().rootVisualElement;
            root.Add(GetExitButton());
        }

        private Button GetExitButton()
        {
            var button = new Button
            {
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
            character.Movement.canMove = !state;
            character.Movement.canRotate = !state;
            character.Interaction.canInteract = !state;
            Cursor.visible = state;
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void Update()
        {
            if (SettingsUI.Opened)
                return;
            
            if (Input.GetKeyDown(settingsOpeningKey))
                SettingsUI.Open();
        }
    }
}