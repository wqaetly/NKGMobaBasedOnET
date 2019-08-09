//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月13日 21:31:05
//------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using ETModel;
using Sirenix.OdinInspector;

namespace ETModel
{
#if SERVER
    public class B2S_CircleColliderDataStructure: B2S_ColliderDataStructureBase, ISupportInitialize
#else
    public class B2S_CircleColliderDataStructure: B2S_ColliderDataStructureBase
#endif
    {
        [LabelText("半径")]
        public float radius;

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