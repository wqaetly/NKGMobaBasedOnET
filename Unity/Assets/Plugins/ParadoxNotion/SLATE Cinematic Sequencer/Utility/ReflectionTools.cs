using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using System.Reflection.Emit;

namespace Slate
{

    ///A collection of helper tools relevant to runtime
    public static class ReflectionTools
    {

#if !NETFX_CORE
        private const BindingFlags flagsEverything = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
#endif

        ///Assemblies
        private static List<Assembly> _loadedAssemblies;
        private static List<Assembly> loadedAssemblies {
            get
            {
                if ( _loadedAssemblies == null ) {

#if NETFX_CORE

				    _loadedAssemblies = new List<Assembly>();
		 		    var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
				    var folderFilesAsync = folder.GetFilesAsync();
				    folderFilesAsync.AsTask().Wait();

				    foreach (var file in folderFilesAsync.GetResults()){
				        if (file.FileType == ".dll" || file.FileType == ".exe"){
				            try
				            {
				                var filename = file.Name.Substring(0, file.Name.Length - file.FileType.Length);
				                AssemblyName name = new AssemblyName { Name = filename };
				                Assembly asm = Assembly.Load(name);
				                _loadedAssemblies.Add(asm);
				            }
				            catch { continue; }
				        }
				    }

#else

                    _loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

#endif
                }

                return _loadedAssemblies;
            }
        }


        //Alternative to Type.GetType to work with FullName instead of AssemblyQualifiedName when looking up a type by string
        private static Dictionary<string, Type> typeMap = new Dictionary<string, Type>();
        public static Type GetType(string typeName) {

            Type type = null;

            if ( typeMap.TryGetValue(typeName, out type) ) {
                return type;
            }

            type = Type.GetType(typeName);
            if ( type != null ) {
                return typeMap[typeName] = type;
            }

            foreach ( var asm in loadedAssemblies ) {
                try { type = asm.GetType(typeName); }
                catch { continue; }
                if ( type != null ) {
                    return typeMap[typeName] = type;
                }
            }

            //worst case scenario
            foreach ( var t in GetAllTypes() ) {
                if ( t.Name == typeName ) {
                    return typeMap[typeName] = t;
                }
            }

            UnityEngine.Debug.LogError(string.Format("Requested Type with name '{0}', could not be loaded", typeName));
            return null;
        }

        ///Get every single type in loaded assemblies
        public static Type[] GetAllTypes() {
            var result = new List<Type>();
            foreach ( var asm in loadedAssemblies ) {
                try { result.AddRange(asm.RTGetExportedTypes()); }
                catch { continue; }
            }
            return result.ToArray();
        }

        private static Dictionary<Type, Type[]> subTypesMap = new Dictionary<Type, Type[]>();
        ///Get a collection of types assignable to provided type, excluding Abstract types
        public static Type[] GetImplementationsOf(Type type) {

            Type[] result = null;
            if ( subTypesMap.TryGetValue(type, out result) ) {
                return result;
            }

            var temp = new List<Type>();
            foreach ( var asm in loadedAssemblies ) {
                try { temp.AddRange(asm.RTGetExportedTypes().Where(t => type.RTIsAssignableFrom(t) && !t.RTIsAbstract())); }
                catch { continue; }
            }
            return subTypesMap[type] = temp.ToArray();
        }

        private static Type[] RTGetExportedTypes(this Assembly asm) {
#if NETFX_CORE
			return asm.ExportedTypes.ToArray();
#else
            return asm.GetExportedTypes();
#endif
        }

        //Just a more friendly name for certain (few) types.
        public static string FriendlyName(this Type type) {
            if ( type == null ) { return "NULL"; }
            if ( type == typeof(float) ) { return "Float"; }
            if ( type == typeof(int) ) { return "Integer"; }
            return type.Name;
        }

        //Is property static?
        public static bool RTIsStatic(this PropertyInfo propertyInfo) {
            return ( ( propertyInfo.CanRead && propertyInfo.RTGetGetMethod().IsStatic ) || ( propertyInfo.CanWrite && propertyInfo.RTGetSetMethod().IsStatic ) );
        }

        public static bool RTIsAbstract(this Type type) {
#if NETFX_CORE
			return type.GetTypeInfo().IsAbstract;
#else
            return type.IsAbstract;
#endif
        }

        public static bool RTIsSubclassOf(this Type type, Type other) {
#if NETFX_CORE
			return type.GetTypeInfo().IsSubclassOf(other);
#else
            return type.IsSubclassOf(other);
#endif
        }

        public static bool RTIsAssignableFrom(this Type type, Type second) {
#if NETFX_CORE
			return type.GetTypeInfo().IsAssignableFrom(second.GetTypeInfo());
#else
            return type.IsAssignableFrom(second);
#endif
        }

        public static FieldInfo RTGetField(this Type type, string name) {
#if NETFX_CORE
			return type.GetRuntimeFields().FirstOrDefault(f => f.Name == name);
#else
            return type.GetField(name, flagsEverything);
#endif
        }

        public static PropertyInfo RTGetProperty(this Type type, string name) {
#if NETFX_CORE
			return type.GetRuntimeProperties().FirstOrDefault(p => p.Name == name);
#else
            return type.GetProperty(name, flagsEverything);
#endif
        }

        public static MethodInfo RTGetMethod(this Type type, string name) {
#if NETFX_CORE
			return type.GetRuntimeMethods().FirstOrDefault(m => m.Name == name);
#else
            return type.GetMethod(name, flagsEverything);
#endif
        }

        public static FieldInfo[] RTGetFields(this Type type) {
#if NETFX_CORE
			return type.GetRuntimeFields().ToArray();
#else
            return type.GetFields(flagsEverything);
#endif
        }

        public static PropertyInfo[] RTGetProperties(this Type type) {
#if NETFX_CORE
			return type.GetRuntimeProperties().ToArray();
#else
            return type.GetProperties(flagsEverything);
#endif
        }

        public static MethodInfo RTGetGetMethod(this PropertyInfo prop) {
#if NETFX_CORE
			return prop.GetMethod;
#else
            return prop.GetGetMethod();
#endif
        }

        public static MethodInfo RTGetSetMethod(this PropertyInfo prop) {
#if NETFX_CORE
			return prop.SetMethod;
#else
            return prop.GetSetMethod();
#endif
        }

        public static Type RTReflectedType(this Type type) {
#if NETFX_CORE
			return type.GetTypeInfo().DeclaringType; //no way to get ReflectedType here that I know of...
#else
            return type.ReflectedType;
#endif
        }

        public static Type RTReflectedType(this MemberInfo member) {
#if NETFX_CORE
			return member.DeclaringType; //no way to get ReflectedType here that I know of...
#else
            return member.ReflectedType;
#endif
        }

        public static T RTGetAttribute<T>(this Type type, bool inherited) where T : Attribute {
#if NETFX_CORE
			return (T)type.GetTypeInfo().GetCustomAttributes(typeof(T), inherited).FirstOrDefault();
#else
            return (T)type.GetCustomAttributes(typeof(T), inherited).FirstOrDefault();
#endif
        }

        public static T RTGetAttribute<T>(this MemberInfo member, bool inherited) where T : Attribute {
#if NETFX_CORE
			return (T)member.GetCustomAttributes(typeof(T), inherited).FirstOrDefault();
#else
            return (T)member.GetCustomAttributes(typeof(T), inherited).FirstOrDefault();
#endif
        }

        public static bool RTIsDefined<T>(this MemberInfo member, bool inherited) where T : Attribute {
#if NETFX_CORE
			return member.IsDefined(typeof(T), inherited);
#else
            return member.IsDefined(typeof(T), inherited);
#endif
        }

        ///Creates a delegate out of Method for target instance
        public static T RTCreateDelegate<T>(this MethodInfo method, object instance) {
#if NETFX_CORE
			return (T)(object)method.CreateDelegate(typeof(T), instance);
#else
            return (T)(object)Delegate.CreateDelegate(typeof(T), instance, method);
#endif
        }

        ///----------------------------------------------------------------------------------------------

        ///Creates and returns an open instance setter for field or property.
        ///In JIT is done with IL Emit. In AOT via direct reflection.
        public static Action<T, TValue> GetFieldOrPropSetter<T, TValue>(MemberInfo info) {

#if !NET_STANDARD_2_0 && ( UNITY_EDITOR || ( !ENABLE_IL2CPP && ( UNITY_STANDALONE || UNITY_ANDROID || UNITY_WSA ) ) )

            var name = string.Format("_set_{0}_field_", info.Name);
            DynamicMethod m = new DynamicMethod(name, typeof(void), new Type[] { typeof(T), typeof(TValue) }, typeof(T));
            ILGenerator cg = m.GetILGenerator();
            cg.Emit(OpCodes.Ldarg_0);
            cg.Emit(OpCodes.Ldarg_1);
            if ( info is FieldInfo ) { cg.Emit(OpCodes.Stfld, (FieldInfo)info); }
            if ( info is PropertyInfo ) { cg.Emit(OpCodes.Call, ( info as PropertyInfo ).GetSetMethod()); }
            cg.Emit(OpCodes.Ret);
            return (Action<T, TValue>)m.CreateDelegate(typeof(Action<T, TValue>));

#else

            return (x, v) => RTSetFieldOrPropValue(info, x, v);

#endif
        }

        ///----------------------------------------------------------------------------------------------

        public static MemberInfo[] RTGetFieldsAndProps(this Type type) {
            var result = new List<MemberInfo>();
            result.AddRange(type.RTGetFields());
            result.AddRange(type.RTGetProperties());
            return result.ToArray();
        }

        public static MemberInfo RTGetFieldOrProp(this Type type, string name) {
            MemberInfo result = type.RTGetField(name);
            if ( result == null ) { result = type.RTGetProperty(name); }
            return result;
        }

        public static object RTGetFieldOrPropValue(this MemberInfo member, object instance, int index = -1) {
            if ( member is FieldInfo ) { return ( member as FieldInfo ).GetValue(instance); }
            if ( member is PropertyInfo ) { return ( member as PropertyInfo ).GetValue(instance, index == -1 ? null : new object[] { index }); }
            return null;
        }

        public static void RTSetFieldOrPropValue(this MemberInfo member, object instance, object value, int index = -1) {
            if ( member is FieldInfo ) { ( member as FieldInfo ).SetValue(instance, value); }
            if ( member is PropertyInfo ) { ( member as PropertyInfo ).SetValue(instance, value, index == -1 ? null : new object[] { index }); }
        }

        public static Type RTGetFieldOrPropType(this MemberInfo member) {
            if ( member is FieldInfo ) { return ( member as FieldInfo ).FieldType; }
            if ( member is PropertyInfo ) { return ( member as PropertyInfo ).PropertyType; }
            return null;
        }

        ///----------------------------------------------------------------------------------------------

        ///Given an instance and a path returns the Field or Property Info from that path
        public static MemberInfo GetRelativeMember(object root, string path) {
            if ( root == null || string.IsNullOrEmpty(path) ) { return null; }
            return GetRelativeMember(root.GetType(), path);
        }

        ///Given an type and a path returns the Field or Property Info from that path
        public static MemberInfo GetRelativeMember(Type type, string path) {
            if ( type == null || string.IsNullOrEmpty(path) ) { return null; }
            MemberInfo result = null;
            var parts = path.Split('.');
            if ( parts.Length == 1 ) { return type.RTGetFieldOrProp(parts[0]); }
            foreach ( var part in parts ) {
                result = type.RTGetFieldOrProp(part);
                if ( result == null ) { return null; }
                type = result.RTGetFieldOrPropType();
                if ( type == null ) { return null; }
            }

            return result;
        }

        ///Given a root object and a relative member path, returns the object that contains the leaf member
        public static object GetRelativeMemberParent(object root, string path) {
            if ( root == null || string.IsNullOrEmpty(path) ) { return null; }
            var parts = path.Split('.');
            if ( parts.Length == 1 ) { return root; }
            var member = root.GetType().RTGetFieldOrProp(parts[0]);
            if ( member == null ) { return null; }
            root = member.RTGetFieldOrPropValue(root);
            return GetRelativeMemberParent(root, string.Join(".", parts, 1, parts.Length - 1));
        }

        ///Utility. Given an expression returns a relative path, eg: '(Transform x) => x.position'
        public static string GetMemberPath<T, TResult>(System.Linq.Expressions.Expression<Func<T, TResult>> func) {
            var result = func.Body.ToString();
            return result.Substring(result.IndexOf('.') + 1);
        }

        ///Digs into starting type provided and returns instance|Public property/fields paths recursively found, based on predicates provided
        public static string[] GetMemberPaths(Type type, Predicate<Type> shouldInclude, Predicate<Type> shouldContinue, string currentPath = "", List<Type> recursionCheck = null) {
            var result = new List<string>();
            if ( recursionCheck == null ) { recursionCheck = new List<Type>(); }
            if ( recursionCheck.Contains(type) ) { return result.ToArray(); }
            recursionCheck.Add(type);
            foreach ( var _prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public) ) {
                var prop = _prop;
                if ( prop.CanRead && prop.CanWrite && shouldInclude(prop.PropertyType) ) {
                    result.Add(currentPath + prop.Name);
                    continue;
                }
                if ( prop.CanRead && shouldContinue(prop.PropertyType) ) {
                    result.AddRange(GetMemberPaths(prop.PropertyType, shouldInclude, shouldContinue, currentPath + prop.Name + ".", recursionCheck));
                }
            }

            foreach ( var _field in type.GetFields(BindingFlags.Instance | BindingFlags.Public) ) {
                var field = _field;
                if ( shouldInclude(field.FieldType) ) {
                    result.Add(currentPath + field.Name);
                    continue;
                }
                if ( shouldContinue(field.FieldType) ) {
                    result.AddRange(GetMemberPaths(field.FieldType, shouldInclude, shouldContinue, currentPath + field.Name + ".", recursionCheck));
                }
            }
            return result.ToArray();
        }

        // ///Given a root object and a relative path, gets and returns the member value
        // public static object GetRelativeMemberValue(object root, string path){
        // 	var member = GetRelativeMember(root, path);
        // 	var parent = GetRelativeMemberParent(root, path);
        // 	return member.RTGetFieldOrPropValue(parent);
        // }

        // ///Given a root object and a relative path, sets the member value
        // public static void SetRelativeMemberValue(object root, string path, object value){
        // 	var member = GetRelativeMember(root, path);
        // 	var parent = GetRelativeMemberParent(root, path);
        // 	member.RTSetFieldOrPropValue(parent, value);
        // }

        ///----------------------------------------------------------------------------------------------

    }
}