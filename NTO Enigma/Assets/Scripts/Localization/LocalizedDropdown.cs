using System.Linq;
using UnityEngine.UIElements;

namespace NTO
{
    [UxmlElement]
    public partial class LocalizedDropdown : DropdownField, ILocalizable
    {
        [UxmlAttribute] private string _key;
        public string Key => _key;
        public void SetValue(string value)
        {
            var parts = value.Split('|').ToList();
            label = parts[0];
            parts.RemoveAt(0);
            choices = parts;
        }
    }
}