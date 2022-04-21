/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using System.Threading.Tasks;

namespace ET
{
    public class FUI_HeadBarAwakeSystem : AwakeSystem<FUI_HeadBar, GObject>
    {
        public override void Awake(FUI_HeadBar self, GObject go)
        {
            self.Awake(go);
        }
    }
        
    public sealed class FUI_HeadBar : FUI
    {	
        public const string UIPackageName = "HeadBar";
        public const string UIResName = "HeadBar";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self;
            
    	public GImage m_n16;
    	public GTextField m_Tex_Level;
    	public GTextField m_Tex_PlayerName;
    	public GImage m_n15;
    	public FUI_Bar_HP m_Bar_HP;
    	public FUI_Bar_MP m_Bar_MP;
    	public GImage m_Img_Gap;
    	public const string URL = "ui://ny5r4rwcqrur4";

       
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        
       
        public static FUI_HeadBar CreateInstance(Entity parent)
        {			
            return parent.AddChild<FUI_HeadBar, GObject>(CreateGObject());
        }
        
       
        public static ETTask<FUI_HeadBar> CreateInstanceAsync(Entity parent)
        {
            ETTask<FUI_HeadBar> tcs = ETTask<FUI_HeadBar>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<FUI_HeadBar, GObject>(go));
            });
    
            return tcs;
        }
        
       
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static FUI_HeadBar Create(Entity parent, GObject go)
        {
            return parent.AddChild<FUI_HeadBar, GObject>(go);
        }
            
       
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static FUI_HeadBar GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<FUI_HeadBar>();
        
            if(fui == null)
            {
                fui = Create(domain, go);
            }
        
            fui.isFromFGUIPool = true;
        
            return fui;
        }
            
        public void Awake(GObject go)
        {
            if(go == null)
            {
                return;
            }
            
            GObject = go;	
            
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Id.ToString();
            }
            
            self = (GComponent)go;
            
            self.Add(this);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                
    			m_n16 = (GImage)com.GetChildAt(0);
    			m_Tex_Level = (GTextField)com.GetChildAt(1);
    			m_Tex_PlayerName = (GTextField)com.GetChildAt(2);
    			m_n15 = (GImage)com.GetChildAt(3);
    			m_Bar_HP = FUI_Bar_HP.Create(this, com.GetChildAt(4));
    			m_Bar_MP = FUI_Bar_MP.Create(this, com.GetChildAt(5));
    			m_Img_Gap = (GImage)com.GetChildAt(6);
    		}
    	}
           
        public override void Dispose()
        {
            if(IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            self.Remove();
            self = null;
            
    		m_n16 = null;
    		m_Tex_Level = null;
    		m_Tex_PlayerName = null;
    		m_n15 = null;
    		m_Bar_HP.Dispose();
    		m_Bar_HP = null;
    		m_Bar_MP.Dispose();
    		m_Bar_MP = null;
    		m_Img_Gap = null;
    	}
    }
}