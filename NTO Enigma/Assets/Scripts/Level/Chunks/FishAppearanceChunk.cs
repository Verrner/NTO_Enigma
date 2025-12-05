using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NTO
{
    public class FishAppearanceChunk : Chunk
    {
        [SerializeField] private Fish[] fish;
        [SerializeField, Min(1)] private int minFishToAppear = 1;
        [SerializeField] private int maxFishToAppear = 3;

        private SubmarineFish _submarineFish;
        private readonly List<Fish> _fishAppeared = new List<Fish>();
        
        private void Awake()
        {
            _submarineFish = FindFirstObjectByType<SubmarineFish>();
        }

        protected override void SubmarineEntered(Submarine submarine)
        {
            var fishToAppear = Random.Range(minFishToAppear, Mathf.Min(maxFishToAppear + 1, fish.Length));

            var fishAvailable = fish.ToList();
            for (var i = 0; i < fishToAppear; i++)
                fishAvailable.Remove(ChoseFish(fishAvailable));
        }

        private Fish ChoseFish(List<Fish> fishAvailable)
        {
            var sumChances = fishAvailable.Select(f => f.ChanceOfAppearance).Sum();
            var previousChance = 0f;

            foreach (var f in fishAvailable)
            {
                if (f.ChanceOfAppearance <= Random.Range(0, sumChances - previousChance))
                {
                    _fishAppeared.Add(f);
                    _submarineFish.FishAppeared(f);
                    return f;
                }
                previousChance += f.ChanceOfAppearance;
            }

            return null;
        }

        protected override void SubmarineLeave(Submarine submarine)
        {
            foreach (var f in _fishAppeared)
                _submarineFish.FishLeave(f);
        }

        public override void SubmarineInside(Submarine submarine){}
    }
}