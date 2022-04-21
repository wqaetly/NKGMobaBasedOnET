using System.Collections.Generic;

namespace ET
{
    public class ProtoTest
    {
        public static void Do()
        {
            NP_BBValue_Float npBbValueFloat = new NP_BBValue_Float();
            npBbValueFloat.Value = 99;
            byte[] s = ProtobufHelper.ToBytes(npBbValueFloat);
            NP_BBValue_Float npBbValueFloat1 = ProtobufHelper.FromBytes<NP_BBValue_Float>(s, 0, s.Length);

            NP_BBValue_List_Long npBbValueListlong = new NP_BBValue_List_Long();
            npBbValueListlong.Value = new List<long>() {99, 98};
            byte[] s1 = ProtobufHelper.ToBytes(npBbValueListlong);
            NP_BBValue_List_Long npBbValueFloat2 = ProtobufHelper.FromBytes<NP_BBValue_List_Long>(s1, 0, s1.Length);

            LSF_ChangeBBValueCmd lsf = new LSF_ChangeBBValueCmd();
            lsf.NP_RuntimeTreeBBSnap = new NP_RuntimeTreeBBSnap();

            lsf.NP_RuntimeTreeBBSnap.NP_FrameBBValues.Add("TestList", npBbValueListlong);
            lsf.NP_RuntimeTreeBBSnap.NP_FrameBBValues.Add("TestFloat", npBbValueFloat);

            lsf.NP_RuntimeTreeBBSnap.NP_FrameBBValueOperations.Add("TestList", NP_RuntimeTreeBBOperationType.ADD);
            lsf.NP_RuntimeTreeBBSnap.NP_FrameBBValueOperations.Add("TestFloat", NP_RuntimeTreeBBOperationType.REMOVE);

            byte[] s2 = ProtobufHelper.ToBytes(lsf);
            LSF_ChangeBBValueCmd lsf1 = ProtobufHelper.FromBytes<LSF_ChangeBBValueCmd>(s2, 0, s2.Length);

            Log.Info(lsf1.ToString());
        }
    }
}