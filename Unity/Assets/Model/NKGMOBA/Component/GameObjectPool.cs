using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using UnityEngine;
using static UnityEngine.GameObject;

namespace ETModel
{
    // 游戏对象缓存队列
    public class GameObjectQueue : Component
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
            get { return this.queue.Count; }
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

    [ObjectSystem]
    public class GameObjectPoolFixedUpdateSystem : UpdateSystem<GameObjectPool>
    {
        public override void Update(GameObjectPool self)
        {
            self.Update();
        }
    }

    /// <summary>
    /// 对象池组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GameObjectPool : Component
    {
        private readonly Dictionary<String, GameObjectQueue> dictionary = new Dictionary<String, GameObjectQueue>();
        private readonly Dictionary<String, GameObject> prefabDict = new Dictionary<String, GameObject>();

        private readonly List<Unit> AllEntityToBeRecycledInNextFrame = new List<Unit>();

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

        public Unit FetchEntity(string type)
        {
            GameObject gameObject = FetchGameObject(type);
            Unit unit = ComponentFactory.Create<Unit, GameObject>(gameObject);
            return unit;
        }

        public Unit FetchEntityWithId(long Id, string type)
        {
            GameObject gameObject = FetchGameObject(type);
            Unit unit = ComponentFactory.CreateWithId<Unit, GameObject>(Id, gameObject);
            return unit;
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
                    Log.Warning($"对象池没有初始化{type}类型的预制体，请在外层确保注册");
                    return null;
                }

                tempGameObject = UnityEngine.Object.Instantiate(prefab);
                tempGameObject.AddComponent<ComponentView>();
            }
            else if (queue.Count == 0)
            {
                GameObject prefab;
                this.prefabDict.TryGetValue(type, out prefab);
                if (prefab == null)
                {
                    Log.Warning($"对象池没有初始化{type}类型的预制体，请在外层确保注册");
                    return null;
                }

                tempGameObject = UnityEngine.Object.Instantiate(prefab);
                tempGameObject.AddComponent<ComponentView>();
            }
            else
            {
                tempGameObject = queue.Dequeue();
                tempGameObject.SetActive(true);
            }

            tempGameObject.transform.position = prefabDict[type].transform.position;
            tempGameObject.transform.rotation = prefabDict[type].transform.rotation;
            tempGameObject.transform.localScale = prefabDict[type].transform.localScale;
            return tempGameObject;
        }

        public Dictionary<long, long> hasRe = new Dictionary<long, long>();

        /// <summary>
        /// Dispose并回收Entity到对象池。TODO 封装到UnitFactory中
        /// </summary>
        /// <param name="entity"></param>
        public void Recycle(Unit entity)
        {
            if (entity == null || entity.IsDisposed) return;
            // 根据tag进行分类，将Entity放到不同的Queue当中
            String type = entity.GameObject.GetComponent<MonoBridge>()?.CustomTag;

            if (type == "")
            {
                throw new Exception("未添加tag的gameobject不能使用对象池，请为目标物体添加Collider Bridge组件，并设置CustomTag");
            }

            GameObjectQueue queue;
            if (!this.dictionary.TryGetValue(type, out queue))
            {
                queue = ComponentFactory.Create<GameObjectQueue>(); // 回收的时候才需要创建
                queue.Parent = this;
                queue.GameObject.name = $"{type}--Pool";
                this.dictionary.Add(type, queue);
            }

            Game.Scene.GetComponent<UnitComponent>().Remove(entity.Id);
            entity.GameObject.SetActive(false);
            entity.GameObject.transform.SetParent(queue.GameObject.transform);
            queue.Enqueue(entity.GameObject);
        }

        /// <summary>
        /// 下一帧回收Entity TODO 封装到UnitFactory中
        /// </summary>
        public void Recycle_NextFrame(Unit entity)
        {
            if (AllEntityToBeRecycledInNextFrame.Contains(entity)) return;
            AllEntityToBeRecycledInNextFrame.Add(entity);
        }

        public override void Dispose()
        {
            if (this.IsDisposed) return;
            base.Dispose();
            foreach (var kv in this.dictionary)
            {
                kv.Value.Dispose(); // 调用GameObjectQueue<T>的Dispose()，再对其中的所有对象进行回收
            }

            if (AllEntityToBeRecycledInNextFrame.Count > 0)
            {
                foreach (var VARIABLE in AllEntityToBeRecycledInNextFrame)
                {
                    Recycle(VARIABLE);
                }

                AllEntityToBeRecycledInNextFrame.Clear();
            }

            this.dictionary.Clear();
            prefabDict.Clear();
        }

        public void Update()
        {
            if (AllEntityToBeRecycledInNextFrame.Count > 0)
            {
                foreach (var VARIABLE in AllEntityToBeRecycledInNextFrame)
                {
                    Recycle(VARIABLE);
                }

                AllEntityToBeRecycledInNextFrame.Clear();
            }
        }
    }
}