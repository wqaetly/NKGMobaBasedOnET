//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月13日 17:30:29
//------------------------------------------------------------

namespace ETModel.NKGMOBA.Battle.State
{
    [ObjectSystem]
    public class FsmStateBaseAwakeSystem: AwakeSystem<FsmStateBase, StateTypes, string, int>
    {
        public override void Awake(FsmStateBase self, StateTypes a, string b, int c)
        {
            self.StateTypes = a;
            self.StateName = b;
            self.Priority = c;
        }
    }

    public class FsmStateBase: Component
    {
        /// <summary>
        /// 状态类型
        /// </summary>
        public StateTypes StateTypes { get; set; }

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 状态的优先级，值越大，优先级越高。
        /// </summary>
        public int Priority { get; set; }
    }
}