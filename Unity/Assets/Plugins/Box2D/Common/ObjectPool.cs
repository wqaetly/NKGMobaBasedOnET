using System;
using System.Buffers;
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
            _create = create ?? throw new NullReferenceException(nameof(create));
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
            _create = create ?? throw new NullReferenceException(nameof(create));
            _destroy = destroy;
            if (maximumRetained < 0)
            {
                throw new InvalidOperationException("maximumRetained must be greater than 0");
            }

            _maximumRetained = maximumRetained;
        }

        /// <summary>
        /// Rent a object
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get()
        {
            return _objects.TryTake(out var item) ? item : _create();
        }

        /// <summary>
        /// Return object, call <see cref="IDisposable.Dispose"/> if "dispose" is true
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dispose">Call Dispose if "dispose == true" and item implements IDisposable</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(T item, bool dispose = false)
        {
            if (_objects.Count < _maximumRetained && (_destroy == null || _destroy.Invoke(item)))
            {
                if (dispose && item is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _objects.Add(item);
            }
        }

        /// <summary>
        /// Return objects, call <see cref="IDisposable.Dispose"/> if <see cref="dispose"/> is true
        /// </summary>
        /// <param name="items"></param>
        /// <param name="dispose">Call Dispose if "<see cref="dispose"/> == true" and item implements <see cref="IDisposable"/></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Return(IEnumerable<T> items, bool dispose = false)
        {
            foreach (var item in items)
            {
                if (_objects.Count < _maximumRetained && (_destroy == null || _destroy.Invoke(item)))
                {
                    if (dispose && item is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }

                    _objects.Add(item);
                }
            }
        }
    }
}