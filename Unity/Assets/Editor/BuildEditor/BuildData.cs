//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月24日 23:10:30
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;

namespace ETEditor
{
    public class BuildData:Object
    {
        public int VersionInfo;
        
        public List<string> IndependentBundleAndAtlas = new List<string>();

        public List<string> BundleAndAtlasWithoutShare = new List<string>();
    }
}