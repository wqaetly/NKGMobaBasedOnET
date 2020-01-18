using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel
{
    public static class UnitFactory
    {
        public static Unit Create(long id)
        {
            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            GameObject bundleGameObject = (GameObject) resourcesComponent.GetAsset("Unit.unity3d", "Unit");
            GameObject prefab = bundleGameObject.Get<GameObject>("NuoKe");

            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();

            Game.Scene.GetComponent<GameObjectPool<Unit>>().Add("NuoKe", prefab);
            Unit unit = Game.Scene.GetComponent<GameObjectPool<Unit>>().FetchWithId(id, "NuoKe");
            //Log.Info($"此英雄的Model层ID为{unit.Id}");

            //增加栈式状态机，辅助动画切换
            unit.AddComponent<StackFsmComponent>();
            unit.AddComponent<AnimationComponent>();
            unit.AddComponent<MoveComponent>();
            unit.AddComponent<TurnComponent>();
            unit.AddComponent<UnitPathComponent>();
            unit.AddComponent<HeroTransformComponent>();

            unitComponent.Add(unit);
            return unit;
        }

        /// <summary>
        /// 用于NPBehave测试
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Unit NPBehaveTestCreate()
        {
            UnitComponent unitComponent = Game.Scene.GetComponent<UnitComponent>();
            Unit unit = ComponentFactory.Create<Unit>();
            unitComponent.Add(unit);
            return unit;
        }
    }
}