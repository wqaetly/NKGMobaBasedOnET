using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Box2DSharp.Common
{
    public static class FixedArrayExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetRef<T>(ref this FixedArray2<T> array, int index)
            where T : unmanaged
        {
            switch (index)
            {
            case 0:
                return ref array.Value0;
            case 1:
                return ref array.Value1;
            default:
                throw new IndexOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetRef<T>(ref this FixedArray3<T> array, int index)
            where T : unmanaged
        {
            switch (index)
            {
            case 0:
                return ref array.Value0;
            case 1:
                return ref array.Value1;
            case 2:
                return ref array.Value2;
            default:
                throw new IndexOutOfRangeException();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetRef<T>(ref this FixedArray8<T> array, int index)
            where T : unmanaged
        {
            switch (index)
            {
            case 0:
                return ref array.Value0;
            case 1:
                return ref array.Value1;
            case 2:
                return ref array.Value2;
            case 3:
                return ref array.Value3;
            case 4:
                return ref array.Value4;
            case 5:
                return ref array.Value5;
            case 6:
                return ref array.Value6;
            case 7:
                return ref array.Value7;
            default:
                throw new IndexOutOfRangeException();
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray2<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public const int Length = 2;

        public unsafe ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index > -1 && index < Length)
                {
                    return ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref Value0), index));
                }

                throw new IndexOutOfRangeException();
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray3<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public const int Length = 3;

        public unsafe ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index > -1 && index < Length)
                {
                    return ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref Value0), index));
                }

                throw new IndexOutOfRangeException();
            }
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

        public const int Length = 8;

        public unsafe ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (index > -1 && index < Length)
                {
                    return ref Unsafe.AsRef<T>(Unsafe.Add<T>(Unsafe.AsPointer(ref Value0), index));
                }

                throw new IndexOutOfRangeException();
            }
        }
    }
}