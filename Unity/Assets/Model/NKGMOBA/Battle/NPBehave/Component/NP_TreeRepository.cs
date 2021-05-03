//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月23日 15:44:40
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel.BBValues;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
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
#if SERVER
        public const string NPDataPath = "../Config/SkillConfigs/Server/";
#else
        public const string NPDataPath = "../Config/SkillConfigs/Client/";
#endif

        /// <summary>
        /// 运行时的行为树仓库，注意，一定不能对这些数据做修改
        /// </summary>
        private Dictionary<long, NP_DataSupportor> m_NpRuntimeTreesDatas = new Dictionary<long, NP_DataSupportor>();

        public void Awake()
        {
            Type[] types = typeof (NodeType).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(typeof (NP_NodeDataBase)) || type.IsSubclassOf(typeof (NP_ClassForStoreAction)) ||
                    type.IsSubclassOf(typeof (BuffNodeDataBase)) || type.IsSubclassOf(typeof (BuffDataBase)) ||
                    type.IsSubclassOf(typeof (ListenBuffEvent_Normal)) || type.IsSubclassOf(typeof (NP_DataSupportorBase)))
                {
                    BsonClassMap.LookupClassMap(type);
                }
            }
            
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Int));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Bool));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Float));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_String));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Vector3));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_Long));
            BsonClassMap.LookupClassMap(typeof (NP_BBValue_List_Long));
#if SERVER
            DirectoryInfo directory = new DirectoryInfo(NPDataPath);
            FileInfo[] fileInfos = directory.GetFiles();

            foreach (var fileInfo in fileInfos)
            {
                byte[] mfile = File.ReadAllBytes(fileInfo.FullName);

                if (mfile.Length == 0) Log.Info("没有读取到文件");

                try
                {
                    NP_DataSupportor MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(mfile);

                    Log.Info($"反序列化行为树:{fileInfo.FullName}完成");

                    NpRuntimeTreesDatas.Add(MnNpDataSupportor.RootId, MnNpDataSupportor);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
#else

            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            GameObject skillConfigs = resourcesComponent.LoadAsset<GameObject>(ABPathUtilities.GetSkillConfigPath("SkillConfigs"));
            foreach (var referenceCollectorData in skillConfigs.GetComponent<ReferenceCollector>().data)
            {
                TextAsset textAsset = skillConfigs.GetTargetObjectFromRC<TextAsset>(referenceCollectorData.key);

                if (textAsset.bytes.Length == 0) Log.Info("没有读取到文件");

                try
                {
                    NP_DataSupportor MnNpDataSupportor = BsonSerializer.Deserialize<NP_DataSupportor>(textAsset.bytes);

                    Log.Info($"反序列化行为树:{referenceCollectorData.key}完成");

                    this.m_NpRuntimeTreesDatas.Add(MnNpDataSupportor.NpDataSupportorBase.NPBehaveTreeDataId, MnNpDataSupportor);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    throw;
                }
            }

#endif
        }
        
        /// <summary>
        /// 获取一棵树的所有数据（默认形式）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NP_DataSupportor GetNP_TreeData(long id)
        {
            if (this.m_NpRuntimeTreesDatas.ContainsKey(id))
            {
                return this.m_NpRuntimeTreesDatas[id];
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }

        /// <summary>
        /// 获取一棵树的所有数据（仅深拷贝黑板数据内容，而忽略例如BuffNodeDataDic的数据内容）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NP_DataSupportor GetNP_TreeData_DeepCopyBBValuesOnly(long id)
        {
            NP_DataSupportor result = new NP_DataSupportor();
            if (this.m_NpRuntimeTreesDatas.ContainsKey(id))
            {
                result.BuffNodeDataDic = this.m_NpRuntimeTreesDatas[id].BuffNodeDataDic;
                result.NpDataSupportorBase = this.m_NpRuntimeTreesDatas[id].NpDataSupportorBase.DeepCopy();
                return result;
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }
        
        /// <summary>
        /// 获取一棵树的所有数据（通过深拷贝形式）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NP_DataSupportor GetNP_TreeData_DeepCopy(long id)
        {
            if (this.m_NpRuntimeTreesDatas.ContainsKey(id))
            {
                return this.m_NpRuntimeTreesDatas[id].DeepCopy();
            }

            Log.Error($"请求的行为树id不存在，id为{id}");
            return null;
        }
    }
}