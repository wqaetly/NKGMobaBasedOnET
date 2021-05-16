using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 管理所有UI Package
    /// </summary>
    public class FUIPackageComponent: Component
    {
        private readonly Dictionary<string, UIPackage> packages = new Dictionary<string, UIPackage>();

        public async ETTask AddPackageAsync(string type)
        {
            if (this.packages.ContainsKey(type))
            {
                return;
            }
            
            TextAsset desTextAsset =
                    await ResourcesComponent.Instance.LoadAssetAsync<TextAsset>(ABPathUtilities.GetFGUIDesPath($"{type}_fui"));

            packages.Add(type, UIPackage.AddPackage(desTextAsset.bytes, type, LoadPackageInternalAsync));
        }

        /// <summary>
        /// 加载资源的异步委托
        /// </summary>
        /// <param name="name">注意，这个name是FGUI内部组装的纹理全名，例如FUILogin_atlas0</param>
        /// <param name="extension"></param>
        /// <param name="type"></param>
        /// <param name="item"></param>
        private static async void LoadPackageInternalAsync(string name, string extension, System.Type type, PackageItem item)
        {
            Texture texture =
                    await ResourcesComponent.Instance.LoadAssetAsync<Texture>(ABPathUtilities.GetFGUIResPath(name, extension));
            item.owner.SetItemAsset(item, texture, DestroyMethod.Unload);
        }

        /// <summary>
        /// 移除一个包，并清理其asset
        /// </summary>
        /// <param name="type"></param>
        public void RemovePackage(string type)
        {
            UIPackage package;

            if (packages.TryGetValue(type, out package))
            {
                var p = UIPackage.GetByName(package.name);
                if (p != null)
                {
                    UIPackage.RemovePackage(package.name);
                }

                packages.Remove(package.name);
            }
        }
    }
}