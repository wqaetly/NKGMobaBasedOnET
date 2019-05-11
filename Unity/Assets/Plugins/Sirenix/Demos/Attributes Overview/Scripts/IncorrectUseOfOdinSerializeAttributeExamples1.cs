#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using Sirenix.Serialization;
    using UnityEngine;

    [TypeInfoBox(
        "None of the members of this class will be serialized by Odin because Odin serialization have not been implemented.\n" +
        "You can verify this for yourself by using the Serialization Debugger.")]
    public class IncorrectUseOfOdinSerializeAttributeExamples1 : MonoBehaviour
    {
        // None of these members will be serialized by Odin because we are not inheriting from SerializedMonoBehaviour.

        [OdinSerialize, ShowInInspector]
        private MyCustomType1 SerializedField1;                             // Serialized by nothing

        [OdinSerialize, ShowInInspector]
        public MyCustomType2 SerializedField2;                              // Serialized by Unity

        [OdinSerialize, ShowInInspector]
        public MyCustomType1 SerializedProperty1 { get; private set; }      // Serialized by nothing

        [OdinSerialize, ShowInInspector]
        public MyCustomType2 SerializedProperty2 { get; private set; }      // Serialized by nothing

        [System.Serializable]
        public class MyCustomType1
        {
            public int Test;
        }

        public class MyCustomType2
        {
            public int Test;
        }
    }
}
#endif
