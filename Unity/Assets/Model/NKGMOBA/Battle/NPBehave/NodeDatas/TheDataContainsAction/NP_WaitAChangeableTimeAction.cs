//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 19:47:29
//------------------------------------------------------------

using System;
using NPBehave;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 等待一个可变化的时间，用于处理突如其来的CD变化
    /// </summary>
    [Title("等待一个可变化的时间", TitleAlignment = TitleAlignments.Centered)]
    public class NP_WaitAChangeableTimeAction: NP_ClassForStoreAction
    {
        [LabelText("要引用的的数据结点ID")]
        public long dataId;

        [LabelText("将要检查的技能ID（QWER：0123）")]
        public int theSkillIDBelongTo;

        [HideInEditorMode]
        public NodeDataForStartSkill m_NodeDataForStartSkill;

        public NP_BlackBoardRelationData NpBlackBoardRelationData;

        [HideInEditorMode]
        public Unit m_Unit;

        private bool hasInit = false;

        private double lastElapsedTime;

        private Blackboard tempBlackboard;

        public override Func<bool, global::NPBehave.Action.Result> GetFunc2ToBeDone()
        {
            this.m_Func2 = WaitTime;
            return this.m_Func2;
        }

        public global::NPBehave.Action.Result WaitTime(bool hasDown)
        {
            if (!this.hasInit)
            {
                this.m_Unit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
                tempBlackboard = this.m_Unit.GetComponent<NP_RuntimeTreeManager>().GetTreeByRuntimeID(this.RuntimeTreeID).GetBlackboard();

                this.lastElapsedTime = SyncContext.Instance.GetClock().ElapsedTime;
                this.m_NodeDataForStartSkill = (NodeDataForStartSkill) Game.Scene.GetComponent<UnitComponent>().Get(Unitid)
                        .GetComponent<NP_RuntimeTreeManager>()
                        .GetTreeByRuntimeID(this.RuntimeTreeID).m_BelongNP_DataSupportor.mSkillDataDic[this.dataId];
                tempBlackboard.Set(NpBlackBoardRelationData.BBKey,
                    m_NodeDataForStartSkill.SkillCD[this.m_Unit.GetComponent<HeroDataComponent>().GetSkillLevel(this.theSkillIDBelongTo)]);
                //Log.Info($"第一次设置Q技能CD：{tempBlackboard[NpBlackBoardRelationData.DicKey]}");
                this.hasInit = true;
            }

            //刷新黑板上的CD信息
            tempBlackboard.Set(this.NpBlackBoardRelationData.BBKey,
                tempBlackboard.Get<float>(this.NpBlackBoardRelationData.BBKey) -
                (float) (SyncContext.Instance.GetClock().ElapsedTime - lastElapsedTime));

            this.lastElapsedTime = SyncContext.Instance.GetClock().ElapsedTime;
            /*Log.Info(
                $"在执行改变CD逻辑，此时剩余CD为{tempBlackboard.Get<float>(this.NpBlackBoardRelationData.DicKey)}");*/
            if (tempBlackboard.Get<float>(this.NpBlackBoardRelationData.BBKey) <= 0)
            {
                //Log.Info("CD刷新完成");
                lastElapsedTime = -1;
                //下次再运行就会初始化了
                this.hasInit = false;
                return global::NPBehave.Action.Result.FAILED;
            }

            return global::NPBehave.Action.Result.PROGRESS;
        }
    }
}