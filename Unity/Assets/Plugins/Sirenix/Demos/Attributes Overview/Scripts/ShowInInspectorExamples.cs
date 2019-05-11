#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    [TypeInfoBox("ShowInInspector is used to display properties that otherwise wouldn't be shown in the inspector. Such as non-serialized fields or properties.")]
    public class ShowInInspectorExamples : MonoBehaviour
    {
        [ShowInInspector]
        private int myPrivateInt;

        [ShowInInspector]
        public int MyPropertyInt { get; set; }

        [ShowInInspector]
        public int ReadOnlyProperty
        {
            get { return this.myPrivateInt; }
        }

        [ShowInInspector]
        public Vector3 WorldSpacePosition
        {
            get { return this.transform.position; }
            set { this.transform.position = value; }
        }

        [ShowInInspector]
        public Vector3 WorldSpaceScale
        {
            get { return this.transform.lossyScale; }
            set
            {
                var m = this.transform.parent == null ? Matrix4x4.identity : this.transform.parent.worldToLocalMatrix;
                this.transform.localScale = new Vector3(m.m00 * value.x, m.m11 * value.y, m.m22 * value.z);
            }
        }

        [ShowInInspector]
        public Quaternion WorldSpaceRotation
        {
            get { return this.transform.rotation; }
            set { this.transform.rotation = value; }
        }
    }
}
#endif
