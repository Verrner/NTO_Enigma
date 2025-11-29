using UnityEngine.UIElements;

namespace NTO
{
    [UxmlElement]
    public partial class LocalizedButton : Button, ILocalizable
    {
        [UxmlAttribute] private string _key;
        public string Key => _key;

        public void SetValue(string value) => text = value;
    }
}