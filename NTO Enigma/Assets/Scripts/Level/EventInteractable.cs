using System;

namespace NTO
{
    public sealed class EventInteractable : Interactable
    {
        public event Action<CharacterInteraction> Interacted;
        
        public override void Interact(CharacterInteraction character)
        {
            Play();
            Interacted?.Invoke(character);
        }
    }
}