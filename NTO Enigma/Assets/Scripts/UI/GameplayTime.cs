using UnityEngine;

namespace NTO
{
    public class GameplayTime : MonoBehaviour
    {
        public static float GameplaySeconds { get; private set; }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            GameplaySeconds += Time.deltaTime;
        }
    }
}