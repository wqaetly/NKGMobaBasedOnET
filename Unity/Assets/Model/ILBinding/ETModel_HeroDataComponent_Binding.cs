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
    unsafe class ETModel_HeroDataComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.HeroDataComponent);

            field = type.GetField("MaxLifeValue", flag);
            app.RegisterCLRFieldGetter(field, get_MaxLifeValue_0);
            app.RegisterCLRFieldSetter(field, set_MaxLifeValue_0);
            field = type.GetField("MaxMagicValue", flag);
            app.RegisterCLRFieldGetter(field, get_MaxMagicValue_1);
            app.RegisterCLRFieldSetter(field, set_MaxMagicValue_1);
            field = type.GetField("CurrentLifeValue", flag);
            app.RegisterCLRFieldGetter(field, get_CurrentLifeValue_2);
            app.RegisterCLRFieldSetter(field, set_CurrentLifeValue_2);
            field = type.GetField("CurrentMagicValue", flag);
            app.RegisterCLRFieldGetter(field, get_CurrentMagicValue_3);
            app.RegisterCLRFieldSetter(field, set_CurrentMagicValue_3);
            field = type.GetField("NodeDataForHero", flag);
            app.RegisterCLRFieldGetter(field, get_NodeDataForHero_4);
            app.RegisterCLRFieldSetter(field, set_NodeDataForHero_4);


        }



        static object get_MaxLifeValue_0(ref object o)
        {
            return ((ETModel.HeroDataComponent)o).MaxLifeValue;
        }
        static void set_MaxLifeValue_0(ref object o, object v)
        {
            ((ETModel.HeroDataComponent)o).MaxLifeValue = (System.Single)v;
        }
        static object get_MaxMagicValue_1(ref object o)
        {
            return ((ETModel.HeroDataComponent)o).MaxMagicValue;
        }
        static void set_MaxMagicValue_1(ref object o, object v)
        {
            ((ETModel.HeroDataComponent)o).MaxMagicValue = (System.Single)v;
        }
        static object get_CurrentLifeValue_2(ref object o)
        {
            return ((ETModel.HeroDataComponent)o).CurrentLifeValue;
        }
        static void set_CurrentLifeValue_2(ref object o, object v)
        {
            ((ETModel.HeroDataComponent)o).CurrentLifeValue = (System.Single)v;
        }
        static object get_CurrentMagicValue_3(ref object o)
        {
            return ((ETModel.HeroDataComponent)o).CurrentMagicValue;
        }
        static void set_CurrentMagicValue_3(ref object o, object v)
        {
            ((ETModel.HeroDataComponent)o).CurrentMagicValue = (System.Single)v;
        }
        static object get_NodeDataForHero_4(ref object o)
        {
            return ((ETModel.HeroDataComponent)o).NodeDataForHero;
        }
        static void set_NodeDataForHero_4(ref object o, object v)
        {
            ((ETModel.HeroDataComponent)o).NodeDataForHero = (ETModel.NodeDataForHero)v;
        }


    }
}
