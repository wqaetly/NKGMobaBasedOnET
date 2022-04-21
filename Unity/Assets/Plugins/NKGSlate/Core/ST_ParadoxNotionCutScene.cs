//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月15日 21:51:50
//------------------------------------------------------------

using System.IO;
using NKGSlate.Runtime;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Slate;

namespace NKGSlate
{
    public class ST_ParadoxNotionCutScene : Cutscene
    {
        /// <summary>
        /// 导出技能数据
        /// </summary>
        [Button("导出技能数据", 35), GUIColor(0.78f, 0.23f, 0.56f)]
        public void ExportSkillDatas()
        {
            ST_CutSceneData stCutSceneData = new ST_CutSceneData();
            foreach (var gCutsceneGroup in this.groups)
            {
                if (gCutsceneGroup is ST_ParadoxNotionGroup stParadoxNotionGroup)
                {
                    ST_GroupData stGroupData = new ST_GroupData();
                    foreach (var cutsceneTrack in stParadoxNotionGroup.tracks)
                    {
                        ST_TrackData stTrackData = new ST_TrackData();
                        foreach (var cutsceneTrackClip in cutsceneTrack.clips)
                        {
                            ST_AParadoxNotionSlateActionBase aParadoxNotionSlateActionBase =
                                cutsceneTrackClip as ST_AParadoxNotionSlateActionBase;
                            ST_DirectableData stActionData = aParadoxNotionSlateActionBase.BindingDate;
                            stActionData.RelativelyStartTime = (long) (aParadoxNotionSlateActionBase.startTime * 1000);
                            stActionData.RelativelyEndTime = (long) (aParadoxNotionSlateActionBase.endTime * 1000);
                            stTrackData.ActionDatas.Add(stActionData as ST_ActionData);
                        }

                        stGroupData.TrackDatas.Add(stTrackData);
                    }

                    stCutSceneData.GroupDatas.Add(stGroupData);
                }
            }


            using (FileStream file = File.Create($"Assets/NKGSlate/Sample/Sample.bytes"))
            {
                byte[] serResult = SerializationUtility.SerializeValue(stCutSceneData, DataFormat.Binary);
                file.Write(serResult, 0, serResult.Length);
            }
        }
    }
}