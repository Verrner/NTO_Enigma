using UnityEngine.UIElements;

namespace NTO
{
    [UxmlElement]
    public partial class LocalizedSlider : Slider, ILocalizable
    {
        [UxmlAttribute] private string _key;

        public string Key => _key;
        public void SetValue(string value) => label = value;
    }
}