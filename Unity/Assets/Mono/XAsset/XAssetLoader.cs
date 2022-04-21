using libx;
using UnityEngine;

namespace ET
{
    public static class XAssetLoader
    {
        #region Assets

        /// <summary>
        /// 加载资源，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            AssetRequest assetRequest = Assets.LoadAsset(path, typeof (T));
            return (T) assetRequest.asset;
        }

        /// <summary>
        /// 异步加载资源，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ETTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            ETTask<T> tcs = ETTask<T>.Create(true);
            AssetRequest assetRequest = Assets.LoadAssetAsync(path, typeof (T));
            
            //如果已经加载完成则直接返回结果（适用于编辑器模式下的异步写法和重复加载）,下面的API如果有需求可按照此格式添加相关代码
            if (assetRequest.isDone)
            {
                tcs.SetResult((T) assetRequest.asset);
                return tcs;
            }

            //+=委托链，否则会导致前面完成委托被覆盖
            assetRequest.completed += (arq) => { tcs.SetResult((T) arq.asset); };
            return tcs;
        }

        /// <summary>
        /// 卸载资源，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        public static void UnLoadAsset(string path)
        {
            Assets.UnloadAsset(path);
        }

        #endregion

        #region Scenes

        /// <summary>
        /// 加载场景，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static ETTask<SceneAssetRequest> LoadSceneAsync(string path)
        {
            ETTask<SceneAssetRequest> tcs = ETTask<SceneAssetRequest>.Create(true);
            SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(path, false);
            sceneAssetRequest.completed += (arq) =>
            {
                tcs.SetResult(sceneAssetRequest);
            };
            return tcs;
        }

        /// <summary>
        /// 卸载场景，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        public static void UnLoadScene(string path)
        {
            Assets.UnloadScene(path);
        }

        #endregion
    }
}