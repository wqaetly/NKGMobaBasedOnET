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
    unsafe class ETModel_NodeDataForSkillBuff_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.NodeDataForSkillBuff);

            field = type.GetField("SkillBuffBases", flag);
            app.RegisterCLRFieldGetter(field, get_SkillBuffBases_0);
            app.RegisterCLRFieldSetter(field, set_SkillBuffBases_0);


        }



        static object get_SkillBuffBases_0(ref object o)
        {
            return ((ETModel.NodeDataForSkillBuff)o).SkillBuffBases;
        }
        static void set_SkillBuffBases_0(ref object o, object v)
        {
            ((ETModel.NodeDataForSkillBuff)o).SkillBuffBases = (ETModel.BuffDataBase)v;
        }


    }
}
