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
    unsafe class ETModel_NodeDataForHero_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.NodeDataForHero);

            field = type.GetField("HeroAvatar", flag);
            app.RegisterCLRFieldGetter(field, get_HeroAvatar_0);
            app.RegisterCLRFieldSetter(field, set_HeroAvatar_0);
            field = type.GetField("Talent_SkillSprite", flag);
            app.RegisterCLRFieldGetter(field, get_Talent_SkillSprite_1);
            app.RegisterCLRFieldSetter(field, set_Talent_SkillSprite_1);
            field = type.GetField("Q_SkillSprite", flag);
            app.RegisterCLRFieldGetter(field, get_Q_SkillSprite_2);
            app.RegisterCLRFieldSetter(field, set_Q_SkillSprite_2);
            field = type.GetField("W_SkillSprite", flag);
            app.RegisterCLRFieldGetter(field, get_W_SkillSprite_3);
            app.RegisterCLRFieldSetter(field, set_W_SkillSprite_3);
            field = type.GetField("E_SkillSprite", flag);
            app.RegisterCLRFieldGetter(field, get_E_SkillSprite_4);
            app.RegisterCLRFieldSetter(field, set_E_SkillSprite_4);
            field = type.GetField("R_SkillSprite", flag);
            app.RegisterCLRFieldGetter(field, get_R_SkillSprite_5);
            app.RegisterCLRFieldSetter(field, set_R_SkillSprite_5);
            field = type.GetField("OriAttackValue", flag);
            app.RegisterCLRFieldGetter(field, get_OriAttackValue_6);
            app.RegisterCLRFieldSetter(field, set_OriAttackValue_6);
            field = type.GetField("ExtAttackValue", flag);
            app.RegisterCLRFieldGetter(field, get_ExtAttackValue_7);
            app.RegisterCLRFieldSetter(field, set_ExtAttackValue_7);
            field = type.GetField("OriMagicStrength", flag);
            app.RegisterCLRFieldGetter(field, get_OriMagicStrength_8);
            app.RegisterCLRFieldSetter(field, set_OriMagicStrength_8);
            field = type.GetField("ExtMagicStrength", flag);
            app.RegisterCLRFieldGetter(field, get_ExtMagicStrength_9);
            app.RegisterCLRFieldSetter(field, set_ExtMagicStrength_9);
            field = type.GetField("ExtMagicRec", flag);
            app.RegisterCLRFieldGetter(field, get_ExtMagicRec_10);
            app.RegisterCLRFieldSetter(field, set_ExtMagicRec_10);
            field = type.GetField("OriArmor", flag);
            app.RegisterCLRFieldGetter(field, get_OriArmor_11);
            app.RegisterCLRFieldSetter(field, set_OriArmor_11);
            field = type.GetField("ExtArmor", flag);
            app.RegisterCLRFieldGetter(field, get_ExtArmor_12);
            app.RegisterCLRFieldSetter(field, set_ExtArmor_12);
            field = type.GetField("OriArmorPenetration", flag);
            app.RegisterCLRFieldGetter(field, get_OriArmorPenetration_13);
            app.RegisterCLRFieldSetter(field, set_OriArmorPenetration_13);
            field = type.GetField("ExtArmorPenetration", flag);
            app.RegisterCLRFieldGetter(field, get_ExtArmorPenetration_14);
            app.RegisterCLRFieldSetter(field, set_ExtArmorPenetration_14);
            field = type.GetField("OriMagicResistance", flag);
            app.RegisterCLRFieldGetter(field, get_OriMagicResistance_15);
            app.RegisterCLRFieldSetter(field, set_OriMagicResistance_15);
            field = type.GetField("ExtMagicResistance", flag);
            app.RegisterCLRFieldGetter(field, get_ExtMagicResistance_16);
            app.RegisterCLRFieldSetter(field, set_ExtMagicResistance_16);
            field = type.GetField("OriMagicPenetration", flag);
            app.RegisterCLRFieldGetter(field, get_OriMagicPenetration_17);
            app.RegisterCLRFieldSetter(field, set_OriMagicPenetration_17);
            field = type.GetField("ExtMagicPenetration", flag);
            app.RegisterCLRFieldGetter(field, get_ExtMagicPenetration_18);
            app.RegisterCLRFieldSetter(field, set_ExtMagicPenetration_18);
            field = type.GetField("OriAttackSpeed", flag);
            app.RegisterCLRFieldGetter(field, get_OriAttackSpeed_19);
            app.RegisterCLRFieldSetter(field, set_OriAttackSpeed_19);
            field = type.GetField("ExtAttackSpeed", flag);
            app.RegisterCLRFieldGetter(field, get_ExtAttackSpeed_20);
            app.RegisterCLRFieldSetter(field, set_ExtAttackSpeed_20);
            field = type.GetField("OriSkillCD", flag);
            app.RegisterCLRFieldGetter(field, get_OriSkillCD_21);
            app.RegisterCLRFieldSetter(field, set_OriSkillCD_21);
            field = type.GetField("ExtSkillCD", flag);
            app.RegisterCLRFieldGetter(field, get_ExtSkillCD_22);
            app.RegisterCLRFieldSetter(field, set_ExtSkillCD_22);
            field = type.GetField("OriCriticalStrikeProbability", flag);
            app.RegisterCLRFieldGetter(field, get_OriCriticalStrikeProbability_23);
            app.RegisterCLRFieldSetter(field, set_OriCriticalStrikeProbability_23);
            field = type.GetField("ExtCriticalStrikeProbability", flag);
            app.RegisterCLRFieldGetter(field, get_ExtCriticalStrikeProbability_24);
            app.RegisterCLRFieldSetter(field, set_ExtCriticalStrikeProbability_24);
            field = type.GetField("OriMoveSpeed", flag);
            app.RegisterCLRFieldGetter(field, get_OriMoveSpeed_25);
            app.RegisterCLRFieldSetter(field, set_OriMoveSpeed_25);
            field = type.GetField("ExtMoveSpeed", flag);
            app.RegisterCLRFieldGetter(field, get_ExtMoveSpeed_26);
            app.RegisterCLRFieldSetter(field, set_ExtMoveSpeed_26);


        }



        static object get_HeroAvatar_0(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).HeroAvatar;
        }
        static void set_HeroAvatar_0(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).HeroAvatar = (System.String)v;
        }
        static object get_Talent_SkillSprite_1(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).Talent_SkillSprite;
        }
        static void set_Talent_SkillSprite_1(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).Talent_SkillSprite = (System.String)v;
        }
        static object get_Q_SkillSprite_2(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).Q_SkillSprite;
        }
        static void set_Q_SkillSprite_2(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).Q_SkillSprite = (System.String)v;
        }
        static object get_W_SkillSprite_3(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).W_SkillSprite;
        }
        static void set_W_SkillSprite_3(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).W_SkillSprite = (System.String)v;
        }
        static object get_E_SkillSprite_4(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).E_SkillSprite;
        }
        static void set_E_SkillSprite_4(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).E_SkillSprite = (System.String)v;
        }
        static object get_R_SkillSprite_5(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).R_SkillSprite;
        }
        static void set_R_SkillSprite_5(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).R_SkillSprite = (System.String)v;
        }
        static object get_OriAttackValue_6(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriAttackValue;
        }
        static void set_OriAttackValue_6(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriAttackValue = (System.Single)v;
        }
        static object get_ExtAttackValue_7(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtAttackValue;
        }
        static void set_ExtAttackValue_7(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtAttackValue = (System.Single)v;
        }
        static object get_OriMagicStrength_8(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriMagicStrength;
        }
        static void set_OriMagicStrength_8(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriMagicStrength = (System.Single)v;
        }
        static object get_ExtMagicStrength_9(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtMagicStrength;
        }
        static void set_ExtMagicStrength_9(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtMagicStrength = (System.Single)v;
        }
        static object get_ExtMagicRec_10(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtMagicRec;
        }
        static void set_ExtMagicRec_10(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtMagicRec = (System.Single)v;
        }
        static object get_OriArmor_11(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriArmor;
        }
        static void set_OriArmor_11(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriArmor = (System.Single)v;
        }
        static object get_ExtArmor_12(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtArmor;
        }
        static void set_ExtArmor_12(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtArmor = (System.Single)v;
        }
        static object get_OriArmorPenetration_13(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriArmorPenetration;
        }
        static void set_OriArmorPenetration_13(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriArmorPenetration = (System.Single)v;
        }
        static object get_ExtArmorPenetration_14(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtArmorPenetration;
        }
        static void set_ExtArmorPenetration_14(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtArmorPenetration = (System.Single)v;
        }
        static object get_OriMagicResistance_15(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriMagicResistance;
        }
        static void set_OriMagicResistance_15(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriMagicResistance = (System.Single)v;
        }
        static object get_ExtMagicResistance_16(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtMagicResistance;
        }
        static void set_ExtMagicResistance_16(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtMagicResistance = (System.Single)v;
        }
        static object get_OriMagicPenetration_17(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriMagicPenetration;
        }
        static void set_OriMagicPenetration_17(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriMagicPenetration = (System.Single)v;
        }
        static object get_ExtMagicPenetration_18(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtMagicPenetration;
        }
        static void set_ExtMagicPenetration_18(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtMagicPenetration = (System.Single)v;
        }
        static object get_OriAttackSpeed_19(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriAttackSpeed;
        }
        static void set_OriAttackSpeed_19(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriAttackSpeed = (System.Single)v;
        }
        static object get_ExtAttackSpeed_20(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtAttackSpeed;
        }
        static void set_ExtAttackSpeed_20(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtAttackSpeed = (System.Single)v;
        }
        static object get_OriSkillCD_21(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriSkillCD;
        }
        static void set_OriSkillCD_21(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriSkillCD = (System.Single)v;
        }
        static object get_ExtSkillCD_22(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtSkillCD;
        }
        static void set_ExtSkillCD_22(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtSkillCD = (System.Single)v;
        }
        static object get_OriCriticalStrikeProbability_23(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriCriticalStrikeProbability;
        }
        static void set_OriCriticalStrikeProbability_23(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriCriticalStrikeProbability = (System.Single)v;
        }
        static object get_ExtCriticalStrikeProbability_24(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtCriticalStrikeProbability;
        }
        static void set_ExtCriticalStrikeProbability_24(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtCriticalStrikeProbability = (System.Single)v;
        }
        static object get_OriMoveSpeed_25(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).OriMoveSpeed;
        }
        static void set_OriMoveSpeed_25(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).OriMoveSpeed = (System.Single)v;
        }
        static object get_ExtMoveSpeed_26(ref object o)
        {
            return ((ETModel.NodeDataForHero)o).ExtMoveSpeed;
        }
        static void set_ExtMoveSpeed_26(ref object o, object v)
        {
            ((ETModel.NodeDataForHero)o).ExtMoveSpeed = (System.Single)v;
        }


    }
}
