using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ETModel
{
    public static class BundleHelper
    {
        public static async ETTask DownloadBundle()
        {
            if (!Define.ResModeIsEditor)
            {
                try
                {
                    using (BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.AddComponent<BundleDownloaderComponent>())
                    {
                        Game.EventSystem.Run(EventIdType.CheckForUpdateBegin);
                        Game.EventSystem.Run(EventIdType.CreateLoadingUI);

                        await bundleDownloaderComponent.StartAsync();

                        await bundleDownloaderComponent.DownloadAsync();
                    }

                    Game.Scene.GetComponent<ResourcesComponent>().LoadOneBundle("StreamingAssets");
                    ResourcesComponent.AssetBundleManifestObject =
                            (AssetBundleManifest) Game.Scene.GetComponent<ResourcesComponent>().GetAsset("StreamingAssets", "AssetBundleManifest");
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }
        }

        /// <summary>
        /// 优先从可读写目录取文件MD5，再从streaming取
        /// </summary>
        /// <param name="streamingVersionConfig"></param>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        public static string GetBundleMD5(VersionConfig streamingVersionConfig, string bundleName)
        {
            string path = Path.Combine(PathHelper.AppHotfixResPath, bundleName);
            if (File.Exists(path))
            {
                return MD5Helper.FileMD5(path);
            }

            if (streamingVersionConfig.FileInfoDict.ContainsKey(bundleName))
            {
                return streamingVersionConfig.FileInfoDict[bundleName].MD5;
            }

            return "";
        }
    }
}