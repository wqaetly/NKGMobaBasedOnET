local function genCode(handler,customSettingTable)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath .. '/' .. codePkgName
    local namespaceName = codePkgName
    
    if settings.packageName ~= nil and settings.packageName ~= '' then
        namespaceName = settings.packageName .. '.' .. namespaceName;
    end
    
    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, nil)
    handler:SetupCodeFolder(exportCodePath, "cs") --check if target folder exists, and delete old files

    local getMemberByName = settings.getMemberByName

    local classCnt = classes.Count
    local writer = CodeWriter.new()
    for i = 0, classCnt - 1 do
        local classInfo = classes[i]
        local members = classInfo.members

        writer:reset()

        writer:writeln('using FairyGUI;')
        writer:writeln('using FairyGUI.Utils;')
        writer:writeln()
        writer:writeln('namespace %s', 'ETModel')
        writer:startBlock()
        writer:writeln('public partial class %s : %s', classInfo.className, classInfo.superClassName)
        writer:startBlock()

        -- 是否为跨包组件标记数组
        local crossPackageFlagsArray = {}

        local memberCnt = members.Count
        for j = 0, memberCnt - 1 do
            local memberInfo = members[j]
            local typeName = memberInfo.type
            crossPackageFlagsArray[j] = false
            -- 判断是不是跨包类型组件
            if memberInfo.res ~= nil then
                typeName = memberInfo.res.name
                crossPackageFlagsArray[j] = true
            end
            writer:writeln('public %s %s;', typeName, memberInfo.varName)
        end
        writer:writeln('public const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln('public static %s CreateInstance()', classInfo.className)
        writer:startBlock()
        writer:writeln('return (%s)UIPackage.CreateObject("%s", "%s");', classInfo.className, handler.pkg.name, classInfo.resName)
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
            if memberInfo.group == 0 then
                if getMemberByName then
                    if crossPackageFlagsArray[j]
                    then
                        writer:writeln('%s = (%s)GetChild("%s");', memberInfo.varName, memberInfo.res.name, memberInfo.name)
                    else
                        writer:writeln('%s = (%s)GetChild("%s");', memberInfo.varName, memberInfo.type, memberInfo.name)
                    end
                else
                    if crossPackageFlagsArray[j]
                    then
                        writer:writeln('%s = (%s)GetChildAt(%s);', memberInfo.varName, memberInfo.res.name, memberInfo.index)
                    else
                        writer:writeln('%s = (%s)GetChildAt(%s);', memberInfo.varName, memberInfo.type, memberInfo.index)
                    end

                end
            elseif memberInfo.group == 1 then
                if getMemberByName then
                    writer:writeln('%s = GetController("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = GetControllerAt(%s);', memberInfo.varName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('%s = GetTransition("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = GetTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                end
            end
        end
        writer:endBlock()

        writer:endBlock() --class
        writer:endBlock() --namepsace

        writer:save(exportCodePath .. '/' .. classInfo.className .. '.cs')
    end

    writer:reset()

    local binderName = codePkgName .. 'Binder'

    writer:writeln('using FairyGUI;')
    writer:writeln()
    writer:writeln('namespace %s', 'ETModel')
    writer:startBlock()
    writer:writeln('public class %s', binderName)
    writer:startBlock()

    writer:writeln('public static void BindAll()')
    writer:startBlock()
    for i = 0, classCnt - 1 do
        local classInfo = classes[i]
        writer:writeln('UIObjectFactory.SetPackageItemExtension(%s.URL, typeof(%s));', classInfo.className, classInfo.className)
    end
    writer:endBlock() --bindall

    writer:endBlock() --class
    writer:endBlock() --namespace

    writer:save(exportCodePath .. '/' .. binderName .. '.cs')
end

return genCode