---@class HotfixCodeGenHandler 热更层代码生成器
local HotfixCodeGenHandler = {}

--- 执行生成热更层代码
---@param handler CS.FairyEditor.PublishHandler
---@param codeGenConfig CodeGenConfig
function HotfixCodeGenHandler.Do(handler, codeGenConfig)
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.

    --- 从自定义配置中读取路径和命名空间
    local exportCodePath = codeGenConfig.HotfixCodeOutPutPath .. '/' .. codePkgName
    local namespaceName = codeGenConfig.HotfixNameSpace

    --- 初始化自定义组件名前缀
    local classNamePrefix = codeGenConfig.ClassNamePrefix
    --- 初始化自定义成员变量名前缀
    local memberVarNamePrefix = codeGenConfig.MemerVarNamePrefix
    
    --- 从FGUI编辑器中读取配置
    ---@type CS.FairyEditor.GlobalPublishSettings.CodeGenerationConfig
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local getMemberByName = settings.getMemberByName

    --- 所有将要导出的类（当前包的所有设置为导出的组件，以及当前包所有被引用的组件）
    ---@type CS.FairyEditor.PublishHandler.ClassInfo[]
    local classes = handler:CollectClasses(codeGenConfig.CodeStrip, codeGenConfig.CodeStrip, nil)
    handler:SetupCodeFolder(exportCodePath, "cs") --check if target folder exists, and delete old files

    local classCnt = classes.Count
    local writer = CodeWriter.new()
    for i = 0, classCnt - 1 do
        local classInfo = classes[i]
        local members = classInfo.members
        writer:reset()

        writer:writeln('using FairyGUI;')
        writer:writeln('using System.Threading.Tasks;')
        writer:writeln()
        writer:writeln('namespace %s', namespaceName)
        writer:startBlock()
        
        --- 组装自定义组件前缀
        local className = classNamePrefix .. classInfo.className
        -- 1
        writer:writeln([[public class %sAwakeSystem : AwakeSystem<%s, GObject>
    {
        public override void Awake(%s self, GObject go)
        {
            self.Awake(go);
        }
    }
        ]], className, className, className)

        writer:writeln([[public sealed class %s : FUI
    {	
        public const string UIPackageName = "%s";
        public const string UIResName = "%s";
        
        /// <summary>
        /// {uiResName}的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public %s self;
            ]], className, codePkgName, classInfo.resName, classInfo.superClassName)

        local memberCnt = members.Count

        -- 是否为自定义类型组件标记数组
        local customComponentFlagsArray = {}
        -- 是否为跨包组件标记数组
        local crossPackageFlagsArray = {}

        for j = 0, memberCnt - 1
        do
            local memberInfo = members[j]
            customComponentFlagsArray[j] = false
            crossPackageFlagsArray[j] = false

            -- 判断是不是我们自定义类型组件
            local typeName = memberInfo.type
            for k = 0, classCnt - 1
            do
                if typeName == classes[k].className
                then
                    typeName = classNamePrefix .. classes[k].className
                    customComponentFlagsArray[j] = true
                    break
                end
            end

            -- 判断是不是跨包类型组件
            if memberInfo.res ~= nil then
                --- 组装自定义组件前缀
                typeName = classNamePrefix .. memberInfo.res.name
                crossPackageFlagsArray[j] = true
            end
            
            --- 组装自定义成员前缀
            writer:writeln('\tpublic %s %s;', typeName, memberVarNamePrefix .. memberInfo.varName)
        end
        writer:writeln('\tpublic const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln([[   
        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }
    
        private static void CreateGObjectAsync(UIPackage.CreateObjectCallback result)
        {
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, result);
        }
        ]])

        writer:writeln([[   
        public static %s CreateInstance(Entity parent)
        {			
            return parent.AddChild<%s, GObject>(CreateGObject());
        }
        ]], className, className)

        writer:writeln([[   
        public static ETTask<%s> CreateInstanceAsync(Entity parent)
        {
            ETTask<%s> tcs = ETTask<%s>.Create(true);
    
            CreateGObjectAsync((go) =>
            {
                tcs.SetResult(parent.AddChild<%s, GObject>(go));
            });
    
            return tcs;
        }
        ]], className, className, className, className)

        writer:writeln([[   
        /// <summary>
        /// 仅用于go已经实例化情况下的创建（例如另一个组件引用了此组件）
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="go"></param>
        /// <returns></returns>
        public static %s Create(Entity parent, GObject go)
        {
            return parent.AddChild<%s, GObject>(go);
        }
            ]], className, className)

        writer:writeln([[   
        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static %s GetFormPool(Entity domain, GObject go)
        {
            var fui = go.Get<%s>();
        
            if(fui == null)
            {
                fui = Create(domain, go);
            }
        
            fui.isFromFGUIPool = true;
        
            return fui;
        }
            ]], className, className)

        writer:writeln([[
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
            
            self = (%s)go;
            
            self.Add(this);
            
            var com = go.asCom;
                
            if(com != null)
            {	
                ]], classInfo.superClassName)

        for j = 0, memberCnt - 1
        do
            local memberInfo = members[j]
            --- 组装自定义成员前缀
            local memberVarName = memberVarNamePrefix .. memberInfo.varName
            if memberInfo.group == 0
            then
                if getMemberByName
                then
                    if customComponentFlagsArray[j]
                    then
                        --- 组装自定义组件前缀
                        writer:writeln('\t\t\t%s = %s.Create(this, com.GetChild("%s"));', memberVarName, classNamePrefix .. memberInfo.type, memberInfo.name)
                    elseif crossPackageFlagsArray[j]
                    then
                        --- 组装自定义组件前缀
                        writer:writeln('\t\t\t%s = %s.Create(this, com.GetChild("%s"));', memberVarName, classNamePrefix .. memberInfo.res.name, memberInfo.name)
                    else
                        writer:writeln('\t\t\t%s = (%s)com.GetChild("%s");', memberVarName, memberInfo.type, memberInfo.name)
                    end

                else
                    if customComponentFlagsArray[j]
                    then
                        --- 组装自定义组件前缀
                        writer:writeln('\t\t\t%s = %s.Create(this, com.GetChildAt(%s));', memberVarName, classNamePrefix .. memberInfo.type, memberInfo.index)
                    elseif crossPackageFlagsArray[j]
                    then
                        --- 组装自定义组件前缀
                        writer:writeln('\t\t\t%s = %s.Create(this, com.GetChildAt(%s));', memberVarName, classNamePrefix .. memberInfo.res.name, memberInfo.index)
                    else
                        writer:writeln('\t\t\t%s = (%s)com.GetChildAt(%s);', memberVarName, memberInfo.type, memberInfo.index)
                    end
                end
            elseif memberInfo.group == 1
            then
                if getMemberByName
                then
                    writer:writeln('\t\t\t%s = com.GetController("%s");', memberVarName, memberInfo.name)
                else
                    writer:writeln('\t\t\t%s = com.GetControllerAt(%s);', memberVarName, memberInfo.index)
                end
            else
                if getMemberByName
                then
                    writer:writeln('\t\t\t%s = com.GetTransition("%s");', memberVarName, memberInfo.name)
                else
                    writer:writeln('\t\t\t%s = com.GetTransitionAt(%s);', memberVarName, memberInfo.index)
                end
            end
        end
        writer:writeln('\t\t}')

        writer:writeln('\t}')

        writer:writeln([[       
        public override void Dispose()
        {
            if(IsDisposed)
            {
                return;
            }
            
            base.Dispose();
            
            self.Remove();
            self = null;
            ]])

        for j = 0, memberCnt - 1 do
            local memberInfo = members[j]
            
            --- 组装自定义成员前缀
            local memberVarName = memberVarNamePrefix .. memberInfo.varName
            if memberInfo.group == 0 then
                if customComponentFlagsArray[j] or crossPackageFlagsArray[j] then
                    writer:writeln('\t\t%s.Dispose();', memberVarName)
                end
                writer:writeln('\t\t%s = null;', memberVarName)
            elseif memberInfo.group == 1 then
                writer:writeln('\t\t%s = null;', memberVarName)
            else
                writer:writeln('\t\t%s = null;', memberVarName)
            end
        end
        writer:writeln('\t}')

        writer:writeln('}')
        writer:endBlock()

        writer:save(exportCodePath .. '/' .. className .. '.cs')
    end

    -- 写入fuipackage
    writer:reset()

    writer:writeln('namespace %s', namespaceName)
    writer:startBlock()
    writer:writeln('public static partial class FUIPackage')
    writer:startBlock()

    writer:writeln('public const string %s = "%s";', codePkgName, codePkgName)

    for i = 0, classCnt - 1 do
        local classInfo = classes[i]
        writer:writeln('public const string %s_%s = "ui://%s/%s";', codePkgName, classInfo.resName, codePkgName, classInfo.resName)
    end

    writer:endBlock() --class
    writer:endBlock() --namespace
    local binderPackageName = 'Package' .. codePkgName
    writer:save(exportCodePath .. '/' .. binderPackageName .. '.cs')
end

return HotfixCodeGenHandler