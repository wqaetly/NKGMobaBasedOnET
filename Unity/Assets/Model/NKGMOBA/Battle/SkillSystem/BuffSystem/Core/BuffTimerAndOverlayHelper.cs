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
        public static void CalculateTimerAndOverlay<A, B>(A buffSystemBase, B buffDataBase) where A : BuffSystemBase where B : BuffDataBase
        {
            BuffManagerComponent buffManagerComponent;
            if (buffDataBase.BuffTargetTypes == BuffTargetTypes.Self)
            {
                buffManagerComponent = buffSystemBase.theUnitFrom.GetComponent<BuffManagerComponent>();
            }
            else
            {
                buffManagerComponent = buffSystemBase.theUnitBelongto.GetComponent<BuffManagerComponent>();
            }

            //先尝试从真正的Buff链表取得Buff
            BuffSystemBase temp = buffManagerComponent.GetBuffByFlagID(buffDataBase.FlagId);

            if (temp != null)
            {
                //可以叠加，并且当前层数未达到最高层
                if (temp.MSkillBuffDataBase.CanOverlay &&
                    temp.CurrentOverlay < temp.MSkillBuffDataBase.MaxOverlay)
                {
                    temp.CurrentOverlay++;
                }

                //如果是有限时长的
                if (temp.MSkillBuffDataBase.SustainTime + 1 > 0)
                {
                    Log.Info($"原本结束时间：{temp.MaxLimitTime},续命之后的结束时间{TimeHelper.Now() + buffDataBase.SustainTime}");
                    temp.MaxLimitTime = TimeHelper.Now() + buffDataBase.SustainTime;
                }

                Log.Info($"本次续命BuffID为{buffDataBase.FlagId}，当前层数{temp.CurrentOverlay}，最高层为{temp.MSkillBuffDataBase.MaxOverlay}");

                //刷新当前已有的Buff
                temp.OnRefresh();

                //TODO 把这个临时的回收，因为已经用不到他了
            }
            else
            {
                //尝试从临时Buff字典取
                temp = buffManagerComponent.FindBuffByFlagID_FromTempDic(buffDataBase.FlagId);

                //如果有，那就计算层数与时间，并且替换临时字典中
                if (temp != null)
                {
                    //可以叠加，并且当前层数未达到最高层
                    if (temp.MSkillBuffDataBase.CanOverlay &&
                        temp.CurrentOverlay < temp.MSkillBuffDataBase.MaxOverlay)
                    {
                        temp.CurrentOverlay++;
                    }

                    //如果是有限时长的
                    if (temp.MSkillBuffDataBase.SustainTime + 1 > 0)
                    {
                        temp.MaxLimitTime = TimeHelper.Now() + buffDataBase.SustainTime;
                    }

                    //刷新当前已有的Buff，因为有些Buff自带事件，需要抛出一下
                    temp.OnRefresh();

                    //TODO 把这个临时的回收，因为已经用不到他了
                }
                else
                {
                    //如果是有限时长的
                    if (buffDataBase.SustainTime + 1 > 0)
                    {
                        buffSystemBase.MaxLimitTime = TimeHelper.Now() + buffDataBase.SustainTime;
                    }

                    buffSystemBase.CurrentOverlay++;

                    Log.Info($"本次新加BuffID为{buffDataBase.FlagId}");
                    buffSystemBase.MBuffState = BuffState.Waiting;
                    buffManagerComponent.m_TempBuffsToBeAdded.Add(buffDataBase.FlagId, buffSystemBase);
                }
            }
        }
    }
}