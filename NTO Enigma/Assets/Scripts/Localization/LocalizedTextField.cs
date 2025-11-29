using UnityEngine.UIElements;

namespace NTO
{
    [UxmlElement]
    public partial class LocalizedTextField : TextField, ILocalizable
    {
        [UxmlAttribute] private string _key;
        public string Key => _key;
        public void SetValue(string value) => label = value;
    }
}