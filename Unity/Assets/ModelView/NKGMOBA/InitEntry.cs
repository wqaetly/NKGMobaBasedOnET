using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class InitEntry
    {
        public static void RegFunction()
        {
            Log.Info("初始化RegFunction");
            GlobalLifeCycle.StartAction = Start;
            GlobalLifeCycle.UpdateAction = Game.Update;
            GlobalLifeCycle.FixedUpdateAction = Game.FixedUpdate;
            GlobalLifeCycle.LateUpdateAction = Game.LateUpdate;
            GlobalLifeCycle.FrameFinishAction = Game.FrameFinish;
            GlobalLifeCycle.OnApplicationQuitAction = Game.Close;
        }

        public static void Start()
        {
            try
            {
                Log.Info("初始化EventSystem");
                {
                    List<Type> types = new List<Type>();
                    types.AddRange(HotfixHelper.GetAssemblyTypes());
                    Game.EventSystem.AddRangeType(types);
                    if (GlobalDefine.ILRuntimeMode)
                    {
                        Game.EventSystem.TypeIlrInit();
                    }
                    else
                    {
                        Game.EventSystem.TypeMonoInit();
                    }

                    Game.EventSystem.EventSystemInit();
                }
                
                ProtobufHelper.Init();
                MongoHelper.Init();

                GlobalDefine.Options = new Options();

                Game.EventSystem.Publish(new EventType.AppStart()).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}