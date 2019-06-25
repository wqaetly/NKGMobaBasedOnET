//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月25日 20:52:13
//------------------------------------------------------------

using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace UnityEngine
{
    public class AnimationData
    {
#if !SERVER
        [HideLabel]
        [InfoBox("技能动画预览，注意，由于是预览，所以此处只有30帧，要查看完整动画，需要点击左边的小毛笔")]
        [InlineEditor(InlineEditorModes.LargePreview)]
        public AnimationClip TestAnimation;
#endif

        [HideLabel]
        [Title("将在此关键帧正式使技能奏效", Bold = false)]
        public int WorkFrame;
        
        
        [HideLabel]
        [InfoBox("这个值不需要手动设置，如果已经把奏效帧设置好，重新进入此界面此值会刷新。")]
        [Title("推算出的时间点", Bold = false)]
        public float WorkTime;
    }
}