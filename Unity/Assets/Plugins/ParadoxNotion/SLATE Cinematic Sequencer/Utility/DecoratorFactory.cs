using System;
using System.Collections.Generic;
using UnityEngine;

namespace Slate
{

    ///Required attribute for decorators
    public class DecoratorAttribute : Attribute
    {
        readonly public Type targetType;
        public DecoratorAttribute(Type targetType) {
            this.targetType = targetType;
        }
    }

    ///Required interface for decorators
    public interface IDecorator
    {
        object Target { get; set; }
    }

    ///Provides Decorator generation
    public static class DecoratorFactory
    {

        private static Dictionary<Type, Type> decoratorsTypeMap = new Dictionary<Type, Type>();
        private static Dictionary<object, IDecorator> decoratorsInstanceMap = new Dictionary<object, IDecorator>();

        ///Returns a cached instance of decorator for target object
        public static T Decorator<T>(this object target) where T : IDecorator { return GetDecorator<T>(target); }

        ///Returns a cached instance of decorator for target object
        public static T GetDecorator<T>(object target) where T : IDecorator {
            if ( target == null ) { return default(T); }
            IDecorator decorator = null;
            if ( decoratorsInstanceMap.TryGetValue(target, out decorator) ) {
                decorator.Target = target;
                return (T)decorator;
            }
            decorator = CreateDecorator<T>(target.GetType());
            if ( decorator != null ) {
                decorator.Target = target;
            }
            return (T)( decoratorsInstanceMap[target] = decorator );
        }

        ///Returns a new decorator instance for target type
        static T CreateDecorator<T>(Type targetType) where T : IDecorator {
            Type decoratorType = null;
            if ( !decoratorsTypeMap.TryGetValue(targetType, out decoratorType) ) {
                Type directType = null;
                Type assignableType = null;
                var decoratorTypes = ReflectionTools.GetImplementationsOf(typeof(T));
                for ( var i = 0; i < decoratorTypes.Length; i++ ) {
                    var current = decoratorTypes[i];
                    var att = current.RTGetAttribute<DecoratorAttribute>(true);
                    if ( att == null ) {
                        Debug.LogWarning(string.Format("Decorator type '{0}' does not has a Decorator Attribute.", current.Name));
                        continue;
                    }
                    if ( att.targetType == targetType ) {
                        directType = current;
                    }
                    if ( att.targetType.RTIsAssignableFrom(targetType) ) {
                        assignableType = current;
                    }
                }
                decoratorType = directType != null ? directType : assignableType;
                decoratorsTypeMap[targetType] = decoratorType;
            }

            if ( decoratorType == null ) {
                Debug.LogError(string.Format("No '{0}' Decorator type found for target type '{1}'.", typeof(T), targetType));
                return default(T);
            }
            if ( decoratorType.IsGenericTypeDefinition ) {
                decoratorType = decoratorType.MakeGenericType(targetType);
            }
            if ( decoratorType.RTIsSubclassOf(typeof(ScriptableObject)) ) {
                return (T)(object)ScriptableObject.CreateInstance(decoratorType);
            }
            return (T)Activator.CreateInstance(decoratorType);
        }
    }
}