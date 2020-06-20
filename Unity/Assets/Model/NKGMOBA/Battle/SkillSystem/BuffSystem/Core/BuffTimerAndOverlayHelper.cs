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
        /// <summary>
        /// 为Buff计算时间和层数
        /// </summary>
        /// <param name="buffSystemBase">Buff逻辑类</param>
        /// <param name="buffDataBase">Buff数据类</param>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        public static void CalculateTimerAndOverlay<A, B>(A buffSystemBase, B buffDataBase) where A : BuffSystemBase where B : BuffDataBase
        {
            BuffManagerComponent buffManagerComponent;
            buffManagerComponent = buffDataBase.BuffTargetTypes == BuffTargetTypes.Self
                    ? buffSystemBase.theUnitFrom.GetComponent<BuffManagerComponent>()
                    : buffSystemBase.theUnitBelongto.GetComponent<BuffManagerComponent>();

            //先尝试从真正的Buff链表取得Buff
            BuffSystemBase targetBuffSystemBase = buffManagerComponent.GetBuffByFlagID(buffDataBase.FlagId);

            if (targetBuffSystemBase != null)
            {
                CalculateTimerAndOverlayHelper(targetBuffSystemBase, buffDataBase);
                //Log.Info($"本次续命BuffID为{buffDataBase.FlagId}，当前层数{temp.CurrentOverlay}，最高层为{temp.MSkillBuffDataBase.MaxOverlay}");

                //刷新当前已有的Buff
                targetBuffSystemBase.OnRefresh();

                //TODO 把这个临时的回收，因为已经用不到他了
            }
            else
            {
                //尝试从临时Buff字典取
                targetBuffSystemBase = buffManagerComponent.FindBuffByFlagID_FromTempDic(buffDataBase.FlagId);

                //如果有，那就计算层数与时间，并且替换临时字典中
                if (targetBuffSystemBase != null)
                {
                    CalculateTimerAndOverlayHelper(targetBuffSystemBase, buffDataBase);
                    //Log.Info($"本次续命BuffID为{buffDataBase.FlagId}，当前层数{temp.CurrentOverlay}，最高层为{temp.MSkillBuffDataBase.MaxOverlay}");

                    //刷新当前已有的Buff
                    targetBuffSystemBase.OnRefresh();

                    //TODO 把这个临时的回收，因为已经用不到他了
                }
                else//如果没有，那就说明确实没有这个Buff，需要重新加入
                {
                    CalculateTimerAndOverlayHelper(buffSystemBase, buffDataBase);

                    //Log.Info($"本次新加BuffID为{buffDataBase.FlagId}");
                    buffSystemBase.MBuffState = BuffState.Waiting;
                    buffManagerComponent.m_TempBuffsToBeAdded.Add(buffDataBase.FlagId, buffSystemBase);
                }
            }
        }

        /// <summary>
        /// 计算刷新的持续时间和层数
        /// </summary>
        private static void CalculateTimerAndOverlayHelper(BuffSystemBase targetBuffSystemBase, BuffDataBase targetBuffDataBase)
        {
            //可以叠加，并且当前层数加上要添加Buff的目标层数未达到最高层
            if (targetBuffSystemBase.MSkillBuffDataBase.CanOverlay)
            {
                if (targetBuffSystemBase.CurrentOverlay + targetBuffSystemBase.MSkillBuffDataBase.TargetOverlay <=
                    targetBuffSystemBase.MSkillBuffDataBase.MaxOverlay)
                {
                    targetBuffSystemBase.CurrentOverlay += targetBuffSystemBase.MSkillBuffDataBase.TargetOverlay;
                }
                else
                {
                    targetBuffSystemBase.CurrentOverlay = targetBuffSystemBase.MSkillBuffDataBase.MaxOverlay;
                }
            }

            //如果是有限时长的 TODO:这里考虑处理持续时间和Buff层数挂钩的情况（比如磕了5瓶药，就是5*单瓶药的持续时间）
            if (targetBuffSystemBase.MSkillBuffDataBase.SustainTime + 1 > 0)
            {
                //Log.Info($"原本结束时间：{temp.MaxLimitTime},续命之后的结束时间{TimeHelper.Now() + buffDataBase.SustainTime}");
                targetBuffSystemBase.MaxLimitTime = TimeHelper.Now() + targetBuffDataBase.SustainTime;
            }
        }
    }
}