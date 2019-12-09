//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 17:03:26
//------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    public static class BuffTimerAndOverlayHelper
    {
        public static void CalculateTimerAndOverlay<A, B>(A BuffSystemBase, B BuffDataBase) where A : BuffSystemBase where B : BuffDataBase
        {
            List<BuffSystemBase> tempList = BuffSystemBase.theUnitBelongto.GetComponent<BuffManagerComponent>().GetBuffByFlagID(BuffDataBase.FlagId);

            if (tempList != null && tempList.Count > 0)
            {
                A tempSystem = (A)tempList[0];
                BuffDataBase tempData = tempSystem.MSkillBuffDataBase;
                //可以叠加，并且当前层数未达到最高层
                if (tempData.CanOverlay &&
                    tempSystem.CurrentOverlay < tempData.MaxOverlay)
                {
                    tempSystem.CurrentOverlay++;
                }

                //如果是有限时长的
                if (tempData.SustainTime + 1 > 0)
                {
                    //Log.Info($"原本结束时间：{tempSystem.MaxLimitTime},续命之后的结束时间{TimeHelper.Now() + BuffDataBase.SustainTime}");
                    tempSystem.MaxLimitTime = TimeHelper.Now() + BuffDataBase.SustainTime;
                }

                //Log.Info($"本次续命BuffID为{BuffDataBase.FlagId}，当前层数{tempSystem.CurrentOverlay}，最高层为{tempData.MaxOverlay}");
                //不用Execute了，因为已经有了
                
                tempSystem.OnRefresh();
                BuffSystemBase.MBuffState = BuffState.Finished;
            }
            else
            {
                //如果是有限时长的
                if (BuffDataBase.SustainTime + 1 > 0)
                {
                    BuffSystemBase.MaxLimitTime = TimeHelper.Now() + BuffDataBase.SustainTime;
                }

                BuffSystemBase.CurrentOverlay++;

                //Log.Info($"本次新加BuffID为{BuffDataBase.FlagId}");
                BuffSystemBase.MBuffState = BuffState.Waiting;
            }
        }
    }
}