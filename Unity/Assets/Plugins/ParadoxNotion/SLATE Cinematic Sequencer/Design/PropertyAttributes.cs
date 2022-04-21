using UnityEngine;
using System;

namespace Slate
{

    ///Attribute to mark a field or property as an animatable parameter
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class AnimatableParameterAttribute : PropertyAttribute
    {
        public string link;
        public readonly float? min;
        public readonly float? max;
        public readonly string customName;
        public AnimatableParameterAttribute() { }
        public AnimatableParameterAttribute(string customName) {
            this.customName = customName;
        }
        public AnimatableParameterAttribute(string customName, float min, float max) {
            this.customName = customName;
            this.min = min;
            this.max = max;
        }
        public AnimatableParameterAttribute(float min, float max) {
            this.min = min;
            this.max = max;
        }
    }

    ///Attribute to mark a class to be parsed for sub animatable parameters
    ///TODO: Support structs
    [AttributeUsage(AttributeTargets.Class)]
    public class ParseAnimatableParametersAttribute : PropertyAttribute { }

    ///Attribute used to show a popup of shader properties of type
    [AttributeUsage(AttributeTargets.Field)]
    public class ShaderPropertyPopupAttribute : PropertyAttribute
    {
        public readonly Type propertyType;
        public ShaderPropertyPopupAttribute() { }
        public ShaderPropertyPopupAttribute(Type propertyType) {
            this.propertyType = propertyType;
        }
    }

    ///Attribute used to make a bool display as left toggle
    [AttributeUsage(AttributeTargets.Field)]
    public class LeftToggleAttribute : PropertyAttribute { }

    ///Attribute used to restrict float or int to a min value
    [AttributeUsage(AttributeTargets.Field)]
    public class MinAttribute : PropertyAttribute
    {
        public readonly float min;
        public MinAttribute(float min) {
            this.min = min;
        }
    }

    ///Show an example text in place of string field if string is null or empty
    [AttributeUsage(AttributeTargets.Field)]
    public class ExampleTextAttribute : PropertyAttribute
    {
        public readonly string text;
        public ExampleTextAttribute(string text) {
            this.text = text;
        }
    }

    ///Shows a HelpBox bellow field
    [AttributeUsage(AttributeTargets.Field)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        public readonly string text;
        public HelpBoxAttribute(string text) {
            this.text = text;
        }
    }

    ///Shows the property only if another property/field returns the specified value
    ///The target value is int type, which means that can both be used for boolean as well as enum targets
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public readonly string propertyName;
        public readonly int value;
        public ShowIfAttribute(string propertyName, int value) {
            this.propertyName = propertyName;
            this.value = value;
        }
    }

    ///Enabled the property only if another property/field returns the specified value
    ///The target value is int type, which means that can both be used for boolean as well as enum targets
    [AttributeUsage(AttributeTargets.Field)]
    public class EnabledIfAttribute : PropertyAttribute
    {
        public readonly string propertyName;
        public readonly int value;
        public EnabledIfAttribute(string propertyName, int value) {
            this.propertyName = propertyName;
            this.value = value;
        }
    }

    ///Callbacks target method when property changes in inspector
    [AttributeUsage(AttributeTargets.Field)]
    public class CallbackAttribute : PropertyAttribute
    {
        public readonly string methodName;
        public CallbackAttribute(string methodName) {
            this.methodName = methodName;
        }
    }

    ///Attribute used on Object or string field to mark them as required (red) if not set
    [AttributeUsage(AttributeTargets.Field)]
    public class RequiredAttribute : PropertyAttribute { }

    ///Attribute used to protect changes when cutscene playing
    [AttributeUsage(AttributeTargets.Field)]
    public class PlaybackProtectedAttribute : PropertyAttribute { }

    ///Attribute used to view field as read-only
    [AttributeUsage(AttributeTargets.Field)]
    public class ReadOnlyAttribute : PropertyAttribute { }

    ///Used for a sorting layer popup
    [AttributeUsage(AttributeTargets.Field)]
    public class SortingLayerAttribute : PropertyAttribute { }

    ///Makes a field of type CutsceneGroup show a dropdown selection of groups
    [AttributeUsage(AttributeTargets.Field)]
    public class ActorGroupPopupAttribute : PropertyAttribute { }
}