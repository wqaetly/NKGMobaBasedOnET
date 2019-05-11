using System;

namespace Box2DSharp.Common
{
    public static class DumpLogger
    {
        public static IDumpLogger Instance { get; set; } = new InternalDumpLogger();

        public static void Log(string message)
        {
            Instance.Log(message);
        }
    }

    public interface IDumpLogger
    {
        void Log(string message);
    }

    public class InternalDumpLogger : IDumpLogger
    {
        /// <inheritdoc />
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}