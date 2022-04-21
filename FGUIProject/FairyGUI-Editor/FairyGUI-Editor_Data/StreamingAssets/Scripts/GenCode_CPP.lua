local function genCode(handler)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath..'/'..codePkgName
    local namespaceName = codePkgName
    local binderName = codePkgName..'Binder'

    if settings.packageName~=nil and settings.packageName~='' then
        namespaceName = settings.packageName..'.'..namespaceName;
    end

    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, nil)
    handler:SetupCodeFolder(exportCodePath, "cpp,h") --check if target folder exists, and delete old files

    local getMemberByName = settings.getMemberByName

    local classCnt = classes.Count
    local writer = CodeWriter.new()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        local members = classInfo.members
        local references = classInfo.references
        writer:reset()

        writer:writeln('#ifndef __%s_%s_H__', codePkgName, classInfo.className)
        writer:writeln('#define __%s_%s_H__', codePkgName, classInfo.className)
        writer:writeln()
        writer:writeln('#include "FairyGUI.h"')
        writer:writeln()
        
        writer:writeln('namespace %s', namespaceName)
        writer:startBlock()

        local refCount = references.Count
        if refCount>0 then
            for j=0,refCount-1 do
                local ref = references[j]
                writer:writeln('class %s;', ref)
            end
        end

        writer:writeln('class %s : public %s', classInfo.className, classInfo.superClassName)
        writer:startBlock()

        writer:writeln('public:')
        writer:incIndent()
        writer:writeln('static const std::string URL;')
        writer:writeln('static %s* create();', classInfo.className)
        writer:writeln()
        local memberCnt = members.Count
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            writer:writeln('%s* %s;', memberInfo.type, memberInfo.varName)
        end
        writer:decIndent()
        writer:writeln()

        writer:writeln('protected:')
        writer:incIndent()
        writer:writeln('virtual void onConstruct() override;')
        writer:decIndent()
        writer:writeln()

        writer:writeln('private:')
        writer:incIndent()
        writer:writeln('static %s* createByBinder();', classInfo.className)
        writer:writeln('friend class %s;', binderName)
        writer:decIndent()

        writer:endBlock()
        writer:endBlock()

        writer:writeln()
        writer:writeln('#endif')

        writer:save(exportCodePath..'/'..classInfo.className..'.h')

        ----------------

        writer:reset()
        writer:writeln('#include "%s.h"', binderName)
        writer:writeln()
        writer:writeln('namespace %s', namespaceName)
        writer:startBlock()
        writer:writeln('USING_NS_FGUI;')
        writer:writeln()
        writer:writeln('const std::string %s::URL = "ui://%s%s";', classInfo.className, handler.pkg.id, classInfo.resId)
        writer:writeln()

        writer:writeln('%s* %s:create()', classInfo.className, classInfo.className)
        writer:startBlock()
        writer:writeln('return dynamic_cast<%s*>(UIPackage::createObject("%s", "%s"));', classInfo.className, handler.pkg.name, classInfo.resName)
        writer:endBlock()
        writer:writeln()

        writer:writeln('%s* %s::createByBinder()', classInfo.className, classInfo.className)
        writer:startBlock()
        writer:writeln('%s *pRet = new(std::nothrow) %s();', classInfo.className, classInfo.className)
        writer:writeln('if (pRet && pRet->init())')
        writer:startBlock()
        writer:writeln('pRet->autorelease();')
        writer:writeln('return pRet;')
        writer:endBlock()
        writer:writeln('else')
        writer:startBlock()
        writer:writeln('delete pRet;')
        writer:writeln('pRet = nullptr;')
        writer:writeln('return nullptr;')
        writer:endBlock()
        writer:endBlock()
        writer:writeln()

        writer:writeln('void %s::onConstruct()', classInfo.className)
        writer:startBlock()

        for j=0,memberCnt-1 do
            local memberInfo = members[j]
            if memberInfo.group==0 then
                if getMemberByName then
                    writer:writeln('%s = dynamic_cast<%s*>(getChild("%s"));', memberInfo.varName, memberInfo.type, memberInfo.name)
                else
                    writer:writeln('%s = dynamic_cast<%s*>(getChildAt(%s));', memberInfo.varName, memberInfo.type, memberInfo.index)
                end
            elseif memberInfo.group==1 then
                if getMemberByName then
                    writer:writeln('%s = getController("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = getControllerAt(%s);', memberInfo.varName, memberInfo.index)
                end
            else
                if getMemberByName then
                    writer:writeln('%s = getTransition("%s");', memberInfo.varName, memberInfo.name)
                else
                    writer:writeln('%s = getTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                end
            end
        end

        writer:endBlock()
        writer:endBlock()

        writer:save(exportCodePath..'/'..classInfo.className..'.cpp')
    end

    writer:reset()

    writer:writeln('#ifndef __%s_%s_H__', codePkgName, binderName)
    writer:writeln('#define __%s_%s_H__', codePkgName, binderName)
    writer:writeln()
    writer:writeln('#include "FairyGUI.h"')

    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('#include "%s.h";', classInfo.className)
    end

    writer:writeln()
    writer:writeln('namespace %s', namespaceName)
    writer:startBlock()
    writer:writeln('class %s', binderName)
    writer:startBlock()
    writer:writeln('public:')
    writer:incIndent()
    writer:writeln('static void bindAll();')
    writer:decIndent()

    writer:endBlock()
    writer:endBlock()
    writer:writeln()
    writer:writeln('#endif')

    writer:save(exportCodePath..'/'..binderName..'.h')

    -----------------------------------

    writer:reset()
    writer:writeln('#include "%s.h"', binderName)
    writer:writeln()
    writer:writeln('namespace %s', namespaceName)
    writer:startBlock()
    writer:writeln('USING_NS_FGUI;')
    writer:writeln()

    writer:writeln('void %s:bindAll()', binderName)
    writer:startBlock()
    for i=0,classCnt-1 do
        local classInfo = classes[i]
        writer:writeln('UIObjectFactory::setExtension(%s::URL, std::bind(&%s::createByBinder));', classInfo.className, classInfo.className)
    end
    writer:endBlock()

    writer:endBlock()
    
    writer:save(exportCodePath..'/'..binderName..'.cpp')
end

return genCode