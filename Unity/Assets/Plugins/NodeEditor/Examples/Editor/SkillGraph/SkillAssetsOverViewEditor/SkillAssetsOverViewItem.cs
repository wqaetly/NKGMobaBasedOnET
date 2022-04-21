//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年6月17日 20:12:30
//------------------------------------------------------------

using Sirenix.OdinInspector.Editor;

namespace Plugins.NodeEditor
{
    public class SkillAssetsOverViewItem
    {
        /// <summary>
        /// 显示在treeView的条目名称
        /// </summary>
        public string Name = "NoName";

        /// <summary>
        /// 分组
        /// </summary>
        public string Category = "Uncategorized";

        /// <summary>
        /// The description of the example.
        /// </summary>
        public string Description;

        public SkillGraph SkillGraph;

        public PropertyTree PropertyTree;

        // Token: 0x06000A23 RID: 2595 RVA: 0x00030DD4 File Offset: 0x0002EFD4
        //[OnInspectorGUI]
        public void Draw()
        {
            if (PropertyTree != null)
            {
                PropertyTree.Draw(false);
            }
            else
            {
                PropertyTree = PropertyTree.Create(this.SkillGraph);
                PropertyTree.Draw(false);
            }
        }
    }
}