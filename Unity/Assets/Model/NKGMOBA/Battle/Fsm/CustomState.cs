//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年12月22日 18:54:08
//------------------------------------------------------------

namespace ETModel.NKGMOBA.Battle.State
{
    /// <summary>
    /// 自定义状态类，用于技能编辑器配置
    /// </summary>
    public class CustomState: AFsmStateBase
    {
        /// <summary>
        /// 互斥的状态
        /// </summary>
        public StateTypes ConflictStateTypes;
        
        public override StateTypes GetConflictStateTypeses()
        {
            return ConflictStateTypes;
        }

        public override void OnEnter(StackFsmComponent stackFsmComponent)
        {

        }

        public override void OnExit(StackFsmComponent stackFsmComponent)
        {

        }

        public override void OnRemoved(StackFsmComponent stackFsmComponent)
        {

        }
    }
}