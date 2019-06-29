using System;
using ETModel;

namespace ETHotfix
{
    public static class MapHelper
    {
        public static async ETTask EnterMapAsync()
        {
            try
            {
                // 加载Unit资源
                ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
                await resourcesComponent.LoadBundleAsync($"unit.unity3d");

                // 加载场景资源
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("map.unity3d");
                // 加载战斗图片资源（英雄头像）
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("heroavatars.unity3d");
                // 加载战斗图片资源（英雄技能图标）
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadBundleAsync("heroskillicons.unity3d");

                // 切换到map场景
                using (SceneChangeComponent sceneChangeComponent = ETModel.Game.Scene.AddComponent<SceneChangeComponent>())
                {
                    await sceneChangeComponent.ChangeSceneAsync(SceneType.Map);
                }

                // 创建5v5游戏
                M5V5GameFactory.CreateM5V5Game();

                // 临时引用5v5游戏
                M5V5Game m5V5Game = Game.Scene.GetComponent<M5V5GameComponent>().m_5V5Game;

                G2C_EnterMap g2CEnterMap = await ETModel.SessionComponent.Instance.Session.Call(new C2G_EnterMap()) as G2C_EnterMap;

                PlayerComponent.Instance.MyPlayer.UnitId = g2CEnterMap.UnitId;

                // 给自己的Unit添加引用
                ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit =
                        ETModel.Game.Scene.GetComponent<UnitComponent>().Get(PlayerComponent.Instance.MyPlayer.UnitId);
                ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit
                        .AddComponent<CameraComponent, Unit>(ETModel.Game.Scene.GetComponent<UnitComponent>().MyUnit);

                // 添加点击地图寻路组件
                m5V5Game.AddComponent<MapClickCompoent, UserInputComponent>(ETModel.Game.Scene.GetComponent<UserInputComponent>());

                Game.EventSystem.Run(EventIdType.EnterMapFinish);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}