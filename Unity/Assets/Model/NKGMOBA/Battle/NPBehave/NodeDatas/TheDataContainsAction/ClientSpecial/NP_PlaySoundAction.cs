//此文件格式由工具自动生成

using System;
using Sirenix.OdinInspector;

namespace ETModel
{
    [Title("播放音效", TitleAlignment = TitleAlignments.Centered)]
    public class NP_PlaySoundAction: NP_ClassForStoreAction
    {
        [LabelText("要播放的音效名称")]
        public string SoundName;

        public override Action GetActionToBeDone()
        {
            this.Action = this.PlaySoundAction;
            return this.Action;
        }

        public void PlaySoundAction()
        {
            Game.Scene.GetComponent<SoundComponent>().PlayClip(this.SoundName, 0.4f).Coroutine();
        }
    }
}