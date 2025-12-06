using UnityEngine;
using UnityEngine.SceneManagement;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineHotel : MonoBehaviour
    {
        [SerializeField] private Tooltip hotelDoorTooltip;
        [SerializeField] private EventInteractable hotelDoorInteractable;

        [HideInInspector, LocalizationDynamicVariable("can-enter-hotel")] public bool canEnterHotel;
        [HideInInspector] public string sceneName;

        private void Awake()
        {
            hotelDoorTooltip.LocalizationSource = this;
            hotelDoorInteractable.Interacted += _ => Interacted();
        }

        private void Interacted()
        {
            if (!canEnterHotel)
                return;

            SceneManager.LoadScene(sceneName);
        }
    }
}