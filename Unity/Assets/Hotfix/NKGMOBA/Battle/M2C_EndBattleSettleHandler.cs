using System.Collections.Generic;

namespace ET
{
    public class  M2C_EndBattleSettleHandler : AMHandler< M2C_EndBattleSettle>
    {
        protected override async ETVoid Run(Session session, M2C_EndBattleSettle message)
        {
            var e = new EventType.BattleEnd() { ZoneScene = session.DomainScene(), Scores = new List<(string name, int score)>() };

            foreach (var playerBattlePoint in message.settleAccount)
            {
                Log.Debug("playerId:"+playerBattlePoint.Account+"Point:"+playerBattlePoint.Point.ToString()+"Count"+playerBattlePoint.SingleMatchCount.ToString());
                e.Scores.Add((playerBattlePoint.Account, playerBattlePoint.SingleMatchCount));
            }

            Game.EventSystem.Publish(e).Coroutine();

            await ETTask.CompletedTask;
        }
    }
}