//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月11日 19:13:17
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETEditor;
using UnityEngine;

namespace ETModel
{
    public class B2S_DebuggerHandler: MonoBehaviour
    {
        public List<B2S_ColliderVisualHelperBase> MB2SColliderVisualHelperBases = new List<B2S_ColliderVisualHelperBase>();


        private void OnDrawGizmos()
        {
            foreach (var VARIABLE in this.MB2SColliderVisualHelperBases)
            {
                VARIABLE.OnUpdate();
                if (VARIABLE.canDraw)
                    VARIABLE.OnDrawGizmos();
            }
        }

        public void CleanCollider()
        {
            MB2SColliderVisualHelperBases.Clear();
        }
    }
}