//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 19:47:29
//------------------------------------------------------------

using System;
using NPBehave;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

namespace ET
{
    /// <summary>
    /// 等待一个可变化的时间，比如CD变化
    /// </summary>
    [Title("等待一个可变化的时间", TitleAlignment = TitleAlignments.Centered)]
    public class NP_WaitAChangeableTimeAction : NP_ClassForStoreAction
    {
        [LabelText("等待的时长")] public NP_BlackBoardRelationData TheTimeToWait = new NP_BlackBoardRelationData();
        private bool HasInited;

        public override Func<bool, Action.Result> GetFunc2ToBeDone()
        {
            this.Func2 = WaitTime;

            return this.Func2;
        }

        public Action.Result WaitTime(bool hasDown)
        {
            if (!HasInited)
            {
                this.BelongtoRuntimeTree.GetBlackboard().Set<uint>(this.TheTimeToWait.BBKey,
                    this.BelongToUnit.LsfComponent.CurrentFrame +
                    TimeAndFrameConverter.Frame_Float2Frame(this.TheTimeToWait.GetTheBBDataValue<float>()));
                HasInited = true;
            }

            if (this.BelongToUnit.LsfComponent.CurrentFrame >=
                TimeAndFrameConverter.Frame_Float2Frame(this.BelongtoRuntimeTree.GetBlackboard()
                    .Get<float>(this.TheTimeToWait.BBKey)))
            {
                HasInited = false;
                return NPBehave.Action.Result.SUCCESS;
            }

            return NPBehave.Action.Result.PROGRESS;
        }
    }
}