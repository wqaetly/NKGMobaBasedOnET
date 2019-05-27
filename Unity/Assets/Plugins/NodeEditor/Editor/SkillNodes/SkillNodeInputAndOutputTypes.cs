//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 16:21:12
//------------------------------------------------------------

using System;
using System.Drawing;
using ETModel;
using NodeEditorFramework;
using Color = UnityEngine.Color;

namespace SkillDemo
{
    public class PrevSkillType: ValueConnectionType //: IConnectionTypeDeclaration
    {
        public override string Identifier => "PrevNodeDatas";

        public override Type Type => typeof (BaseNodeData);

        public override Color Color => Color.yellow;
    }

    public class NextSkillType: ValueConnectionType // : IConnectionTypeDeclaration
    {
        public override string Identifier => "NextNodeDatas";

        public override Type Type => typeof (BaseNodeData);

        public override Color Color => Color.cyan;
    }
}