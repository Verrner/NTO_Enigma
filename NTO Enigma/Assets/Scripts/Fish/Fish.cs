using UnityEngine;

namespace NTO
{
    public abstract class Fish : ScriptableObject
    {
        [SerializeField, Range(0, 100)] private float chanceOfAppearance;
        [SerializeField] private Texture2D texture;
        [SerializeField] private string uiNameLocalizationKey;
        [SerializeField] private string descriptionLocalizationKey;

        public float ChanceOfAppearance => chanceOfAppearance;
        public Texture2D Texture => texture;
        public string UINameLocalizationKey => uiNameLocalizationKey;
        public string DescriptionLocalizationKey => descriptionLocalizationKey;

        public abstract void Appeared(SubmarineFish submarineFish);
        public abstract void UpdateFish();
        public abstract void Leave();
    }
}