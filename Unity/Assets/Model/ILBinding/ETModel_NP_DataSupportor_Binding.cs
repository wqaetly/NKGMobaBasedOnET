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
    unsafe class ETModel_NP_DataSupportor_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.NP_DataSupportor);

            field = type.GetField("mSkillDataDic", flag);
            app.RegisterCLRFieldGetter(field, get_mSkillDataDic_0);
            app.RegisterCLRFieldSetter(field, set_mSkillDataDic_0);


        }



        static object get_mSkillDataDic_0(ref object o)
        {
            return ((ETModel.NP_DataSupportor)o).mSkillDataDic;
        }
        static void set_mSkillDataDic_0(ref object o, object v)
        {
            ((ETModel.NP_DataSupportor)o).mSkillDataDic = (System.Collections.Generic.Dictionary<System.Int64, ETModel.SkillBaseNodeData>)v;
        }


    }
}
