using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public class TutorialUI : MonoBehaviour
    {
        [SerializeField] private EventInteractable interactable;
        [SerializeField] private string[] pagesKeys;
        [SerializeField] private string contentLabelName = "content-label";

        private VisualElement _root;
        private Label _contentLabel;
        private bool _showing;
        private int _currentPageIndex;
        private Character _character;

        private int CurrentPageIndex
        {
            get => _currentPageIndex;
            set
            {
                _currentPageIndex = value >= pagesKeys.Length ? 0 : value < 0 ? 0 : value;
                RefreshPage();
            }
        }

        private void Awake()
        {
            interactable.Interacted += interaction => ShowTutorial(interaction.Character);
            LocalizationManager.LanguageChanged += RefreshPage;
        }

        private void OnEnable()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _contentLabel = _root.Q<Label>(contentLabelName);
            _root.visible = false;
            LocalizationManager.LocalizeUI(_root, this);
            RefreshPage();
        }

        private void Update()
        {
            if (!_showing)
                return;
            
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseTutorial();
            if (Input.GetKeyDown(KeyCode.D))
                CurrentPageIndex++;
            else if (Input.GetKeyDown(KeyCode.A))
                CurrentPageIndex--;
        }

        private void ShowTutorial(Character character)
        {
            _character = character;
            SetTutorialShowState(true);
        }

        private void CloseTutorial() => SetTutorialShowState(false);

        private void SetTutorialShowState(bool state)
        {
            _showing = state;
            _root.visible = state;
            _character.Movement.canMove = !state;
            _character.Movement.canRotate = !state;
            _character.Interaction.canInteract = !state;
        }

        private void RefreshPage()
        {
            var key = pagesKeys[_currentPageIndex];
            var value = LocalizationManager.GetValue(key, this);
            _contentLabel.text = value;
        }
    }
}