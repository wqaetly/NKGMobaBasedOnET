using System;

namespace ETModel
{
    public static class Log
    {
        private static readonly ILog globalLog = new NLogAdapter();

        public static void Trace(string message)
        {
            globalLog.Trace(message);
        }

        public static void Warning(string message)
        {
            globalLog.Warning(message);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(string.Concat("警告:", message));
        }

        public static void Info(string message)
        {
            globalLog.Info(message);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[{DateTime.Now}] 信息：{message}");
        }

        public static void Debug(string message)
        {
            globalLog.Debug(message);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(string.Concat("测试:", message));
        }

        public static void Error(Exception e)
        {
            globalLog.Error(e.ToString());
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(string.Concat("异常:", e));
        }

        public static void Error(string message)
        {
            globalLog.Error(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(string.Concat("错误:", message));
        }

        public static void Fatal(Exception e)
        {
            globalLog.Fatal(e.ToString());
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(string.Concat("毁灭:", e));
        }

        public static void Fatal(string message)
        {
            globalLog.Fatal(message);
        }

        public static void Msg(object message)
        {
            globalLog.Debug(MongoHelper.ToJson(message));
        }
    }
}