using UnityEngine;

namespace NTO
{
    [CreateAssetMenu(menuName = "Configs/Fish/Stalker", fileName = "Stalker Fish Config")]
    public class StalkerFish : Fish
    {
        [Header("Stalker"), SerializeField, Min(0)] private float timeUntilDeath;
        [SerializeField] private string sentenceKey = "stalker-sentence";
        [SerializeField, Min(0)] private float timeForSwimAway;

        private SubmarineMovement _submarineMovement;
        private SubmarineFish _submarineFish;
        
        private float _timeForSwimAway;
        private float _timeUntilDeath;
        
        public override void Appeared(SubmarineFish submarineFish)
        {
            _submarineFish = submarineFish;
            _submarineMovement = submarineFish.Submarine.Movement;
            _timeForSwimAway = 0;
            _timeUntilDeath = 0;
        }

        public override void UpdateFish()
        {
            if (_timeForSwimAway >= timeForSwimAway)
            {
                SwimAway();
                return;
            }

            if (_timeUntilDeath >= timeUntilDeath)
            {
                Die();
                return;
            }

            _timeForSwimAway = _submarineMovement.speedMode == SubmarineMovement.SpeedMode.Fast
                ? _timeForSwimAway + Time.deltaTime : 0;
            _timeUntilDeath += Time.deltaTime;
        }

        public override void Leave(){}
        
        private void SwimAway()
        {
            _submarineFish.FishLeave(this);
        }

        private void Die()
        {
            Death.ShowDeath(sentenceKey, this);
            SwimAway();
        }
    }
}