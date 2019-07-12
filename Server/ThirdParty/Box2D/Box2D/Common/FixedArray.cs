using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Box2DSharp.Common
{
    public interface IFixedArray<T>
        where T : unmanaged
    {
        int Length { get; }

        ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        T[] ToArray();
    }

    public static class FixedArray
    {
        public static IFixedArray<T> Create<T>(int count)
            where T : unmanaged
        {
            switch (count)
            {
            case 1:
                return new FixedArray1<T>();
            case 2:
                return new FixedArray2<T>();
            case 3:
                return new FixedArray3<T>();
            case 4:
                return new FixedArray4<T>();
            case 5:
                return new FixedArray5<T>();
            case 6:
                return new FixedArray6<T>();
            case 7:
                return new FixedArray7<T>();
            case 8:
                return new FixedArray8<T>();
            default:
                throw new NotImplementedException($"FixedArray with {count} items doesn't implement");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static unsafe ref T GetRef<T>(this IFixedArray<T> array, T* ptr, int index)
            where T : unmanaged
        {
            if (index > -1 && index < array.Length)
            {
                return ref *(ptr + index);
            }

            throw new IndexOutOfRangeException($"{nameof(array.GetType)}'s index can't be {index}");
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray1<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        int IFixedArray<T>.Length => Length;

        private const int Length = 1;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray2<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        int IFixedArray<T>.Length => Length;

        private const int Length = 2;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray3<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        int IFixedArray<T>.Length => Length;

        private const int Length = 3;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1, Value2};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray4<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public T Value3;

        int IFixedArray<T>.Length => Length;

        private const int Length = 4;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1, Value2, Value3};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray5<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public T Value3;

        public T Value4;

        int IFixedArray<T>.Length => Length;

        private const int Length = 5;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1, Value2, Value3, Value4};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray6<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public T Value3;

        public T Value4;

        public T Value5;

        int IFixedArray<T>.Length => Length;

        private const int Length = 6;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1, Value2, Value3, Value4, Value5};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray7<T> : IFixedArray<T>
        where T : unmanaged
    {
        public T Value0;

        public T Value1;

        public T Value2;

        public T Value3;

        public T Value4;

        public T Value5;

        public T Value6;

        int IFixedArray<T>.Length => Length;

        private const int Length = 7;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1, Value2, Value3, Value4, Value5, Value6};
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FixedArray8<T> : IFixedArray<T>
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

        int IFixedArray<T>.Length => Length;

        private const int Length = 8;

        public ref T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                unsafe
                {
                    fixed (T* ptr = &Value0)
                    {
                        return ref this.GetRef(ptr, index);
                    }
                }
            }
        }

        public T[] ToArray()
        {
            return new[] {Value0, Value1, Value2, Value3, Value4, Value5, Value6, Value7};
        }
    }
}