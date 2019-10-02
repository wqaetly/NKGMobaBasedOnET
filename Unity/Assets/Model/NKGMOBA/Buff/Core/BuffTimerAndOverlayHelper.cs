//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年10月2日 17:03:26
//------------------------------------------------------------

namespace ETModel
{
    public static class BuffTimerAndOverlayHelper
    {
        public static void CalculateTimerAndOverlay<A, B>(A BuffSystemBase, B BuffDataBase) where A : BuffSystemBase where B : BuffDataBase
        {
            A tempSystem =
                    (A) BuffSystemBase.theUnitBelongto.GetComponent<BuffManagerComponent>().GetBuffByFlagID(BuffDataBase.FlagId);

            if (tempSystem != null)
            {
                BindStateBuffData tempData = tempSystem.MSkillBuffDataBase as BindStateBuffData;
                //可以叠加，并且当前层数未达到最高层
                if (tempData.CanOverlay &&
                    tempSystem.CurrentOverlay < tempData.MaxOverlay)
                {
                    //如果是有限时长的
                    if (tempData.SustainTime + 1 > 0)
                    {
                        tempSystem.MaxLimitTime += tempData.SustainTime;
                    }

                    tempSystem.CurrentOverlay++;
                }

                //不用Execute了，因为已经有了
                BuffSystemBase.MBuffState = BuffState.Finished;
            }
            else
            {
                //如果是有限时长的
                if (BuffDataBase.SustainTime + 1 > 0)
                {
                    BuffSystemBase.MaxLimitTime = TimeHelper.ClientNow() + BuffDataBase.SustainTime;
                }

                BuffSystemBase.CurrentOverlay++;

                BuffSystemBase.MBuffState = BuffState.Waiting;
            }
        }
    }
}