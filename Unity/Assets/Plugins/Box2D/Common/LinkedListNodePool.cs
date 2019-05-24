using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Box2DSharp.Common
{
    public class LinkedListNodePool<T>
    {
        private readonly ConcurrentBag<LinkedListNode<T>> _objects = new ConcurrentBag<LinkedListNode<T>>();

        private readonly int _maximumRetained;

        public static readonly LinkedListNodePool<T> Shared = new LinkedListNodePool<T>();

        public LinkedListNodePool(int maximumRetained = 1024)
        {
            _maximumRetained = maximumRetained;
        }

        public LinkedListNode<T> Get(T value)
        {
            if (_objects.TryTake(out var item))
            {
                item.Value = value;
            }
            else
            {
                item = new LinkedListNode<T>(value);
            }

            return item;
        }

        public void Return(LinkedListNode<T> item)
        {
            if (_objects.Count < _maximumRetained)
            {
                item.Value = default;
                _objects.Add(item);
            }
        }
    }
}