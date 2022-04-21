using UnityEngine;

namespace ET
{
    /// <summary>
    /// 时间-帧数转换器
    /// </summary>
    public static class TimeAndFrameConverter
    {
        public static long MS_Float2Long(float time)
        {
            return (long) (time * 1000f);
        }

        public static float Float_Long2Float(long time)
        {
            return (time / 1000f);
        }

        public static uint Frame_Long2Frame(long time)
        {
            return (uint)Mathf.CeilToInt(((time * 1.0f) / GlobalDefine.FixedUpdateTargetDTTime_Long));
        }

        public static uint Frame_Float2Frame(float time)
        {
            return (uint)Mathf.CeilToInt(time / GlobalDefine.FixedUpdateTargetDTTime_Float);
        }

        public static uint Frame_Float2FrameWithHalfRTT(float time, long halfRTT)
        {
            return Frame_Long2Frame(((long) (time * 1000) + halfRTT));
        }
        
        public static uint Frame_Long2FrameWithHalfRTT(long time, long halfRTT)
        {
            return Frame_Long2Frame(time + halfRTT);
        }
    }
}