using UnityEngine;

namespace NTO
{
    public class CharacterAudio : MonoBehaviour
    {
        [Header("Steps"), SerializeField] private AudioSource stepsSource;

        public void RefreshStepsSource(bool enable)
        {
            if (enable)
                stepsSource.Play();
            else
                stepsSource.Stop();
        }
    }
}