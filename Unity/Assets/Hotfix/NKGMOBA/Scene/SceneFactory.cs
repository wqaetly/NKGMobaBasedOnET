namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> CreateZoneScene(int zone, string name, Entity parent)
        {
            Scene zoneScene = EntitySceneFactory.CreateScene(Game.IdGenerater.GenerateInstanceId(), zone, SceneType.Zone, name, parent);
            
            zoneScene.AddComponent<ZoneSceneFlagComponent>();
            zoneScene.AddComponent<NetKcpComponent>();
            zoneScene.AddComponent<RoomManagerComponent>();
            zoneScene.AddComponent<NP_TreeDataRepositoryComponent>();
            zoneScene.AddComponent<UnitAttributesDataRepositoryComponent>();
            zoneScene.AddComponent<B2S_ColliderDataRepositoryComponent>();
            zoneScene.AddComponent<RecastNavMeshManagerComponent>();

            // UI层的初始化
            await Game.EventSystem.Publish(new EventType.AfterCreateZoneScene() {ZoneScene = zoneScene});

            await XAssetLoader.LoadSceneAsync(XAssetPathUtilities.GetScenePath("TempScene"));
            return zoneScene;
        }
    }
}