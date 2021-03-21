using System;
using ETModel;

namespace ETHotfix
{
    public static class MapHelper
    {
        /// <summary>
        /// 进入战斗地图
        /// </summary>
        /// <returns></returns>
        public static async ETVoid EnterMapAsync()
        {
            try
            {
                // 切换到map场景
                // 加载场景资源
                await ETModel.Game.Scene.GetComponent<ResourcesComponent>().LoadSceneAsync(ABPathUtilities.GetScenePath(SceneType.Map));

                // 创建5v5游戏
                M5V5GameFactory.CreateM5V5Game();

                // 临时引用5v5游戏
                M5V5Game m5V5Game = Game.Scene.GetComponent<M5V5GameComponent>().m_5V5Game;

                G2C_EnterMap g2CEnterMap = await ETModel.SessionComponent.Instance.Session.Call(new C2G_EnterMap()) as G2C_EnterMap;

                //ETModel.Log.Info($"{DateTime.UtcNow}处理完成服务端发来的进入Map后的信息");

                PlayerComponent.Instance.MyPlayer.UnitId = g2CEnterMap.UnitId;

                // 添加点击地图寻路组件
                m5V5Game.AddComponent<MapClickCompoent>();
                ETModel.Game.EventSystem.Run(ETModel.EventIdType.CloseLoadingUI);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        /// <summary>
        /// 离开战斗地图
        /// </summary>
        public static void ExitMap()
        {
            // 创建5v5游戏
            M5V5GameFactory.DisposeM5V5Game();
        }
    }
}