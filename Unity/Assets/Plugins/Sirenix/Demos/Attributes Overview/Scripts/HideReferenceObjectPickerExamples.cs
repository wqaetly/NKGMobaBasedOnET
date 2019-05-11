#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    [TypeInfoBox(
        "When the object picker is hidden, you can right click and set the instance to null, in order to set a new value.\n\n" +
        "If you don't want this behavior, you can use DisableContextMenu attribute to ensure people can't change the value.")]
    public class HideReferenceObjectPickerExamples : SerializedMonoBehaviour
    {
        [Title("Hidden Object Pickers")]
        [HideReferenceObjectPicker]
        public MyCustomReferenceType OdinSerializedProperty1;

        [HideReferenceObjectPicker]
        public MyCustomReferenceType OdinSerializedProperty2;

        [Title("Shown Object Pickers")]
        public MyCustomReferenceType OdinSerializedProperty3;

        [InfoBox("Protip: You can also put the HideInInspector attribute on the class definition itself to hide it globally for all members.")]
        public MyCustomReferenceType OdinSerializedProperty4;

        // [HideReferenceObjectPicker]
        public class MyCustomReferenceType
        {
            public int A;
            public int B;
            public int C;
        }
    }
}
#endif
