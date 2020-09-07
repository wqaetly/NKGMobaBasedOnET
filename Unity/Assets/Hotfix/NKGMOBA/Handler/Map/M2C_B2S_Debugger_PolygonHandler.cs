//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年8月10日 15:16:57
//------------------------------------------------------------

using System.Collections.Generic;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [MessageHandler]
    public class M2C_B2S_Debugger_PolygonHandler: AMHandler<M2C_B2S_Debugger_Polygon>
    {
        protected override async ETTask Run(ETModel.Session session, M2C_B2S_Debugger_Polygon message)
        {
            List<Vector2> vets = new List<Vector2>();
            foreach (var vect in message.Vects)
            {
                vets.Add(new Vector2(vect.X, vect.Y));
            }
            vets.Add(new Vector2(vets[0].x,vets[0].y));
            ETModel.Game.Scene.GetComponent<B2S_DebuggerComponent>().SetColliderInfo(vets.ToArray(), message.SustainTime);
            await ETTask.CompletedTask;
        }
    }
}