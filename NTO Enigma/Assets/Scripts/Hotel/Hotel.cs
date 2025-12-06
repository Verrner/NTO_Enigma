using UnityEngine;

namespace NTO
{
    public class Hotel : MonoBehaviour
    {
        [SerializeField] private EventInteractable bedInteractable;
        [SerializeField] private GameObject gameOverUIObject;

        private void Awake()
        {
            bedInteractable.Interacted += _ => BedInteracted();
        }

        private void BedInteracted()
        {
            gameOverUIObject.SetActive(true);
        }
    }
}