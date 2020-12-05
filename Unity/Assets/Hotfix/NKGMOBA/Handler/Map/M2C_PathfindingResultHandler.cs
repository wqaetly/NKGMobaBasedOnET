using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_PathfindingResultHandler: AMHandler<M2C_PathfindingResult>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_PathfindingResult message)
        {
            Unit unit = ETModel.Game.Scene.GetComponent<UnitComponent>().Get(message.Id);
            UnitPathComponent unitPathComponent = unit.GetComponent<UnitPathComponent>();

            unitPathComponent.StartMove(message).Coroutine();

            GizmosDebug.Instance.ClearData(message.Id);
            GizmosDebug.Instance.AddData(message.Id, new Vector3(message.X, message.Y, message.Z));
            for (int i = 0; i < message.Xs.Count; ++i)
            {
                GizmosDebug.Instance.AddData(message.Id, new Vector3(message.Xs[i], message.Ys[i], message.Zs[i]));
            }

            await ETTask.CompletedTask;
        }
    }
}