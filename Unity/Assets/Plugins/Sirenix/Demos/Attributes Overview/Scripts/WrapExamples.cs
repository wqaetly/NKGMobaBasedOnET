#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class WrapExamples : MonoBehaviour
    {
        [Title("Angle and radian")]
        [Wrap(0f, 360)]
        public float AngleWrap;

        [Wrap(0f, Mathf.PI * 2)]
        public float RadianWrap;

        [Title("Type tests")]
        [Wrap(0f, 100f)]
        public short ShortWrapFrom0To100;

        [Wrap(0f, 100f)]
        public int IntWrapFrom0To100;

        [Wrap(0f, 100f)]
        public long LongWrapFrom0To100;

        [Wrap(0f, 100f)]
        public float FloatWrapFrom0To100;

        [Wrap(0f, 100f)]
        public double DoubleWrapFrom0To100;

        [Wrap(0f, 100f)]
        public decimal DecimalWrapFrom0To100;

        [Title("Vectors")]
        [Wrap(0f, 100f)]
        public Vector2 Vector2WrapFrom0To100;

        [Wrap(0f, 100f)]
        public Vector3 Vector3WrapFrom0To100;

        [Wrap(0f, 100f)]
        public Vector4 Vector4WrapFrom0To100;
    }
}
#endif
