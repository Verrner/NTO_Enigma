using System;
using System.Collections.Generic;
using UnityEngine;

namespace NTO
{
    [RequireComponent(typeof(Submarine))]
    public class SubmarineFish : MonoBehaviour
    {
        [SerializeField] private Submarine submarine;
        [SerializeField] private float cameraWorkEnergyCost;

        public Submarine Submarine => submarine;
        
        private readonly List<(FishAppearanceOrientation, Fish)> _fish = new List<(FishAppearanceOrientation, Fish)>();

        private void Update()
        {
            foreach (var f in _fish)
                f.Item2.UpdateFish();
        }

        public void FishAppeared(Fish fish)
        {
            Debug.Log($"Appeared {fish.name}");

            if (IsFishAppeared(fish))
                return;
            
            if (!TryGetCorrectOrientation(fish, out FishAppearanceOrientation orientation))
                throw new Exception("All orientations used");
            
            fish.Appeared(this);

            _fish.Add((orientation, fish));
        }

        public void FishLeave(Fish fish)
        {
            Debug.Log($"Leave {fish.name}");

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