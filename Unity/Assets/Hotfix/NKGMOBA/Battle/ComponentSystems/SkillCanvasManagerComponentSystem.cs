using System.Collections.Generic;

namespace ET
{
    public class SkillCanvasManagerComponentComponentDestroySystem : DestroySystem<SkillCanvasManagerComponent>
    {
        public override void Destroy(SkillCanvasManagerComponent self)
        {
            foreach (var skillContent in self.Skills)
            {
                foreach (var skillCanvas in skillContent.Value)
                {
                    skillCanvas.Dispose();
                }
            }

            self.Skills.Clear();
        }
    }
    
    public static class SkillCanvasManagerComponentUtitlites
    {
        #region 公有成员

        /// <summary>
        /// 添加技能Canvas
        /// </summary>
        /// <param name="skillId">归属技能Id，不是技能图本身的id</param>
        /// <param name="npRuntimeTree">对应行为树</param>
        public static void AddSkillCanvas(this SkillCanvasManagerComponent self, long skillId,
            NP_RuntimeTree npRuntimeTree)
        {
            if (npRuntimeTree == null)
            {
                Log.Error($"试图添加的id为{skillId}的技能图为空");
                return;
            }

            if (self.Skills.TryGetValue(skillId, out var skillContent))
            {
                skillContent.Add(npRuntimeTree);
            }
            else
            {
                self.Skills.Add(skillId, new List<NP_RuntimeTree>() {npRuntimeTree});
            }

            //TODO 这里默认一级了
            if (!self.SkillLevels.ContainsKey(skillId))
            {
                self.SkillLevels.Add(skillId, 1);
            }
        }


        /// <summary>
        /// 获取所有技能行为树
        /// </summary>
        /// <param name="skillId">技能标识</param>
        public static Dictionary<long, List<NP_RuntimeTree>> GetAllSkillCanvas(this SkillCanvasManagerComponent self)
        {
            return self.Skills;
        }

        /// <summary>
        /// 获取行为树
        /// </summary>
        /// <param name="skillId">技能标识</param>
        public static List<NP_RuntimeTree> GetSkillCanvas(this SkillCanvasManagerComponent self, long skillId)
        {
            if (self.Skills.TryGetValue(skillId, out var skillContent))
            {
                return skillContent;
            }
            else
            {
                Log.Error($"请求的ID标识为{skillId}的技能图不存在");
                return null;
            }
        }

        /// <summary>
        /// 移除行为树(移除一个技能标识对应所有技能图)
        /// </summary>
        /// <param name="skillId">技能标识</param>
        public static void RemoveSkillCanvas(this SkillCanvasManagerComponent self, long skillId)
        {
            foreach (var skillCanvas in self.GetSkillCanvas(skillId))
            {
                self.RemoveSkillCanvas(skillId, skillCanvas);
            }

            if (self.SkillLevels.ContainsKey(skillId))
            {
                self.SkillLevels.Remove(skillId);
            }
        }

        /// <summary>
        /// 移除行为树(移除一个技能标识对应的目标技能图)
        /// </summary>
        /// <param name="skillId">技能标识</param>
        /// <param name="npRuntimeTree">对应行为树</param>
        public static void RemoveSkillCanvas(this SkillCanvasManagerComponent self, long skillId,
            NP_RuntimeTree npRuntimeTree)
        {
            List<NP_RuntimeTree> targetSkillContent = self.GetSkillCanvas(skillId);
            if (targetSkillContent != null)
            {
                for (int i = targetSkillContent.Count - 1; i >= 0; i--)
                {
                    if (targetSkillContent[i] == npRuntimeTree)
                    {
                        self.GetParent<Unit>().GetComponent<NP_RuntimeTreeManager>().RemoveTree(npRuntimeTree.Id);
                        targetSkillContent.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 给技能升级
        /// </summary>
        /// <param name="skillId"></param>
        /// <param name="count"></param>
        public static void AddSkillLevel(this SkillCanvasManagerComponent self, long skillId, int count = 1)
        {
            if (self.SkillLevels.TryGetValue(skillId, out var level))
            {
                self.SkillLevels[skillId] = level + count;
            }
            else
            {
                Log.Error($"请求升级的SkillId:{skillId}不存在");
            }
        }

        /// <summary>
        /// 获取技能等级
        /// </summary>
        /// <param name="skillId"></param>
        public static int GetSkillLevel(this SkillCanvasManagerComponent self, long skillId)
        {
            if (self.SkillLevels.TryGetValue(skillId, out var level))
            {
                return level;
            }
            else
            {
                Log.Error($"请求等级的SkillId:{skillId}不存在");
                return -1;
            }
        }

        #endregion
    }
}