#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR

    using Sirenix.OdinInspector.Editor;
    using System.Reflection;
    using Sirenix.Utilities;
    using Sirenix.Utilities.Editor;

#endif

    // Example demonstrating how reflection can be used to enhance custom drawers.
    [TypeInfoBox(
            "This example demonstrates how reflection can be used to extend drawers from what otherwise would be possible.\n" +
            "In this case, a user can specify one of their own methods to receive a callback from the drawer chain.")]
    public class ReflectionExample : MonoBehaviour
    {
        [OnClickMethod("OnClick")]
        public int InstanceMethod;

        [OnClickMethod("StaticOnClick")]
        public int StaticMethod;

        [OnClickMethod("InvalidOnClick")]
        public int InvalidMethod;

        private void OnClick()
        {
            Debug.Log("Hello?");
        }

        private static void StaticOnClick()
        {
            Debug.Log("Static Hello?");
        }
    }

    // Attribute with name of call back method.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class OnClickMethodAttribute : Attribute
    {
        public string MethodName { get; private set; }

        public OnClickMethodAttribute(string methodName)
        {
            this.MethodName = methodName;
        }
    }

#if UNITY_EDITOR

    // Place the drawer script file in an Editor folder.
    // Remember to add the OdinDrawer to your custom drawer classes, or they will not be found by Odin.
    public class OnClickMethodAttributeDrawer : OdinAttributeDrawer<OnClickMethodAttribute>
    {
        // This field is used to display errors messages to the user, if something goes wrong.
        private string ErrorMessage;

        // Reference to the method specified by the user in the attribute.
        private MethodInfo Method;

        protected override void Initialize()
        {
            // Use MemberFinder to find the specified method, and store the method info in the context object.
            this.Method = MemberFinder.Start(this.Property.ParentType)
                .IsMethod()
                .HasNoParameters()
                .IsNamed(this.Attribute.MethodName)
                .GetMember<MethodInfo>(out this.ErrorMessage);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Display any error that might have occured.
            if (this.ErrorMessage != null)
            {
                SirenixEditorGUI.ErrorMessageBox(this.ErrorMessage);

                // Continue drawing the rest of the property as normal.
                this.CallNextDrawer(label);
            }
            else
            {
                // Get the mouse down event.
                bool clicked = Event.current.rawType == EventType.MouseDown && Event.current.button == 0 && this.Property.LastDrawnValueRect.Contains(Event.current.mousePosition);

                if (clicked)
                {
                    // Invoke the method stored in the context object.
                    if (this.Method.IsStatic)
                    {
                        this.Method.Invoke(null, null);
                    }
                    else
                    {
                        this.Method.Invoke(this.Property.ParentValues[0], null);
                    }
                }

                // Draw the property.
                this.CallNextDrawer(label);

                if (clicked)
                {
                    // If the event havn't been used yet, then use it here.
                    Event.current.Use();
                }
            }
        }
    }

#endif
}
#endif
