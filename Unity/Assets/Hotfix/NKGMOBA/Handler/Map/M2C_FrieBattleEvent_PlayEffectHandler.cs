//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 16:42:12
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_FireBattleEvent_PlayEffectHandler: AMHandler<M2C_FrieBattleEvent_PlayEffect>
    {
        protected override ETTask Run(ETModel.Session session, M2C_FrieBattleEvent_PlayEffect message)
        {
            switch (message.BattleKey)
            {
                case "Darius_Q_OutHit":
                    ETModel.Log.Info($"这里进行客户端Q技能打中后的逻辑!!!!!!!!!!!");
                    UnitComponent unitComponent = ETModel.Game.Scene.GetComponent<UnitComponent>();
                    Unit unit = unitComponent.Get(message.FromUnitId);
                    Dictionary<long, SkillBaseNodeData> skillNodeDataSupporter =
                            unit.GetComponent<NP_RuntimeTreeManager>()
                                    .GetTreeByPrefabID(NP_Client_TreeIds.Darius_Q_Client).m_BelongNP_DataSupportor.mSkillDataDic;
                    ETModel.Game.Scene.GetComponent<BuffPoolComponent>()
                            .AcquireBuff(((NodeDataForSkillBuff) skillNodeDataSupporter[10003]).SkillBuffBases, unit,
                                unitComponent.Get(message.BelongToUnitId)).AutoAddBuff();
                    break;
            }

            return ETTask.CompletedTask;
        }
    }
}