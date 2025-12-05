using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    public class SavingIconUI : MonoBehaviour
    {
        [SerializeField] private string savingIconName = "saving-icon";

        private SavingManager _savingManager;
        private VisualElement _savingIcon;

        private void Awake()
        {
            _savingIcon = GetComponent<UIDocument>().rootVisualElement.Q(savingIconName);
            _savingIcon.visible = false;
            _savingManager = FindFirstObjectByType<SavingManager>();
            _savingManager.SavingStarted += () => _savingIcon.visible = true;
            _savingManager.SavingEnded += (_, _) => _savingIcon.visible = false;
        }
    }
}