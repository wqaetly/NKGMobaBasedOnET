//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:44:40
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using ETModel.TheDataContainsAction;
using MongoDB.Bson.Serialization;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class NP_RuntimeTreeRepositoryAwakeSystem: AwakeSystem<NP_RuntimeTreeRepository>
    {
        public override void Awake(NP_RuntimeTreeRepository self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 行为树数据仓库组件
    /// </summary>
    public class NP_RuntimeTreeRepository: Component
    {
        public const string NPDataPath = "../Config/NPBehaveConfig/";

        public Dictionary<long, NP_DataSupportor> NpRuntimeTrees = new Dictionary<long, NP_DataSupportor>();

        public void Awake()
        {
            Type[] types = typeof (NodeType).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (NP_NodeDataBase)) && !type.IsSubclassOf(typeof (NP_ClassForStoreAction)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            DirectoryInfo directory = new DirectoryInfo(NPDataPath);
            FileInfo[] fileInfos = directory.GetFiles();
            
            foreach (var VARIABLE in fileInfos)
            {
                byte[] mfile = File.ReadAllBytes(VARIABLE.FullName);

                if (mfile.Length == 0) Log.Info("没有读取到文件");

                    NP_DataSupportor MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);


                Log.Info(VARIABLE.FullName);
                NpRuntimeTrees.Add(MnNpDataSupportor.RootId, MnNpDataSupportor);
            }
        }

        public NP_DataSupportor GetNPRuntimeTree(long id)
        {
            if (this.NpRuntimeTrees.ContainsKey(id))
            {
                return NpRuntimeTrees[id];
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }
    }
}