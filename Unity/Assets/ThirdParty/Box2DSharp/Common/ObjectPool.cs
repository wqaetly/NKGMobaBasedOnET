using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Box2DSharp.Common
{
    public class ObjectPool<T>
    {
        private readonly Func<T> _create;

        private readonly Func<T, bool> _destroy;

        private readonly ConcurrentBag<T> _objects = new ConcurrentBag<T>();

        private readonly int _maximumRetained;

        public ObjectPool(Func<T> create, Func<T, bool> destroy, int maximumRetained = 1024)
        {
            _create = create;
            _destroy = destroy;
            _maximumRetained = maximumRetained;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get()
        {
            return _objects.TryTake(out var item) ? item : _create();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T item)
        {
            if (_destroy(item) && _objects.Count < _maximumRetained)
            {
                _objects.Add(item);
            }
        }
    }

    public class SimpleObjectPool<T>
        where T : new()
    {
        private readonly Func<T> _create;

        private readonly Func<T, bool> _destroy;

        private readonly ConcurrentBag<T> _objects = new ConcurrentBag<T>();

        private readonly int _maximumRetained;

        public static readonly SimpleObjectPool<T> Shared = new SimpleObjectPool<T>(() => new T(), null);

        public SimpleObjectPool(Func<T> create, Func<T, bool> destroy, int maximumRetained = 1024)
        {
            _create = create;
            if (_create == null)
            { }

            _destroy = destroy;
            _maximumRetained = maximumRetained;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get()
        {
            return _objects.TryTake(out var item) ? item : _create();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T item)
        {
            if (_objects.Count < _maximumRetained && (_destroy == null || _destroy.Invoke(item)))
            {
                _objects.Add(item);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                if (_objects.Count < _maximumRetained && (_destroy == null || _destroy.Invoke(item)))
                {
                    _objects.Add(item);
                }
            }
        }
    }
}