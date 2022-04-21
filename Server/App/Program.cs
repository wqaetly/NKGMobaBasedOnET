using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using CommandLine;
using NLog;

namespace ET
{
    internal static class Program
    {
        private static FixedUpdate m_FixedUpdate;

        // 常用于多媒体定时器中，与GetTickCount类似，也是返回操作系统启动到现在所经过的毫秒数，精度为1毫秒。
        [DllImport("winmm")]
        static extern uint timeGetTime();

        // 一般默认的精度不止1毫秒（不同操作系统有所不同），需要调用timeBeginPeriod与timeEndPeriod来设置精度
        [DllImport("winmm")]
        static extern void timeBeginPeriod(int t);

        [DllImport("winmm")]
        static extern void timeEndPeriod(int t);
        
        private static void Main(string[] args)
        {
            // 用法
            timeBeginPeriod(1);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

            try
            {
                //初始化EventSystem
                {
                    List<Type> types = new List<Type>();
                    types.AddRange(typeof(Game).Assembly.GetTypes());
                    types.AddRange(DllHelper.GetHotfixAssembly().GetTypes());
                    Game.EventSystem.AddRangeType(types);
                    Game.EventSystem.TypeMonoInit();
                    Game.EventSystem.EventSystemInit();
                }

                ProtobufHelper.Init();
                MongoHelper.Init();

                // 命令行参数
                Options options = null;
                Parser.Default.ParseArguments<Options>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
                    .WithParsed(o => { options = o; });

                GlobalDefine.Options = options;

                // 如果是正式模式，就重新配置下模式和NLog
                if (options.Develop == 0)
                {
                    GlobalDefine.DevelopMode = false;
                    GlobalDefine.ILog = new NLogger(GlobalDefine.Options.AppType.ToString());
                }
                else
                {
                    GlobalDefine.ILog = new NLogger("ServerDevelop");
                }

                LogManager.Configuration.Variables["appIdFormat"] = $"{GlobalDefine.Options.Process:000000}";

                Log.Info($"server start........................ {Game.Scene.Id}");

                Game.EventSystem.Publish(new EventType.AppStart());

                // 注册FixedUpdate，注意必须是在这实例化，否则会因为初始化时间与下一个Tick时间间隔过长而产生追帧操作，造成大量的重复执行后果
                m_FixedUpdate = new FixedUpdate() {UpdateCallback = Game.FixedUpdate};

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);
                        m_FixedUpdate.Tick();
                        Game.Update();
                        Game.LateUpdate();
                        Game.FrameFinish();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }

            timeEndPeriod(1);
        }
    }
}