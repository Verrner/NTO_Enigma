using UnityEngine;

namespace NTO
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] private AudioSource interactionAudioSource;
        
        public abstract void Interact(CharacterInteraction character);

        protected void Play()
        {
            if (interactionAudioSource != null)
                interactionAudioSource.Play();
        }
    }
}