using System.Collections.Generic;
using System.Threading.Tasks;
using FairyGUI;
using UnityEngine;

namespace ETModel
{
    /// <summary>
    /// 管理所有UI Package
    /// </summary>
    public class FUIPackageComponent: Component
    {
        public const string FUI_PACKAGE_DIR = "Assets/Bundles/FUI";

        private readonly Dictionary<string, UIPackage> packages = new Dictionary<string, UIPackage>();

        public async ETTask AddPackageAsync(string type)
        {
            if (this.packages.ContainsKey(type)) return;
            UIPackage uiPackage;
            if (Define.ResModeIsEditor)
            {
                uiPackage = UIPackage.AddPackage($"{FUI_PACKAGE_DIR}/{type}");
            }
            else
            {
                string uiBundleDesName = $"{type}_fui";
                string uiBundleResName = $"{type}_atlas0";
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();

                AssetBundle desAssetBundle = await resourcesComponent.LoadAssetBundleAsync(ABPathUtilities.GetFGUIDesPath(uiBundleDesName));
                AssetBundle resAssetBundle = await resourcesComponent.LoadAssetBundleAsync(ABPathUtilities.GetFGUIResPath(uiBundleResName));
                uiPackage = UIPackage.AddPackage(desAssetBundle, resAssetBundle);
            }

            packages.Add(type, uiPackage);
        }

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

            if (!Define.ResModeIsEditor)
            {
                string uiBundleDesName = $"{type}_fui";
                string uiBundleResName = $"{type}_atlas0";
                Game.Scene.GetComponent<ResourcesComponent>().UnLoadAssetBundle(ABPathUtilities.GetFGUIDesPath(uiBundleDesName));
                Game.Scene.GetComponent<ResourcesComponent>().UnLoadAssetBundle(ABPathUtilities.GetFGUIResPath(uiBundleResName));
            }
        }
    }
}