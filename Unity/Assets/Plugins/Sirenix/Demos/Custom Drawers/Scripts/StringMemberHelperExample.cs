#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using System;
    using Sirenix.OdinInspector;

#if UNITY_EDITOR

    using Sirenix.OdinInspector.Editor;
    using Sirenix.Utilities.Editor;
    using UnityEditor;

#endif

    [TypeInfoBox("Using StringMemberHelper, it's possible to get a static string, or refer to a member string with very little effort.")]
    public class StringMemberHelperExample : MonoBehaviour
    {
        [PostLabel("A static label")]
        public int MyIntValue;

        [PostLabel("$DynamicLabel")]
        public string DynamicLabel = "A dynamic label";

        [PostLabel("$Invalid")]
        public float InvalidReference;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PostLabelAttribute : Attribute
    {
        public string Name { get; private set; }

        public PostLabelAttribute(string name)
        {
            this.Name = name;
        }
    }

#if UNITY_EDITOR

    public sealed class PostLabelAttributeDrawer : OdinAttributeDrawer<PostLabelAttribute>
    {
        private StringMemberHelper stringMemberHelper;

        protected override void Initialize()
        {
            this.stringMemberHelper = new StringMemberHelper(this.Property.ParentType, this.Attribute.Name);
        }

        protected override void DrawPropertyLayout(GUIContent label)
        {
            // Display error
            if (this.stringMemberHelper.ErrorMessage != null)
            {
                SirenixEditorGUI.ErrorMessageBox(this.stringMemberHelper.ErrorMessage);
                this.CallNextDrawer(label);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                this.CallNextDrawer(null);

                // Get the string from the string member helper.
                EditorGUILayout.PrefixLabel(this.stringMemberHelper.GetString(this.Property));

                EditorGUILayout.EndHorizontal();
            }
        }
    }

#endif
}
#endif
