using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ETModel_NP_RuntimeTree_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.NP_RuntimeTree);
            args = new Type[]{};
            method = type.GetMethod("GetBlackboard", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GetBlackboard_0);

            field = type.GetField("m_NPRuntimeTreeRootNode", flag);
            app.RegisterCLRFieldGetter(field, get_m_NPRuntimeTreeRootNode_0);
            app.RegisterCLRFieldSetter(field, set_m_NPRuntimeTreeRootNode_0);
            field = type.GetField("m_BelongNP_DataSupportor", flag);
            app.RegisterCLRFieldGetter(field, get_m_BelongNP_DataSupportor_1);
            app.RegisterCLRFieldSetter(field, set_m_BelongNP_DataSupportor_1);


        }


        static StackObject* GetBlackboard_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            ETModel.NP_RuntimeTree instance_of_this_method = (ETModel.NP_RuntimeTree)typeof(ETModel.NP_RuntimeTree).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.GetBlackboard();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_m_NPRuntimeTreeRootNode_0(ref object o)
        {
            return ((ETModel.NP_RuntimeTree)o).m_NPRuntimeTreeRootNode;
        }
        static void set_m_NPRuntimeTreeRootNode_0(ref object o, object v)
        {
            ((ETModel.NP_RuntimeTree)o).m_NPRuntimeTreeRootNode = (NPBehave.Root)v;
        }
        static object get_m_BelongNP_DataSupportor_1(ref object o)
        {
            return ((ETModel.NP_RuntimeTree)o).m_BelongNP_DataSupportor;
        }
        static void set_m_BelongNP_DataSupportor_1(ref object o, object v)
        {
            ((ETModel.NP_RuntimeTree)o).m_BelongNP_DataSupportor = (ETModel.NP_DataSupportor)v;
        }


    }
}
