using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using ET;
using MonKey;

[System.Reflection.Obfuscation(Exclude = true)]
public class ILRuntimeCrossBinding
{
    [Command("ETEditor_GenerateCrossbindAdapter", "生成跨域继承适配器", Category = "ETEditor")]
    static void GenerateCrossbindAdapter()
    {
        //由于跨域继承特殊性太多，自动生成无法实现完全无副作用生成，所以这里提供的代码自动生成主要是给大家生成个初始模版，简化大家的工作
        //大多数情况直接使用自动生成的模版即可，如果遇到问题可以手动去修改生成后的文件，因此这里需要大家自行处理是否覆盖的问题

        // using (System.IO.StreamWriter sw = new System.IO.StreamWriter("Assets/Mono/ILRuntimeGenerate/ILAdaptor/IDisposableAdapter.cs"))
        // {
        //     sw.WriteLine(ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(IDisposable), "ET"));
        // }

        using (System.IO.StreamWriter sw =
            new System.IO.StreamWriter("Assets/Mono/ILRuntimeGenerate/ILAdaptor/ISupportInitializeAdapter.cs"))
        {
            sw.WriteLine(
                ILRuntime.Runtime.Enviorment.CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(
                    typeof(System.ComponentModel.ISupportInitialize), "ET"));
        }

        AssetDatabase.Refresh();

        Debug.Log("生成适配器完成");
    }
}