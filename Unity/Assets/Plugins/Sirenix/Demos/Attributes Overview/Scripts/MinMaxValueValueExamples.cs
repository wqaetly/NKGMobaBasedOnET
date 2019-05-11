#if UNITY_EDITOR
namespace Sirenix.OdinInspector.Demos
{
    using UnityEngine;

    public class MinMaxValueValueExamples : MonoBehaviour
    {
        // Bytes
        [Title("Bytes")]
        [MinValue(100)]
        public byte ByteMinValue100;

        [MaxValue(100)]
        public byte ByteMaxValue100;

        [MinValue(0)]
        public sbyte SbyteMinValue0;

        [MaxValue(0)]
        public sbyte SbyteMaxValue0;

        // Shorts
        [Title("Int 16")]
        [MinValue(0)]
        public short ShortMinValue0;

        [MaxValue(0)]
        public short ShortMaxValue0;

        [MinValue(100)]
        public ushort UshortMinValue100;

        [MaxValue(100)]
        public ushort UshortMaxValue100;

        // Ints
        [Title("Int 32")]
        [MinValue(0)]
        public int IntMinValue0;

        [MaxValue(0)]
        public int IntMaxValue0;

        [MinValue(100)]
        public uint UintMinValue100;

        [MaxValue(100)]
        public uint UintMaxValue100;

        // Longs
        [Title("Int 64")]
        [MinValue(0)]
        public long LongMinValue0;

        [MaxValue(0)]
        public long LongMaxValue0;

        [MinValue(100)]
        public ulong UlongMinValue100;

        [MaxValue(100)]
        public ulong UlongMaxValue100;

        // Floats
        [Title("Float")]
        [MinValue(0)]
        public float FloatMinValue0;

        [MaxValue(0)]
        public float FloatMaxValue0;

        [MinValue(0)]
        public double DoubleMinValue0;

        [MaxValue(0)]
        public double DoubleMaxValue0;

        // Decimal
        [Title("Decimal")]
        [MinValue(0)]
        public decimal DecimalMinValue0;

        [MaxValue(0)]
        public decimal DecimalMaxValue0;

        // Vectors
        [Title("Vectors")]
        [MinValue(0)]
        public Vector2 Vector2MinValue0;

        [MaxValue(0)]
        public Vector2 Vector2MaxValue0;

        [MinValue(0)]
        public Vector3 Vector3MinValue0;

        [MaxValue(0)]
        public Vector3 Vector3MaxValue0;

        [MinValue(0)]
        public Vector4 Vector4MinValue0;

        [MaxValue(0)]
        public Vector4 Vector4MaxValue0;
    }
}
#endif
