//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 19:22:48
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ChangeMaterialBuffSystem: ABuffSystemBase<ChangeMaterialBuffData>
    {
#if !SERVER 
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long m_SelfNextimer;
#endif
        public override void OnExecute(uint currentFrame)
        {
#if !SERVER 
            Game.EventSystem.Publish(new EventType.ChangeMaterialBuffSystemExcuteEvent()
                {ChangeMaterialBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget()}).Coroutine();
#endif
        }

        public override void OnFinished(uint currentFrame)
        {
#if !SERVER 
            Game.EventSystem.Publish(new EventType.ChangeMaterialBuffSystemFinishEvent()
                {ChangeMaterialBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget()}).Coroutine();
#endif
        }

    }
}