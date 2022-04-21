//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月17日 20:54:29
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public class VTD_BuffInfo
    {
        [BoxGroup("Buff节点Id信息")]
        [HideLabel]
        public VTD_Id BuffNodeId;

        [LabelText("层数是否被黑板值决定")]
        public bool LayersDetermindByBBValue = false;

        [Tooltip("如果为绝对层数，且此时Layers设置为10，意思是添加Buff到10层，否则就是添加10层Buff")]
        [LabelText("层数是否为绝对层数")]
        public bool LayersIsAbs;

        [HideIf("LayersDetermindByBBValue")]
        [LabelText("操作Buff层数")]
        public int Layers = 1;

        [ShowIf("LayersDetermindByBBValue")]
        [LabelText("操作Buff层数")]
        public NP_BlackBoardRelationData LayersThatDetermindByBBValue;
    }
}