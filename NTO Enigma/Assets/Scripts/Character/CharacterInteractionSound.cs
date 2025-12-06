using UnityEngine;

namespace NTO
{
    public class CharacterInteractionSound : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        private static CharacterInteractionSound _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static void Play(AudioClip clip)
        {
            _instance.source.clip = clip;
            _instance.source.Play();
        }
    }
}