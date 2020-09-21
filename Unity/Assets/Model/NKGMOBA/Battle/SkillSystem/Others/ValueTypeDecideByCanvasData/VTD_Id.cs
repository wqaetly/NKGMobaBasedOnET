//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月7日 20:53:05
//------------------------------------------------------------

using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace ETModel
{
    [HideReferenceObjectPicker]
    public struct VTD_Id
    {
        [LabelText("此节点ID在数据仓库中的Key")]
        [ValueDropdown("GetIdKey")]
        [OnValueChanged("ApplayId")]
        [BsonIgnore]
        public string IdKey;

        [LabelText("Id")]
        [InfoBox("无法对其直接赋值，需要在CanvasDataManager中Ids中注册键值对，然后选择NodeIdKey的值")]
        [ReadOnly]
        public long Value;

#if UNITY_EDITOR
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
                        return npBehaveCanvasDataManager.Ids.Keys;
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
                        if (npBehaveCanvasDataManager.Ids.TryGetValue(IdKey, out var targetId))
                        {
                            Value = targetId;
                        }
                    }
                }
            }
        }
#endif
    }
}