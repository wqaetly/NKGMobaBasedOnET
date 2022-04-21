//此文件格式由工具自动生成
using System;
using Sirenix.OdinInspector;

namespace ET
{
    [Title("停止播放音效",TitleAlignment = TitleAlignments.Centered)]
    public class NP_StopSoundAction:NP_ClassForStoreAction
    {   
        [LabelText("要停止的音效名称")]
        public string SoundName;
        
        public override Action GetActionToBeDone()
        {
            this.Action = this.StopSoundAction;
            return this.Action;
        }

        public void StopSoundAction()
        {
#if !SERVER
                        Game.Scene.GetComponent<SoundComponent>().StopAndCleanMusic(this.SoundName);
#endif
        }
    }
}
