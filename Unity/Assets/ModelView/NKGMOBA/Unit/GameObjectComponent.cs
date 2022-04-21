using UnityEngine;

namespace ET
{
    public class GameObjectComponentAwakeSystem : AwakeSystem<GameObjectComponent, string>
    {
        public override void Awake(GameObjectComponent self, string a)
        {
            self.Name = a;
            self.GameObject = GameObjectPoolComponent.Instance.FetchGameObject(self.Name, GameObjectType.Unit);
        }
    }

    public class GameObjectComponentDestroySystem : DestroySystem<GameObjectComponent>
    {
        public override void Destroy(GameObjectComponent self)
        {
            GameObjectPoolComponent.Instance.RecycleGameObject(self.Name, self.GameObject);
        }
    }

    public class GameObjectComponent : Entity
    {
        public string Name;
        public GameObject GameObject;
    }
}