using UnityEngine;

namespace ET
{
    public class UnitTransformComponentSystems : AwakeSystem<UnitTransformComponent>
    {
        public override void Awake(UnitTransformComponent self)
        {
            self.BelongToUnit = self.GetParent<Unit>();
            GameObjectComponent gameObjectComponent = self.BelongToUnit.GetComponent<GameObjectComponent>();
            self.headPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_HeadPos").transform;
            self.groundPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_GroundPos").transform;
            self.channelPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_FrontPos").transform;
            self.centerPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_CenterPos").transform;
            self.leftHeadPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_LeftHandPos").transform;
            self.rightHeadPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_RightHandPos").transform;

            self.weaponStartPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_WeaponStartPos").transform;
            self.weaponCenterPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_WeaponCenterPos").transform;
            self.weaponEndPos = gameObjectComponent.GameObject.Get<GameObject>("Trans_WeaponEndPos").transform;
        }
    }

    public static class UnitTransformUtilities
    {
        /// <summary>
        /// 获取目标位置
        /// </summary>
        /// <param name="posType"></param>
        /// <returns></returns>
        public static Transform GetTranform(this UnitTransformComponent self, PosType posType)
        {
            switch (posType)
            {
                case PosType.HEAD:
                    return self.headPos;
                case PosType.GROUND:
                    return self.groundPos;
                case PosType.FRONT:
                    return self.channelPos;
                case PosType.CENTER:
                    return self.centerPos;
                case PosType.LEFTHAND:
                    return self.leftHeadPos;
                case PosType.RIGHTTHAND:
                    return self.rightHeadPos;
                case PosType.WEAPONSTART:
                    return self.weaponStartPos;
                case PosType.WEAPONCENTER:
                    return self.weaponCenterPos;
                case PosType.WEAPONEND:
                    return self.weaponEndPos;
            }

            return null;
        }
    }
}