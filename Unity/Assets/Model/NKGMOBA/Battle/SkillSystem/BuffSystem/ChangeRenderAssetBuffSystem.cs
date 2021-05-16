//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 18:19:55
//------------------------------------------------------------

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ETModel
{
    public class ChangeRenderAssetBuffSystem: ABuffSystemBase
    {
        /// <summary>
        /// 自身下一个时间点
        /// </summary>
        private long m_SelfNextimer;

        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
        }

        public override void OnExecute()
        {
            SetScriptableRendererFeatureState(true);
            //Log.Info($"作用间隔为{selfNextimer - TimeHelper.Now()},持续时间为{temp.SustainTime},持续到{this.selfNextimer}");
            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                //Log.Info($"执行持续伤害的Update,当前时间是{TimeHelper.Now()}");
                if (TimeHelper.Now() > MaxLimitTime)
                {
                    this.BuffState = BuffState.Finished;
                    //Log.Info("持续伤害结束了");
                }
                else if (TimeHelper.Now() > this.m_SelfNextimer)
                {
                    //ExcuteDamage();
                }
            }
        }

        public override void OnFinished()
        {
            SetScriptableRendererFeatureState(false);
        }

        private void SetScriptableRendererFeatureState(bool state)
        {
            ChangeRenderAssetBuffData changeRenderAssetBuffData = this.BuffData as ChangeRenderAssetBuffData;
            foreach (var renderFeatureNameToActive in changeRenderAssetBuffData.RenderFeatureNameToActive)
            {
                ForwardRenderBridge.Instance.SetScriptableRendererFeatureState(renderFeatureNameToActive, state);
            }
        }
    }
}