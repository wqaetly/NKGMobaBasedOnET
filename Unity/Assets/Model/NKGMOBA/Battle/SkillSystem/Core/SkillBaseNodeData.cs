//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月17日 20:57:44
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;       
#endif

namespace ETModel
{
    public class SkillBaseNodeData
    {
#if UNITY_EDITOR
        [LabelText("此节点ID在数据仓库中的Key")]
        [ValueDropdown("GetIdKey")]
        [OnValueChanged("ApplayId")]
        [BsonIgnore]
        public string NodeIdKey;
        
        private IEnumerable<string> GetIdKey()
        {
            string path = UnityEngine.PlayerPrefs.GetString("LastCanvasPath");

            UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(path);
            if (subAssets != null)
            {
                foreach (var subAsset in subAssets)
                {
                    if (subAsset is NPBehaveCanvasDataManager npBehaveCanvasDataManager)
                    {
                        return npBehaveCanvasDataManager.NodeIds.Keys;
                    }
                }
            }

            return null;
        }

        private void ApplayId()
        {
            string path = UnityEngine.PlayerPrefs.GetString("LastCanvasPath");

            UnityEngine.Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(path);
            if (subAssets != null)
            {
                foreach (var subAsset in subAssets)
                {
                    if (subAsset is NPBehaveCanvasDataManager npBehaveCanvasDataManager)
                    {
                        if (npBehaveCanvasDataManager.NodeIds.TryGetValue(NodeIdKey, out var targetId))
                        {
                            NodeID = targetId;
                        }
                    }
                }
            }
        }
#endif
        [LabelText("节点ID")]
        [ReadOnly]
        public long NodeID;
    }
}