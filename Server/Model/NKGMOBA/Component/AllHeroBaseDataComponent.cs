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
    public class AllHeroBaseDataComponentAwakeSystem: AwakeSystem<AllHeroBaseDataComponent>
    {
        public override void Awake(AllHeroBaseDataComponent self)
        {
            self.Awake();
        }
    }

    public class AllHeroBaseDataComponent: Component
    {
        public NodeDataSupporter m_AllHeroBaseDataDic;

        public void Awake()
        {
            Type[] types = typeof (AllHeroBaseDataComponent).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (BaseNodeData)) && !type.IsSubclassOf(typeof (SkillBuffBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }
            
            byte[] mfile = File.ReadAllBytes("../Config/HeroBaseDatas/AllHeroDatas.bytes");
            Console.WriteLine($"所读取的英雄属性大小为:{mfile.Length}");
            this.m_AllHeroBaseDataDic = BsonSerializer.Deserialize<NodeDataSupporter>(mfile);
        }
    }
}