//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月28日 22:41:25
//------------------------------------------------------------

using System;
using ETMode;
using ETModel;
using NodeEditorFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Plugins.NodeEditor
{
    public class PrevB2SType: ValueConnectionType //: IConnectionTypeDeclaration
    {
        public override string Identifier => "PrevB2SDatas";

        public override Type Type => typeof (B2S_CollisionInstance);

        public override Color Color => Color.green;

    }

    public class NextB2SType: ValueConnectionType // : IConnectionTypeDeclaration
    {
        public override string Identifier => "NextB2SDatas";

        public override Type Type => typeof (B2S_CollisionInstance);

        public override Color Color => Color.magenta;
    }
}