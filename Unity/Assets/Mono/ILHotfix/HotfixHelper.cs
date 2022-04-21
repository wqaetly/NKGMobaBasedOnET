using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ILRuntime.CLR.TypeSystem;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace ET
{
    public static class HotfixHelper
    {
        private static MemoryStream s_hotfixDllStream;
        private static MemoryStream s_hotfixPdbStream;
        
        private static IStaticMethod _entryMethod;

        private static AppDomain _appDomain;
        private static Assembly _ModelAssembly;
        private static Assembly _ModelViewassembly;
        private static Assembly _Hotfixassembly;
        private static Assembly _HotfixViewassembly;

        /// <summary>
        /// 这里开始正式进入游戏逻辑
        /// </summary>
        public static void GoToHotfix()
        {
            byte[] hotfixdllByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixDllPath("Hotfix"))
                .bytes;
            byte[] hotfixpdbByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixPdbPath("Hotfix"))
                .bytes;
            byte[] hotfixViewdllByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixDllPath("HotfixView"))
                .bytes;
            byte[] hotfixViewpdbByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixPdbPath("HotfixView"))
                .bytes;
            byte[] ModeldllByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixDllPath("Model"))
                .bytes;
            byte[] ModelpdbByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixPdbPath("Model"))
                .bytes;
            byte[] ModelViewdllByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixDllPath("ModelView"))
                .bytes;
            byte[] ModelViewpdbByte = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetHotfixPdbPath("ModelView"))
                .bytes;
            if (GlobalDefine.ILRuntimeMode)
            {
                _appDomain = new ILRuntime.Runtime.Enviorment.AppDomain();
                s_hotfixDllStream = new MemoryStream(hotfixdllByte);
                s_hotfixPdbStream = new MemoryStream(hotfixpdbByte);
                _appDomain.LoadAssembly(s_hotfixDllStream, s_hotfixPdbStream,
                    new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());

                ILHelper.InitILRuntime(_appDomain);
                
                _entryMethod = new ILStaticMethod(_appDomain, "ET.InitEntry", "RegFunction", 0);
            }
            else
            {
                _ModelAssembly = Assembly.Load(ModeldllByte, ModelpdbByte);
                _ModelViewassembly = Assembly.Load(ModelViewdllByte, ModelViewpdbByte);
                _Hotfixassembly = Assembly.Load(hotfixdllByte, hotfixpdbByte);
                _HotfixViewassembly = Assembly.Load(hotfixViewdllByte, hotfixViewpdbByte);
                _entryMethod = new MonoStaticMethod(_ModelViewassembly, "ET.InitEntry", "RegFunction");
            }

            _entryMethod.Run();
        }

        public static Type[] GetAssemblyTypes()
        {
            List<Type> types = new List<Type>();
            if (GlobalDefine.ILRuntimeMode)
            {
                types = _appDomain.LoadedTypes.Values.Select(t => t.ReflectionType).ToList();
            }
            else
            {
                types.AddRange(_ModelAssembly.GetTypes());
                types.AddRange(_ModelViewassembly.GetTypes());
                types.AddRange(_Hotfixassembly.GetTypes());
                types.AddRange(_HotfixViewassembly.GetTypes());
            }

            return types.ToArray();
        }

        public static List<Type> GetIlrAttributeTypes(List<Type> types)
        {
            List<Type> attributeTypes = new List<Type>();
            foreach (Type item in types)
            {
                if (item.IsAbstract)
                {
                    continue;
                }

                if (item.IsSubclassOf(typeof(Attribute)))
                {
                    attributeTypes.Add(item);
                }
            }

            return attributeTypes;
        }
    }
}