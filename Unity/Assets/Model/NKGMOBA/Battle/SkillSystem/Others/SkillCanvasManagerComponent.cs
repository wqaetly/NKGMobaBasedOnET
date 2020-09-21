//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentAwakeSystem: AwakeSystem<SkillCanvasManagerComponentComponent>
    {
        public override void Awake(SkillCanvasManagerComponentComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentUpdateSystem: UpdateSystem<SkillCanvasManagerComponentComponent>
    {
        public override void Update(SkillCanvasManagerComponentComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentFixedUpdateSystem: FixedUpdateSystem<SkillCanvasManagerComponentComponent>
    {
        public override void FixedUpdate(SkillCanvasManagerComponentComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class SkillCanvasManagerComponentComponentDestroySystem: DestroySystem<SkillCanvasManagerComponentComponent>
    {
        public override void Destroy(SkillCanvasManagerComponentComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    /// <summary>
    /// 技能行为树管理器
    /// </summary>
    public class SkillCanvasManagerComponentComponent: Component
    {
        #region 私有成员

        /// <summary>
        /// 技能标识与其对应行为树映射,因为一个技能可能由多个行为树组成，所以value使用了List的形式
        /// 这里的技能标识为0，1，2，3这种形式
        /// </summary>
        private Dictionary<int, List<NP_RuntimeTree>> Skills = new Dictionary<int, List<NP_RuntimeTree>>();

        #endregion

        #region 公有成员

        /// <summary>
        /// 添加技能Canvas
        /// </summary>
        /// <param name="skillId">技能标识</param>
        /// <param name="npRuntimeTree">对应行为树</param>
        public void AddSkillCanvas(int skillId, NP_RuntimeTree npRuntimeTree)
        {
            if (Skills.TryGetValue(skillId, out var skillContent))
            {
                skillContent.Add(npRuntimeTree);
            }
            else
            {
                Skills.Add(skillId, new List<NP_RuntimeTree>() { npRuntimeTree });
            }
        }

        /// <summary>
        /// 获取行为树
        /// </summary>
        /// <param name="skillId">技能标识</param>
        public List<NP_RuntimeTree> GetSkillCanvas(int skillId)
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
        public void RemoveSkillCanvas(int skillId)
        {
            foreach (var skillCanvas in GetSkillCanvas(skillId))
            {
                RemoveSkillCanvas(skillId, skillCanvas);
            }
        }

        /// <summary>
        /// 移除行为树(移除一个技能标识对应的目标技能图)
        /// </summary>
        /// <param name="skillId">技能标识</param>
        /// <param name="npRuntimeTree">对应行为树</param>
        public void RemoveSkillCanvas(int skillId, NP_RuntimeTree npRuntimeTree)
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