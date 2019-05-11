#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using Sirenix.Serialization;

    [TypeInfoBox(
        "This example demonstrates how a field in some cases can be serialized by both Odin and Unity at the same time.\n" +
        "You can verify this for yourself by using the Serialization Debugger.")]
    public class IncorrectUseOfOdinSerializeAttributeExamples2 : SerializedMonoBehaviour
    {
        [OdinSerialize]
        public MyCustomType UnityAndOdinSerializedField1;

        [OdinSerialize]
        public int UnityAndOdinSerializedField2;

        [System.Serializable]
        public class MyCustomType
        {
            public int Test;
        }
    }
}
#endif
