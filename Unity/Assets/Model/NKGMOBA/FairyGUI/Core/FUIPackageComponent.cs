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

        public void AddPackage(string type)
        {
            if (this.packages.ContainsKey(type)) return;
            
            UIPackage uiPackage;
            if (Define.ResModeIsEditor)
            {
                uiPackage = UIPackage.AddPackage($"{FUI_PACKAGE_DIR}/{type}");
            }
            else
            {
                string uiBundleDesName = $"{type}_fui".StringToAB();
                string uiBundleResName = type.StringToAB();
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                resourcesComponent.LoadBundle(uiBundleDesName);
                resourcesComponent.LoadBundle(uiBundleResName);

                AssetBundle desAssetBundle = resourcesComponent.GetAssetBundle(uiBundleDesName);
                AssetBundle resAssetBundle = resourcesComponent.GetAssetBundle(uiBundleResName);
                uiPackage = UIPackage.AddPackage(desAssetBundle, resAssetBundle);
            }

            packages.Add(type, uiPackage);
        }

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
                string uiBundleDesName = $"{type}_fui".StringToAB();
                string uiBundleResName = type.StringToAB();
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync(uiBundleDesName);
                await resourcesComponent.LoadBundleAsync(uiBundleResName);

                AssetBundle desAssetBundle = resourcesComponent.GetAssetBundle(uiBundleDesName);
                AssetBundle resAssetBundle = resourcesComponent.GetAssetBundle(uiBundleResName);
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
                string uiBundleDesName = $"{type}_fui".StringToAB();
                string uiBundleResName = type.StringToAB();
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiBundleDesName);
                Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle(uiBundleResName);
            }
        }
    }
}