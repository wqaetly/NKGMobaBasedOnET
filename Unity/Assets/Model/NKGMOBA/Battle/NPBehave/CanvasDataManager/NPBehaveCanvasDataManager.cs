//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年8月23日 17:36:42
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ETModel;
using ETModel.BBValues;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace ETModel
{
#if UNITY_EDITOR
    //TODO 我们是不是可以把所有数据都做到这里，实现编辑时逻辑数据分离，也就可以做到Excel和节点编辑器数据互相导出
    /// <summary>
    /// 单个Canvas数据管理器，包括但不限于黑板数据，常用字符串数据等
    /// </summary>
    public class NPBehaveCanvasDataManager: SerializedScriptableObject
    {
        [InfoBox("这是这个NPBehaveCanvas的所有黑板数据\n键为string，值为NP_BBValue子类\n如果要添加新的黑板数据类型，请参照BBValues文件夹下的定义")]
        [Title("黑板数据", TitleAlignment = TitleAlignments.Centered)]
        [LabelText("黑板内容")]
        [BoxGroup]
        [DictionaryDrawerSettings(KeyLabel = "键(string)", ValueLabel = "值(NP_BBValue)", DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
        public Dictionary<string, ANP_BBValue> BBValues = new Dictionary<string, ANP_BBValue>();
        
        [InfoBox("这是这个NPBehaveCanvas的所有事件数据")]
        [Title("事件名", TitleAlignment = TitleAlignments.Centered)]
        [LabelText("事件内容")]
        [BoxGroup]
        public List<string> EventValues = new List<string>();
        
        [InfoBox("这是这个NPBehaveCanvas的所有ID相关的映射数据，key为ID描述，value为Id的值")]
        [Title("Id描述映射", TitleAlignment = TitleAlignments.Centered)]
        [LabelText("Id描述与值")]
        [BoxGroup]
        [DictionaryDrawerSettings(KeyLabel = "键(string)", ValueLabel = "值(long)", DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
        public Dictionary<string,long> Ids = new Dictionary<string, long>();
    }
#endif
}