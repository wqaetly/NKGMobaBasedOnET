//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月5日 19:50:32
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ETModel;
using MongoDB.Bson.Serialization;

namespace ETModel
{
    [ObjectSystem]
    public class B2S_CollisionRelationRepositoryComponentAwakeSystem: AwakeSystem<B2S_CollisionRelationRepositoryComponent>
    {
        public override void Awake(B2S_CollisionRelationRepositoryComponent self)
        {
            self.Awake();
        }
    }

    public class B2S_CollisionRelationRepositoryComponent: Component
    {
        /// <summary>
        /// 碰撞关系数据载体
        /// </summary>
        public Dictionary<long, B2S_CollisionsRelationSupport> m_B2S_CollisionsRelationSupportDic =
                new Dictionary<long, B2S_CollisionsRelationSupport>();

        /// <summary>
        /// 数据所处路径
        /// </summary>
        private string collisionRelationsDataPath = "../Config/CollisionRelations/";

        public void Awake()
        {
            this.ReadcollisionRelationsData();
        }

        private void ReadcollisionRelationsData()
        {
            string[] filePaths = Directory.GetFiles(this.collisionRelationsDataPath);
            foreach (var VARIABLE in filePaths)
            {
                byte[] mfile0 = File.ReadAllBytes(VARIABLE);

                if (mfile0.Length > 0)
                {
                    B2S_CollisionsRelationSupport temp = BsonSerializer.Deserialize<B2S_CollisionsRelationSupport>(mfile0);
                    this.m_B2S_CollisionsRelationSupportDic.Add(temp.SupportId, temp);
                    //Log.Info($"加载碰撞关系数据成功，ID为{temp.SupportId}");
                }
            }
        }

        /// <summary>
        /// 通过ID获取碰撞关系数据载体
        /// </summary>
        /// <returns></returns>
        public B2S_CollisionsRelationSupport GetB2S_CollisionsRelationSupportById(long id)
        {
            if (this.m_B2S_CollisionsRelationSupportDic.ContainsKey(id))
                return m_B2S_CollisionsRelationSupportDic[id];
            else
            {
                Log.Error($"请求的碰撞关系载体不存在，id为{id}");
                return null;
            }
        }
    }
}