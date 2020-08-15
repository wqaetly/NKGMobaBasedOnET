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
    public class NP_RuntimeTreeRepositoryAwakeSystem: AwakeSystem<NP_TreeDataRepository>
    {
        public override void Awake(NP_TreeDataRepository self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 行为树数据仓库组件
    /// </summary>
    public class NP_TreeDataRepository: Component
    {
        /// <summary>
        /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
        /// </summary>
        public Dictionary<long, NP_DataSupportor> NpRuntimeTreesDatas = new Dictionary<long, NP_DataSupportor>();

        public void Awake()
        {
            Type[] types = typeof (NodeType).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (NP_NodeDataBase)) && !type.IsSubclassOf(typeof (NP_ClassForStoreAction)) &&
                    !type.IsSubclassOf(typeof (SkillBaseNodeData)) && !type.IsSubclassOf(typeof (BuffDataBase)) &&
                    !type.IsSubclassOf(typeof (ListenBuffEventBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            resourcesComponent.LoadBundle("skillconfigs.unity3d");
            GameObject skillConfigs = (GameObject) resourcesComponent.GetAsset("skillconfigs.unity3d", "SkillConfigs");
            foreach (var VARIABLE in skillConfigs.GetComponent<ReferenceCollector>().data)
            {
                TextAsset textAsset = skillConfigs.Get<TextAsset>(VARIABLE.key);

                if (textAsset.bytes.Length == 0) Log.Info("没有读取到文件");

                try
                {
                    NP_DataSupportor MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(textAsset.bytes);

                    Log.Info($"反序列化行为树:{VARIABLE.key}完成，Id为{MnNpDataSupportor.RootId}");

                    NpRuntimeTreesDatas.Add(MnNpDataSupportor.RootId, MnNpDataSupportor);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取一棵树的所有数据（通过深拷贝形式）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NP_DataSupportor GetNP_TreeData_DeepCopy(long id)
        {
            if (this.NpRuntimeTreesDatas.ContainsKey(id))
            {
                return NpRuntimeTreesDatas[id].DeepCopy();
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }
    }
}