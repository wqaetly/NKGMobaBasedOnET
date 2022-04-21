#if UNITY_EDITOR
using System.IO;
using ET;
using ILRuntime.Runtime.CLRBinding;
using MonKey;
using UnityEditor;

public class ILRuntimeCLRBinding
{
    private static string s_ILRuntimeAnalysisHotfixDllDir = "Assets/Res/Code/Hotfix.dll.bytes";
    private static string s_ILRuntimeGeneratedCodeOutputDir = "Assets/Mono/ILRuntimeGenerate/ILBindingAuto";
    
    [Command("ETEditor_GenerateCLRBindingByAnalysis", "分析热更dll调用引用来生成CLR绑定代码", Category = "ETEditor")]
    static void GenerateCLRBindingByAnalysis()
    {
        //分析热更dll调用引用来生成绑定代码
        ILRuntime.Runtime.Enviorment.AppDomain domain = new ILRuntime.Runtime.Enviorment.AppDomain();
        using (FileStream fsHotfix = new FileStream(s_ILRuntimeAnalysisHotfixDllDir, FileMode.Open, FileAccess.Read))
        {
            domain.LoadAssembly(fsHotfix);
            ILHelper.InitILRuntime(domain);
            BindingCodeGenerator.GenerateBindingCode(domain, s_ILRuntimeGeneratedCodeOutputDir);
        }

        AssetDatabase.Refresh();
    }
}
#endif