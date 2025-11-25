using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Character))]
    public sealed class CharacterInteraction : MonoBehaviour
    {
        [SerializeField, Min(0)] private float radius;
        [SerializeField] private new Camera camera;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Character character;

        public bool canInteract = true;
        public Character Character => character;

        private void Update()
        {
            UpdateInteraction();
        }

        private void UpdateInteraction()
        {
            if (!canInteract)
                return;
            
            if (Input.GetMouseButtonDown(0))
                TryToInteract();
        }

        private void TryToInteract()
        {
            var ray = camera.ScreenPointToRay(new Vector3(Screen.width, Screen.height) / 2);
            
            if (!Physics.Raycast(ray, out RaycastHit hit, radius, layerMask))
                return;

            Debug.Log(hit.transform.name);
            
            var interactable = hit.transform.GetComponent<Interactable>();
            if (interactable != null)
                interactable.Interact(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var ray = camera.ScreenPointToRay(new Vector3(Screen.width, Screen.height) / 2);
            Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * radius);
        }
    }
}