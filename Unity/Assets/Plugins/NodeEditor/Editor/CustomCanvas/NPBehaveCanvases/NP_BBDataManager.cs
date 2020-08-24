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

namespace Plugins.NodeEditor.Editor.Canvas
{
    /// <summary>
    /// 黑板数据管理器
    /// </summary>
    public class NP_BBDataManager: SerializedScriptableObject
    {
        [InfoBox("这是这个NPBehaveCanvas的所有黑板数据\n键为string，值为NP_BBValue子类\n如果要添加新的黑板数据类型，请参照BBValues文件夹下的定义")]
        [Title("黑板数据", TitleAlignment = TitleAlignments.Centered)]
        [LabelText("内容")]
        [BoxGroup]
        [DictionaryDrawerSettings(KeyLabel = "键(string)", ValueLabel = "值(NP_BBValue)", DisplayMode = DictionaryDisplayOptions.CollapsedFoldout)]
        [OnValueChanged("IncreaseVersion",true)]
        public Dictionary<string, ANP_BBValue> BBValues = new Dictionary<string, ANP_BBValue>();

        [HideInInspector]
        public NPBehaveCanvasBase NpBehaveCanvasBase;

        /// <summary>
        /// 版本号，数据同步时以高版本号为准
        /// </summary>
        [HideInInspector]
        public int Version;
        
        public void IncreaseVersion()
        {
            Version++;
        }
    }
}