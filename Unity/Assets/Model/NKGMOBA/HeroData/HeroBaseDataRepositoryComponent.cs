//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月27日 13:13:10
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;

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
        public HeroDataSupportor m_AllHeroBaseDataDic;

        public void Awake()
        {
            byte[] mfile = File.ReadAllBytes("../Config/HeroBaseDatas/AllHeroDatas.bytes");
            Log.Info($"所读取的英雄属性大小为:{mfile.Length}");
            this.m_AllHeroBaseDataDic = BsonSerializer.Deserialize<HeroDataSupportor>(mfile);
        }

        /// <summary>
        /// 根据id来获取指定英雄数据
        /// </summary>
        /// <param name="id"></param>
        public NodeDataForHero GetHeroDataById(long id)
        {
            if (this.m_AllHeroBaseDataDic.MHeroDataSupportorDic.ContainsKey(id))
            {
                //Log.Info("序列化深拷贝");
                return this.m_AllHeroBaseDataDic.MHeroDataSupportorDic[id].DeepCopy();
            }
            //Log.Error($"查询英雄基础数据失败,id为{id}");
            return null;
        }
    }
}