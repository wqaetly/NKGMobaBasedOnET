//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年7月25日 13:09:27
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MonKey;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace ET
{
    public class LSF_NetInfoWindow : OdinEditorWindow
    {
        [BoxGroup("客户端网络状态")] [LabelText("C2GPing值（ms）")]
        public float C2GPing;

        [BoxGroup("客户端网络状态")] [LabelText("C2MPing值（ms）")]
        public float C2MPing;

        [BoxGroup("客户端网络状态")] [LabelText("客户端当前FixedUpdate间隔（ms）")]
        public long ClientFixedUpdateInternal;

        [BoxGroup("客户端网络状态")] [LabelText("客户端当前FixedUpdate帧数（uint）")]
        public float ClientFixedUpdateFrame;

        [BoxGroup("客户端网络状态")] [LabelText("客户端应当超前服务器的帧数（uint）")]
        public int ShouldAdvancedFrame;

        [BoxGroup("客户端网络状态")] [LabelText("客户端当前超前服务器的帧数（uint）")]
        public int CurrentAdvancedFrame;

        [BoxGroup("客户端网络状态")] [LabelText("客户端当前用户输入缓冲区")] [InfoBox("客户端当前用户输入缓冲区")]
        public Dictionary<uint, Queue<ALSF_Cmd>> CurrentPlayerInputBuffer;

        [BoxGroup("网络同步状态")]
        [LabelText("客户端当前帧（uint）")]
        [ProgressBar(nameof(GetCompareMinFrame), nameof(GetCompareMaxFrame), Segmented = true, Height = 30,
            DrawValueLabel = true)]
        public uint ClientCurrentFrame;

        [BoxGroup("网络同步状态")]
        [LabelText("服务端当前帧（uint）")]
        [ProgressBar(nameof(GetCompareMinFrame), nameof(GetCompareMaxFrame), Segmented = true, Height = 30,
            DrawValueLabel = true)]
        public uint ServerCurrentFrame;

        private uint GetCompareMinFrame()
        {
            return (uint) Mathf.Clamp(ServerCurrentFrame - 50, 0.0f, Single.NaN);
        }

        private uint GetCompareMaxFrame()
        {
            return ServerCurrentFrame + 50;
        }

        [Command("ETEditor_LSF_NetInfoWindow", "监测状态帧同步网络情况", Category = "ETEditor")]
        private static void OpenWindow()
        {
            var window = GetWindow<LSF_NetInfoWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(600, 600);
            window.titleContent = new GUIContent("监测状态帧同步网络情况");
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                ShowNotification(new GUIContent("请运行游戏并进入战斗以查看状态帧同步状况"));
                return;
            }


            LSF_Component lsfComponent =
                Game.Scene.GetComponent<PlayerComponent>()?.BelongToRoom?.GetComponent<LSF_Component>();
            if (lsfComponent != null)
            {
                if (lsfComponent.FixedUpdate != null)
                {
                    this.ShouldAdvancedFrame = lsfComponent.TargetAheadOfFrame;
                    this.CurrentAdvancedFrame = lsfComponent.CurrentAheadOfFrame;
                    this.ClientFixedUpdateFrame = 1000.0f / lsfComponent.FixedUpdate.TargetElapsedTime.Milliseconds;
                    this.ClientFixedUpdateInternal = lsfComponent.FixedUpdate.TargetElapsedTime.Milliseconds;
                    this.ClientCurrentFrame = lsfComponent.CurrentFrame;
                    this.ServerCurrentFrame = lsfComponent.ServerCurrentFrame;
                    this.CurrentPlayerInputBuffer = lsfComponent.PlayerInputCmdsBuffer;
                }
            }

            PingComponent pingComponent =
                Game.Scene.GetComponent<PlayerComponent>()?.GateSession?.GetComponent<PingComponent>();
            if (pingComponent != null)
            {
                C2GPing = pingComponent.C2GPingValue;
                C2MPing = pingComponent.C2MPingValue;
            }

            Repaint();
        }
    }
}