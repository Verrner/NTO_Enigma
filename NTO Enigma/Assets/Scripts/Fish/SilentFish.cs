using UnityEngine;

namespace NTO
{
    [CreateAssetMenu(menuName = "Configs/Fish/Silent", fileName = "Silent Fish Config")]
    public class SilentFish : Fish
    {
        [Header("Silent"), SerializeField, Min(0)] private float timeUntilDeath;
        [SerializeField] private string sentenceKey = "silent-sentence";
        [SerializeField, Min(0)] private float verticalOffsetForSwimAway;

        private float _startVerticalPosition;
        private Rigidbody _submarineRigidbody;
        private SubmarineFish _submarineFish;
        
        private float _timeUntilDeath;
        
        public override void Appeared(SubmarineFish submarineFish)
        {
            _submarineFish = submarineFish;
            _submarineRigidbody = submarineFish.GetComponent<Rigidbody>();
            _startVerticalPosition = _submarineRigidbody.position.y;
            _timeUntilDeath = 0;
        }

        public override void UpdateFish()
        {
            if (_timeUntilDeath >= timeUntilDeath)
            {
                if (_startVerticalPosition - _submarineRigidbody.position.y >= verticalOffsetForSwimAway)
                {
                    SwimAway();
                    return;
                }
                
                Die();
                return;
            }

            _timeUntilDeath += Time.deltaTime;
        }

        public override void Leave(){}
        
        private void SwimAway()
        {
            _submarineFish.FishLeave(this);
        }

        private void Die()
        {
            DeathUI.ShowDeath(sentenceKey, this);
            SwimAway();
        }
    }
}