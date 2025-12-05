using UnityEngine;

namespace NTO
{
    public class Autosaving : MonoBehaviour
    {
        [SerializeField, Min(0)] private float timeForAutosaving;

        private SavingManager _savingManager;
        private float _timeSinceLastAutosaving;

        private void Awake()
        {
            _savingManager = FindAnyObjectByType<SavingManager>();
        }

        private void Update()
        {
            if (_timeSinceLastAutosaving >= timeForAutosaving)
            {
                _savingManager.Save();
                _timeSinceLastAutosaving -= timeForAutosaving;
            }

            _timeSinceLastAutosaving += Time.deltaTime;
        }
    }
}