//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月26日 19:47:29
//------------------------------------------------------------

using System;
using NPBehave;
using Sirenix.OdinInspector;
using Action = NPBehave.Action;

namespace ETModel
{
    /// <summary>
    /// 等待一个可变化的时间，用于处理突如其来的CD变化
    /// </summary>
    [Title("等待一个可变化的时间", TitleAlignment = TitleAlignments.Centered)]
    public class NP_WaitAChangeableTimeAction: NP_ClassForStoreAction
    {
        [BoxGroup("引用数据的Id")]
        [LabelText("技能数据结点Id")]
        public VTD_Id DataId = new VTD_Id();
        
        [BoxGroup("引用数据的Id")]
        [LabelText("检查技能的Id")]
        public VTD_Id SkillIdBelongTo = new VTD_Id();

        [LabelText("等待的时长")]
        public NP_BlackBoardRelationData TheTimeToWait = new NP_BlackBoardRelationData();

        [HideInEditorMode]
        public SkillDesNodeData SkillDesNodeData;

        [HideInEditorMode]
        public Unit m_Unit;

        private bool hasInit = false;

        private double lastElapsedTime;

        private Blackboard tempBlackboard;

        public override Func<bool, global::NPBehave.Action.Result> GetFunc2ToBeDone()
        {
            this.Func2 = WaitTime;
            return this.Func2;
        }

        public Action.Result WaitTime(bool hasDown)
        {
            if (!this.hasInit)
            {
                this.m_Unit = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
                tempBlackboard = this.BelongtoRuntimeTree.GetBlackboard();

                this.lastElapsedTime = SyncContext.Instance.GetClock().ElapsedTime;
                this.SkillDesNodeData = (SkillDesNodeData) this.BelongtoRuntimeTree.BelongNP_DataSupportor.BuffNodeDataDic[this.DataId.Value];
                tempBlackboard.Set(this.TheTimeToWait.BBKey,
                    this.SkillDesNodeData.SkillCD[
                        this.m_Unit.GetComponent<SkillCanvasManagerComponent>().GetSkillLevel(this.SkillIdBelongTo.Value)]);
                //Log.Info($"第一次设置Q技能CD：{tempBlackboard[NpBlackBoardRelationData.DicKey]}");
                this.hasInit = true;
            }

            //刷新黑板上的CD信息
            tempBlackboard.Set(this.TheTimeToWait.BBKey,
                tempBlackboard.Get<float>(this.TheTimeToWait.BBKey) -
                (float) (SyncContext.Instance.GetClock().ElapsedTime - lastElapsedTime));

            this.lastElapsedTime = SyncContext.Instance.GetClock().ElapsedTime;
            /*Log.Info(
                $"在执行改变CD逻辑，此时剩余CD为{tempBlackboard.Get<float>(this.NpBlackBoardRelationData.DicKey)}");*/
            if (tempBlackboard.Get<float>(this.TheTimeToWait.BBKey) <= 0)
            {
                //Log.Info("CD刷新完成");
                lastElapsedTime = -1;
                //下次再运行就会初始化了
                this.hasInit = false;
                return NPBehave.Action.Result.FAILED;
            }

            return NPBehave.Action.Result.PROGRESS;
        }
    }
}