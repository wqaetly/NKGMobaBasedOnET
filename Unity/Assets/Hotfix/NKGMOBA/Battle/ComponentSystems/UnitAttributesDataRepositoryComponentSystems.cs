using System.IO;
using MongoDB.Bson.Serialization;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class UnitAttributesDataRepositoryComponentAwakeSystem : AwakeSystem<UnitAttributesDataRepositoryComponent>
    {
        public override void Awake(UnitAttributesDataRepositoryComponent self)
        {
#if !SERVER
            //TODO 这里是所有英雄的属性数据，后续需要拓展小兵，野怪属性
            TextAsset textAsset =
                XAssetLoader.LoadAsset<TextAsset>(
                    XAssetPathUtilities.GetUnitAttributeConfigPath("AllHeroAttributesData"));

            if (textAsset.bytes.Length == 0) Log.Info("没有读取到文件");

            UnitAttributesDataSupportor unitAttributesDataSupportor =
                BsonSerializer.Deserialize<UnitAttributesDataSupportor>(textAsset.bytes);
            self.AllUnitAttributesBaseDataDic[unitAttributesDataSupportor.SupportId] = unitAttributesDataSupportor;
#else
            DirectoryInfo directoryInfo = new DirectoryInfo("../Config/UnitAttributesDatas/");
            foreach (var unitAttributesDataConfigFile in directoryInfo.GetFiles())
            {
                byte[] mfile = File.ReadAllBytes(unitAttributesDataConfigFile.FullName);
                UnitAttributesDataSupportor unitAttributesDataSupportor =
                    BsonSerializer.Deserialize<UnitAttributesDataSupportor>(mfile);
                self.AllUnitAttributesBaseDataDic[unitAttributesDataSupportor.SupportId] = unitAttributesDataSupportor;
            }
#endif
        }
    }


    public static class UnitAttributesDataRepositoryComponentSystems
    {
        /// <summary>
        /// 根据id来获取指定Unit属性数据(通过深拷贝的形式获得，对数据的更改不会影响到原本的数据)
        /// </summary>
        /// <param name="dataSupportId">数据载体Id</param>
        /// <param name="nodeDataId">数据载体中的节点Id</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetUnitAttributesDataById_DeepCopy<T>(this UnitAttributesDataRepositoryComponent self,
            long dataSupportId, long nodeDataId)
            where T : UnitAttributesNodeDataBase
        {
            if (self.AllUnitAttributesBaseDataDic.TryGetValue(dataSupportId, out var unitAttributesDataSupportor))
            {
                if (unitAttributesDataSupportor.UnitAttributesDataSupportorDic.TryGetValue(nodeDataId,
                    out var unitAttributesNodeDataBase))
                {
                    return unitAttributesNodeDataBase.DeepCopy() as T;
                }
            }
            
            Log.Error($"查询Unit属性数据失败，数据载体Id为{dataSupportId}，数据载体中的节点Id为{nodeDataId}");
            return null;
        }
    }
}