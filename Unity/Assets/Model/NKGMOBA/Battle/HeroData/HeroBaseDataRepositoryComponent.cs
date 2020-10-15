//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月27日 13:13:10
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;
#if !SERVER
using UnityEngine;    
#endif


namespace ETModel
{
    [ObjectSystem]
    public class AllHeroBaseDataComponentAwakeSystem: AwakeSystem<HeroBaseDataRepositoryComponent>
    {
        public override void Awake(HeroBaseDataRepositoryComponent self)
        {
            self.Awake();
        }
    }

    public class HeroBaseDataRepositoryComponent: Component
    {
        public HeroDataSupportor AllHeroBaseDataDic;

        public void Awake()
        {
#if SERVER
            byte[] mfile = File.ReadAllBytes("../Config/HeroBaseDatas/AllHeroDatas.bytes");
            this.AllHeroBaseDataDic = BsonSerializer.Deserialize<HeroDataSupportor>(mfile);
#else

            ResourcesComponent resourcesComponent = Game.Scene.GetComponent<ResourcesComponent>();
            GameObject heroDataConfigs = resourcesComponent.LoadAsset<GameObject>(ABPathUtilities.GetNormalConfigPath("HeroDataConfigs"));
            foreach (var referenceCollectorData in heroDataConfigs.GetComponent<ReferenceCollector>().data)
            {
                TextAsset textAsset = heroDataConfigs.GetTargetObjectFromRC<TextAsset>(referenceCollectorData.key);

                if (textAsset.bytes.Length == 0) Log.Info("没有读取到文件");

                this.AllHeroBaseDataDic = BsonSerializer.Deserialize<HeroDataSupportor>(textAsset.bytes);
            }
#endif
        }

        /// <summary>
        /// 根据id来获取指定英雄数据(通过深拷贝的形式获得，对数据的更改不会影响到原本的数据)
        /// </summary>
        /// <param name="id"></param>
        public NodeDataForHero GetHeroDataById_DeepCopy(long id)
        {
            if (this.AllHeroBaseDataDic.MHeroDataSupportorDic.ContainsKey(id))
            {
                //Log.Info("序列化深拷贝");
                return this.AllHeroBaseDataDic.MHeroDataSupportorDic[id].DeepCopy();
            }

            //Log.Error($"查询英雄基础数据失败,id为{id}");
            return null;
        }

        /// <summary>
        /// 根据id来获取指定英雄数据(直接获得，请确保不会对数据进行修改！不然会导致未知错误)
        /// </summary>
        /// <param name="id"></param>
        public NodeDataForHero GetHeroDataById_Normal(long id)
        {
            if (this.AllHeroBaseDataDic.MHeroDataSupportorDic.ContainsKey(id))
            {
                return this.AllHeroBaseDataDic.MHeroDataSupportorDic[id];
            }

            return null;
        }
    }
}