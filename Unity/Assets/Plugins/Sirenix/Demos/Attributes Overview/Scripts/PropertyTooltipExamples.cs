#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("PropertyTooltip is used to add tooltips to properties in the inspector.\nPropertyTooltip can also be applied to properties and methods, unlike Unity's Tooltip attribute.")]
    public class PropertyTooltipExamples : MonoBehaviour
    {
        [PropertyTooltip("This is tooltip on an int property.")]
        public int MyInt;

        [InfoBox("Use $ to refer to a member string.")]
        [PropertyTooltip("$Tooltip")]
        public string Tooltip = "Dynamic tooltip.";

        [Button, PropertyTooltip("Button Tooltip")]
        private void ButtonWithTooltip()
        {

        }
    }
}
#endif
