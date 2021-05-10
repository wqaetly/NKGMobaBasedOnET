//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月30日 9:52:29
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class HeroSkillBehaveomponentAwakeSystem: AwakeSystem<HeroTransformComponent>
    {
        public override void Awake(HeroTransformComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 位置类型
    /// </summary>
    public enum PosType
    {
        /// <summary>
        /// 头顶
        /// </summary>
        [LabelText("头顶")]
        HEAD,

        /// <summary>
        /// 正中央
        /// </summary>
        [LabelText("正中央")]
        CENTER,

        /// <summary>
        /// 底部
        /// </summary>
        [LabelText("底部")]
        GROUND,

        /// <summary>
        /// 正前方
        /// </summary>
        [LabelText("正前方")]
        FRONT,
        
        /// <summary>
        /// 左手
        /// </summary>
        [LabelText("左手")]
        LEFTHAND,
        
        /// <summary>
        /// 右手
        /// </summary>
        [LabelText("右手")]
        RIGHTTHAND,
        
        /// <summary>
        /// 武器前端
        /// </summary>
        [LabelText("武器前端")]
        WEAPONSTART,
        
        /// <summary>
        /// 武器中间
        /// </summary>
        [LabelText("武器中间")]
        WEAPONCENTER,

        /// <summary>
        /// 武器末端
        /// </summary>
        [LabelText("武器末端")]
        WEAPONEND,
    }

    /// <summary>
    /// 英雄的位置组件，主要用于让外部取得想要的位置，从而做一些操作
    /// 例如特效的插拔
    /// </summary>
    public class HeroTransformComponent: Component
    {
        private Unit MyHero;
        private Transform headPos;
        private Transform channelPos;
        private Transform groundPos;
        private Transform centerPos;
        private Transform leftHeadPos;
        private Transform rightHeadPos;
        private Transform weaponStartPos;
        private Transform weaponCenterPos;
        private Transform weaponEndPos;

        public void Awake()
        {
            this.MyHero = this.GetParent<Unit>();
            this.headPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_HeadPos");
            this.groundPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_GroundPos");
            this.channelPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_FrontPos");
            this.centerPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_CenterPos");
            this.leftHeadPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_LeftHandPos");
            this.rightHeadPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_RightHandPos");
            
            this.weaponStartPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_WeaponStatrPos");
            this.weaponCenterPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_WeaponCenterPos");
            this.weaponEndPos = this.MyHero.GameObject.GetRCInternalComponent<Transform>("Trans_WeaponEndPos");
        }

        /// <summary>
        /// 获取目标位置
        /// </summary>
        /// <param name="posType"></param>
        /// <returns></returns>
        public Transform GetTranform(PosType posType)
        {
            switch (posType)
            {
                case PosType.HEAD:
                    return this.headPos;
                case PosType.GROUND:
                    return this.groundPos;
                case PosType.FRONT:
                    return this.channelPos;
                case PosType.CENTER:
                    return this.centerPos;
                case PosType.LEFTHAND:
                    return this.leftHeadPos;
                case PosType.RIGHTTHAND:
                    return this.rightHeadPos;
                case PosType.WEAPONSTART:
                    return this.weaponStartPos;
                case PosType.WEAPONCENTER:
                    return this.weaponCenterPos;
                case PosType.WEAPONEND:
                    return this.weaponEndPos;
            }
            return null;
        }
    }
}