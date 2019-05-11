#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos.RPGEditor
{
    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;
    using UnityEditor;
    using UnityEngine;

    //
    // All items are scriptable objects, but we don't want to draw them with the boring scriptable object icon.
    // Here we create a custom drawer for all Item types, that renders a preview-field using the item icon followed
    // by the name of the item.
    //

    public class ItemDrawer<TItem> : OdinValueDrawer<TItem>
        where TItem : Item
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rect = EditorGUILayout.GetControlRect(label != null, 45);

            if (label != null)
            {
                rect.xMin = EditorGUI.PrefixLabel(rect.AlignCenterY(15), label).xMin;
            }
            else
            {
                rect = EditorGUI.IndentedRect(rect);
            }

            Item item = this.ValueEntry.SmartValue;
            Texture texture = null;

            if (item)
            {
                texture = GUIHelper.GetAssetThumbnail(item.Icon, typeof(TItem), true);
                GUI.Label(rect.AddXMin(50).AlignMiddle(16), EditorGUI.showMixedValue ? "-" : item.Name);
            }

            this.ValueEntry.WeakSmartValue = SirenixEditorFields.UnityPreviewObjectField(rect.AlignLeft(45), item, texture, this.ValueEntry.BaseValueType);
        }
    }
}
#endif
