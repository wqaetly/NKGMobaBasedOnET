using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Reflection.Emit;

namespace Slate
{

    ///An interface for animated_X models
    public interface IAnimatedParameterModel
    {

        ///Should step be forced?
        bool ForceStepMode();
        ///Amount of curves required to represent data
		int RequiredCurvesCount();

        ///Floats to data object
        object ConvertToObject(params float[] floats);
        ///Data object to floats
		float[] ConvertToFloats(object value);

        ///Nice string for keys
        string GetKeyLabel(params float[] floats);

        ///Set field/prop info value on target object with value provided in floats[]
        void SetDirect(object target, MemberInfo info, params float[] floats);
        ///Get field/prop info value on target object with value being a float[]
        float[] GetDirect(object target, MemberInfo info);
    }

    ///----------------------------------------------------------------------------------------------
    [Decorator(typeof(bool))]
    public struct Animated_Bool : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return true; }
        public int RequiredCurvesCount() { return 1; }

        public object ConvertToObject(params float[] floats) {
            return floats[0] >= 1;
        }

        public float[] ConvertToFloats(object value) {
            return new float[1] { ( (bool)value ) ? 1 : 0 };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0})", floats[0] >= 1 ? "true" : "false");
        }

        Action<object, bool> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, bool>(info); }
            setter(target, floats[0] >= 1);
        }

        public float[] GetDirect(object target, MemberInfo info) {
            return new float[1] { ( (bool)ReflectionTools.RTGetFieldOrPropValue(info, target) ) ? 1 : 0 };
        }
    }

    ///----------------------------------------------------------------------------------------------
    [Decorator(typeof(int))]
    public struct Animated_Int : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 1; }

        public object ConvertToObject(params float[] floats) {
            return (int)floats[0];
        }

        public float[] ConvertToFloats(object value) {
            return new float[1] { (int)value };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0})", floats[0].ToString("0"));
        }

        Action<object, int> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, int>(info); }
            setter(target, (int)floats[0]);
        }

        public float[] GetDirect(object target, MemberInfo info) {
            return new float[1] { (int)ReflectionTools.RTGetFieldOrPropValue(info, target) };
        }
    }

    ///----------------------------------------------------------------------------------------------
    [Decorator(typeof(float))]
    public struct Animated_Float : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 1; }

        public object ConvertToObject(params float[] floats) {
            return floats[0];
        }

        public float[] ConvertToFloats(object value) {
            return new float[1] { (float)value };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0})", floats[0].ToString("0.0"));
        }

        Action<object, float> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, float>(info); }
            setter(target, (float)floats[0]);
        }

        public float[] GetDirect(object target, MemberInfo info) {
            return new float[1] { (float)ReflectionTools.RTGetFieldOrPropValue(info, target) };
        }
    }

    ///----------------------------------------------------------------------------------------------
    [Decorator(typeof(Vector2))]
    public struct Animated_Vector2 : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 2; }

        public object ConvertToObject(params float[] floats) {
            return new Vector2(floats[0], floats[1]);
        }

        public float[] ConvertToFloats(object value) {
            var vector = (Vector2)value;
            return new float[2] { vector.x, vector.y };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0},{1})", floats[0].ToString("0"), floats[1].ToString("0"));
        }

        Action<object, Vector2> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Vector2>(info); }
            setter(target, new Vector2(floats[0], floats[1]));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var vector = (Vector2)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[2] { vector.x, vector.y };
        }
    }

    ///----------------------------------------------------------------------------------------------
	[Decorator(typeof(Vector3))]
    public struct Animated_Vector3 : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 3; }

        public object ConvertToObject(params float[] floats) {
            return new Vector3(floats[0], floats[1], floats[2]);
        }

        public float[] ConvertToFloats(object value) {
            var vector = (Vector3)value;
            return new float[3] { vector.x, vector.y, vector.z };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0},{1},{2})", floats[0].ToString("0"), floats[1].ToString("0"), floats[2].ToString("0"));
        }

        Action<object, Vector3> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Vector3>(info); }
            setter(target, new Vector3(floats[0], floats[1], floats[2]));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var vector = (Vector3)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[3] { vector.x, vector.y, vector.z };
        }
    }

    ///----------------------------------------------------------------------------------------------
    [Decorator(typeof(Color))]
    public struct Animated_Color : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 4; }

        public object ConvertToObject(params float[] floats) {
            return new Color(floats[0], floats[1], floats[2], floats[3]);
        }

        public float[] ConvertToFloats(object value) {
            var color = (Color)value;
            return new float[4] { color.r, color.g, color.b, color.a };
        }

        public string GetKeyLabel(params float[] floats) {
            Color32 color = new Color(floats[0], floats[1], floats[2], floats[3]);
            var hexColor = ( "#" + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2") ).ToLower();
            return string.Format("(<color={0}><size=14>●</size></color>)", hexColor);
        }

        Action<object, Color> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Color>(info); }
            setter(target, new Color(floats[0], floats[1], floats[2], floats[3]));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var color = (Color)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[4] { color.r, color.g, color.b, color.a };
        }
    }

    ///----------------------------------------------------------------------------------------------
	[Decorator(typeof(Vector4))]
    public struct Animated_Vector4 : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 4; }

        public object ConvertToObject(params float[] floats) {
            return new Vector4(floats[0], floats[1], floats[2], floats[3]);
        }

        public float[] ConvertToFloats(object value) {
            var vector = (Vector4)value;
            return new float[4] { vector.x, vector.y, vector.z, vector.w };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0},{1},{2},{3})", floats[0].ToString("0"), floats[1].ToString("0"), floats[2].ToString("0"), floats[3].ToString("0"));
        }

        Action<object, Vector4> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Vector4>(info); }
            setter(target, new Vector4(floats[0], floats[1], floats[2], floats[3]));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var vector = (Vector4)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[4] { vector.x, vector.y, vector.z, vector.w };
        }
    }

    ///----------------------------------------------------------------------------------------------
	[Decorator(typeof(Quaternion))]
    public struct Animated_Quaternion : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 4; }

        public object ConvertToObject(params float[] floats) {
            return new Quaternion(floats[0], floats[1], floats[2], floats[3]);
        }

        public float[] ConvertToFloats(object value) {
            var quaternion = (Quaternion)value;
            return new float[4] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0},{1},{2},{3})", floats[0].ToString("0"), floats[1].ToString("0"), floats[2].ToString("0"), floats[3].ToString("0"));
        }

        Action<object, Quaternion> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Quaternion>(info); }
            setter(target, new Quaternion(floats[0], floats[1], floats[2], floats[3]));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var quaternion = (Quaternion)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[4] { quaternion.x, quaternion.y, quaternion.z, quaternion.w };
        }
    }

    ///----------------------------------------------------------------------------------------------
	[Decorator(typeof(Rect))]
    public struct Animated_Rect : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 4; }

        public object ConvertToObject(params float[] floats) {
            return new Rect(floats[0], floats[1], floats[2], floats[3]);
        }

        public float[] ConvertToFloats(object value) {
            var rect = (Rect)value;
            return new float[4] { rect.x, rect.y, rect.width, rect.height };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0},{1},{2},{3})", floats[0].ToString("0"), floats[1].ToString("0"), floats[2].ToString("0"), floats[3].ToString("0"));
        }

        Action<object, Rect> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Rect>(info); }
            setter(target, new Rect(floats[0], floats[1], floats[2], floats[3]));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var rect = (Rect)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[4] { rect.x, rect.y, rect.width, rect.height };
        }
    }

    ///----------------------------------------------------------------------------------------------
	[Decorator(typeof(Bounds))]
    public struct Animated_Bounds : IAnimatedParameterModel
    {
        public bool ForceStepMode() { return false; }
        public int RequiredCurvesCount() { return 6; }

        public object ConvertToObject(params float[] floats) {
            return new Bounds(new Vector3(floats[0], floats[1], floats[2]), new Vector3(floats[3], floats[4], floats[5]));
        }

        public float[] ConvertToFloats(object value) {
            var bounds = (Bounds)value;
            return new float[6] { bounds.center.x, bounds.center.y, bounds.center.z, bounds.size.x, bounds.size.y, bounds.size.z };
        }

        public string GetKeyLabel(params float[] floats) {
            return string.Format("({0},{1},{2},{3},{4},{5})", floats[0].ToString("0"), floats[1].ToString("0"), floats[2].ToString("0"), floats[3].ToString("0"), floats[4].ToString("0"), floats[5].ToString("0"));
        }

        Action<object, Bounds> setter;
        public void SetDirect(object target, MemberInfo info, params float[] floats) {
            if ( setter == null ) { setter = ReflectionTools.GetFieldOrPropSetter<object, Bounds>(info); }
            setter(target, new Bounds(new Vector3(floats[0], floats[1], floats[2]), new Vector3(floats[3], floats[4], floats[5])));
        }

        public float[] GetDirect(object target, MemberInfo info) {
            var bounds = (Bounds)ReflectionTools.RTGetFieldOrPropValue(info, target);
            return new float[6] { bounds.center.x, bounds.center.y, bounds.center.z, bounds.size.x, bounds.size.y, bounds.size.z };
        }
    }

}