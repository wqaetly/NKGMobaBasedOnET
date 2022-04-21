import { FairyEditor } from 'csharp';
import CodeWriter from './CodeWriter';

function genCode(handler: FairyEditor.PublishHandler) {
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    let namespaceName = codePkgName;
    let ns = 'fairygui'

    if (settings.packageName)
        namespaceName = settings.packageName + '.' + namespaceName;

    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, ns);
    handler.SetupCodeFolder(exportCodePath, "ts"); //check if target folder exists, and delete old files

    let getMemberByName = settings.getMemberByName;

    let classCnt = classes.Count;
    let writer = new CodeWriter({ blockFromNewLine: false, usingTabs: true });
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let members = classInfo.members;
        writer.reset();

        writer.writeln('module %s', namespaceName);
        writer.startBlock();
        writer.writeln();
        writer.writeln('export class %s extends %s', classInfo.className, classInfo.superClassName);
        writer.startBlock();

        let memberCnt = members.Count
        for (let j: number = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            writer.writeln('public %s:%s;', memberInfo.varName, memberInfo.type);
        }
        writer.writeln('public static URL:string = "ui://%s%s";', handler.pkg.id, classInfo.resId);
        writer.writeln();

        writer.writeln('public static createInstance():%s', classInfo.className);
        writer.startBlock();
        writer.writeln('return <%s>(%s.UIPackage.createObject("%s", "%s"));', classInfo.className, ns, handler.pkg.name, classInfo.resName);
        writer.endBlock();
        writer.writeln();

        writer.writeln('protected constructFromXML(xml:any):void');
        writer.startBlock();
        writer.writeln('super.constructFromXML(xml);');
        writer.writeln();
        for (let j: number = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            if (memberInfo.group == 0) {
                if (getMemberByName)
                    writer.writeln('this.%s = <%s>(this.getChild("%s"));', memberInfo.varName, memberInfo.type, memberInfo.name);
                else
                    writer.writeln('this.%s = <%s>(this.getChildAt(%s));', memberInfo.varName, memberInfo.type, memberInfo.index);
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    writer.writeln('this.%s = this.getController("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('this.%s = this.getControllerAt(%s);', memberInfo.varName, memberInfo.index);
            }
            else {
                if (getMemberByName)
                    writer.writeln('this.%s = this.getTransition("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('this.%s = this.getTransitionAt(%s);', memberInfo.varName, memberInfo.index);
            }
        }

        writer.endBlock();

        writer.endBlock(); //class
        writer.endBlock(); //namepsace

        writer.save(exportCodePath + '/' + classInfo.className + '.ts');
    }

    writer.reset();

    let binderName = codePkgName + 'Binder';

    writer.writeln('module %s', namespaceName);
    writer.startBlock();
    writer.writeln();
    writer.writeln('export class %s', binderName);
    writer.startBlock();

    writer.writeln('public static bindAll():void');
    writer.startBlock();
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        writer.writeln('%s.UIObjectFactory.setPackageItemExtension(%s.URL, %s);', ns, classInfo.className, classInfo.className);
    }
    writer.endBlock(); //bindall

    writer.endBlock(); //class
    writer.endBlock(); //namespace

    writer.save(exportCodePath + '/' + binderName + '.ts');
}

export { genCode };