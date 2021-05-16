//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年5月12日 19:22:48
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ETModel
{
    public class ChangeMaterialBuffSystem: ABuffSystemBase
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
            //Log.Info("持续伤害Buff初始化完成");
        }

        public override void OnExecute()
        {
            ChangeMaterialBuffData changeMaterialBuffData = this.BuffData as ChangeMaterialBuffData;
            SkinnedMeshRenderer skinnedMeshRenderer = this.GetBuffTarget().GameObject.GetRCInternalComponent<SkinnedMeshRenderer>("Materials");

            List<Material> currentMats = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(currentMats);

            foreach (var changeMaterialName in changeMaterialBuffData.TheMaterialNameWillBeAdded)
            {
                currentMats.Add(this.GetBuffTarget().GameObject.GetComponent<ReferenceCollector>().Get<Material>(changeMaterialName));
            }

            skinnedMeshRenderer.sharedMaterials = currentMats.ToArray();
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
            }
        }

        public override void OnFinished()
        {
            ChangeMaterialBuffData changeMaterialBuffData = this.BuffData as ChangeMaterialBuffData;
            SkinnedMeshRenderer skinnedMeshRenderer = this.GetBuffTarget().GameObject.GetRCInternalComponent<SkinnedMeshRenderer>("Materials");

            List<Material> currentMats = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(currentMats);

            foreach (var changeMaterialName in changeMaterialBuffData.TheMaterialNameWillBeAdded)
            {
                for (int i = currentMats.Count - 1; i >= 0; i--)
                {
                    if (currentMats[i].name == changeMaterialName)
                    {
                        currentMats.RemoveAt(i);
                    }
                }
            }

            skinnedMeshRenderer.sharedMaterials = currentMats.ToArray();
        }
    }
}