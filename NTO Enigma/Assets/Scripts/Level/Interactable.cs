using UnityEngine;

namespace NTO
{
    public abstract class Interactable : MonoBehaviour
    {
        public abstract void Interact(CharacterInteraction character);
    }
}