using UnityEngine.UIElements;

namespace NTO
{
    [UxmlElement]
    public partial class TooltipVisualElement : VisualElement, ITooltipUIElement
    {
        [UxmlAttribute] private string _key;
        public string Key => _key;
    }
}