using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        
        [Header("General"), SerializeField] private SavingManager savingManager;

        private UIDocument _uiDocument;

        private VisualElement _savesListParent;
        private int _selectedSaveIndex;
        private List<(string, SavingManager.GeneralData)> _saves;

        private VisualElement _saveSettingsContent;
        private Label _savePathLabel;
        
        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();
            SetElements();
            ShowAllSaves();
            RedrawSettings();
            LocalizationManager.LocalizeUI(_uiDocument.rootVisualElement, this);
        }

        private void SetElements()
        {
            _savesListParent = _uiDocument.rootVisualElement.Q(savesListParentName);
            
            _saveSettingsContent = _uiDocument.rootVisualElement.Q(saveSettingsContentName);
            _savePathLabel = _uiDocument.rootVisualElement.Q<Label>(savePathLabelName);
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
        }

        private void RedrawSettings()
        {
            _saveSettingsContent.style.opacity = _selectedSaveIndex >= _saves.Count ? 0 : 1;
            if (_selectedSaveIndex >= _saves.Count) return;

            creatingDate = _saves[_selectedSaveIndex].Item2.creatingDate;
            updatingDate = _saves[_selectedSaveIndex].Item2.updatingDate;
            _savePathLabel.text = _saves[_selectedSaveIndex].Item1;
            
            LocalizationManager.LocalizeUI(_uiDocument.rootVisualElement, this);
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