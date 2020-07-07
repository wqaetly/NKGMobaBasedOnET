using System;
using System.Collections.Generic;
using MongoDB.Bson;
using UnityEngine;
using static UnityEngine.GameObject;

namespace ETModel
{
    // 游戏对象缓存队列
    public class GameObjectQueue: Component
    {
        private readonly Queue<GameObject> queue = new Queue<GameObject>();

        public void Enqueue(GameObject gameObject)
        {
            this.queue.Enqueue(gameObject);
        }

        public GameObject Dequeue()
        {
            return this.queue.Dequeue();
        }

        public GameObject Peek()
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

            foreach (var VARIABLE in this.queue)
            {
                UnityEngine.Object.Destroy(VARIABLE);
            }

            queue.Clear();
        }
    }

    /// <summary>
    /// 对象池组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameObjectPool<T>: Component where T : Entity
    {
        private readonly Dictionary<String, GameObjectQueue> dictionary = new Dictionary<String, GameObjectQueue>();
        private readonly Dictionary<String, GameObject> prefabDict = new Dictionary<String, GameObject>();

        // 初始化预制体
        public void Add(string gameObjectType, GameObject goPrefab)
        {
            if (this.prefabDict.ContainsKey(gameObjectType))
            {
                return;
            }

            //Log.Info($"初始化了{gameObjectType}预制体");
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

        public T FetchEntity(string type)
        {
            return ComponentFactory.Create<T, GameObject>(FetchGameObject(type));
        }

        public T FetchEntityWithId(long Id, string type)
        {
            return ComponentFactory.CreateWithId<T, GameObject>(Id, FetchGameObject(type));
        }

        /// <summary>
        /// 从对象池里获取对象
        /// </summary>
        /// <param name="type">GameObject Tag</param>
        /// <returns></returns>
        public GameObject FetchGameObject(string type)
        {
            GameObject tempGameObject = null;
            if (!this.dictionary.TryGetValue(type, out GameObjectQueue queue))
            {
                GameObject prefab;
                this.prefabDict.TryGetValue(type, out prefab);
                if (prefab == null)
                {
                    Log.Error($"对象池没有初始化{type}类型的预制体");
                    throw new Exception($"对象池没有初始化{type}类型的预制体");
                }

                tempGameObject = UnityEngine.Object.Instantiate(prefab);
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

                tempGameObject = UnityEngine.Object.Instantiate(prefab);
            }
            else
            {
                tempGameObject = queue.Dequeue();
                tempGameObject.SetActive(true);
            }

            tempGameObject.tag = type;
            tempGameObject.transform.position = prefabDict[type].transform.position;
            tempGameObject.transform.rotation = prefabDict[type].transform.rotation;
            tempGameObject.transform.localScale = prefabDict[type].transform.localScale;
            return tempGameObject;
        }

        /// <summary>
        /// Dispose并回收Entity到对象池。
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

            GameObjectQueue queue;
            if (!this.dictionary.TryGetValue(type, out queue))
            {
                queue = ComponentFactory.Create<GameObjectQueue>(); // 回收的时候才需要创建
                queue.Parent = this;
                queue.GameObject.name = $"{type}--Pool";
                this.dictionary.Add(type, queue);
            }
            entity.Dispose();
            entity.GameObject.SetActive(false);
            entity.GameObject.transform.SetParent(queue.GameObject.transform);
            queue.Enqueue(entity.GameObject);

        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            foreach (var kv in this.dictionary)
            {
                kv.Value.Dispose(); // 调用GameObjectQueue<T>的Dispose()，再对其中的所有对象进行回收
            }

            foreach (var VARIABLE in prefabDict)
            {
                UnityEngine.Object.Destroy(VARIABLE.Value);
            }

            this.dictionary.Clear();
        }
    }
}