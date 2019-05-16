#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR

    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;

#endif

    // Example demonstrating how use context objects in custom drawers.
    [InfoBox("As of Odin 2.0, all drawers are now instanced per property. This means that the previous context system is now unnecessary as you can just make fields directly in the drawer.")]
    public class InstancedDrawerExample : MonoBehaviour
    {
        [InstancedDrawerExample]
        public int Field;
    }

    // The attribute used by the InstancedDrawerExampleAttributeDrawer.
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class InstancedDrawerExampleAttribute : Attribute
    { }

#if UNITY_EDITOR

    // Place the drawer script file in an Editor folder.
    public class InstancedDrawerExampleAttributeDrawer : OdinAttributeDrawer<InstancedDrawerExampleAttribute>
    {
        private int counter;
        private bool counterEnabled;

        // The new Initialize method is called when the drawer is first instanciated.
        protected override void Initialize()
        {
            this.counter = 0;
            this.counterEnabled = false;
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Count the frames.
            if (Event.current.type == EventType.Layout && this.counterEnabled)
            {
                this.counter++;
                GUIHelper.RequestRepaint();
            }

            // Draw the current frame count, and a start stop button.
            SirenixEditorGUI.BeginBox();
            {
                GUILayout.Label("Frame Count: " + this.counter);

                if (GUILayout.Button(this.counterEnabled ? "Stop" : "Start"))
                {
                    this.counterEnabled = !this.counterEnabled;
                }
            }
            SirenixEditorGUI.EndBox();

            // Continue the drawer chain.
            this.CallNextDrawer(label);
        }
    }

#endif
}
#endif
