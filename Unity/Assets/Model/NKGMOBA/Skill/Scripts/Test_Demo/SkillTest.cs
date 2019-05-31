using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

public class SkillTest : MonoBehaviour
{
    #region 技能按钮和Log信息

    public Button Q;
    public Button W;
    public Button E;
    public Button R;

    public Text LogInfos;

    #endregion

    /// <summary>
    /// 技能文件目录（二进制）
    /// </summary>
    private const string SkillBytesPath = "Assets/SkillDemo/SkillBytes/XiaoPaoSkills.bytes";


    private NodeDataSupporter m_XiaoPaoSkills = new NodeDataSupporter();


    private void Awake()
    {
        Type[] types = typeof(SkillTest).Assembly.GetTypes();
        foreach (Type type in types)
        {
            if (!type.IsSubclassOf(typeof(BaseNodeData)) && !type.IsSubclassOf(typeof(SkillBuffBase)))
            {
                continue;
            }

            BsonClassMap.LookupClassMap(type);
        }
    }

    void Start()
    {
        byte[] mfile = File.ReadAllBytes("Assets/SkillDemo/SkillBytes/XiaoPaoSkills.bytes");

        if (mfile.Length == 0) Debug.Log("没有读取到文件");

        m_XiaoPaoSkills = BsonSerializer.Deserialize<NodeDataSupporter>(mfile);

        Q.onClick.AddListener(PressQ);
        W.onClick.AddListener(PressW);
        E.onClick.AddListener(PressE);
        R.onClick.AddListener(PressR);
        
        AddInfoToLog("被动技能名称为:" + ((NodeDataForStartSkill) (m_XiaoPaoSkills.GetNodeById(0).NodeDataInnerDic[0]))
                     .SkillName);
        foreach (KeyValuePair<int, BaseNodeData> temp in m_XiaoPaoSkills.GetNodeById(0).NodeDataInnerDic)
        {
            AddInfoToLog($"ID为{temp.Value.NodeID}的结点名称为{temp.Value.GetType().Name}");
        }
    }

    private void PressQ()
    {
        AddInfoToLog("按下了Q技能");
        AddInfoToLog("Q技能名称为:" + ((NodeDataForStartSkill) (m_XiaoPaoSkills.GetNodeById(1).NodeDataInnerDic[0]))
                     .SkillName);
        foreach (KeyValuePair<int, BaseNodeData> temp in m_XiaoPaoSkills.GetNodeById(1).NodeDataInnerDic)
        {
            AddInfoToLog($"ID为{temp.Value.NodeID}的结点名称为{temp.Value.GetType().Name}");
        }
    }

    private void PressW()
    {
        AddInfoToLog("按下了W技能");
        AddInfoToLog("W技能名称为:" + ((NodeDataForStartSkill) (m_XiaoPaoSkills.GetNodeById(2).NodeDataInnerDic[0]))
                     .SkillName);
        foreach (KeyValuePair<int, BaseNodeData> temp in m_XiaoPaoSkills.GetNodeById(2).NodeDataInnerDic)
        {
            AddInfoToLog($"ID为{temp.Value.NodeID}的结点名称为{temp.Value.GetType().Name}");
        }
    }

    private void PressE()
    {
        AddInfoToLog("按下了E技能");
        AddInfoToLog("E技能名称为:" + ((NodeDataForStartSkill) (m_XiaoPaoSkills.GetNodeById(3).NodeDataInnerDic[0]))
                     .SkillName);
        foreach (KeyValuePair<int, BaseNodeData> temp in m_XiaoPaoSkills.GetNodeById(3).NodeDataInnerDic)
        {
            AddInfoToLog($"ID为{temp.Value.NodeID}的结点名称为{temp.Value.GetType().Name}");
        }
    }

    private void PressR()
    {
        AddInfoToLog("按下了R技能");
        AddInfoToLog("R技能名称为:" + ((NodeDataForStartSkill) (m_XiaoPaoSkills.GetNodeById(4).NodeDataInnerDic[0]))
                     .SkillName);
        foreach (KeyValuePair<int, BaseNodeData> temp in m_XiaoPaoSkills.GetNodeById(4).NodeDataInnerDic)
        {
            AddInfoToLog($"ID为{temp.Value.NodeID}的结点名称为{temp.Value.GetType().Name}");
        }
    }


    /// <summary>
    /// 添加信息到log
    /// </summary>
    private void AddInfoToLog(string info)
    {
        LogInfos.text += $"{info}\n\n";
    }
}