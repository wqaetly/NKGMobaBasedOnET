#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    using System;

    public class DelegateExamples : SerializedMonoBehaviour
    {
        public Action<Vector3> A;
        public Func<int> B;
        public Action C;
        public Action<int> D;

        public void TestMethod(int someInt)
        {
            Debug.Log(someInt);
        }
    }
}
#endif
