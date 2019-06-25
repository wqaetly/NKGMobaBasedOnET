#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;

    public class EnumPagingExamples : MonoBehaviour
    {
        [EnumPaging]
        public SomeEnum SomeEnumField;
        
        public enum SomeEnum
        {
            A, B, C
        }

#if UNITY_EDITOR

        [ShowInInspector]
        [EnumPaging, OnValueChanged("SetCurrentTool")]
        [InfoBox("Example of using EnumPaging together with OnValueChanged.")]
        private UnityEditor.Tool sceneTool;



        private void SetCurrentTool()
        {
            UnityEditor.Tools.current = this.sceneTool;
        }

#endif
    }
}
#endif
