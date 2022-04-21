import { FairyEditor } from 'csharp';
import CodeWriter from './CodeWriter';

function genCode(handler: FairyEditor.PublishHandler) {
    let settings = (<FairyEditor.GlobalPublishSettings>handler.project.GetSettings("Publish")).codeGeneration;
    let codePkgName = handler.ToFilename(handler.pkg.name); //convert chinese to pinyin, remove special chars etc.
    let exportCodePath = handler.exportCodePath + '/' + codePkgName;
    let namespaceName = codePkgName;
    let isMonoGame = handler.project.type == FairyEditor.ProjectType.MonoGame;

    if (settings.packageName)
        namespaceName = settings.packageName + '.' + namespaceName;

    //CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    let classes = handler.CollectClasses(settings.ignoreNoname, settings.ignoreNoname, null);
    handler.SetupCodeFolder(exportCodePath, "cs"); //check if target folder exists, and delete old files

    let getMemberByName = settings.getMemberByName;

    let classCnt = classes.Count;
    let writer = new CodeWriter();
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        let members = classInfo.members;
        writer.reset();

        writer.writeln('using FairyGUI;');
        writer.writeln('using FairyGUI.Utils;');
        writer.writeln();
        writer.writeln('namespace %s', namespaceName);
        writer.startBlock();
        writer.writeln('public partial class %s : %s', classInfo.className, classInfo.superClassName);
        writer.startBlock();

        let memberCnt = members.Count
        for (let j: number = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            writer.writeln('public %s %s;', memberInfo.type, memberInfo.varName);
        }
        writer.writeln('public const string URL = "ui://%s%s";', handler.pkg.id, classInfo.resId);
        writer.writeln();

        writer.writeln('public static %s CreateInstance()', classInfo.className);
        writer.startBlock();
        writer.writeln('return (%s)UIPackage.CreateObject("%s", "%s");', classInfo.className, handler.pkg.name, classInfo.resName);
        writer.endBlock();
        writer.writeln();

        if (isMonoGame) {
            writer.writeln("protected override void OnConstruct()");
            writer.startBlock();
        }
        else {
            writer.writeln('public override void ConstructFromXML(XML xml)');
            writer.startBlock();
            writer.writeln('base.ConstructFromXML(xml);');
            writer.writeln();
        }
        for (let j: number = 0; j < memberCnt; j++) {
            let memberInfo = members.get_Item(j);
            if (memberInfo.group == 0) {
                if (getMemberByName)
                    writer.writeln('%s = (%s)GetChild("%s");', memberInfo.varName, memberInfo.type, memberInfo.name);
                else
                    writer.writeln('%s = (%s)GetChildAt(%s);', memberInfo.varName, memberInfo.type, memberInfo.index);
            }
            else if (memberInfo.group == 1) {
                if (getMemberByName)
                    writer.writeln('%s = GetController("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('%s = GetControllerAt(%s);', memberInfo.varName, memberInfo.index);
            }
            else {
                if (getMemberByName)
                    writer.writeln('%s = GetTransition("%s");', memberInfo.varName, memberInfo.name);
                else
                    writer.writeln('%s = GetTransitionAt(%s);', memberInfo.varName, memberInfo.index);
            }
        }
        writer.endBlock();

        writer.endBlock(); //class
        writer.endBlock(); //namepsace

        writer.save(exportCodePath + '/' + classInfo.className + '.cs');
    }

    writer.reset();

    let binderName = codePkgName + 'Binder';

    writer.writeln('using FairyGUI;');
    writer.writeln();
    writer.writeln('namespace %s', namespaceName);
    writer.startBlock();
    writer.writeln('public class %s', binderName);
    writer.startBlock();

    writer.writeln('public static void BindAll()');
    writer.startBlock();
    for (let i: number = 0; i < classCnt; i++) {
        let classInfo = classes.get_Item(i);
        writer.writeln('UIObjectFactory.SetPackageItemExtension(%s.URL, typeof(%s));', classInfo.className, classInfo.className);
    }
    writer.endBlock(); //bindall

    writer.endBlock(); //class
    writer.endBlock(); //namespace

    writer.save(exportCodePath + '/' + binderName + '.cs');
}

export { genCode };