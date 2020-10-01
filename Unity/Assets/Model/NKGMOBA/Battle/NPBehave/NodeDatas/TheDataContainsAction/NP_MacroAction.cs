//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月19日 10:56:54
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ETModel
{
    /// <summary>
    /// 用于封装一系列ActionNode，旨在增强易读性，默认只能是Action，而不能是Func
    /// </summary>
    [Title("宏行为节点", TitleAlignment = TitleAlignments.Centered)]
    public class NP_MacroAction: NP_ClassForStoreAction
    {
        [LabelText("宏行为节点集合")]
        public List<NP_ClassForStoreAction> NpClassForStoreActions = new List<NP_ClassForStoreAction>();

        public override Action GetActionToBeDone()
        {
            foreach (var npClassForStoreAction in NpClassForStoreActions)
            {
                npClassForStoreAction.Unitid = this.Unitid;
                npClassForStoreAction.BelongtoRuntimeTree = this.BelongtoRuntimeTree;
            }

            this.Action = this.DoMacro;
            return this.Action;
        }

        public void DoMacro()
        {
            //Log.Info("准备执行初始化的行为操作");
            foreach (var classForStoreAction in NpClassForStoreActions)
            {
                classForStoreAction.GetActionToBeDone().Invoke();
            }
        }
    }
}