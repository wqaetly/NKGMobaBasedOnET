using System;
using ETModel;
using ETModel.TheDataContainsAction;
using Sirenix.OdinInspector;

namespace Model.Inotia5.NPBehave.NodeDatas.TheDataContainsAction
{
    [Title("获取并设置自身生命值百分比，黑板键[SelfHealthPercentage]", TitleAlignment = TitleAlignments.Centered)]
    public class NP_GetAndSetSelfHealthPercentageAction : NP_ClassForStoreAction
    {
        public override Action GetActionToBeDone()
        {
            this.m_Action = this.GetAndSetSelfHealthPercentage;
            return this.m_Action;
        }

        public void GetAndSetSelfHealthPercentage()
        {
            Unit self = Game.Scene.GetComponent<UnitComponent>().Get(this.Unitid);
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            self.GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(this.RuntimeTreeID)
                    .GetBlackboard()["SelfHealthPercentage"] =
                numericComponent[NumericType.Hp] * 1.0f / numericComponent[NumericType.MaxHp];
            //Log.Info($"当前生命百分比：{numericComponent[NumericType.Hp] * 1.0f / numericComponent[NumericType.MaxHp]}");
        }
    }
}