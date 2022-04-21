---@class ModelCodeGenHandler 非热更层代码生成器
local ModelCodeGenHandler = {}

--- 执行生成非热更层代码
---@param handler CS.FairyEditor.PublishHandler
---@param codeGenConfig CodeGenConfig
function ModelCodeGenHandler.Do(handler, codeGenConfig)
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.

    --- 从自定义配置中读取路径和命名空间
    local exportCodePath = codeGenConfig.ModelCodeOutPutPath .. '/' .. codePkgName
    local namespaceName = codeGenConfig.ModelNameSpace

    --- 初始化自定义组件名前缀
    local classNamePrefix = codeGenConfig.ClassNamePrefix
    --- 初始化自定义成员变量名前缀
    local memberVarNamePrefix = codeGenConfig.MemerVarNamePrefix
    
    --- 从FGUI编辑器中读取配置
    ---@type CS.FairyEditor.GlobalPublishSettings.CodeGenerationConfig
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local getMemberByName = settings.getMemberByName

    --- 所有将要导出的类
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
        writer:writeln('using FairyGUI.Utils;')
        writer:writeln()
        writer:writeln('namespace %s', namespaceName)
        writer:startBlock()
        writer:writeln('public partial class %s%s : %s', classNamePrefix, classInfo.className, classInfo.superClassName)
        writer:startBlock()

        -- 是否为自定义组件标记数组
        local crossPackageFlagsArray = {}

        local memberCnt = members.Count
        for j = 0, memberCnt - 1 do
            ---@type CS.FairyEditor.PublishHandler.MemberInfo
            local memberInfo = members[j]
            local typeName =  memberInfo.type
            crossPackageFlagsArray[j] = false
            -- 判断是不是自定义类型组件
            if memberInfo.res ~= nil then
                --- 组装自定义组件前缀
                typeName = classNamePrefix .. memberInfo.res.name
                crossPackageFlagsArray[j] = true
            end
            --- 组装自定义成员前缀
            writer:writeln('public %s %s;', typeName, memberVarNamePrefix .. memberInfo.varName)
        end

        --- 组装自定义组件前缀
        local className = classNamePrefix .. classInfo.className

        writer:writeln('public const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln('public static %s CreateInstance()', className)
        writer:startBlock()
        writer:writeln('return (%s)UIPackage.CreateObject("%s", "%s", typeof(%s));', className, handler.pkg.name, classInfo.resName, className)
        writer:endBlock()
        writer:writeln()

        if handler.project.type == ProjectType.MonoGame then
            writer:writeln("protected override void OnConstruct()")
            writer:startBlock()
        else
            writer:writeln('public override void ConstructFromXML(XML xml)')
            writer:startBlock()
            writer:writeln('base.ConstructFromXML(xml);')
            writer:writeln()
        end
        for j = 0, memberCnt - 1 do
            local memberInfo = members[j]
            --- 组装自定义成员前缀
            local memberVarName = memberVarNamePrefix .. memberInfo.varName
            if memberInfo.group == 0 then
                if getMemberByName then
                    if crossPackageFlagsArray[j]
                    then
                        --- 组装自定义组件前缀
                        writer:writeln('%s = (%s)GetChild("%s");', memberVarName, classNamePrefix .. memberInfo.res.name, memberInfo.name)
                    else
                        writer:writeln('%s = (%s)GetChild("%s");', memberVarName, memberInfo.type, memberInfo.name)
                    end
                else
                    if crossPackageFlagsArray[j]
                    then
                        --- 组装自定义组件前缀
                        writer:writeln('%s = (%s)GetChildAt(%s);', memberVarName, classNamePrefix .. memberInfo.res.name, memberInfo.index)
                    else
                        writer:writeln('%s = (%s)GetChildAt(%s);', memberVarName, memberInfo.type, memberInfo.index)
                    end

                end
            elseif memberInfo.group == 1 then
                if getMemberByName then
                    writer:writeln('%s = GetController("%s");', memberVarName, memberInfo.name)
                else
                    writer:writeln('%s = GetControllerAt(%s);', memberVarName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('%s = GetTransition("%s");', memberVarName, memberInfo.name)
                else
                    writer:writeln('%s = GetTransitionAt(%s);', memberVarName, memberInfo.index)
                end
            end
        end
        writer:endBlock()

        writer:endBlock() --class
        writer:endBlock() --namepsace

        writer:save(exportCodePath .. '/' .. className .. '.cs')
    end

    writer:reset()

    --- 为了统一命名，这里也组装自定义组件前缀
    local binderName = classNamePrefix .. codePkgName .. 'Binder'

    writer:writeln('using FairyGUI;')
    writer:writeln()
    writer:writeln('namespace %s', 'ET')
    writer:startBlock()
    writer:writeln('public class %s', binderName)
    writer:startBlock()

    writer:writeln('public static void BindAll()')
    writer:startBlock()
    for i = 0, classCnt - 1 do
        local classInfo = classes[i]
        --- 组装自定义组件前缀
        local className = classNamePrefix .. classInfo.className
        writer:writeln('UIObjectFactory.SetPackageItemExtension(%s.URL, typeof(%s));', className, className)
    end
    writer:endBlock() --bindall

    writer:endBlock() --class
    writer:endBlock() --namespace

    writer:save(exportCodePath .. '/' .. binderName .. '.cs')
end

return ModelCodeGenHandler