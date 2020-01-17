//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月30日 9:52:29
//------------------------------------------------------------

using System.Collections.Generic;
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
        HEAD,

        /// <summary>
        /// 正中央
        /// </summary>
        CENTER,

        /// <summary>
        /// 底部
        /// </summary>
        GROUND,

        /// <summary>
        /// 正前方
        /// </summary>
        FRONT
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

        public void Awake()
        {
            this.MyHero = this.GetParent<Unit>();
            this.headPos = this.MyHero.GameObject.Get<GameObject>("C_BuffBone_Glb_Overhead_Loc").transform;
            this.groundPos = this.MyHero.GameObject.Get<GameObject>("BUFFBONE_GLB_GROUND_LOC").transform;
            this.channelPos = this.MyHero.GameObject.Get<GameObject>("BUFFBONE_GLB_CHANNEL_LOC").transform;
            this.centerPos = this.MyHero.GameObject.Get<GameObject>("C_BUFFBONE_GLB_CENTER_LOC").transform;
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
            }

            return null;
        }
    }
}