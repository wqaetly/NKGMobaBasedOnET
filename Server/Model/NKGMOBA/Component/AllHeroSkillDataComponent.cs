//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月27日 14:41:15
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;

namespace ETModel
{
    [ObjectSystem]
    public class AllHeroSkillDataComponentAwakeSystem: AwakeSystem<AllHeroSkillDataComponent>
    {
        public override void Awake(AllHeroSkillDataComponent self)
        {
            self.Awake();
        }
    }

    public class AllHeroSkillDataComponent: Component
    {
        public Dictionary<int, NodeDataSupporter> m_AllHeroSkillDataDic = new Dictionary<int, NodeDataSupporter>();

        public NodeDataSupporter TempDataSupporter;

        public void Awake()
        {
            Type[] types = typeof (AllHeroSkillDataComponent).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (BaseNodeData)) && !type.IsSubclassOf(typeof (SkillBuffBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            string[] m_SkillInfos;
            m_SkillInfos = Directory.GetFiles("../Config/HeroSkillDatas/");
            foreach (var VARIABLE in m_SkillInfos)
            {
                this.TempDataSupporter = new NodeDataSupporter();
                byte[] mfile = File.ReadAllBytes(VARIABLE);
                Console.WriteLine($"所读取的英雄技能大小为:{mfile.Length}");
                this.TempDataSupporter = BsonSerializer.Deserialize<NodeDataSupporter>(mfile);
                this.m_AllHeroSkillDataDic.Add(this.TempDataSupporter.ID, TempDataSupporter);
            }
        }
    }
}