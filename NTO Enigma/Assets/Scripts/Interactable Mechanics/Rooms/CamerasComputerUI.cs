using UnityEngine;
using UnityEngine.UIElements;

namespace NTO
{
    public class CamerasComputerUI : RoomUI
    {
        [Header("Objects"), SerializeField] private SubmarineFish submarineFish;
        
        [Header("UI"), SerializeField] private string cameraViewElementName = "camera-view-element";
        [SerializeField] private string topCameraButtonName = "top-camera-button";
        [SerializeField] private string rightCameraButtonName = "right-camera-button";
        [SerializeField] private string backCameraButtonName = "back-camera-button";
        [SerializeField] private string leftCameraButtonName = "left-camera-button";

        private VisualElement _cameraViewElement;
        
        private FishAppearanceOrientation _cameraOrientation;
        
        protected override void Enable()
        {
            SetElements();
        }

        protected override void RoomOpened(Character character)
        {
            submarineFish.MadePhoto();
            RefreshCameraElement();
        }

        private void SetElements()
        {
            _cameraViewElement = Root.Q(cameraViewElementName);

            Root.Q<Button>(topCameraButtonName).clicked += () => SetCameraOrientation(FishAppearanceOrientation.Top);
            Root.Q<Button>(rightCameraButtonName).clicked += () => SetCameraOrientation(FishAppearanceOrientation.Right);
            Root.Q<Button>(backCameraButtonName).clicked += () => SetCameraOrientation(FishAppearanceOrientation.Back);
            Root.Q<Button>(leftCameraButtonName).clicked += () => SetCameraOrientation(FishAppearanceOrientation.Left);
        }

        private void SetCameraOrientation(FishAppearanceOrientation orientation)
        {
            if (_cameraOrientation == orientation)
                return;
            
            _cameraOrientation = orientation;
            RefreshCameraElement();
        }

        private void RefreshCameraElement()
        {
            var fishToDisplay = submarineFish.GetFishByOrientation(_cameraOrientation);
            if (fishToDisplay == null)
                DisplayEmpty();
            else
                DisplayFish(fishToDisplay);
        }

        private void DisplayEmpty()
        {
            _cameraViewElement.style.backgroundImage = null;
            _cameraViewElement.style.backgroundColor = Color.black;
        }

        private void DisplayFish(Fish fish)
        {
            _cameraViewElement.style.backgroundImage = new StyleBackground(fish.Texture);
            _cameraViewElement.style.backgroundColor = Color.white;
        }
    }
}