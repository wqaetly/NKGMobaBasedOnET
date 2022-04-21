using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 管理所有UI Package
    /// </summary>
    public class FUIPackageManagerComponent: Entity
    {
        private Dictionary<string, UIPackage> s_Packages = new Dictionary<string, UIPackage>();

        public async ETTask AddPackageAsync(string type)
        {
            if (s_Packages.ContainsKey(type))
            {
                return;
            }

            TextAsset desTextAsset =
                    await XAssetLoader.LoadAssetAsync<TextAsset>(XAssetPathUtilities.GetFGUIDesPath($"{type}_fui"));
            s_Packages.Add(type, UIPackage.AddPackage(desTextAsset.bytes, type, LoadPackageInternalAsync));
        }

        /// <summary>
        /// 加载资源的异步委托
        /// </summary>
        /// <param name="name">注意，这个name是FGUI内部组装的纹理全名，例如FUILogin_atlas0</param>
        /// <param name="extension"></param>
        /// <param name="type"></param>
        /// <param name="item"></param>
        private async void LoadPackageInternalAsync(string name, string extension, System.Type type, PackageItem item)
        {
            Texture texture =
                    await XAssetLoader.LoadAssetAsync<Texture>(XAssetPathUtilities.GetFGUIResPath(name, extension));
            item.owner.SetItemAsset(item, texture, DestroyMethod.Unload);
        }

        /// <summary>
        /// 移除一个包，并清理其asset
        /// </summary>
        /// <param name="type"></param>
        public void RemovePackage(string type)
        {
            UIPackage package;

            if (s_Packages.TryGetValue(type, out package))
            {
                var p = UIPackage.GetByName(package.name);
                if (p != null)
                {
                    UIPackage.RemovePackage(package.name);
                    XAssetLoader.UnLoadAsset(XAssetPathUtilities.GetFGUIDesPath($"{type}_fui"));
                    XAssetLoader.UnLoadAsset(XAssetPathUtilities.GetFGUIResPath($"{type}_atlas0", ".png"));
                }

                s_Packages.Remove(package.name);
            }
        }
    }
}