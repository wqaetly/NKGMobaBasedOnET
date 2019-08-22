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
        public const string NPDataPath = "../Config/NPBehaveConfig/NPBehaveDataTest.bytes";

        public NP_DataSupportor MnNpDataSupportor = new NP_DataSupportor();

        public void Awake()
        {
            Type[] types = typeof (NodeType).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (NP_NodeDataBase))&&!type.IsSubclassOf(typeof (NP_ClassForStoreAction)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            byte[] mfile = File.ReadAllBytes(NPDataPath);

            if (mfile.Length == 0) Debug.Log("没有读取到文件");

            MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);

            //先要把叶子结点都实例化好，这是基础
            foreach (var VARIABLE in MnNpDataSupportor.mNP_DataSupportorDic)
            {
                if (VARIABLE.Value.NodeType == NodeType.Task)
                {
                    VARIABLE.Value.CreateTask();
                }
            }
            //然后开始处理非叶子结点
            foreach (var VARIABLE in MnNpDataSupportor.mNP_DataSupportorDic)
            {
                switch (VARIABLE.Value.NodeType)
                {
                    case NodeType.Decorator:
                        VARIABLE.Value.CreateDecoratorNode(MnNpDataSupportor.mNP_DataSupportorDic[VARIABLE.Value.linkedID[0]].NP_GetNode());
                        break;
                    case NodeType.Composite:
                        List<Node> temp = new List<Node>();
                        foreach (var VARIABLE1 in VARIABLE.Value.linkedID)
                        {
                            temp.Add(MnNpDataSupportor.mNP_DataSupportorDic[VARIABLE1].NP_GetNode());
                        }
                        VARIABLE.Value.CreateComposite(temp.ToArray());
                        break;
                }
            }

            ((Root) this.MnNpDataSupportor.mNP_DataSupportorDic[this.MnNpDataSupportor.RootId].NP_GetNode()).Start();
            Log.Info("穿件完成");
        }
    }
}