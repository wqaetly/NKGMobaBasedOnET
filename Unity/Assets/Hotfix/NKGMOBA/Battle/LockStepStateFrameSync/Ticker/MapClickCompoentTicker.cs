#if !SERVER

using FairyGUI;
namespace ET
{
    /// <summary>
    /// 留作警示，33ms的fixedUpdate基本上会丢失大部分的玩家输入指令，因为很难保证自己的输入恰好处于这每33ms的更新间隔里
    /// </summary>
    [LSF_Tickable(EntityType = typeof(MapClickCompoent))]
    public class MapClickCompoentTicker: ALSF_TickHandler<MapClickCompoent>
    {
        public override void OnLSF_Tick(MapClickCompoent entity, uint currentFrame, long deltaTime)
        {
            // if (entity.m_UserInputComponent.RightMouseDown)
            // {
            //     // 状态帧系统测试代码
            //
            //     if (Stage.isTouchOnUI) //点在了UI上
            //     {
            //         //Log.Info("点在UI上");
            //     }
            //     else //没有点在UI上
            //     {
            //         if (entity.m_MouseTargetSelectorComponent.TargetGameObject?.GetComponent<MonoBridge>().CustomTag ==
            //             "Map")
            //         {
            //             Log.Info($"寻路：{TimeHelper.ClientNow()}");
            //             entity.MapPathFinder(entity.m_MouseTargetSelectorComponent.TargetHitPoint);
            //         }
            //     }
            // }
        }
    }
}
#endif


