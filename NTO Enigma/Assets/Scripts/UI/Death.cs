using System;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class Death : MonoBehaviour
    {
        [Header("General"), SerializeField] private Character character;
        
        [Header("UI"), SerializeField] private string backgroundName = "background";
        [SerializeField] private string deathLabelName = "death-label";
        [SerializeField] private string deathSentenceLabelName = "death-sentence-label";
        [SerializeField] private string exitButtonName = "exit-button";

        [Header("Animation"), SerializeField] private AnimationCurve backgroundAppearanceCurve;

        private static Death _instance;
        
        private VisualElement _background;
        private Label _deathLabel;
        private Label _deathSentenceLabel;
        private Button _exitButton;

        private bool _opened;
        private float _backgroundAppearanceTime;

        public static bool Dead => _instance._opened;
        
        private void Awake()
        {
            _instance = this;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            SetElements(root);
            LocalizationManager.LocalizeUI(root, this);
        }

        private void Update()
        {
            if (!_opened)
                return;
            
            var backgroundOpacity = backgroundAppearanceCurve.Evaluate(_backgroundAppearanceTime);
            _background.style.opacity = backgroundOpacity;
            _backgroundAppearanceTime += Time.deltaTime;
        }

        private void SetElements(VisualElement root)
        {
            _background = root.Q(backgroundName);
            _deathLabel = root.Q<Label>(deathLabelName);
            _deathSentenceLabel = root.Q<Label>(deathSentenceLabelName);
            _exitButton = root.Q<Button>(exitButtonName);

            _exitButton.clicked += () =>
            {
                var savingManager = FindFirstObjectByType<SavingManager>();
                savingManager.Save((_, _) =>
                {
                    Application.Quit();
                });
            };
        }

        public static void ShowDeath(string sentenceKey, object source)
        {
            _instance._opened = true;
            
            _instance.gameObject.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _instance.character.Movement.canMove = false;
            _instance.character.Movement.canRotate = false;
            _instance.character.Interaction.canInteract = false;
            
            var sentence = LocalizationManager.GetValue(sentenceKey, source);
            _instance._deathSentenceLabel.text = sentence;
            LocalizationManager.LanguageChanged += () => _instance._deathSentenceLabel.text = LocalizationManager.GetValue(sentenceKey, source);
        }
    }
}