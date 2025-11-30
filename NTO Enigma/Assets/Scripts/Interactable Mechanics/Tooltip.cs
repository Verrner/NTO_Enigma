using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Collider))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private string tooltipKey;
        
        private void OnMouseEnter()
        {
            MainGameplayUI.SetTooltip(tooltipKey, this);    
        }

        private void OnMouseExit()
        {
            MainGameplayUI.ResetTooltip();
        }
    }
}