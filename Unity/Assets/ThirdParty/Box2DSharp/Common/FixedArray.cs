using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Box2DSharp.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray2<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public unsafe T* Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (T* ptr = &Value0)
                {
                    return ptr;
                }
            }
        }

        public unsafe T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Values[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Values[index] = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray3<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public unsafe T* Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (T* ptr = &Value0)
                {
                    return ptr;
                }
            }
        }

        public unsafe T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Values[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Values[index] = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray4<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public T Value3;

        public unsafe T* Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (T* ptr = &Value0)
                {
                    return ptr;
                }
            }
        }

        public unsafe T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Values[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Values[index] = value;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray8<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public T Value3;

        public T Value4;

        public T Value5;

        public T Value6;

        public T Value7;

        public unsafe T* Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (T* ptr = &Value0)
                {
                    return ptr;
                }
            }
        }

        public unsafe T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Values[index];
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Values[index] = value;
        }
    }
}