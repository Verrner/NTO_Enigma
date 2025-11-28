using UnityEngine.UIElements;

namespace NTO
{
    [UxmlElement]
    public partial class LocalizedLabel : Label, ILocalizable
    {
        [UxmlAttribute] private string key;
        public string Key => key;

        public void SetValue(string value) => text = value;
    }
}