//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 14:15:24
//------------------------------------------------------------

using UnityEngine;

namespace ET
{
    public class PlayEffectBuffSystem : ABuffSystemBase<PlayEffectBuffData>
    {
#if !SERVER
        public override void OnExecute(uint currentFrame)
        {
            Game.EventSystem.Publish(new EventType.PlayEffectBuffSystemExcuteEvent()
            {
                PlayEffectBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget(),
                CurrentOverlay = this.CurrentOverlay
            }).Coroutine();
            Log.Info($"Execute播放：{GetBuffDataWithTType.EffectName}");
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }

        public override void OnFinished(uint currentFrame)
        {
            Game.EventSystem.Publish(new EventType.PlayEffectBuffSystemFinishEvent()
            {
                PlayEffectBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget(),
                CurrentOverlay = this.CurrentOverlay
            }).Coroutine();
        }

        public override void OnRefreshed(uint currentFrame)
        {
            Game.EventSystem.Publish(new EventType.PlayEffectBuffSystemExcuteEvent()
            {
                PlayEffectBuffData = GetBuffDataWithTType, Target = this.GetBuffTarget(),
                CurrentOverlay = this.CurrentOverlay
            }).Coroutine();
            Log.Info($"Refresh播放：{GetBuffDataWithTType.EffectName}");
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystemComponent>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
        }
#else
        public override void OnExecute(uint currentFrame)
        {

        }

#endif
    }
}