//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月22日 9:19:21
//------------------------------------------------------------

using System;
using System.IO;
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
                if (!type.IsSubclassOf(typeof (NP_NodeDataBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            byte[] mfile = File.ReadAllBytes(NPDataPath);

            if (mfile.Length == 0) Debug.Log("没有读取到文件");

            MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);

            foreach (var VARIABLE in MnNpDataSupportor.mNP_DataSupportorDic)
            {
                if (VARIABLE.Value.NodeType == NodeType.Action)
                {
                    VARIABLE.Value.CreateAction();
                }
            }
            
            foreach (var VARIABLE in MnNpDataSupportor.mNP_DataSupportorDic)
            {
                switch (VARIABLE.Value.NodeType)
                {
                    case NodeType.Root:
                        Log.Info("创建根节点");
                        VARIABLE.Value.CreateRoot((Action) MnNpDataSupportor.mNP_DataSupportorDic[VARIABLE.Value.linkedID[0]].NP_GetNode());
                        break;
                    case NodeType.Parallel:
                        break;
                    case NodeType.Sequence:
                        break;
                    case NodeType.BlackboardCondition:
                        break;
                    case NodeType.Service:
                        break;
                }
            }

            
            ((Root)this.MnNpDataSupportor.mNP_DataSupportorDic[this.MnNpDataSupportor.RootId].NP_GetNode()).Start();
            Log.Info("穿件完成");
        }
    }
}