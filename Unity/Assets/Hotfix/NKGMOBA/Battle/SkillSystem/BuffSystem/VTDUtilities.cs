namespace ET
{
    public static class VTD_BuffInfoExtension
    {
        public static void AutoAddBuff(this VTD_BuffInfo self, long dataId, long buffNodeId, Unit theUnitFrom, Unit theUnitBelongTo,
        NP_RuntimeTree theSkillCanvasBelongTo)
        {
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
                IBuffSystem nextBuffSystemBase = BuffFactory.AcquireBuff(dataId, buffNodeId, theUnitFrom, theUnitBelongTo,
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
                BuffFactory.AcquireBuff(dataId, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
            }
        }

        public static void AutoAddBuff(this VTD_BuffInfo self, NP_DataSupportor npDataSupportor, long buffNodeId, Unit theUnitFrom,
        Unit theUnitBelongTo,
        NP_RuntimeTree theSkillCanvasBelongTo)
        {
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
                IBuffSystem nextBuffSystemBase = BuffFactory.AcquireBuff(npDataSupportor, buffNodeId, theUnitFrom, theUnitBelongTo,
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
                BuffFactory.AcquireBuff(npDataSupportor, buffNodeId, theUnitFrom, theUnitBelongTo,
                    theSkillCanvasBelongTo);
            }
        }
    }
}