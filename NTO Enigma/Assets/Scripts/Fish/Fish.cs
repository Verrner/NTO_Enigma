using UnityEngine;

namespace NTO
{
    public abstract class Fish : ScriptableObject
    {
        [SerializeField, Range(0, 100)] private float chanceOfAppearance;
        [SerializeField] private Texture2D texture;
        [SerializeField] private AudioClip audioClip;

        public float ChanceOfAppearance => chanceOfAppearance;
        public Texture2D Texture => texture;
        public AudioClip AudioClip => audioClip;

        public abstract void Appeared(SubmarineFish submarineFish);
        public abstract void UpdateFish();
        public abstract void Leave();

        public virtual FishAppearanceOrientation[] GetAvailableOrientations() => new []
        {
            FishAppearanceOrientation.Top, FishAppearanceOrientation.Right, FishAppearanceOrientation.Left, FishAppearanceOrientation.Back
        };
    }
}