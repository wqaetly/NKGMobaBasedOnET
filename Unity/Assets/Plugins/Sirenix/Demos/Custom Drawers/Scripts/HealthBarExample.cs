#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR

    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using Sirenix.Utilities.Editor;
    using Sirenix.Utilities;

#endif

    // Example demonstrating how to create a custom drawer for an attribute.
    [TypeInfoBox("Here a visualization of a health bar being drawn with with a custom attribute drawer.")]
    public class HealthBarExample : MonoBehaviour
    {
        [HealthBar(100)]
        public float Health;
    }

    // Attribute used by HealthBarAttributeDrawer.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HealthBarAttribute : Attribute
    {
        public float MaxHealth { get; private set; }

        public HealthBarAttribute(float maxHealth)
        {
            this.MaxHealth = maxHealth;
        }
    }

#if UNITY_EDITOR

    // Place the drawer script file in an Editor folder or wrap it in a #if UNITY_EDITOR condition.
    public class HealthBarAttributeDrawer : OdinAttributeDrawer<HealthBarAttribute, float>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Call the next drawer, which will draw the float field.
            this.CallNextDrawer(label);

            // Get a rect to draw the health-bar on. You Could also use GUILayout instead, but using rects makes it simpler to draw the health bar.
            Rect rect = EditorGUILayout.GetControlRect();

            // Draw the health bar.
            float width = Mathf.Clamp01(this.ValueEntry.SmartValue / this.Attribute.MaxHealth);
            SirenixEditorGUI.DrawSolidRect(rect, new Color(0f, 0f, 0f, 0.3f), false);
            SirenixEditorGUI.DrawSolidRect(rect.SetWidth(rect.width * width), Color.red, false);
            SirenixEditorGUI.DrawBorders(rect, 1);
        }
    }

#endif
}
#endif
