using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    public abstract class RoomUI : MonoBehaviour
    {
        [Header("General"), SerializeField] private UIDocument document;
        [SerializeField] private bool closeByKey = true;
        [SerializeField] private KeyCode exitKey = KeyCode.Escape;
        [SerializeField] private string backgroundName = "background";
        
        [Header("Tooltips"), SerializeField] private bool tooltipsEnabled = true;
        [SerializeField] private string tooltipLabelName = "tooltip-label";

        protected VisualElement Root { get; private set; }
        protected VisualElement Background { get; private set; }
        protected bool OpenedOneFrame { get; private set; }

        private Character _character;
        private Label _tooltipLabel;
        private string _currentTooltipKey = "";

        private string CurrentTooltipKey
        {
            get => _currentTooltipKey;
            set
            {
                _currentTooltipKey = value;
                RefreshTooltip();
            }
        }

        public bool Opened { get; private set; }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            Root = document.rootVisualElement;
            Background = document.rootVisualElement.Q(backgroundName);
            if (tooltipsEnabled)
                _tooltipLabel = Root.Q<Label>(tooltipLabelName);
            Enable();
            PrepareTooltipElements(Root);

            LocalizationManager.LanguageChanged += RefreshTooltip;
            LocalizationManager.LocalizeUI(Root, this);
        }

        protected void RefreshTooltip()
        {
            if (!tooltipsEnabled)
                return;
            _tooltipLabel.text = CurrentTooltipKey == "" ? "" : LocalizationManager.GetValue(CurrentTooltipKey, this);
        }

        private void Update()
        {
            if (Opened && !OpenedOneFrame)
                OpenedOneFrame = true;
            if (closeByKey && Input.GetKeyDown(exitKey))
                Close();
            
            UpdateRoom();
        }
        
        protected virtual void UpdateRoom(){}

        public void Open(Character character)
        {
            OpenedOneFrame = false;
            SetRoomOpening(character, true);
            RoomOpened(character);
            RefreshTooltip();
        }
        
        protected void Close()
        {
            SetRoomOpening(_character, false);
        }

        private void SetRoomOpening(Character character, bool opened)
        {
            Opened = opened;
            _character = opened ? character : null;
            gameObject.SetActive(opened);
            character.Movement.canMove = !opened;
            character.Movement.canRotate = !opened;
            character.Interaction.canInteract = !opened;
            Cursor.visible = opened;
            Cursor.lockState = opened ? CursorLockMode.None : CursorLockMode.Locked;
        }

        private void PrepareTooltipElements(VisualElement root)
        {
            if (!tooltipsEnabled)
                return;
            
            if (root is ITooltipUIElement element)
            {
                root.RegisterCallback<PointerEnterEvent>(_ => TooltipUIElementPointerEnter(element));
                root.RegisterCallback<PointerLeaveEvent>(_ => TooltipUIElementPointerExit());
            }
            
            foreach (var child in root.Children())
                PrepareTooltipElements(child);
        }

        private void TooltipUIElementPointerEnter(ITooltipUIElement element) => CurrentTooltipKey = element.Key;
        
        private void TooltipUIElementPointerExit() => CurrentTooltipKey = "";
        
        protected abstract void Enable();
        protected abstract void RoomOpened(Character character);
    }
}