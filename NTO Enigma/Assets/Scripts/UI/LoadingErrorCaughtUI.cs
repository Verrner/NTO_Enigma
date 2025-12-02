using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public class LoadingErrorCaughtUI : MonoBehaviour
    {
        [Header("UI"), SerializeField] private string errorMessageLabelName = "error-message-label";
        [SerializeField] private string exitButtonName = "exit-button";

        private static LoadingErrorCaughtUI _instance;
        
        private Label _errorMessageLabel;
        private Button _exitButton;

        private void Awake()
        {
            _instance = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _errorMessageLabel = root.Q<Label>(errorMessageLabelName);
            _exitButton = root.Q<Button>(exitButtonName);
            _exitButton.clicked += Application.Quit;
            LocalizationManager.LocalizeUI(root, this);
        }

        public static void ShowError(string message)
        {
            _instance.gameObject.SetActive(true);
            _instance._errorMessageLabel.text = message;
        }
    }
}