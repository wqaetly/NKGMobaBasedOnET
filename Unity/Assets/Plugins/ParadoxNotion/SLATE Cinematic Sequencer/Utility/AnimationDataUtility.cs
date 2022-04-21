using System.Collections.Generic;
using System.Linq;
using System;

namespace Slate
{

    public static class AnimationDataUtility
    {

        ///Given an object, returns possible field and prop paths marked with [AnimatableParameter] attribute
        public static string[] GetAnimatableMemberPaths(object root) {
            return Internal_GetAnimatableMemberPaths(root.GetType(), string.Empty);
        }

        //...
        static string[] Internal_GetAnimatableMemberPaths(Type type, string path) {
            var result = new List<string>();
            foreach ( var field in type.RTGetFields() ) {
                var current = path + field.Name;
                if ( field.RTIsDefined<AnimatableParameterAttribute>(true) ) {
                    result.Add(current);
                }

                if ( field.FieldType.RTIsDefined<ParseAnimatableParametersAttribute>(true) ) {
                    current += '.';
                    result.AddRange(Internal_GetAnimatableMemberPaths(field.FieldType, current));
                }
            }

            foreach ( var prop in type.RTGetProperties() ) {
                var current = path + prop.Name;
                if ( prop.RTIsDefined<AnimatableParameterAttribute>(true) ) {
                    if ( AnimatedParameter.supportedTypes.Contains(prop.PropertyType) ) {
                        result.Add(current);
                    }
                }

                if ( prop.PropertyType.RTIsDefined<ParseAnimatableParametersAttribute>(true) ) {
                    current += '.';
                    result.AddRange(Internal_GetAnimatableMemberPaths(prop.PropertyType, current));
                }
            }

            return result.ToArray();
        }
    }
}