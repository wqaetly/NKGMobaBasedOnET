//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2021年4月28日 12:02:49
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Utils;
using ILRuntime.Runtime;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ProtoBuf;
using UnityEngine;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace ET
{
    public static class ILHelper
    {
        public static void LaunchDebugService(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
#if DEBUG && (UNITY_EDITOR || UNITY_ANDROID || UNITY_IPHONE)
            //由于Unity的Profiler接口只允许在主线程使用，为了避免出异常，需要告诉ILRuntime主线程的线程ID才能正确将函数运行耗时报告给Profiler
            appdomain.UnityMainThreadID = Thread.CurrentThread.ManagedThreadId;
            appdomain.DebugService.StartDebugService(56000);
#endif
        }

        public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            // 注册跨域继承适配器
            RegisterCrossBindingAdaptor(appdomain);
            // 注册重定向函数
            RegisterILRuntimeCLRRedirection(appdomain);
            // 注册委托
            RegisterMethodDelegate(appdomain);
            RegisterFunctionDelegate(appdomain);
            RegisterDelegateConvertor(appdomain);
            // 注册值类型绑定
            RegisterValueTypeBinder(appdomain);

            ////////////////////////////////////
            // CLR绑定的注册，一定要记得将CLR绑定的注册写在CLR重定向的注册后面，因为同一个方法只能被重定向一次，只有先注册的那个才能生效
            ////////////////////////////////////
            Type t = Type.GetType("ILRuntime.Runtime.Generated.CLRBindings");
            if (t != null)
            {
                t.GetMethod("Initialize")?.Invoke(null, new object[] {appdomain});
            }
        }

        /// <summary>
        /// 注册跨域继承适配器
        /// </summary>
        /// <param name="appdomain"></param>
        static void RegisterCrossBindingAdaptor(AppDomain appdomain)
        {
            //自动注册一波，无需再手动添加了，如果想要性能也可以手动自己加
            Assembly assembly = typeof(ILHelper).Assembly;
            foreach (Type type in assembly.GetTypes().ToList()
                .FindAll(t => t.IsSubclassOf(typeof(CrossBindingAdaptor))))
            {
                object obj = Activator.CreateInstance(type);
                CrossBindingAdaptor adaptor = obj as CrossBindingAdaptor;
                if (adaptor == null)
                {
                    continue;
                }

                appdomain.RegisterCrossBindingAdaptor(adaptor);
            }
        }

        /// <summary>
        /// 注册CLR重定向
        /// </summary>
        /// <param name="appdomain"></param>
        static unsafe void RegisterILRuntimeCLRRedirection(AppDomain appdomain)
        {
            //注册3种Log
            Type debugType = typeof(Debug);
            var logMethod = debugType.GetMethod("Log", new[] {typeof(object)});
            appdomain.RegisterCLRMethodRedirection(logMethod, Log);
            var logWarningMethod = debugType.GetMethod("LogWarning", new[] {typeof(object)});
            appdomain.RegisterCLRMethodRedirection(logWarningMethod, LogWarning);
            var logErrorMethod = debugType.GetMethod("LogError", new[] {typeof(object)});
            appdomain.RegisterCLRMethodRedirection(logErrorMethod, LogError);

            //LitJson适配
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);
            //Protobuf适配
            PBType.RegisterILRuntimeCLRRedirection(appdomain);
        }

        /// <summary>
        /// Debug.LogError 实现
        /// </summary>
        /// <param name="__intp"></param>
        /// <param name="__esp"></param>
        /// <param name="__mStack"></param>
        /// <param name="__method"></param>
        /// <param name="isNewObj"></param>
        /// <returns></returns>
        unsafe static StackObject* LogError(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

            object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var stacktrace = __domain.DebugService.GetStackTrace(__intp);
            Debug.LogError(message + "\n\n==========ILRuntime StackTrace==========\n" + stacktrace);
            return __ret;
        }

        /// <summary>
        /// Debug.LogWarning 实现
        /// </summary>
        /// <param name="__intp"></param>
        /// <param name="__esp"></param>
        /// <param name="__mStack"></param>
        /// <param name="__method"></param>
        /// <param name="isNewObj"></param>
        /// <returns></returns>
        unsafe static StackObject* LogWarning(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

            object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var stacktrace = __domain.DebugService.GetStackTrace(__intp);
            Debug.LogWarning(message + "\n\n==========ILRuntime StackTrace==========\n" + stacktrace);
            return __ret;
        }

        /// <summary>
        /// Debug.Log 实现
        /// </summary>
        /// <param name="__intp"></param>
        /// <param name="__esp"></param>
        /// <param name="__mStack"></param>
        /// <param name="__method"></param>
        /// <param name="isNewObj"></param>
        /// <returns></returns>
        unsafe static StackObject* Log(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);
            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);

            object message = typeof(object).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var stacktrace = __domain.DebugService.GetStackTrace(__intp);
            Debug.Log(message + "\n\n==========ILRuntime StackTrace==========\n" + stacktrace);
            return __ret;
        }

        /// <summary>
        /// 注册委托（不带返回值）
        /// </summary>
        static void RegisterMethodDelegate(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, MemoryStream>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, IPEndPoint>();

            appdomain.DelegateManager.RegisterMethodDelegate<System.Single, LitJson.JsonWriter>();

            #region FGUI

            appdomain.DelegateManager
                .RegisterMethodDelegate<System.String, System.String, System.Type, FairyGUI.PackageItem>();
            appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.GObject>();
            appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.EventContext>();

            #endregion
        }

        /// <summary>
        /// 注册委托（带返回值）
        /// </summary>
        static void RegisterFunctionDelegate(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterFunctionDelegate<System.String, System.Single>();

            appdomain.DelegateManager.RegisterFunctionDelegate<UnityEngine.Events.UnityAction>();
            appdomain.DelegateManager.RegisterFunctionDelegate<System.Object, ET.ETTask>();
            appdomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance, bool>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>,
                    System.String>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.Int32, System.Int32>,
                    System.Boolean>();
            appdomain.DelegateManager
                .RegisterFunctionDelegate<System.Collections.Generic.KeyValuePair<System.String, System.Int32>,
                    System.Int32>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, int>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<int, bool>(); //Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<int, int, int>(); //Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, List<int>>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, int>, KeyValuePair<int, int>, int>();
        }

        /// <summary>
        /// 注册委托转换器
        /// </summary>
        static void RegisterDelegateConvertor(AppDomain appdomain)
        {
            appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((action) =>
            {
                return new UnityEngine.Events.UnityAction((System.Action) action);
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<LitJson.ExporterFunc<System.Single>>((act) =>
            {
                return new LitJson.ExporterFunc<System.Single>((obj, writer) =>
                {
                    ((Action<System.Single, LitJson.JsonWriter>) act)(obj, writer);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<LitJson.ImporterFunc<System.String, System.Single>>(
                (act) =>
                {
                    return new LitJson.ImporterFunc<System.String, System.Single>((input) =>
                    {
                        return ((Func<System.String, System.Single>) act)(input);
                    });
                });
            appdomain.DelegateManager.RegisterDelegateConvertor<Comparison<KeyValuePair<int, int>>>((act) =>
            {
                return new Comparison<KeyValuePair<int, int>>((x, y) =>
                {
                    return ((Func<KeyValuePair<int, int>, KeyValuePair<int, int>, int>) act)(x, y);
                });
            });

            #region FGUI

            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.LoadResourceAsync>((act) =>
            {
                return new FairyGUI.UIPackage.LoadResourceAsync((name, extension, type, item) =>
                {
                    ((Action<System.String, System.String, System.Type, FairyGUI.PackageItem>) act)(name,
                        extension, type, item);
                });
            });
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.CreateObjectCallback>((act) =>
            {
                return new FairyGUI.UIPackage.CreateObjectCallback((result) =>
                {
                    ((Action<FairyGUI.GObject>) act)(result);
                });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback0>((act) =>
            {
                return new FairyGUI.EventCallback0(() => { ((Action) act)(); });
            });

            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback1>((act) =>
            {
                return new FairyGUI.EventCallback1((context) =>
                {
                    ((Action<FairyGUI.EventContext>) act)(context);
                });
            });

            #endregion
        }

        /// <summary>
        /// 注册值类型绑定
        /// </summary>
        static void RegisterValueTypeBinder(AppDomain appdomain)
        {
            // appdomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            // appdomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            // appdomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());
        }
    }
}