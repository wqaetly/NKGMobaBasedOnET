using System;
using UnityEngine;
using Random = System.Random;

namespace ETModel
{
    public static class RandomHelper
    {
        private static readonly Random random = new Random();

        public static UInt64 RandUInt64()
        {
            var bytes = new byte[8];
            random.NextBytes(bytes);
            return BitConverter.ToUInt64(bytes, 0);
        }

        public static Int64 RandInt64()
        {
            var bytes = new byte[8];
            random.NextBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        public static float RandFloat()
        {
            return (float) random.NextDouble();
        }

        public static Color RandColor()
        {
            return new Color(RandomHelper.RandFloat(), RandomHelper.RandFloat(), RandomHelper.RandFloat());
        }

        /// <summary>
        /// 获取lower与Upper之间的随机数
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static int RandomNumber(int lower, int upper)
        {
            int value = random.Next(lower, upper);
            return value;
        }
    }
}