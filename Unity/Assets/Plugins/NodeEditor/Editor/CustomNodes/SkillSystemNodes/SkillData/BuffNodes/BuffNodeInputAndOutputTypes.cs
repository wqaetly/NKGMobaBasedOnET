//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月14日 16:21:12
//------------------------------------------------------------

using System;
using NodeEditorFramework;
using Plugins;
using Color = UnityEngine.Color;

namespace SkillDemo
{
    public class PrevBuffType: ValueConnectionType //: IConnectionTypeDeclaration
    {
        public override string Identifier => "PrevNodeDatas";

        public override Type Type => typeof (BuffNodeBase);

        public override Color Color => Color.yellow;
    }

    public class NextBuffType: ValueConnectionType // : IConnectionTypeDeclaration
    {
        public override string Identifier => "NextNodeDatas";

        public override Type Type => typeof (BuffNodeBase);

        public override Color Color => Color.cyan;
    }
}