local function genCode(handler)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath..'/'..codePkgName
    local namespaceName = codePkgName
    local ns = 'fairygui'
    
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
        writer:reset()

        writer:writeln('module %s', namespaceName)
        writer:startBlock()
        writer:writeln()
        writer:writeln('export class %s extends %s', classInfo.className, classInfo.superClassName)
        writer:startBlock()

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

        writer:writeln('protected constructFromXML(xml:any):void')
        writer:startBlock()
        writer:writeln('super.constructFromXML(xml);')
        writer:writeln()
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
        writer:endBlock() --namepsace

        writer:save(exportCodePath..'/'..classInfo.className..'.ts')
    end

    writer:reset()

    local binderName = codePkgName..'Binder'

    writer:writeln('module %s', namespaceName)
    writer:startBlock()
    writer:writeln()
    writer:writeln('export class %s', binderName)
    writer:startBlock()

    writer:writeln('public static bindAll():void')
    writer:startBlock()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('%s.UIObjectFactory.setPackageItemExtension(%s.URL, %s);', ns, classInfo.className, classInfo.className)
    end
    writer:endBlock() --bindall

    writer:endBlock() --class
    writer:endBlock() --namespace
    
    writer:save(exportCodePath..'/'..binderName..'.ts')
end

return genCode