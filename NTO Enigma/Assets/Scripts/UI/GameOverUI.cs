using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private string exitButtonName = "exit-button";

        private void OnEnable()
        {
            var saving = FindFirstObjectByType<SavingManager>();
            saving.generalData.gameOver = true;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>(exitButtonName).clicked += () =>
            {
                saving.Save((_, _) =>
                {
                    Application.Quit();
                });
            };
            LocalizationManager.LocalizeUI(root, this);
        }
    }
}