//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 15:35:42
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 会检查一次是否达到指定层数，然后再决定是否触发事件
    /// </summary>
    public class ListenBuffEvent_CheckOverlay: ListenBuffEventBase
    {
        [LabelText("目标层数")]
        public int targetOverlay;

        public override void Run(BuffSystemBase a)
        {
            //Log.Info($"层数判定_通过监听机制添加Buff");
            if (a.CurrentOverlay == this.targetOverlay)
            {
                foreach (var VARIABLE in m_BuffsWillBeAdded)
                {
                    //Log.Info($"层数判定_通过监听机制添加id为{VARIABLE.FlagId}的Buff");
                    Game.Scene.GetComponent<BuffPoolComponent>().AcquireBuff(VARIABLE, a.theUnitFrom, a.theUnitBelongto);
                }
            }
        }
    }
}