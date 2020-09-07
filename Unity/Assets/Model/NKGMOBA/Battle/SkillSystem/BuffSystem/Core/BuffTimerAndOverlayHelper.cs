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
        public static void CalculateTimerAndOverlay<A, B>(A buffSystemBase, B buffDataBase) where A : ABuffSystemBase where B : BuffDataBase
        {
            BuffManagerComponent buffManagerComponent;
            buffManagerComponent = buffDataBase.BuffTargetTypes == BuffTargetTypes.Self
                    ? buffSystemBase.TheUnitFrom.GetComponent<BuffManagerComponent>()
                    : buffSystemBase.TheUnitBelongto.GetComponent<BuffManagerComponent>();

            //先尝试从真正的Buff链表取得Buff
            ABuffSystemBase targetABuffSystemBase = buffManagerComponent.GetBuffById(buffDataBase.BuffId);

            if (targetABuffSystemBase != null)
            {
                CalculateTimerAndOverlayHelper(targetABuffSystemBase, buffDataBase);
                //Log.Info($"本次续命BuffID为{buffDataBase.FlagId}，当前层数{temp.CurrentOverlay}，最高层为{temp.MSkillBuffDataBase.MaxOverlay}");

                //刷新当前已有的Buff
                targetABuffSystemBase.OnRefresh();

                //TODO 把这个临时的回收，因为已经用不到他了
            }
            else
            {
                //尝试从临时Buff字典取
                targetABuffSystemBase = buffManagerComponent.GetBuffById_FromTempDic(buffDataBase.BuffId);

                //如果有，那就计算层数与时间，并且替换临时字典中
                if (targetABuffSystemBase != null)
                {
                    CalculateTimerAndOverlayHelper(targetABuffSystemBase, buffDataBase);
                    //Log.Info($"本次续命BuffID为{buffDataBase.FlagId}，当前层数{temp.CurrentOverlay}，最高层为{temp.MSkillBuffDataBase.MaxOverlay}");

                    //刷新当前已有的Buff
                    targetABuffSystemBase.OnRefresh();

                    //TODO 把这个临时的回收，因为已经用不到他了
                }
                else//如果没有，那就说明确实没有这个Buff，需要重新加入
                {
                    CalculateTimerAndOverlayHelper(buffSystemBase, buffDataBase);

                    //Log.Info($"本次新加BuffID为{buffDataBase.FlagId}");
                    buffSystemBase.BuffState = BuffState.Waiting;
                    buffManagerComponent.TempBuffsToBeAdded.Add(buffDataBase.BuffId, buffSystemBase);
                }
            }
        }

        /// <summary>
        /// 计算刷新的持续时间和层数
        /// </summary>
        private static void CalculateTimerAndOverlayHelper(ABuffSystemBase targetABuffSystemBase, BuffDataBase targetBuffDataBase)
        {
            //可以叠加，并且当前层数加上要添加Buff的目标层数未达到最高层
            if (targetABuffSystemBase.BuffData.CanOverlay)
            {
                if (targetABuffSystemBase.CurrentOverlay + targetABuffSystemBase.BuffData.TargetOverlay <=
                    targetABuffSystemBase.BuffData.MaxOverlay)
                {
                    targetABuffSystemBase.CurrentOverlay += targetABuffSystemBase.BuffData.TargetOverlay;
                }
                else
                {
                    targetABuffSystemBase.CurrentOverlay = targetABuffSystemBase.BuffData.MaxOverlay;
                }
            }

            //如果是有限时长的 TODO:这里考虑处理持续时间和Buff层数挂钩的情况（比如磕了5瓶药，就是5*单瓶药的持续时间）
            if (targetABuffSystemBase.BuffData.SustainTime + 1 > 0)
            {
                //Log.Info($"原本结束时间：{temp.MaxLimitTime},续命之后的结束时间{TimeHelper.Now() + buffDataBase.SustainTime}");
                targetABuffSystemBase.MaxLimitTime = TimeHelper.Now() + targetBuffDataBase.SustainTime;
            }
        }
    }
}