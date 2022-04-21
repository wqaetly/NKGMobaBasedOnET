using ET.EventType;
using UnityEngine;

namespace ET
{
    public class PlayEffectBuffSystemEvent : AEvent<EventType.PlayEffectBuffSystemExcuteEvent>
    {
        protected override async ETTask Run(PlayEffectBuffSystemExcuteEvent a)
        {
            PlayEffectBuffData playEffectBuffData = a.PlayEffectBuffData;
            string targetEffectName = playEffectBuffData.EffectName;

            if (playEffectBuffData.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{playEffectBuffData.EffectName}{a.CurrentOverlay}";
                //Log.Info($"播放{targetEffectName}");
            }

            //如果想要播放的特效正在播放，就返回
            if (a.Target.GetComponent<EffectComponent>().CheckState(targetEffectName)) return;

            GameObject effectUnit =
                GameObjectPoolComponent.Instance.FetchGameObject(targetEffectName, GameObjectType.Effect);

            if (playEffectBuffData.FollowUnit)
            {
                effectUnit.transform.SetParent(a.Target.GetComponent<UnitTransformComponent>()
                    .GetTranform(playEffectBuffData.PosType));

                effectUnit.transform.localPosition = Vector3.zero;
            }

            a.Target.GetComponent<EffectComponent>().Play(targetEffectName, effectUnit);

            await ETTask.CompletedTask;
        }
    }

    public class PlayEffectBuffSystemEvent1 : AEvent<EventType.PlayEffectBuffSystemFinishEvent>
    {
        protected override async ETTask Run(PlayEffectBuffSystemFinishEvent a)
        {
            PlayEffectBuffData playEffectBuffData = a.PlayEffectBuffData;
            string targetEffectName = playEffectBuffData.EffectName;
            if (playEffectBuffData.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{playEffectBuffData.EffectName}{a.CurrentOverlay}";
            }

            a.Target.GetComponent<EffectComponent>()
                .Remove(targetEffectName);

            await ETTask.CompletedTask;
        }
    }
}