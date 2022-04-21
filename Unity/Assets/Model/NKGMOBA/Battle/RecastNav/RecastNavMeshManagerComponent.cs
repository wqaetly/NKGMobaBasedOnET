using System;
using System.Collections.Generic;

namespace ET
{
    public class RecastNavMeshManagerComponent : Entity
    {
        public static RecastNavMeshManagerComponent Instance;

        public const string ServerNavDataPath = "../Config/RecastNavData";
        
        // 所有的Nav数据，意为数据仓库
        public Dictionary<string, byte[]> AllNavData = new Dictionary<string, byte[]>();

        // 所有实例化出来的Nav数据，为寻路数据实例
        public Dictionary<string, IntPtr> Navmeshs = new Dictionary<string, IntPtr>();
    }
}