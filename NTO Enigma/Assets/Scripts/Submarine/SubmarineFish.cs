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
            
            fish.Appeared(this);

            if (!TryGetNotUsedFishAppearanceOrientation(out FishAppearanceOrientation orientation))
                throw new Exception("All orientations used");
            
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

        private bool TryGetNotUsedFishAppearanceOrientation(out FishAppearanceOrientation orientation)
        {
            for (var i = 0; i < 4; i++)
            {
                var curOrientation = (FishAppearanceOrientation)i;
                var index = _fish.FindIndex(f => f.Item1 == curOrientation);
                if (index != -1) continue;
                orientation = curOrientation;
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