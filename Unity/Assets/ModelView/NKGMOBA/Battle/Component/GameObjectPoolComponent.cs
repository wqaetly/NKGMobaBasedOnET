using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class GameObjectPoolComponentAwakeSystem : AwakeSystem<GameObjectPoolComponent>
    {
        public override void Awake(GameObjectPoolComponent self)
        {
            GameObjectPoolComponent.Instance = self;
        }
    }

    public enum GameObjectType
    {
        /// <summary>
        /// Unit
        /// </summary>
        Unit,

        /// <summary>
        /// 音效
        /// </summary>
        Sound,

        /// <summary>
        /// 特效
        /// </summary>
        Effect,
        
        /// <summary>
        /// 技能指示器
        /// </summary>
        SkillIndictor,
    }

    public class GameObjectPoolComponent : Entity
    {
        public static GameObjectPoolComponent Instance { get; set; }

        /// <summary>
        /// 所有Prefab的缓存
        /// </summary>
        public Dictionary<string, GameObject> AllPrefabs = new Dictionary<string, GameObject>();

        public Dictionary<string, Queue<GameObject>> AllCachedGos = new Dictionary<string, Queue<GameObject>>();

        /// <summary>
        /// 处理起来比较麻烦，先不做异步了
        /// </summary>
        /// <param name="resName"></param>
        /// <returns></returns>
        public GameObject FetchGameObject(string resName, GameObjectType gameObjectType)
        {
            GameObject gameObject;
            if (AllCachedGos.TryGetValue(resName, out var gameObjects))
            {
                if (gameObjects.Count > 0)
                {
                    gameObject = gameObjects.Dequeue();
                }
                else
                {
                    gameObject =
                        UnityEngine.Object.Instantiate(AllPrefabs[resName], GlobalComponent.Instance.Unit, true);
                }
            }
            else
            {
                GameObject targetprefab;
                if (AllPrefabs.TryGetValue(resName, out var prefab))
                {
                    targetprefab = prefab;
                }
                else
                {
                    targetprefab = null;
                    switch (gameObjectType)
                    {
                        case GameObjectType.Unit:
                            targetprefab = XAssetLoader.LoadAsset<GameObject>(XAssetPathUtilities.GetUnitPath(resName));
                            break;
                        case GameObjectType.Sound:
                            targetprefab =
                                XAssetLoader.LoadAsset<GameObject>(XAssetPathUtilities.GetSoundPath(resName));
                            break;
                        case GameObjectType.Effect:
                            targetprefab =
                                XAssetLoader.LoadAsset<GameObject>(XAssetPathUtilities.GetEffectPath(resName));
                            break;
                        case GameObjectType.SkillIndictor:
                            targetprefab =
                                XAssetLoader.LoadAsset<GameObject>(XAssetPathUtilities.GetSkillIndicatorPath(resName));
                            break;
                    }

                    if (targetprefab == null)
                    {
                        return null;
                    }

                    AllPrefabs.Add(resName, targetprefab);
                    AllCachedGos.Add(resName, new Queue<GameObject>());
                }

                gameObject = UnityEngine.Object.Instantiate(targetprefab, GlobalComponent.Instance.Unit, true);
                gameObject.transform.position = targetprefab.transform.position;
            }

            gameObject.SetActive(true);
            return gameObject;
        }

        public void RecycleGameObject(string resName, GameObject gameObject)
        {
            gameObject.SetActive(false);
            if (string.IsNullOrEmpty(resName))
            {
                return;
            }

            AllCachedGos[resName].Enqueue(gameObject);
        }
    }
}