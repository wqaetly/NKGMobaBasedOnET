using System.Collections.Generic;
using System.Linq;
using ET;
using FairyGUI;
using UnityEngine;

namespace ET
{
    public class FUIManagerComponentAwakeSystem : AwakeSystem<FUIManagerComponent>
    {
        public override void Awake(FUIManagerComponent self)
        {
            self.Root = self.Domain.AddChild<FUI, GObject>(GRoot.inst);
        }
    }

    /// <summary>
    /// 管理所有顶层UI, 顶层UI都是GRoot的孩子
    /// </summary>
    public class FUIManagerComponent : Entity
    {
        public FUI Root;

        /// <summary>
        /// 所有的FUI对象
        /// </summary>
        private Dictionary<string, FUI> m_AllHotfixFuis = new Dictionary<string, FUI>();

        /// <summary>
        /// 所有的FUIComponent对象
        /// </summary>
        private Dictionary<string, Entity> m_AllHotfixFuiComponents = new Dictionary<string, Entity>();

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.m_AllHotfixFuis.Clear();
            Root.Dispose();
            Root = null;
        }

        public void Add(string name, FUI ui, Entity uiComponent, bool asChildGObject = true)
        {
            if (m_AllHotfixFuis.TryGetValue(name, out var fui))
            {
                Log.Error($"已有名为：{name} 的FUI，请勿重复添加！");
                fui.Dispose();
                return;
            }
            else
            {
                m_AllHotfixFuis[name] = ui;
                m_AllHotfixFuiComponents[name] = uiComponent;

                Root?.Add(ui, asChildGObject);
            }
        }

        public void Remove(string name)
        {
            if (m_AllHotfixFuis.TryGetValue(name, out var fui))
            {
                m_AllHotfixFuis.Remove(name);
                Root?.Remove(fui.Name);
                m_AllHotfixFuiComponents[name].Dispose();
                m_AllHotfixFuiComponents.Remove(name);
            }
            else
            {
                Log.Warning($"不存在名为：{name} 的FUI，请检查逻辑！");
                return;
            }
        }

        /// <summary>
        /// 通过名字获得FUI
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetFUIComponent<T>(string name) where T : Entity
        {
            if (m_AllHotfixFuiComponents.TryGetValue(name, out var fuiComponent))
            {
                return fuiComponent as T;
            }

            return default;
        }

        public FUI[] GetAllFUIs()
        {
            return Root?.GetAll();
        }

        public void Clear()
        {
            var childrens = GetAllFUIs();
            if (childrens != null)
            {
                foreach (var fui in childrens)
                {
                    Remove(fui.Name);
                }
            }
        }
    }
}