//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ETModel
{
    #region System

    [ObjectSystem]
    public class CDComponentAwakeSystem: AwakeSystem<CDComponent>
    {
        public override void Awake(CDComponent self)
        {
            self.Awake();
        }
    }

    [ObjectSystem]
    public class CDComponentUpdateSystem: UpdateSystem<CDComponent>
    {
        public override void Update(CDComponent self)
        {
            self.Update();
        }
    }

    [ObjectSystem]
    public class CDComponentFixedUpdateSystem: FixedUpdateSystem<CDComponent>
    {
        public override void FixedUpdate(CDComponent self)
        {
            self.FixedUpdate();
        }
    }

    [ObjectSystem]
    public class CDComponentDestroySystem: DestroySystem<CDComponent>
    {
        public override void Destroy(CDComponent self)
        {
            self.Destroy();
        }
    }

    #endregion

    public class CDInfo: IReference
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 上次触发时间点
        /// </summary>
        public long LastTriggerTimer;

        /// <summary>
        /// 时间间隔（CD）
        /// </summary>
        public long Interval;

        /// <summary>
        /// CD是否转好了
        /// </summary>
        public bool Result;

        public void Clear()
        {
            Name = null;
            this.LastTriggerTimer = 0;
            this.Interval = 0;
            this.Result = false;
        }
    }

    /// <summary>
    /// CD组件，用于统一管理所有的CD类型的数据，比如攻速CD，服务器上因试图攻击导致的循环MoveTo CD
    /// </summary>
    public class CDComponent: Component
    {
        #region 私有成员

        /// <summary>
        /// 包含所有CD信息的字典
        /// 键为id，值为对应所有CD信息
        /// </summary>
        private Dictionary<long, Dictionary<string, CDInfo>> CDInfos = new Dictionary<long, Dictionary<string, CDInfo>>();

        #endregion

        #region 公有成员

        private static CDComponent m_Instance;

        public static CDComponent Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    Log.Error("请先注册CDComponent到Game.Scene中");
                    
                    return null;
                }
                else
                {
                    return m_Instance;
                }

            }
        }

        /// <summary>
        /// 新增一条CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cdInfo"></param>
        public void AddCDData(long id, CDInfo cdInfo)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                cdInfoDic.Add(cdInfo.Name, cdInfo);
            }
            else
            {
                CDInfos.Add(id, new Dictionary<string, CDInfo>() { { cdInfo.Name, cdInfo } });
            }
        }

        /// <summary>
        /// 触发某个CD，更新其LastTriggerTimer与结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void TriggerCD(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out var cdInfo))
                {
                    cdInfo.LastTriggerTimer = TimeHelper.Now();
                    cdInfo.Result = false;
                    return;
                }
            }

            Log.Error($"尚未注册id为：{id}，Name为：{name}的CD信息");
        }

        /// <summary>
        /// 获取CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CDInfo GetCDData(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out var cdInfo))
                {
                    return cdInfo;
                }
            }

            Log.Error($"尚未注册id为：{id}，Name为：{name}的CD信息");
            return null;
        }

        /// <summary>
        /// 获取CD结果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool GetCDResult(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                if (cdInfoDic.TryGetValue(name, out var cdInfo))
                {
                    return cdInfo.Result;
                }
            }

            Log.Error($"尚未注册id为：{id}，Name为：{name}的CD信息");
            return false;
        }

        /// <summary>
        /// 移除一条CD数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public void RemoveCDData(long id, string name)
        {
            if (this.CDInfos.TryGetValue(id, out var cdInfoDic))
            {
                cdInfoDic.Remove(name);
            }
        }

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
            m_Instance = this;
        }

        public void Update()
        {
            //此处填写Update逻辑
            long currentTime = TimeHelper.Now();
            foreach (var cdInfoDic in this.CDInfos)
            {
                foreach (var cdInfo in cdInfoDic.Value)
                {
                    if (currentTime - cdInfo.Value.LastTriggerTimer >= cdInfo.Value.Interval)
                    {
                        cdInfo.Value.Result = true;
                    }
                }
            }
        }

        public void FixedUpdate()
        {
            //此处填写FixedUpdate逻辑
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
            foreach (var cdInfoList in CDInfos)
            {
                foreach (var cdInfo in cdInfoList.Value)
                {
                    ReferencePool.Release(cdInfo.Value);
                }

                cdInfoList.Value.Clear();
            }

            this.CDInfos.Clear();
        }

        public override void Dispose()
        {
            if (IsDisposed)
                return;
            base.Dispose();
            //此处填写释放逻辑,但涉及Entity的操作，请放在Destroy中
        }

        #endregion
    }
}