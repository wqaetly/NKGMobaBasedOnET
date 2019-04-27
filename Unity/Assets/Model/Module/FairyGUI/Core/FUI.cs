using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    [ObjectSystem]
    public class FUIAwakeSystem: AwakeSystem<FUI, GObject>
    {
        public override void Awake(FUI self, GObject gObject)
        {
            self.GObject = gObject;
        }
    }

    public sealed class FUI: Entity
    {
        public GObject GObject;

        public string Name
        {
            get
            {
                if (GObject == null)
                {
                    return string.Empty;
                }

                return GObject.name;
            }

            set
            {
                if (GObject == null)
                {
                    return;
                }

                GObject.name = value;
            }
        }

        public bool Visible
        {
            get
            {
                if (GObject == null)
                {
                    return false;
                }

                return GObject.visible;
            }
            set
            {
                if (GObject == null)
                {
                    return;
                }

                GObject.visible = value;
            }
        }

        public bool IsWindow
        {
            get
            {
                return GObject is GWindow;
            }
        }

        public bool IsComponent
        {
            get
            {
                return GObject is GComponent;
            }
        }

        public bool IsRoot
        {
            get
            {
                return GObject is GRoot;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return GObject == null;
            }
        }

        private Dictionary<string, FUI> children = new Dictionary<string, FUI>();

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            base.Dispose();

            // 从父亲中删除自己
            GetParent<FUI>()?.RemoveNoDispose(Name);

            // 删除所有的孩子
            foreach (FUI ui in children.Values.ToArray())
            {
                ui.Dispose();
            }

            children.Clear();

            // 删除自己的UI
            if (!IsRoot)
            {
                GObject.Dispose();
            }

            GObject = null;
        }

        public void Add(FUI ui)
        {
            if (ui == null || ui.IsEmpty)
            {
                throw new Exception($"ui can not be empty");
            }

            if (string.IsNullOrWhiteSpace(ui.Name))
            {
                throw new Exception($"ui.Name can not be empty");
            }

            if (!IsComponent)
            {
                throw new Exception($"this must be GComponent");
            }

            if (children.ContainsKey(ui.Name))
            {
                throw new Exception($"ui.Name({ui.Name}) already exist");
            }

            children.Add(ui.Name, ui);

            GObject.asCom.AddChild(ui.GObject);

            ui.Parent = this;
        }

        public void MakeFullScreen()
        {
            GObject?.asCom?.MakeFullScreen();
        }

        public void Remove(string name)
        {
            if (IsDisposed)
            {
                return;
            }

            FUI ui;

            if (children.TryGetValue(name, out ui))
            {
                children.Remove(name);

                if (ui != null)
                {
                    if (IsComponent)
                    {
                        GObject.asCom.RemoveChild(ui.GObject, false);
                    }

                    ui.Parent = null;
                    ui.Dispose();
                }
            }
        }

        /// <summary>
        /// 一般情况不要使用此方法，如需使用，需要自行管理返回值的FUI的释放。
        /// </summary>
        public FUI RemoveNoDispose(string name)
        {
            if (IsDisposed)
            {
                return null;
            }

            FUI ui;

            if (children.TryGetValue(name, out ui))
            {
                children.Remove(name);

                if (ui != null)
                {
                    if (IsComponent)
                    {
                        GObject.asCom.RemoveChild(ui.GObject, false);
                    }

                    ui.Parent = null;
                }
            }

            return ui;
        }

        public void RemoveChildren()
        {
            foreach (var child in children.Values.ToArray())
            {
                child.Dispose();
            }

            children.Clear();
        }

        public FUI Get(string name)
        {
            FUI child;

            if (children.TryGetValue(name, out child))
            {
                return child;
            }

            return null;
        }

        public FUI[] GetAll()
        {
            return children.Values.ToArray();
        }

        public void Show()
        {
            if (Visible)
            {
                return;
            }

            Visible = true;

            if (IsWindow)
            {
                GObject.asGWindow().Show();
            }
        }

        public void Hide()
        {
            if (!Visible)
            {
                return;
            }

            Visible = true;

            if (IsWindow)
            {
                GObject.asGWindow().Hide();
            }
        }
    }
}