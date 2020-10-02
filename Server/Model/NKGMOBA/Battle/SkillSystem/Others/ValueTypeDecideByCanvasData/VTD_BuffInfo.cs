//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2020年9月17日 20:54:29
//------------------------------------------------------------

using Sirenix.OdinInspector;
using UnityEngine;

namespace ETModel
{
    public class VTD_BuffInfo
    {
        [BoxGroup("Buff节点Id信息")]
        [HideLabel]
        public VTD_Id BuffNodeId;

        [LabelText("层数是否被黑板值决定")]
        public bool LayersDetermindByBBValue = false;

        [Tooltip("如果为绝对层数，且此时Layers设置为10，意思是添加Buff到10层，否则就是添加10层Buff")]
        [LabelText("层数是否为绝对层数")]
        public bool LayersIsAbs;

        [HideIf("LayersDetermindByBBValue")]
        [LabelText("添加Buff层数")]
        public int Layers = 1;

        [ShowIf("LayersDetermindByBBValue")]
        [LabelText("添加Buff层数")]
        public NP_BlackBoardRelationData LayersThatDetermindByBBValue;
    }

    public static class VTD_BuffInfoExtension
    {
        public static void AutoAddBuff(this VTD_BuffInfo self, long dataId, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo,
        NP_RuntimeTree theSkillCanvasBelongTo)
        {
            BuffPoolComponent buffPoolComponent = Game.Scene.GetComponent<BuffPoolComponent>();
            int Layers = 0;
            if (self.LayersDetermindByBBValue)
            {
                Layers = theSkillCanvasBelongTo.GetBlackboard().Get<int>(self.LayersThatDetermindByBBValue.BBKey);
            }
            else
            {
                Layers = self.Layers;
            }

            if (self.LayersIsAbs)
            {
                ABuffSystemBase nextBuffSystemBase = buffPoolComponent.AcquireBuff(dataId, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
                if (nextBuffSystemBase.CurrentOverlay < nextBuffSystemBase.BuffData.MaxOverlay && nextBuffSystemBase.CurrentOverlay < Layers)
                {
                    Layers -= nextBuffSystemBase.CurrentOverlay;
                }
                else
                {
                    return;
                }
            }

            for (int i = 0; i < Layers; i++)
            {
                buffPoolComponent.AcquireBuff(dataId, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
            }
        }

        public static void AutoAddBuff(this VTD_BuffInfo self, NP_DataSupportor npDataSupportor, long buffNodeId, Unit theUnitFrom,
        Unit theUnitBelongTo,
        NP_RuntimeTree theSkillCanvasBelongTo)
        {
            BuffPoolComponent buffPoolComponent = Game.Scene.GetComponent<BuffPoolComponent>();
            int Layers = 0;
            if (self.LayersDetermindByBBValue)
            {
                Layers = theSkillCanvasBelongTo.GetBlackboard().Get<int>(self.LayersThatDetermindByBBValue.BBKey);
            }
            else
            {
                Layers = self.Layers;
            }

            if (self.LayersIsAbs)
            {
                ABuffSystemBase nextBuffSystemBase = buffPoolComponent.AcquireBuff(npDataSupportor, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
                if (nextBuffSystemBase.CurrentOverlay < nextBuffSystemBase.BuffData.MaxOverlay && nextBuffSystemBase.CurrentOverlay < Layers)
                {
                    Layers -= nextBuffSystemBase.CurrentOverlay;
                }
                else
                {
                    return;
                }
            }

            for (int i = 0; i < Layers; i++)
            {
                buffPoolComponent.AcquireBuff(npDataSupportor, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
            }
        }
    }
}