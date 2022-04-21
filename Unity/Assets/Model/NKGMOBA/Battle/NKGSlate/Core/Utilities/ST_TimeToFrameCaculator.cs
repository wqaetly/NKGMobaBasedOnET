//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年8月14日 22:48:30
//------------------------------------------------------------

using UnityEngine;

namespace NKGSlate.Runtime
{
    public static class ST_TimeToFrameCaculator
    {
        /// <summary>
        /// 帧率，即每秒Step多少次 TODO 现在使用的Unity FixedUpdate所以间隔为0.02，即刷新间隔为50ms
        /// </summary>
        public const int FrameFrequency = 50;

        /// <summary>
        /// 每帧的时间间隔，毫秒单位
        /// </summary>
        public const float FrameInterval = 1000f / FrameFrequency;

        /// <summary>
        /// 从时间长度计算帧数
        /// 例如FrameFrequency为 30 ，FrameInterval为 33.33 ，timeLength为600（ms），就有 600 / 33.33 = 18.0018，最后向上取整，最终结果为 19
        /// </summary>
        /// <returns></returns>
        public static uint CaculateFrameCountFromTimeLength(long timeLength)
        {
            if (timeLength < 0)
            {
                Debug.LogError($"传递的时间长度：{timeLength} 非法，需要为大于等于0的长整形! ");
                return 0;
            }

            return (uint) Mathf.CeilToInt(timeLength / FrameInterval);
        }
    }
}