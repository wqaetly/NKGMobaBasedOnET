using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class OutLineComponentAwakeSystem : AwakeSystem<OutLineComponent>
    {
        public override void Awake(OutLineComponent self)
        {
            //此处填写Awake逻辑
            self.PlayerUnit = self.DomainScene().GetComponent<RoomManagerComponent>().GetBattleRoom()
                .GetComponent<UnitComponent>().MyUnit;

            self.MouseTargetSelectorComponent =
                self.PlayerUnit.BelongToRoom.GetComponent<MouseTargetSelectorComponent>();
        }
    }

    [ObjectSystem]
    public class OutLineComponentUpdateSystem : UpdateSystem<OutLineComponent>
    {
        public override void Update(OutLineComponent self)
        {
            //此处填写Update逻辑
            if (self.MouseTargetSelectorComponent.TargetUnit != null)
            {
                B2S_RoleCastComponent roleCastComponent =
                    self.MouseTargetSelectorComponent.TargetUnit.GetComponent<B2S_RoleCastComponent>();
                if (roleCastComponent == null)
                {
                    self.ResetUnitOutLineInfo(self.CachedUnit);
                    self.CachedUnit = null;
                    return;
                }

                if (self.CachedUnit != self.MouseTargetSelectorComponent.TargetUnit)
                {
                    self.ResetUnitOutLineInfo(self.CachedUnit);
                    self.CachedUnit = self.MouseTargetSelectorComponent.TargetUnit;
                    GameObject selfUnitGo = self.CachedUnit.GetComponent<GameObjectComponent>().GameObject;
                    selfUnitGo.Get<GameObject>("Materials").GetComponent<Renderer>()
                        .GetPropertyBlock(OutLineComponent.MaterialPropertyBlock);
                    OutLineComponent.MaterialPropertyBlock.SetFloat("OutLineWidth", 0.07f);

                    if (roleCastComponent.GetRoleCastToTarget(self.PlayerUnit) == RoleCast.Friendly)
                    {
                        OutLineComponent.MaterialPropertyBlock.SetColor("OutLineColor", Color.blue);
                    }
                    else
                    {
                        OutLineComponent.MaterialPropertyBlock.SetColor("OutLineColor", Color.red);
                    }

                    selfUnitGo.Get<GameObject>("Materials").GetComponent<Renderer>()
                        .SetPropertyBlock(OutLineComponent.MaterialPropertyBlock);
                }
            }
            else
            {
                self.ResetUnitOutLineInfo(self.CachedUnit);
                self.CachedUnit = null;
            }
        }
    }

    [ObjectSystem]
    public class OutLineComponentDestroySystem : DestroySystem<OutLineComponent>
    {
        public override void Destroy(OutLineComponent self)
        {
            self.CachedUnit = null;
        }
    }

    public static class OutLineComponentUtilities
    {
        /// <summary>
        /// 重置Unit描边信息
        /// </summary>
        public static void ResetUnitOutLineInfo(this OutLineComponent self, Unit targetUnit)
        {
            if (targetUnit != null)
            {
                UnityEngine.GameObject selfUnitGo = targetUnit.GetComponent<GameObjectComponent>().GameObject;
                selfUnitGo.Get<GameObject>("Materials").GetComponent<Renderer>()
                    .GetPropertyBlock(OutLineComponent.MaterialPropertyBlock);
                OutLineComponent.MaterialPropertyBlock.SetInt("OutLineWidth", 0);
                selfUnitGo.Get<GameObject>("Materials").GetComponent<Renderer>()
                    .SetPropertyBlock(OutLineComponent.MaterialPropertyBlock);
            }
        }
    }
}