using UnityEngine;

namespace ET
{
    public class SpriteSpecialSkillComponentDeatroySystem : DestroySystem<SpriteSpecialSkillComponent>
    {
        public override void Destroy(SpriteSpecialSkillComponent self)
        {
            GameObjectPoolComponent.Instance.RecycleGameObject(self.Name, self.Effect);
        }
    }

    public class SpriteSpecialSkillComponentUpdateSystem : UpdateSystem<SpriteSpecialSkillComponent>
    {
        public override void Update(SpriteSpecialSkillComponent self)
        {
            self.Effect.transform.position =
                self.GetParent<Unit>().Forward + self.GetParent<Unit>().Position + Vector3.up;
            self.Effect.transform.localRotation = self.GetParent<Unit>().Rotation;
        }
    }

    public class SpriteSpecialSkillComponent : Entity
    {
        public string Name;
        public GameObject Effect;

        public void SetActive()
        {
            Effect.SetActive(true);
        }

        public void SetInActive()
        {
            Effect.SetActive(false);
        }
    }
}