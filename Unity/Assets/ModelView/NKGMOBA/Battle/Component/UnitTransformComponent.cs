using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class UnitTransformComponent : Entity
    {
        public Unit BelongToUnit;
        public Transform headPos;
        public Transform channelPos;
        public Transform groundPos;
        public Transform centerPos;
        public Transform leftHeadPos;
        public Transform rightHeadPos;
        public Transform weaponStartPos;
        public Transform weaponCenterPos;
        public Transform weaponEndPos;
    }
}