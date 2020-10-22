using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FairyGUI;
using libx;
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
                ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();

                uiPackage = UIPackage.AddPackage($"{FUI_PACKAGE_DIR}/{type}", (string name, string extension, Type type1, out DestroyMethod method) =>
                {
                    method = DestroyMethod.Unload;
                    switch (extension)
                    {
                        case ".bytes":
                        {
                            var req = resourcesComponent.LoadAsset<TextAsset>($"{name}{extension}");
                            return req;
                        }
                        case ".png":
                        {
                            var req = resourcesComponent.LoadAsset<Texture>($"{name}{extension}");
                            return req;
                        }
                    }

                    return null;
                });
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
                Game.Scene.GetComponent<ResourcesComponent>().UnLoadAsset(ABPathUtilities.GetFGUIDesPath($"{type}_fui"));
                Game.Scene.GetComponent<ResourcesComponent>().UnLoadAsset(ABPathUtilities.GetFGUIResPath($"{type}_atlas0"));
            }
        }
    }
}