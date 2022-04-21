//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年7月25日 17:25:41
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NKGSlate.Runtime
{
    public class ST_Director
    {
        /// <summary>
        /// 一个导演只能导一部片子
        /// </summary>
        private ST_CutSceneData m_TargetCutSceneData;

        /// <summary>
        /// 每个导演自己管理一个CurrentFrame，用于处理顿帧的情况
        /// </summary>
        public uint CurrentFrame;
        
        private uint m_PreviousFrame;
        private uint m_MaxFrame;
        public bool HasPaused;
        public bool HasExited;
        public bool HasInited;

        /// <summary>
        /// 已排序的全部时间节点，之所以排序是因为前置触发可能会影响后续触发的条件
        /// </summary>
        private List<ST_ITimePointer> m_TimePoints;

        /// <summary>
        /// 未排序的开始时间节点，之所以不需要排序，是因为在其Tick之前，m_TimePoints已经处理了进入/退出时触发的逻辑
        /// 所以这里只要条件符合，就可以进行乱序Tick
        /// </summary>
        private List<ST_StartTimePoint> m_UnsortedTimePoints;

        /// <summary>
        /// 从头开始播放
        /// </summary>
        /// <param name="currentFrame"></param>
        public void BeginPlay(uint currentFrame, ST_CutSceneData targetCustSceneData)
        {
            m_TargetCutSceneData = targetCustSceneData;
            InitializeTimePointers(currentFrame);
            HasInited = true;
            HasPaused = false;
            HasExited = false;
        }

        /// <summary>
        /// 暂停播放
        /// </summary>
        public void Pause()
        {
            HasPaused = true;
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        public void Stop()
        {
            HasExited = true;
        }

        /// <summary>
        /// 从暂停恢复播放
        /// </summary>
        public void Resume()
        {
            HasPaused = false;
        }

        /// <summary>
        /// 对Group进行Tick
        /// </summary>
        /// <param name="currentFrame">当前帧数</param>
        public void Sample(uint currentFrame)
        {
            if (HasPaused || HasExited || !HasInited) return;

            Internal_SamplePointers(currentFrame, m_PreviousFrame);
            m_PreviousFrame = currentFrame;
        }


        private void InitializeTimePointers(uint currentFrame)
        {
            m_TimePoints = new List<ST_ITimePointer>();
            m_UnsortedTimePoints = new List<ST_StartTimePoint>();

            foreach (ST_GroupData stGroupData in m_TargetCutSceneData.GroupDatas.AsEnumerable().Reverse())
            {
                ST_Group stGroup = new ST_Group();

                if (stGroup.Initialize(currentFrame, stGroupData))
                {
                    var p1 = new ST_StartTimePoint(stGroup);
                    m_TimePoints.Add(p1);

                    foreach (ST_TrackData stTrackData in stGroupData.TrackDatas.AsEnumerable().Reverse())
                    {
                        ST_Track stTrack = new ST_Track();

                        if (stTrack.Initialize(currentFrame, stTrackData))
                        {
                            var p2 = new ST_StartTimePoint(stTrack);
                            m_TimePoints.Add(p2);

                            foreach (ST_ActionData stActionData in stTrackData.ActionDatas)
                            {
                                ST_IDirectable stAction =
                                    Activator.CreateInstance(SlateEntry.TypeInfos[stActionData.GetType()]) as
                                        ST_IDirectable;

                                if (stAction.Initialize(currentFrame, stActionData))
                                {
                                    var p3 = new ST_StartTimePoint(stAction);
                                    m_TimePoints.Add(p3);

                                    m_UnsortedTimePoints.Add(p3);
                                    m_TimePoints.Add(new ST_EndTimePoint(stAction));
                                }
                            }

                            m_UnsortedTimePoints.Add(p2);
                            m_TimePoints.Add(new ST_EndTimePoint(stTrack));
                        }
                    }

                    m_UnsortedTimePoints.Add(p1);
                    m_TimePoints.Add(new ST_EndTimePoint(stGroup));
                }
            }

            m_TimePoints = m_TimePoints.OrderBy(p => p.TriggerFrame).ToList();
        }

        private void Internal_SamplePointers(uint currentTime, uint previousTime)
        {
            if (currentTime > previousTime)
            {
                foreach (var t in m_TimePoints)
                {
                    try
                    {
                        t.TriggerForward(currentTime, previousTime);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            foreach (var t in m_UnsortedTimePoints)
            {
                try
                {
                    t.Update(currentTime, previousTime);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}