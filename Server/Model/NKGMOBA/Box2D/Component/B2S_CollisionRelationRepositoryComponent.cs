//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月5日 19:50:32
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using ETMode;
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
        public Dictionary<long, B2S_CollisionsRelationSupport> m_B2S_CollisionsRelationSupportDic;

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
                    this.BoxColliderDatas =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile0);
            }
        }
    }
}