using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(EventInteractable), typeof(Tooltip), typeof(BoxCollider)),
     RequireComponent(typeof(Rigidbody))]
    public class RoomDoor : MonoBehaviour
    {
        [SerializeField] private RoomUI roomUI;

        private void Awake()
        {
            GetComponent<EventInteractable>().Interacted += interaction => roomUI.Open(interaction.Character);
        }
    }
}