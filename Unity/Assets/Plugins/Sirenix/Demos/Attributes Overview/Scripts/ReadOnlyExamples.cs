#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("ReadOnly disables properties in the inspector.")]
    public class ReadOnlyExamples : MonoBehaviour
    {
        [ReadOnly]
        public string MyString = "This is displayed as text";

        [ReadOnly]
        public int MyInt = 9001;

        [ReadOnly]
        public int[] MyIntList = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
    }
}
#endif
