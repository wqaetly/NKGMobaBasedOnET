using System;
using System.Diagnostics;
using System.IO;
using System.Net;

#if SERVER
using NLog;
#endif

namespace ET
{
    public static class Log
    {
        private const int TraceLevel = 1;
        private const int DebugLevel = 2;
        private const int InfoLevel = 3;
        private const int WarningLevel = 4;

        static Log()
        {
#if !SERVER
            GlobalDefine.ILog = new UnityLogger();
#endif
        }

        public static bool CheckLogLevel(int level)
        {
            if (GlobalDefine.Options == null)
            {
                return true;
            }
            
            return GlobalDefine.Options.LogLevel <= level;
        }
        
        public static void Trace(string msg)
        {
            if (!CheckLogLevel(DebugLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            GlobalDefine.ILog.Trace($"{msg}\n{st}");
        }

        public static void Debug(string msg)
        {
            if (!CheckLogLevel(DebugLevel))
            {
                return;
            }
            GlobalDefine.ILog.Debug(msg);
        }

        public static void Info(string msg)
        {
            if (!CheckLogLevel(InfoLevel))
            {
                return;
            }
            GlobalDefine.ILog.Info(msg);
        }

        public static void TraceInfo(string msg)
        {
            if (!CheckLogLevel(InfoLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            GlobalDefine.ILog.Trace($"{msg}\n{st}");
        }

        public static void Warning(string msg)
        {
            if (!CheckLogLevel(WarningLevel))
            {
                return;
            }

            GlobalDefine.ILog.Warning(msg);
        }

        public static void Error(string msg)
        {
            StackTrace st = new StackTrace(1, true);
            GlobalDefine.ILog.Error($"{msg}\n{st}");
        }

        public static void Error(Exception e)
        {
            string str = e.ToString();
            GlobalDefine.ILog.Error(str);
        }

        public static void Trace(string message, params object[] args)
        {
            if (!CheckLogLevel(TraceLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            GlobalDefine.ILog.Trace($"{string.Format(message, args)}\n{st}");
        }

        public static void Warning(string message, params object[] args)
        {
            if (!CheckLogLevel(WarningLevel))
            {
                return;
            }
            GlobalDefine.ILog.Warning(string.Format(message, args));
        }

        public static void Info(string message, params object[] args)
        {
            if (!CheckLogLevel(InfoLevel))
            {
                return;
            }
            GlobalDefine.ILog.Info(string.Format(message, args));
        }

        public static void Debug(string message, params object[] args)
        {
            if (!CheckLogLevel(DebugLevel))
            {
                return;
            }
            GlobalDefine.ILog.Debug(string.Format(message, args));

        }

        public static void Error(string message, params object[] args)
        {
            StackTrace st = new StackTrace(1, true);
            string s = string.Format(message, args) + '\n' + st;
            GlobalDefine.ILog.Error(s);
        }
        
        public static void Console(string message)
        {
            if (GlobalDefine.Options.Console == 1)
            {
                System.Console.WriteLine(message);
            }
            GlobalDefine.ILog.Debug(message);
        }
        
        public static void Console(string message, params object[] args)
        {
            string s = string.Format(message, args);
            if (GlobalDefine.Options.Console == 1)
            {
                System.Console.WriteLine(s);
            }
            GlobalDefine.ILog.Debug(s);
        }
    }
}