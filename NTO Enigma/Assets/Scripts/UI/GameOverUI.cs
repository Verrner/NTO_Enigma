using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

namespace NTO
{
    [RequireComponent(typeof(UIDocument))]
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private string exitButtonName = "exit-button";
        
        [HideInInspector, LocalizationDynamicVariable("game-time")] public string gameTime;

        private void OnEnable()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            var gameSeconds = GameplayTime.GameplaySeconds;
            gameTime = $"{Mathf.FloorToInt(gameSeconds / 3600)}:{Mathf.FloorToInt(gameSeconds % 3600 / 60)}:{Mathf.FloorToInt(gameSeconds % 216000)}";
                
            var root = GetComponent<UIDocument>().rootVisualElement;
            root.Q<Button>(exitButtonName).clicked += () =>
            {
                var saving = FindFirstObjectByType<SavingManager>();
                saving.Save((_, _) =>
                {
                    Application.Quit();
                });
            };
            LocalizationManager.LocalizeUI(root, this);
        }
    }
}