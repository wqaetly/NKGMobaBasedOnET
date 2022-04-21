import { FairyEditor } from 'csharp';
import CodeWriter from './CodeWriter';

function genCode(handler: FairyEditor.PublishHandler) {
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    let namespaceName = codePkgName;

    if (settings.packageName)
        namespaceName = settings.packageName + '.' + namespaceName;

    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, null);
    handler.SetupCodeFolder(exportCodePath, "hx"); //check if target folder exists, and delete old files

    let getMemberByName = settings.getMemberByName

    let classCnt = classes.Count;
    let writer = new CodeWriter();
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let members = classInfo.members;
        writer.reset();

        writer.writeln('package %s', namespaceName);
        writer.startBlock();
        writer.writeln('import fairygui.*;');
        writer.writeln();
        writer.writeln('class %s extends %s', classInfo.className, classInfo.superClassName);
        writer.startBlock();

        let memberCnt = members.Count
        for (let j: number = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            writer.writeln('public var %s:%s;', memberInfo.varName, memberInfo.type);
        }
        writer.writeln('public static inline var URL:String = "ui://%s%s";', handler.pkg.id, classInfo.resId);
        writer.writeln();

        writer.writeln('public static function createInstance():%s', classInfo.className);
        writer.startBlock();
        writer.writeln('return cast(UIPackage.createObject("%s", "%s"), %s);', handler.pkg.name, classInfo.resName, classInfo.className);
        writer.endBlock();
        writer.writeln();

        writer.writeln('protected override function constructFromXML(xml:FastXML):Void');
        writer.startBlock();
        writer.writeln('super.constructFromXML(xml);');
        writer.writeln();
        for (let j: number = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            if (memberInfo.group == 0) {
                if (getMemberByName)
                    writer.writeln('%s = cast(this.getChild("%s"), %s);', memberInfo.varName, memberInfo.type, memberInfo.name);
                else
                    writer.writeln('%s = cast(this.getChildAt(%s), %s);', memberInfo.varName, memberInfo.type, memberInfo.index);
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    writer.writeln('%s = this.getController("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('%s = this.getControllerAt(%s);', memberInfo.varName, memberInfo.index);
            }
            else {
                if (getMemberByName)
                    writer.writeln('%s = this.getTransition("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('%s = this.getTransitionAt(%s);', memberInfo.varName, memberInfo.index);
            }
        }
        writer.endBlock();

        writer.endBlock(); //class
        writer.endBlock(); //namepsace

        writer.save(exportCodePath + '/' + classInfo.className + '.hx');
    }

    writer.reset();

    let binderName = codePkgName + 'Binder';

    writer.writeln('package %s', namespaceName);
    writer.startBlock();
    writer.writeln('import fairygui.*;');
    writer.writeln();
    writer.writeln('class %s', binderName);
    writer.startBlock();

    writer.writeln('public static function bindAll():Void');
    writer.startBlock();
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        writer.writeln('UIObjectFactory.setPackageItemExtension(%s.URL, cast %s);', classInfo.className, classInfo.className);
    }
    writer.endBlock(); //bindall

    writer.endBlock(); //class
    writer.endBlock(); //namespace

    writer.save(exportCodePath + '/' + binderName + '.hx');
}

export { genCode };