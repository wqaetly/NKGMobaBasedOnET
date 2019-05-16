#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR

    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;

#endif

    // Example demonstrating drawer priorities.
    [TypeInfoBox(
        "In this example, we have three different drawers, with different priorities, all drawing the same value.\n\n" +
        "The purpose is to demonstrate the drawer chain, and the general purpose of each drawer priority.")]
    public class PriorityExamples : MonoBehaviour
    {
        [ShowDrawerChain] // Displays all drawers involved with drawing the property.
        public MyClass MyClass;
    }

    [Serializable]
    public class MyClass
    {
        public string Name;
        public float Value;
    }

#if UNITY_EDITOR

    // This drawer is configured to have super priority. Of the three drawers here, this class will be called first.
    // In our example here, the super drawer instanciates the value, if it's null.
    [DrawerPriority(1, 0, 0)]
    public class CUSTOM_SuperPriorityDrawer : OdinValueDrawer<MyClass>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Create the value, if it's not created already.
            if (this.ValueEntry.SmartValue == null)
            {
                this.ValueEntry.SmartValue = new MyClass();
            }

            this.CallNextDrawer(label);
        }
    }

    // This drawer is configured to have wrapper priority, and is therefore be called second.
    // In this example, the wrapper drawer draws a box around the property.
    [DrawerPriority(0, 1, 0)]
    public class CUSTOM_WrapperPriorityDrawer : OdinValueDrawer<MyClass>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Draw a box around the property.
            SirenixEditorGUI.BeginBox(label);
            this.CallNextDrawer(null);
            SirenixEditorGUI.EndBox();
        }
    }

    // This drawer is configured to have value priority, and is therefore called last.'
    // In this example, the value drawer draws the fields of the PriorityClass object.
    [DrawerPriority(0, 0, 1)]
    public class CUSTOM_ValuePriorityDrawer : OdinValueDrawer<MyClass>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Draw the value fields.
            this.ValueEntry.Property.Children["Name"].Draw();
            this.ValueEntry.Property.Children["Value"].Draw();
        }
    }

#endif
}
#endif
