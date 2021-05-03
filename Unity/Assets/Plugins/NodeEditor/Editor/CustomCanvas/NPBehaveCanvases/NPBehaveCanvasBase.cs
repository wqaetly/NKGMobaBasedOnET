//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月9日 20:02:37
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ETModel;
using ETModel.BBValues;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NodeEditorFramework;
using Plugins.NodeEditor.Editor.NPBehaveNodes;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Plugins.NodeEditor.Editor.Canvas
{
    /// <summary>
    /// 行为树基类，继承此类即可自行拓展基于行为树的逻辑图
    /// 必须实现的有以下几点
    /// 1.继承NP_DataSupportorBase的数据基类
    /// 2.自动配置所有结点数据：AddAllNodeData
    /// 3.保存行为树信息为二进制文件：Save
    /// 4.自定义的的额外数据块配置
    /// 需要注意的点
    /// 要在NPBehaveNodes文件夹下面的除了NP_NodeBase之外的所有Node的Node特性的type里加上自定义的Canvas的Type，不然创建不了行为树组件
    /// 推荐的按钮样式：[Button("XXX", 25), GUIColor(0.4f, 0.8f, 1)]
    /// ----------------------------------------数据导出相关-----------------------------------------------------------------
    /// 最后在进行数据导出的时候，会涉及到一个数据的Id问题，每次都需要手动将Id记录到Excel中，为了简化这个操作，
    /// 先尝试从根据提供的ConfigType（A）和IdInConfig（B）从配置表读取一个Id为B的数据行
    /// 然后以这个数据行的NPBehaveId作为导出Id，这样每次导出数据就不需要去修改Excel那边了，如果没有A类型的配置表，或者没有Id为B的数据行，就随机生成一个Id，然后需要手动填入Excel中
    /// </summary>
    public abstract class NPBehaveCanvasBase: NodeCanvas
    {
        public override string canvasName => Name;

        [BoxGroup("本Canvas所有数据整理部分")]
        [LabelText("保存文件名"), GUIColor(0.9f, 0.7f, 1)]
        public string Name;

        [BoxGroup("本Canvas所有数据整理部分")]
        [LabelText("对应的配置表"), GUIColor(0.9f, 0.7f, 1)]
        public TextAsset Config;

        [BoxGroup("本Canvas所有数据整理部分")]
        [LabelText("对应的配置表类型"), GUIColor(0.9f, 0.7f, 1)]
        public Type ConfigType;

        [BoxGroup("本Canvas所有数据整理部分")]
        [LabelText("配置表中的Id"), GUIColor(0.9f, 0.7f, 1)]
        public int IdInConfig;

        [BoxGroup("本Canvas所有数据整理部分")]
        [LabelText("保存路径"), GUIColor(0.1f, 0.7f, 1)]
        [FolderPath]
        public string SavePath;

        /// <summary>
        /// 黑板数据管理器
        /// </summary>
        [HideInInspector]
        public NPBehaveCanvasDataManager npBehaveCanvasDataManager;

        public NPBehaveCanvasDataManager GetCurrentCanvasDatas()
        {
            if (this.npBehaveCanvasDataManager == null)
            {
                Object[] subAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(this.savePath);
                foreach (var subAsset in subAssets)
                {
                    if (subAsset is NPBehaveCanvasDataManager npBbDataManager)
                    {
                        return npBbDataManager;
                    }
                }

                this.npBehaveCanvasDataManager = CreateInstance<NPBehaveCanvasDataManager>();
                //Log.Info("新建数据仓库");
                this.npBehaveCanvasDataManager.name = "当前Canvas数据库";
                NodeEditorSaveManager.AddSubAsset(this.npBehaveCanvasDataManager, this);
            }

            return this.npBehaveCanvasDataManager;
        }

        /// <summary>
        /// 自动配置当前图所有数据（结点，黑板）
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        public virtual void AutoSetCanvasDatas(NP_DataSupportorBase npDataSupportorBase)
        {
            this.AutoSetNP_NodeData(npDataSupportorBase);
            this.AutoSetNP_BBDatas(npDataSupportorBase);
        }

        /// <summary>
        /// 自动配置所有行为树结点
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        private void AutoSetNP_NodeData(NP_DataSupportorBase npDataSupportorBase)
        {
            npDataSupportorBase.NPBehaveTreeDataId = 0;
            npDataSupportorBase.NP_DataSupportorDic.Clear();

            //当前Canvas所有NP_Node
            List<NP_NodeBase> allNodes = new List<NP_NodeBase>();

            foreach (var node in this.nodes)
            {
                if (node is NP_NodeBase mNode)
                {
                    allNodes.Add(mNode);
                }
            }

            //排序
            allNodes.Sort((x, y) => -x.position.y.CompareTo(y.position.y));

            //配置每个节点Id
            foreach (var node in allNodes)
            {
                node.NP_GetNodeData().id = IdGenerater.GenerateId();
            }
            
            //设置导出的Id
            foreach (string str in this.Config.text.Split(new[] { "\n" }, StringSplitOptions.None))
            {
                try
                {
                    string str2 = str.Trim();
                    if (str2 == "")
                    {
                        continue;
                    }

                    object config = JsonHelper.FromJson(this.ConfigType, str2);
                    //目前行为树只有三种类型，直接在这里写出
                    switch (config)
                    {
                        case Server_AICanvasConfig serverAICanvasConfig:
                            if (serverAICanvasConfig.Id == this.IdInConfig)
                            {
                                npDataSupportorBase.NPBehaveTreeDataId = serverAICanvasConfig.NPBehaveId;
                            }
                            break;
                        case Client_SkillCanvasConfig clientSkillCanvasConfig:
                            if (clientSkillCanvasConfig.Id == this.IdInConfig)
                            {
                                npDataSupportorBase.NPBehaveTreeDataId = clientSkillCanvasConfig.NPBehaveId;
                            }
                            break;
                        case Server_SkillCanvasConfig serverSkillCanvasConfig:
                            if (serverSkillCanvasConfig.Id == this.IdInConfig)
                            {
                                npDataSupportorBase.NPBehaveTreeDataId = serverSkillCanvasConfig.NPBehaveId;
                            }
                            break;
                    }

                    if (npDataSupportorBase.NPBehaveTreeDataId != 0)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"parser json fail: {str}", e);
                }
            }

            if (npDataSupportorBase.NPBehaveTreeDataId == 0)
            {
                //设置为根结点Id
                npDataSupportorBase.NPBehaveTreeDataId = allNodes[allNodes.Count - 1].NP_GetNodeData().id;
                Log.Error($"注意，名为{this.canvasName}的Canavs首次导出，或者未在配置表中找到Id为{this.IdInConfig}的数据行，行为树Id被设置为{npDataSupportorBase.NPBehaveTreeDataId}，请前往Excel表中进行添加，然后导出Excel");
            }
            else
            {
                allNodes[allNodes.Count - 1].NP_GetNodeData().id = npDataSupportorBase.NPBehaveTreeDataId;
            }
            
            foreach (var node in allNodes)
            {
                //获取结点对应的NPData
                NP_NodeDataBase mNodeData = node.NP_GetNodeData();
                if (mNodeData.LinkedIds == null)
                {
                    mNodeData.LinkedIds = new List<long>();
                }

                mNodeData.LinkedIds.Clear();

                //出结点连接的Nodes
                List<NP_NodeBase> theNodesConnectedToOutNode = new List<NP_NodeBase>();

                List<ValueConnectionKnob> valueConnectionKnobs = node.GetNextNodes()?.connections;

                if (valueConnectionKnobs != null)
                {
                    foreach (var valueConnectionKnob in valueConnectionKnobs)
                    {
                        theNodesConnectedToOutNode.Add((NP_NodeBase) valueConnectionKnob.body);
                    }

                    //对所连接的节点们进行排序
                    theNodesConnectedToOutNode.Sort((x, y) => x.position.x.CompareTo(y.position.x));

                    //配置连接的Id，运行时实时构建行为树
                    foreach (var npNodeBase in theNodesConnectedToOutNode)
                    {
                        mNodeData.LinkedIds.Add(npNodeBase.NP_GetNodeData().id);
                    }
                }

                //将此结点数据写入字典
                npDataSupportorBase.NP_DataSupportorDic.Add(mNodeData.id, mNodeData);
            }
        }

        /// <summary>
        /// 自动配置黑板数据
        /// </summary>
        /// <param name="npDataSupportorBase">自定义的继承于NP_DataSupportorBase的数据体</param>
        private void AutoSetNP_BBDatas(NP_DataSupportorBase npDataSupportorBase)
        {
            npDataSupportorBase.NP_BBValueManager.Clear();
            //设置黑板数据
            foreach (var bbvalues in this.GetCurrentCanvasDatas().BBValues)
            {
                npDataSupportorBase.NP_BBValueManager.Add(bbvalues.Key, bbvalues.Value);
            }
        }

        public override void DrawToolbar()
        {
            GUI.backgroundColor = new Color(1, 0.3f, 0.3f, 1);

            if (GUILayout.Button("DataBase", NodeEditorGUI.toolbarButton, GUILayout.Width(100)))
            {
                Selection.activeObject = this.GetCurrentCanvasDatas();
            }

            GUI.backgroundColor = Color.white;
        }
    }
}