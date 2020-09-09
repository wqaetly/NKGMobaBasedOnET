//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年1月24日 14:15:24
//------------------------------------------------------------

using UnityEngine;

namespace ETModel
{
    public class PlayEffectBuffSystem: ABuffSystemBase
    {
        public override void OnInit(BuffDataBase buffData, Unit theUnitFrom, Unit theUnitBelongto)
        {
            //设置Buff来源Unit和归属Unit
            this.TheUnitFrom = theUnitFrom;
            this.TheUnitBelongto = theUnitBelongto;
            this.BuffData = buffData;

            BuffTimerAndOverlayHelper.CalculateTimerAndOverlay(this, this.BuffData);
        }

        public override void OnExecute()
        {
            PlayEffect();
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }

            this.BuffState = BuffState.Running;
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            if (this.BuffData.SustainTime + 1 > 0)
            {
                if (TimeHelper.Now() > this.MaxLimitTime)
                {
                    this.BuffState = BuffState.Finished;
                }
            }
        }

        public override void OnFinished()
        {
            PlayEffectBuffData playEffectBuffData = this.BuffData as PlayEffectBuffData;
            string targetEffectName = playEffectBuffData.EffectName;
            if (playEffectBuffData.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{playEffectBuffData.EffectName}{this.CurrentOverlay}";
            }

            this.TheUnitBelongto.GetComponent<EffectComponent>()
                    .Remove(targetEffectName);
        }

        public override void OnRefresh()
        {
            PlayEffect();
            if (this.BuffData.EventIds != null)
            {
                foreach (var eventId in this.BuffData.EventIds)
                {
                    Game.Scene.GetComponent<BattleEventSystem>().Run($"{eventId}{this.TheUnitFrom.Id}", this);
                    //Log.Info($"抛出了{this.MSkillBuffDataBase.theEventID}{this.theUnitFrom.Id}");
                }
            }
            this.BuffState = BuffState.Running;
        }

        void PlayEffect()
        {
            PlayEffectBuffData playEffectBuffData = this.BuffData as PlayEffectBuffData;
            string targetEffectName = playEffectBuffData.EffectName;

            if (playEffectBuffData.CanChangeNameByCurrentOverlay)
            {
                targetEffectName = $"{playEffectBuffData.EffectName}{this.CurrentOverlay}";
                //Log.Info($"播放{targetEffectName}");
            }

            //如果想要播放的特效正在播放，就返回
            if (this.TheUnitBelongto.GetComponent<EffectComponent>().CheckState(targetEffectName)) return;

            GameObjectPool gameObjectPool = Game.Scene.GetComponent<GameObjectPool>();

            if (!gameObjectPool.HasRegisteredPrefab(targetEffectName))
            {
                gameObjectPool.Add(targetEffectName,
                    this.TheUnitFrom.GameObject.GetComponent<ReferenceCollector>()
                            .Get<GameObject>(targetEffectName));
            }

            Unit effectUnit = gameObjectPool.FetchEntity(targetEffectName);

            if (playEffectBuffData.FollowUnit)
            {
                if (playEffectBuffData.BuffTargetTypes == BuffTargetTypes.Self)
                {
                    effectUnit.GameObject.transform.SetParent(this.TheUnitFrom.GetComponent<HeroTransformComponent>()
                            .GetTranform(playEffectBuffData.PosType));
                }
                else
                {
                    effectUnit.GameObject.transform.SetParent(this.TheUnitBelongto.GetComponent<HeroTransformComponent>()
                            .GetTranform(playEffectBuffData.PosType));
                }

                effectUnit.GameObject.transform.localPosition = Vector3.zero;
            }

            this.TheUnitBelongto.GetComponent<EffectComponent>()
                    .Play(targetEffectName, effectUnit);
        }
    }
}