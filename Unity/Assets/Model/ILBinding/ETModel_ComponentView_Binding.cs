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
    unsafe class ETModel_ComponentView_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.ComponentView);

            field = type.GetField("Component", flag);
            app.RegisterCLRFieldGetter(field, get_Component_0);
            app.RegisterCLRFieldSetter(field, set_Component_0);


        }



        static object get_Component_0(ref object o)
        {
            return ((ETModel.ComponentView)o).Component;
        }
        static void set_Component_0(ref object o, object v)
        {
            ((ETModel.ComponentView)o).Component = (System.Object)v;
        }


    }
}
