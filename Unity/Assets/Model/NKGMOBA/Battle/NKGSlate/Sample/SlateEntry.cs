//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月12日 21:10:15
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.Serialization;
using UnityEngine;

namespace NKGSlate.Runtime
{
    public class SlateEntry : MonoBehaviour
    {
        public static Dictionary<Type, Type> TypeInfos = new Dictionary<Type, Type>()
        {
            {typeof(ST_LogActionData), typeof(ST_LogAction)},
            {typeof(ST_EventData), typeof(ST_Event)},
        };

        public ST_Director Director = new ST_Director();
        public ST_CutSceneData CutSceneData;

        private ST_CutSceneData DeserializeFromFile()
        {
            ST_CutSceneData sceneData =
                SerializationUtility.DeserializeValue<ST_CutSceneData>(
                    File.ReadAllBytes($"Assets/NKGSlate/Sample/Sample.bytes"), DataFormat.Binary);
            return sceneData;
        }

        private void Awake()
        {
            CutSceneData = DeserializeFromFile();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Director.HasPaused)
                    Director.Resume();
                else
                {
                    Director.Pause();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Director.BeginPlay(Director.CurrentFrame, CutSceneData);
            }
        }

        private void FixedUpdate()
        {
            if (!Director.HasPaused && Director.HasInited)
            {
                Director.CurrentFrame += 1;
            }

            Director.Sample(Director.CurrentFrame);
        }
    }
}