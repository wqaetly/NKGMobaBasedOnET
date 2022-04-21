//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 技能行为树管理器
    /// </summary>
    public class SkillCanvasManagerComponent: Entity
    {
        /// <summary>
        /// 技能Id与其对应行为树映射,因为一个技能可能由多个行为树组成，所以value使用了List的形式
        /// </summary>
        public Dictionary<long, List<NP_RuntimeTree>> Skills = new Dictionary<long, List<NP_RuntimeTree>>();

        /// <summary>
        /// 技能Id与其等级映射
        /// </summary>
        public Dictionary<long, int> SkillLevels = new Dictionary<long, int>();
    }
}