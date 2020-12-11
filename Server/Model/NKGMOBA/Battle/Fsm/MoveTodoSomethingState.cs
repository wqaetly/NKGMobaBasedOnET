//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月10日 18:20:00
//------------------------------------------------------------

using ETModel.NKGMOBA.Battle.State;
using UnityEngine;

namespace ETModel.NKGMOBA.Battle.Fsm
{
    /// <summary>
    /// 移动以做某事，比如因为攻击距离/施法距离不够而进行的移动来 
    /// </summary>
    public class MoveTodoSomethingState: AFsmStateBase
    {
        /// <summary>
        /// 目标距离
        /// </summary>
        public float TargetDistance;

        /// <summary>
        /// 目标位置，这里之所以不直接计算好目标位置是因为浮点误差会导致逻辑不便
        /// </summary>
        public Vector3 TargetPos;

        /// <summary>
        /// 自身Unit引用
        /// </summary>
        public Unit SelfUnit;

        public MoveTodoSomethingState()
        {
            StateTypes = StateTypes.Run;
            this.StateName = "Anim_Run1";
            this.Priority = 1;
        }

        /// <summary>
        /// 互斥的状态，如果当前身上有这些状态，将无法切换至此状态
        /// </summary>
        public static StateTypes[] ConflictState =
        {
            StateTypes.RePluse, StateTypes.Dizziness, StateTypes.Striketofly, StateTypes.Sneer, StateTypes.Fear
        };

        public override StateTypes[] GetConflictStateTypeses()
        {
            return ConflictState;
        }

        public override void OnEnter(StackFsmComponent stackFsmComponent)
        {
            //SelfUnit.GetComponent<UnitPathComponent>().m
        }
        
        public override void OnExit(StackFsmComponent stackFsmComponent)
        {
            
        }

        public override void OnRemoved(StackFsmComponent stackFsmComponent)
        {
            
        }

        public override void Clear()
        {
            SelfUnit = null;
            TargetDistance = 0;
            TargetPos = Vector3.zero;
        }
    }
}