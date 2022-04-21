using System;

namespace ET
{
    public static class MonoTimeHelper
    {
        private static readonly DateTime epochDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static readonly long epoch = epochDateTime.Ticks;

        /// <summary>
        /// 客户端时间 毫秒  Ticks/1000 是毫秒  Ticks 一百毫微秒 
        /// </summary>
        /// <returns></returns>
        public static long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000;
        }

        /// <summary>
        /// 客户端时间戳 秒
        /// </summary>
        /// <returns></returns>
		public static long ClientNowSeconds()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000000;
        }

        public static long Now()
        {
            return ClientNow();
        }
    }
}