namespace Plugins.NodeEditor
{
    public static class NPBehaveGraphExportDataHelper
    {
        public static bool IsServerSpecialNode(NPBehaveGraph npBehaveGraph, NP_NodeBase npNodeBase)
        {
            return (npBehaveGraph.groups.Exists((group) =>
                @group.title == "服务端专属" && @group.innerNodeGUIDs.Contains(npNodeBase.GUID)));
        }
    }
}