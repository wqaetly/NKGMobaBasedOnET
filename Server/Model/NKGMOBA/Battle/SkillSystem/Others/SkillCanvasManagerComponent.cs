//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentAwakeSystem: AwakeSystem<SkillCanvasManagerComponent>
    {
        public override void Awake(SkillCanvasManagerComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentUpdateSystem: UpdateSystem<SkillCanvasManagerComponent>
    {
        public override void Update(SkillCanvasManagerComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentFixedUpdateSystem: FixedUpdateSystem<SkillCanvasManagerComponent>
    {
        public override void FixedUpdate(SkillCanvasManagerComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentDestroySystem: DestroySystem<SkillCanvasManagerComponent>
    {
        public override void Destroy(SkillCanvasManagerComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    /// <summary>
    /// 技能行为树管理器
    /// </summary>
    public class SkillCanvasManagerComponent: Component
    {
        #region 私有成员

        /// <summary>
        /// 技能Id与其对应行为树映射,因为一个技能可能由多个行为树组成，所以value使用了List的形式
        /// </summary>
        private Dictionary<long, List<NP_RuntimeTree>> Skills = new Dictionary<long, List<NP_RuntimeTree>>();

        /// <summary>
        /// 技能Id与其等级映射
        /// </summary>
        private Dictionary<long, int> SkillLevels = new Dictionary<long, int>();

        #endregion

        #region 公有成员

        /// <summary>
        /// 添加技能Canvas
        /// </summary>
        /// <param name="skillId">归属技能Id，不是技能图本身的id</param>
        /// <param name="npRuntimeTree">对应行为树</param>
        public void AddSkillCanvas(long skillId, NP_RuntimeTree npRuntimeTree)
        {
            if (npRuntimeTree == null)
            {
                Log.Error($"试图添加的id为{skillId}的技能图为空");
                return;
            }

            if (Skills.TryGetValue(skillId, out var skillContent))
            {
                skillContent.Add(npRuntimeTree);
            }
            else
            {
                Skills.Add(skillId, new List<NP_RuntimeTree>() { npRuntimeTree });
            }

            if (!this.SkillLevels.ContainsKey(skillId))
            {
                SkillLevels.Add(skillId, 1);
            }
        }

        /// <summary>
        /// 获取行为树
        /// </summary>
        /// <param name="skillId">技能标识</param>
        public List<NP_RuntimeTree> GetSkillCanvas(long skillId)
        {
            if (Skills.TryGetValue(skillId, out var skillContent))
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
        public void RemoveSkillCanvas(long skillId)
        {
            foreach (var skillCanvas in GetSkillCanvas(skillId))
            {
                RemoveSkillCanvas(skillId, skillCanvas);
            }
            if (this.SkillLevels.ContainsKey(skillId))
            {
                SkillLevels.Remove(skillId);
            }
        }

        /// <summary>
        /// 移除行为树(移除一个技能标识对应的目标技能图)
        /// </summary>
        /// <param name="skillId">技能标识</param>
        /// <param name="npRuntimeTree">对应行为树</param>
        public void RemoveSkillCanvas(long skillId, NP_RuntimeTree npRuntimeTree)
        {
            List<NP_RuntimeTree> targetSkillContent = GetSkillCanvas(skillId);
            if (targetSkillContent != null)
            {
                for (int i = targetSkillContent.Count - 1; i >= 0; i--)
                {
                    if (targetSkillContent[i] == npRuntimeTree)
                    {
                        this.Entity.GetComponent<NP_RuntimeTreeManager>().RemoveTree(npRuntimeTree.Id);
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
        public void AddSkillLevel(long skillId, int count = 1)
        {
            if (this.SkillLevels.TryGetValue(skillId, out var level))
            {
                SkillLevels[skillId] = level + count;
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
        public int GetSkillLevel(long skillId)
        {
            if (this.SkillLevels.TryGetValue(skillId, out var level))
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

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
        }

        public void Update()
        {
            //此处填写Update逻辑
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            foreach (var skillContent in Skills)
            {
                foreach (var skillCanvas in skillContent.Value)
                {
                    skillCanvas.Dispose();
                }
            }

            Skills.Clear();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
        }

        #endregion
    }
}