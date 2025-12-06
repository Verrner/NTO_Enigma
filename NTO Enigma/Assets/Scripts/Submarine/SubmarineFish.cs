using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineFish : MonoBehaviour
    {
        [Serializable]
        private struct FishSourceInfo
        {
            public FishAppearanceOrientation orientation;
            public AudioSource source;
        }
        
        [SerializeField] private Submarine submarine;
        [SerializeField] private float cameraWorkEnergyCost;
        [SerializeField] private FishSourceInfo[] fishSources;

        public Submarine Submarine => submarine;
        
        private readonly List<(FishAppearanceOrientation, Fish)> _fish = new List<(FishAppearanceOrientation, Fish)>();

        private void Update()
        {
            if (Death.Dead)
                return;
            
            foreach (var f in _fish)
                f.Item2.UpdateFish();
        }

        public void FishAppeared(Fish fish)
        {
            if (IsFishAppeared(fish))
                return;
            
            if (!TryGetCorrectOrientation(fish, out FishAppearanceOrientation orientation))
                throw new Exception("All orientations used");
            
            var source = fishSources.ToList().Find(o => o.orientation == orientation);
            source.source.clip = fish.AudioClip;
            source.source.Play();
            
            fish.Appeared(this);

            _fish.Add((orientation, fish));
        }

        public void FishLeave(Fish fish)
        {
            if (!IsFishAppeared(fish))
                return;
            
            fish.Leave();
            _fish.RemoveAt(_fish.FindIndex(f => f.Item2 == fish));
        }

        private bool IsFishAppeared(Fish fish) => _fish.Exists(f => f.Item2 == fish);

        private bool TryGetCorrectOrientation(Fish fish, out FishAppearanceOrientation orientation)
        {
            var availableOrientations = fish.GetAvailableOrientations();

            foreach (var o in availableOrientations)
            {
                var index = _fish.FindIndex(f => f.Item1 == o);
                if (index != -1)
                    continue;
                orientation = o;
                return true;
            }

            orientation = FishAppearanceOrientation.Top;
            return false;
        }

        public Fish GetFishByOrientation(FishAppearanceOrientation orientation)
        {
            var index = _fish.FindIndex(f => f.Item1 == orientation);
            return index == -1 ? null : _fish[index].Item2;
        }

        public void MadePhoto()
        {
            submarine.Energy.Energy -= cameraWorkEnergyCost;
        }
    }
}