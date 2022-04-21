local function genCode(handler)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath..'/'..codePkgName
    local namespaceName = codePkgName
    local ns = 'fgui'
    
    if settings.packageName~=nil and settings.packageName~='' then
        namespaceName = settings.packageName..'.'..namespaceName;
    end

    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, ns)
    handler:SetupCodeFolder(exportCodePath, "ts") --check if target folder exists, and delete old files

    local getMemberByName = settings.getMemberByName

    local classCnt = classes.Count
    local writer = CodeWriter.new({ blockFromNewLine=false, usingTabs=true  })
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        local members = classInfo.members
        local references = classInfo.references
        writer:reset()

        local refCount = references.Count
        if refCount>0 then
            for j=0,refCount-1 do
                local ref = references[j]
                writer:writeln('import %s from "./%s";', ref, ref)
            end
            writer:writeln()
        end

        if handler.project.type==ProjectType.ThreeJS then
            writer:writeln('import * as fgui from "fairygui-three";');
            if refCount==0 then writer:writeln() end
        end

        writer:writeln('export default class %s extends %s', classInfo.className, classInfo.superClassName)
        writer:startBlock()
        writer:writeln()
        
        local memberCnt = members.Count
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            writer:writeln('public %s:%s;', memberInfo.varName, memberInfo.type)
        end
        writer:writeln('public static URL:string = "ui://%s%s";', handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln('public static createInstance():%s', classInfo.className)
        writer:startBlock()
        writer:writeln('return <%s>(%s.UIPackage.createObject("%s", "%s"));', classInfo.className, ns, handler.pkg.name, classInfo.resName)
        writer:endBlock()
        writer:writeln()

        writer:writeln('protected onConstruct():void')
        writer:startBlock()
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            if memberInfo.group==0 then
                if getMemberByName then
                    writer:writeln('this.%s = <%s>(this.getChild("%s"));', memberInfo.varName, memberInfo.type, memberInfo.name)
                else
                    writer:writeln('this.%s = <%s>(this.getChildAt(%s));', memberInfo.varName, memberInfo.type, memberInfo.index)
                end
            elseif memberInfo.group==1 then
                if getMemberByName then
                    writer:writeln('this.%s = this.getController("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('this.%s = this.getControllerAt(%s);', memberInfo.varName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('this.%s = this.getTransition("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('this.%s = this.getTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                end
            end
        end
        writer:endBlock()

        writer:endBlock() --class

        writer:save(exportCodePath..'/'..classInfo.className..'.ts')
    end

    writer:reset()

    local binderName = codePkgName..'Binder'

    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('import %s from "./%s";', classInfo.className, classInfo.className)
    end

    if handler.project.type==ProjectType.ThreeJS then
        writer:writeln('import * as fgui from "fairygui-three";');
        writer:writeln();
    end
    
    writer:writeln()
    writer:writeln('export default class %s', binderName)
    writer:startBlock()

    writer:writeln('public static bindAll():void')
    writer:startBlock()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('%s.UIObjectFactory.setExtension(%s.URL, %s);', ns, classInfo.className, classInfo.className)
    end
    writer:endBlock() --bindall

    writer:endBlock() --class
    
    writer:save(exportCodePath..'/'..binderName..'.ts')
end

return genCode