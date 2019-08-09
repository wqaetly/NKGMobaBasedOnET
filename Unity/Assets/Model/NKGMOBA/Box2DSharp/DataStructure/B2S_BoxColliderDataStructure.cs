//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:29:50
//------------------------------------------------------------

using System.ComponentModel;
using System.Numerics;
using Sirenix.OdinInspector;

namespace ETModel
{
#if SERVER
    public class B2S_BoxColliderDataStructure: B2S_ColliderDataStructureBase, ISupportInitialize
#else
    public class B2S_BoxColliderDataStructure: B2S_ColliderDataStructureBase
#endif
    {
        [LabelText("x轴方向上的一半长度")]
        [DisableInEditorMode]
        public float hx;

        [LabelText("y轴方向上的一半长度")]
        [DisableInEditorMode]
        public float hy;

#if SERVER
        public void BeginInit()
        {
        }

        public void EndInit()
        {
            this.finalOffset = new Vector2(this.offset.x, this.offset.y);
        }
#endif
    }
}