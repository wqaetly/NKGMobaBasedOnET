using System;
using ETModel;
using ETModel.TheDataContainsAction;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Model.Inotia5.NPBehave.NodeDatas.TheDataContainsAction
{
    [Title("获取与玩家距离并设置到黑板里，黑板键 [DistanceToPlayer]", TitleAlignment = TitleAlignments.Centered)]
    public class NP_GetAndSetDistanceToPlayer : NP_ClassForStoreAction
    {
        public override Action GetActionToBeDone()
        {
            this.m_Action = this.GetAndSetDistance;
            return this.m_Action;
        }

        public void GetAndSetDistance()
        {
            Unit self = Game.Scene.GetComponent<UnitComponent>().Get(Unitid),
                player = Game.Scene.GetComponent<UnitComponent>().MyUnit;
            self.GetComponent<NP_RuntimeTreeManager>()
                    .GetTreeByRuntimeID(RuntimeTreeID).GetBlackboard()["DistanceToPlayer"] =
                Vector2.Distance(self.Position, player.Position);
            //Log.Info($"距离赋值{Vector2.Distance(self.Position, player.Position)}");
        }
    }
}