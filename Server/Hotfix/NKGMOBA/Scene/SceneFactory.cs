

using System.Net;

namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, string name, SceneType sceneType)
        {
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            return await Create(parent, instanceId, instanceId, parent.DomainZone(), name, sceneType);
        }
        
        public static async ETTask<Scene> Create(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            await ETTask.CompletedTask;
            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);

            // 添加注册邮箱，因为内网通信都是通过Actor消息
            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm:
                    scene.AddComponent<NetKcpComponent, IPEndPoint>(startSceneConfig.OuterIPPort);
                    break;
                case SceneType.Gate:
                    scene.AddComponent<NetKcpComponent, IPEndPoint>(startSceneConfig.OuterIPPort);
                    scene.AddComponent<GateSessionKeyComponent>();
                    break;
                case SceneType.Map:
                    // 因为战斗服也会有房间的概念，一个房间代表一场战斗，每个战斗都有自己的UnitManager，寻路网格，碰撞世界
                    scene.AddComponent<RoomManagerComponent>();
                    scene.AddComponent<NP_TreeDataRepositoryComponent>();
                    scene.AddComponent<UnitAttributesDataRepositoryComponent>();
                    scene.AddComponent<B2S_ColliderDataRepositoryComponent>();
                    scene.AddComponent<RecastNavMeshManagerComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
                case SceneType.Lobby:
                    scene.AddComponent<NetKcpComponent, IPEndPoint>(startSceneConfig.OuterIPPort);
                    scene.AddComponent<RoomManagerComponent>();
                    break;
            }

            return scene;
        }
    }
}