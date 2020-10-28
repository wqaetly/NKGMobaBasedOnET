using System;
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
                uiPackage = UIPackage.AddPackage($"{FUI_PACKAGE_DIR}/{type}", LoadPackageInternal);
            }

            packages.Add(type, uiPackage);
        }

        private static UnityEngine.Object LoadPackageInternal(string name, string extension, Type type, out DestroyMethod method)
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            method = DestroyMethod.Unload;
            switch (extension)
            {
                case ".bytes":
                {
                    var req = resourcesComponent.LoadAsset<TextAsset>($"{name}{extension}");
                    return req;
                }
                case ".png": //如果FGUI导出时没有选择分离通明通道，会因为加载不到!a结尾的Asset而报错，但是不影响运行
                {
                    var req = resourcesComponent.LoadAsset<Texture>($"{name}{extension}");
                    return req;
                }
            }

            return null;
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
                Game.Scene.GetComponent<ResourcesComponent>().UnLoadAsset(ABPathUtilities.GetFGUIDesPath($"{type}_fui"));
                Game.Scene.GetComponent<ResourcesComponent>().UnLoadAsset(ABPathUtilities.GetFGUIResPath($"{type}_atlas0"));
            }
        }
    }
}