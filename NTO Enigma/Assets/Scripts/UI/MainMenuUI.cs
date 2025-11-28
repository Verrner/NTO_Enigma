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
        [SerializeField] private string saveNameFieldName = "save-name-field";
        [SerializeField] private string saveCreatingDateLabelName = "save-creating-date-label";
        [SerializeField] private string saveUpdatingDateLabelName = "save-updating-date-label";
        [SerializeField] private string savePathLabelName = "save-path-label";
        
        [Header("General"), SerializeField] private SavingManager savingManager;

        [LocalizationDynamicVariable("test-1"), SerializeField] public int Test1;
        [LocalizationDynamicVariable("test-2"), SerializeField] public string Test2;
        
        private VisualElement _root;

        private VisualElement _savesListParent;
        private int _selectedSaveIndex;
        private List<(string, SavingManager.GeneralData)> _saves;

        private VisualElement _saveSettingsContent;
        private TextField _saveNameField;
        private Label _saveCreatingDateLabel;
        private Label _saveUpdatingDateLabel;
        private Label _savePathLabel;
        
        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            SetElements();
            ShowAllSaves();
            RedrawSettings();
            Debug.Log("Main Menu");
            LocalizationManager.LocalizeUI(_root, this);
            LocalizationManager.LanguageChanged += () =>
            {
                Debug.Log("Event Main Menu");
                LocalizationManager.LocalizeUI(_root, this);
            };
        }

        private void SetElements()
        {
            _savesListParent = _root.Q(savesListParentName);
            
            _saveSettingsContent = _root.Q(saveSettingsContentName);
            _saveNameField = _root.Q<TextField>(saveNameFieldName);
            _saveCreatingDateLabel = _root.Q<Label>(saveCreatingDateLabelName);
            _saveUpdatingDateLabel = _root.Q<Label>(saveUpdatingDateLabelName);
            _savePathLabel = _root.Q<Label>(savePathLabelName);

            _saveNameField.RegisterValueChangedCallback(e =>
            {
                if (string.IsNullOrWhiteSpace(e.newValue))
                {
                    _saveNameField.value = "New Save";
                    savingManager.generalData.name = "New Save";
                    return;
                }

                savingManager.generalData.name = e.newValue;
            });
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

            _saveNameField.value = _saves[_selectedSaveIndex].Item2.name;
            _saveCreatingDateLabel.text = _saves[_selectedSaveIndex].Item2.creatingDate;
            _saveUpdatingDateLabel.text = _saves[_selectedSaveIndex].Item2.updatingDate;
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