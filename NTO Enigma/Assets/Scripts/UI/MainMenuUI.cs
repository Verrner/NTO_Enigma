using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuUI : MonoBehaviour
    {
        [Header("Saves List"), SerializeField] private string savesListParentName = "saves-list-scroll-view";
        [SerializeField] private VisualTreeAsset saveButtonTemplate;
        
        [Header("Save Settings"), SerializeField] private string saveSettingsContentName = "save-name-field";
        [SerializeField] private string savePathLabelName = "save-path-label";
        [HideInInspector, LocalizationDynamicVariable("creating-date")] public string creatingDate;
        [HideInInspector, LocalizationDynamicVariable("updating-date")] public string updatingDate;
        
        [Header("Controls"), SerializeField] private string newSaveNameFieldName = "new-save-name-field";
        [SerializeField] private string createNewSaveButtonName = "create-new-save-button";
        [SerializeField] private string loadExistingSaveButtonName = "load-existing-save-button";
        [SerializeField] private string settingsButtonName = "settings-button";
        [SerializeField] private string exitButtonName = "exit-button";
        
        [Header("General"), SerializeField] private SavingManager savingManager;
        [SerializeField] private string gameplaySceneName = "Main Gameplay Scene";

        private VisualElement _root;

        private VisualElement _savesListParent;
        private int _selectedSaveIndex;
        private List<(string, SavingManager.GeneralData)> _saves;

        private VisualElement _saveSettingsContent;
        private Label _savePathLabel;
        
        private Button _loadExistingSaveButton;
        
        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            SetElements();
            try
            {
                ShowAllSaves();
                RedrawSettings();
            }
            catch (Exception e)
            {
                LoadingErrorCaughtUI.ShowError(e.Message);
                throw;
            }

            if (_saves.Count == 0) _loadExistingSaveButton.SetEnabled(false);
            LocalizationManager.LocalizeUI(_root, this);
        }

        private void SetElements()
        {
            _savesListParent = _root.Q(savesListParentName);
            
            _saveSettingsContent = _root.Q(saveSettingsContentName);
            _savePathLabel = _root.Q<Label>(savePathLabelName);

            var newSaveNameField = _root.Q<TextField>(newSaveNameFieldName);
            newSaveNameField.RegisterValueChangedCallback(callback =>
            {
                if (string.IsNullOrWhiteSpace(newSaveNameField.value))
                    return;
                savingManager.generalData.name = newSaveNameField.value;
            });
            _root.Q<Button>(createNewSaveButtonName).clicked += () =>
            {
                if (!string.IsNullOrWhiteSpace(newSaveNameField.value))
                    savingManager.generalData.name = newSaveNameField.value;
                LoadGameplayScene();
            };
            _loadExistingSaveButton = _root.Q<Button>(loadExistingSaveButtonName);
            _loadExistingSaveButton.clicked += LoadExistingSave;
            _root.Q<Button>(settingsButtonName).clicked += SettingsUI.Open;
            _root.Q<Button>(exitButtonName).clicked += Application.Quit;
        }

        private void LoadExistingSave()
        {
            savingManager.Load(Path.GetFileNameWithoutExtension(_saves[_selectedSaveIndex].Item1), (message, success) =>
            {
                switch (success)
                {
                    case true:
                        LoadGameplayScene();
                        return;
                    case false:
                        LoadingErrorCaughtUI.ShowError(message);
                        return;
                }
            });
        }

        private void LoadGameplayScene()
        {
            SceneManager.LoadScene(gameplaySceneName);
        }

        private void ShowAllSaves()
        {
            _saves = new List<(string, SavingManager.GeneralData)>();
            _saves = savingManager.GetAllSavesData().ToList();
            
            for (var i = 0; i < _saves.Count; i++)
            {
                var (path, data) = _saves[i];
                var template = GetSaveButton(data.name);
                var button = (Button)template.Children().First();
                button.clicked += () => SaveButtonClicked((path, data));
                _savesListParent.Add(template);
            }
        }

        private void SaveButtonClicked((string, SavingManager.GeneralData) save)
        {
            _selectedSaveIndex = _saves.IndexOf(save);
            RedrawSettings();
            LocalizationManager.LocalizeUI(_root, this);
        }

        private void RedrawSettings()
        {
            _saveSettingsContent.style.opacity = _selectedSaveIndex >= _saves.Count ? 0 : 1;
            if (_selectedSaveIndex >= _saves.Count) return;

            creatingDate = _saves[_selectedSaveIndex].Item2.creatingDate;
            updatingDate = _saves[_selectedSaveIndex].Item2.updatingDate;
            _savePathLabel.text = _saves[_selectedSaveIndex].Item1;
        }

        private TemplateContainer GetSaveButton(string name)
        {
            var template = saveButtonTemplate.Instantiate();
            var element = (Button)template.Children().First();
            element.text = name;
            return template;
        }
    }
}