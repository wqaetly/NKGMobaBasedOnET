//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年10月20日 20:25:48
//------------------------------------------------------------

using System;
using ETModel.NKGMOBA.Battle.State;

namespace ETModel.NKGMOBA.Battle.Fsm
{
    public class CommonAttackState: AFsmStateBase
    {
        /// <summary>
        /// 互斥的状态，如果当前身上有这些状态，将无法切换至此状态
        /// </summary>
        public static StateTypes[] ConflictState =
        {
            StateTypes.RePluse, StateTypes.Dizziness, StateTypes.Striketofly, StateTypes.Sneer, StateTypes.Fear
        };

        public CommonAttackState()
        {
            StateTypes = StateTypes.CommonAttack;
            this.StateName = "CommonAttack";
            this.Priority = 1;
        }

        public override StateTypes[] GetConflictStateTypeses()
        {
            return ConflictState;
        }

        public override void OnEnter(StackFsmComponent stackFsmComponent)
        {

        }

        public override void OnUpdate(StackFsmComponent stackFsmComponent)
        {
            
        }

        public override void OnExit(StackFsmComponent stackFsmComponent)
        {
            stackFsmComponent.Entity.GetComponent<CommonAttackComponent>().CancelCommonAttack();
        }
    }
}