//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:29:50
//------------------------------------------------------------

using Sirenix.OdinInspector;

namespace ETModel
{
    public class B2S_BoxColliderDataStructure: B2S_ColliderDataStructureBase
    {
        [LabelText("x轴方向上的一半长度")]
        [DisableInEditorMode]
        public float hx;

        [LabelText("y轴方向上的一半长度")]
        [DisableInEditorMode]
        public float hy;
    }
}