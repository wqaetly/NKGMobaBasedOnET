using libx;
using UnityEngine;

namespace ETModel
{
    public class ResourcesComponent: Component
    {
        #region Assets

        /// <summary>
        /// 加载资源，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
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
        public ETTask<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            ETTaskCompletionSource<T> tcs = new ETTaskCompletionSource<T>();
            AssetRequest assetRequest = Assets.LoadAssetAsync(path, typeof (T));
            assetRequest.completed = (arq) => { tcs.SetResult((T) arq.asset); };
            return tcs.Task;
        }

        /// <summary>
        /// 卸载资源，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        public void UnLoadAsset(string path)
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
        public ETTask<SceneAssetRequest> LoadSceneAsync(string path)
        {
            ETTaskCompletionSource<SceneAssetRequest> tcs = new ETTaskCompletionSource<SceneAssetRequest>();
            SceneAssetRequest sceneAssetRequest = Assets.LoadSceneAsync(path, false);
            sceneAssetRequest.completed = (arq) =>
            {
                tcs.SetResult(sceneAssetRequest);
            };
            return tcs.Task;
        }

        /// <summary>
        /// 卸载场景，path需要是全路径
        /// </summary>
        /// <param name="path"></param>
        public void UnLoadScene(string path)
        {
            Assets.UnloadScene(path);
        }

        #endregion
    }
}