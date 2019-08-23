//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 9:19:21
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using ETModel.TheDataContainsAction;
using MongoDB.Bson.Serialization;
using NPBehave;
using UnityEngine;
using Action = NPBehave.Action;

namespace ETModel
{
    [ObjectSystem]
    public class TestDeSeriComponentAwakeSystem: AwakeSystem<TestDeSeriComponent>
    {
        public override void Awake(TestDeSeriComponent self)
        {
            self.Awake();
        }
    }

    public class TestDeSeriComponent: Component
    {
        public void Awake()
        {
            Unit mUnit = UnitFactory.Create(10000);
            mUnit.AddComponent<NP_RuntimeTreeManager>();
            NP_RuntimeTreeFactory.CreateNpRuntimeTree(mUnit, 102658009006093).m_NPRuntimeTreeRootNode.Start();
        }
    }
}