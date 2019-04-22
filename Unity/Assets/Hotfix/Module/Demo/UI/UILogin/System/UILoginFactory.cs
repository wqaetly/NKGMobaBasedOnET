using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    public static class UILoginFactory
    {
        public static UI Create()
        {
	        try
	        {
		        //获取资源组件，用以加载资源
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
				//加载UI的AB包
				resourcesComponent.LoadBundle(UIType.UILogin.StringToAB());
				//获取到登录UI
				GameObject bundleGameObject = (GameObject)resourcesComponent.GetAsset(UIType.UILogin.StringToAB(), UIType.UILogin);
				//实例化登录UI
				GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
				//在组件工厂创建UI（主要是为了能给游戏物体添加上组件做准备）
		        UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.UILogin, gameObject, false);
		        //为UI添加登录UI组件
				ui.AddComponent<UILoginComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}
    }
}