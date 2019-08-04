//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年7月20日 21:22:16
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;

namespace ETModel
{
    [ObjectSystem]
    public class B2S_ColliderDataRepositoryComponentAwakeSystem: AwakeSystem<B2S_ColliderDataRepositoryComponent>
    {
        public override void Awake(B2S_ColliderDataRepositoryComponent self)
        {
            self.Awake();
        }
    }

    /// <summary>
    /// 碰撞体数据仓库，从二进制文件读取数据
    /// </summary>
    public class B2S_ColliderDataRepositoryComponent: Component
    {
        private string colliderDataPath = "../Config/ColliderDatas/";
        private List<string> colliderDataName = new List<string>() { "BoxColliderData", "CircleColliderData", "PolygonColliderData" };
        public ColliderDataSupporter BoxColliderDatas = new ColliderDataSupporter();
        public ColliderDataSupporter CircleColliderDatas = new ColliderDataSupporter();
        public ColliderDataSupporter PolygonColliderDatas = new ColliderDataSupporter();

        public void Awake()
        {
            this.ReadcolliderData();
        }

        /// <summary>
        /// 读取所有碰撞数据
        /// </summary>
        private void ReadcolliderData()
        {
            Type[] types = typeof (ColliderDataSupporter).Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (!type.IsSubclassOf(typeof (B2S_ColliderDataStructureBase)))
                {
                    continue;
                }

                BsonClassMap.LookupClassMap(type);
            }

            if (File.Exists($"{this.colliderDataPath}/{this.colliderDataName[0]}.bytes"))
            {
                byte[] mfile0 = File.ReadAllBytes($"{this.colliderDataPath}/{this.colliderDataName[0]}.bytes");
                //这里不进行长度判断会报错，正在试图访问一个已经关闭的流，咱也不懂，咱也不敢问
                if (mfile0.Length > 0)
                    this.BoxColliderDatas =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile0);
                //Log.Info($"已经读取矩形数据，数据大小为{mfile0.Length}");
            }

            if (File.Exists($"{this.colliderDataPath}/{this.colliderDataName[1]}.bytes"))
            {
                byte[] mfile1 = File.ReadAllBytes($"{this.colliderDataPath}/{this.colliderDataName[1]}.bytes");
                if (mfile1.Length > 0)
                    this.CircleColliderDatas =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile1);
                //Log.Info($"已经读取圆形数据，数据大小为{mfile1.Length}");
            }

            if (File.Exists($"{this.colliderDataPath}/{this.colliderDataName[2]}.bytes"))
            {
                byte[] mfile2 = File.ReadAllBytes($"{this.colliderDataPath}/{this.colliderDataName[2]}.bytes");
                if (mfile2.Length > 0)
                {
                    this.PolygonColliderDatas =
                            BsonSerializer.Deserialize<ColliderDataSupporter>(mfile2);
                }

                //Log.Info($"已经读取多边形数据，数据大小为{mfile2.Length}");
            }
        }

        public B2S_ColliderDataStructureBase GetDataById(long id)
        {
            long flag = id / 10000;
            switch (flag)
            {
                case 1:
                    return this.BoxColliderDatas.colliderDataDic[id];
                case 2:
                    return this.CircleColliderDatas.colliderDataDic[id];
                case 3:
                    return this.PolygonColliderDatas.colliderDataDic[id];
            }

            Log.Error($"未找到碰撞体数据，所查找的ID：{id}");
            return null;
        }
    }
}