local function genCode(handler)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath..'/'..codePkgName
    local namespaceName = codePkgName
    
    if settings.packageName~=nil and settings.packageName~='' then
        namespaceName = settings.packageName..'.'..namespaceName;
    end

    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, nil)
    handler:SetupCodeFolder(exportCodePath, "hx") --check if target folder exists, and delete old files

    local getMemberByName = settings.getMemberByName

    local classCnt = classes.Count
    local writer = CodeWriter.new()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        local members = classInfo.members
        writer:reset()

        writer:writeln('package %s', namespaceName)
        writer:startBlock()
        writer:writeln('import fairygui.*;')
        writer:writeln()
        writer:writeln('class %s extends %s', classInfo.className, classInfo.superClassName)
        writer:startBlock()

        local memberCnt = members.Count
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            writer:writeln('public var %s:%s;', memberInfo.varName, memberInfo.type)
        end
        writer:writeln('public static inline var URL:String = "ui://%s%s";', handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln('public static function createInstance():%s', classInfo.className)
        writer:startBlock()
        writer:writeln('return cast(UIPackage.createObject("%s", "%s"), %s);', handler.pkg.name, classInfo.resName, classInfo.className)
        writer:endBlock()
        writer:writeln()

        writer:writeln('protected override function constructFromXML(xml:FastXML):Void')
        writer:startBlock()
        writer:writeln('super.constructFromXML(xml);')
        writer:writeln()
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            if memberInfo.group==0 then
                if getMemberByName then
                    writer:writeln('%s = cast(this.getChild("%s"), %s);', memberInfo.varName, memberInfo.type, memberInfo.name)
                else
                    writer:writeln('%s = cast(this.getChildAt(%s), %s);', memberInfo.varName, memberInfo.type, memberInfo.index)
                end
            elseif memberInfo.group==1 then
                if getMemberByName then
                    writer:writeln('%s = this.getController("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = this.getControllerAt(%s);', memberInfo.varName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('%s = this.getTransition("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = this.getTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                end
            end
        end
        writer:endBlock()

        writer:endBlock() --class
        writer:endBlock() --namepsace

        writer:save(exportCodePath..'/'..classInfo.className..'.hx')
    end

    writer:reset()

    local binderName = codePkgName..'Binder'

    writer:writeln('package %s', namespaceName)
    writer:startBlock()
    writer:writeln('import fairygui.*;')
    writer:writeln()
    writer:writeln('class %s', binderName)
    writer:startBlock()

    writer:writeln('public static function bindAll():Void')
    writer:startBlock()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('UIObjectFactory.setPackageItemExtension(%s.URL, cast %s);', classInfo.className, classInfo.className)
    end
    writer:endBlock() --bindall

    writer:endBlock() --class
    writer:endBlock() --namespace
    
    writer:save(exportCodePath..'/'..binderName..'.hx')
end

return genCode