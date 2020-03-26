using System;
using System.Collections.Generic;
using MongoDB.Bson;
using UnityEngine;

namespace ETModel
{
    // 游戏对象缓存队列
    public class GameObjectQueue<T>: Component where T : ComponentWithId
    {
        public string TypeName { get; }

        private readonly Queue<T> queue = new Queue<T>();

        public GameObjectQueue(string typeName)
        {
            this.TypeName = typeName;
        }

        public void Enqueue(T gameObject)
        {
            this.queue.Enqueue(gameObject);
        }

        public T Dequeue()
        {
            return this.queue.Dequeue();
        }

        public T Peek()
        {
            return this.queue.Peek();
        }

        public int Count
        {
            get
            {
                return this.queue.Count;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            while (this.queue.Count > 0)
            {
                T entity = this.queue.Dequeue();
                entity.Dispose();
            }
        }
    }

    /// <summary>
    /// 对象池组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameObjectPool<T>: Component where T : ComponentWithId
    {
        private readonly Dictionary<String, GameObjectQueue<T>> dictionary = new Dictionary<String, GameObjectQueue<T>>();
        private readonly Dictionary<String, GameObject> prefabDict = new Dictionary<String, GameObject>();

        // 初始化预制体
        public void Add(string gameObjectType, GameObject goPrefab)
        {
            if (this.prefabDict.ContainsKey(gameObjectType))
            {
                return;
            }
            Log.Info($"初始化了{gameObjectType}预制体");
            this.prefabDict.Add(gameObjectType, goPrefab);
        }

        /// <summary>
        /// 检查是否已经注册过Prefab
        /// </summary>
        /// <returns></returns>
        public bool HasRegisteredPrefab(string gameObjectType)
        {
            if (this.prefabDict.ContainsKey(gameObjectType))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// 从对象池里获取对象
        /// </summary>
        /// <param name="type">GameObject Tag</param>
        /// <returns></returns>
        public T Fetch(string type)
        {
            T entity = null;
            if (!this.dictionary.TryGetValue(type, out GameObjectQueue<T> queue))
            {
                GameObject prefab;
                this.prefabDict.TryGetValue(type, out prefab);
                if (prefab == null)
                {
                    Log.Error($"对象池没有初始化{type}类型的预制体");
                    throw new Exception($"对象池没有初始化{type}类型的预制体");
                }

                GameObject obj = UnityEngine.Object.Instantiate(prefab);
                obj.tag = type;
                entity = ComponentFactory.CreateWithId<T, GameObject>(IdGenerater.GenerateId(), obj, false); // 使用自己的对象池时，不能使用ET的对象池进行创建，否则会回到ET对象池
            }
            else if (queue.Count == 0)
            {
                GameObject prefab;
                this.prefabDict.TryGetValue(type, out prefab);
                if (prefab == null)
                {
                    Log.Error($"对象池没有初始化{type}类型的预制体");
                    throw new Exception($"对象池没有初始化{type}类型的预制体");
                }

                GameObject obj = UnityEngine.Object.Instantiate(prefab);
                obj.tag = type;
                entity = ComponentFactory.CreateWithId<T, GameObject>(IdGenerater.GenerateId(), obj, false);
            }
            else
            {
                entity = queue.Dequeue();
                entity.GameObject.SetActive(true);
            }

            return entity;
        }

        /// <summary>
        /// 从对象池里获取对象，并指定ID
        /// </summary>
        /// <param name="type">GameObject Tag</param>
        /// <returns></returns>
        public T FetchWithId(long id, string type)
        {
            T entity = null;
            if (!this.dictionary.TryGetValue(type, out GameObjectQueue<T> queue))
            {
                GameObject prefab;
                this.prefabDict.TryGetValue(type, out prefab);
                if (prefab == null)
                {
                    Log.Error($"对象池没有初始化{type}类型的预制体");
                    throw new Exception($"对象池没有初始化{type}类型的预制体");
                }

                GameObject obj = UnityEngine.Object.Instantiate(prefab);
                obj.tag = type;
                entity = ComponentFactory.CreateWithId<T, GameObject>(id, obj, false); // 使用自己的对象池时，不能使用ET的对象池进行创建，否则会回到ET对象池
            }
            else if (queue.Count == 0)
            {
                GameObject prefab;
                this.prefabDict.TryGetValue(type, out prefab);
                if (prefab == null)
                {
                    Log.Error($"对象池没有初始化{type}类型的预制体");
                    throw new Exception($"对象池没有初始化{type}类型的预制体");
                }

                GameObject obj = UnityEngine.Object.Instantiate(prefab);
                obj.tag = type;
                entity = ComponentFactory.CreateWithId<T, GameObject>(id, obj, false);
            }
            else
            {
                entity = queue.Dequeue();
                entity.GameObject.SetActive(true);
            }

            return entity;
        }

        /// <summary>
        /// 回收Entity到对象池。
        /// </summary>
        /// <param name="entity"></param>
        public void Recycle(T entity)
        {
            // 根据tag进行分类，将Entity放到不同的Queue当中
            String type = entity.GameObject.tag;

            if (type == "Untagged")
            {
                throw new Exception("未添加tag的gameobject不能使用对象池，因为不同tag的gameobject，身上的资源不同，若混在一起则无法复用gameobject");
            }

            GameObjectQueue<T> queue;
            if (!this.dictionary.TryGetValue(type, out queue))
            {
                queue = new GameObjectQueue<T>(type); // 回收的时候才需要创建
                queue.Parent = this;
                queue.GameObject.name = type;
                this.dictionary.Add(type, queue);
            }

            entity.GameObject.SetActive(false);
            entity.GameObject.transform.position = new Vector3(0, 0, 0);
            entity.GameObject.transform.parent = queue.GameObject.transform;
            queue.Enqueue(entity);
        }

        public void Clear()
        {
            foreach (var kv in this.dictionary)
            {
                kv.Value.IsFromPool = false;
                kv.Value.Dispose(); // 调用GameObjectQueue<T>的Dispose()，再对其中的所有对象进行回收
            }

            this.dictionary.Clear();
        }
    }
}