//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月10日 15:16:45
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_B2S_Debugger_CircleHandler: AMHandler<M2C_B2S_Debugger_Circle>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_B2S_Debugger_Circle message)
        {
            ETModel.Log.Info($"收到圆形绘制请求,半径为{message.Radius}");
            List<Vector2> mVets = new List<Vector2>();

            var originFrom = new Vector2(message.Pos.X + message.Radius,
                message.Pos.Y);

            var originTo = new Vector2(message.Pos.X + message.Radius,
                message.Pos.Y);

            var step = Mathf.RoundToInt(360 / 30f);
            for (int i = 0; i <= 360; i += step)
            {
                Vector3 tempVector3 = new Vector3(message.Radius * Mathf.Sin(i * Mathf.Deg2Rad),
                    message.Radius * Mathf.Cos(i * Mathf.Deg2Rad));

                originTo = new Vector2(tempVector3.x + message.Pos.X,
                    tempVector3.y + message.Pos.Y);
                mVets.Add(originFrom);
                mVets.Add(originTo);
                originFrom = originTo;
            }

            //mVets.Add(mVets[0]);
            ETModel.Game.Scene.GetComponent<B2S_DebuggerComponent>().SetColliderInfo(mVets.ToArray(), message.SustainTime);
            await ETTask.CompletedTask;
        }
    }
}