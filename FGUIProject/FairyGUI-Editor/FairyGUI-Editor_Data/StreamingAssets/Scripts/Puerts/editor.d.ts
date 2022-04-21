
declare module 'csharp' {
    interface $Ref<T> {
        value: T
    }
    
    namespace System {
        interface Array$1<T> extends System.Array {
            get_Item(index: number):T;
            
            set_Item(index: number, value: T):void;
        }
    }
    
    type $Task<T> = System.Threading.Tasks.Task$1<T>
    
    namespace System {
        class Int32 extends System.ValueType {
            
        }
        class ValueType extends System.Object {
            
        }
        class Object {
            public constructor();
            public Equals($obj: any):boolean;
            public static Equals($objA: any, $objB: any):boolean;
            public GetHashCode():number;
            public GetType():System.Type;
            public ToString():string;
            public static ReferenceEquals($objA: any, $objB: any):boolean;
            
        }
        class Void extends System.ValueType {
            
        }
        class Boolean extends System.ValueType {
            
        }
        type Converter$2<TInput,TOutput> = (input: TInput) => TOutput;
        type MulticastDelegate = (...args:any[]) => any;
        var MulticastDelegate: {new (func: (...args:any[]) => any): MulticastDelegate;}
        class Delegate extends System.Object {
            public Method: System.Reflection.MethodInfo;
            public Target: any;
            public static CreateDelegate($type: System.Type, $firstArgument: any, $method: System.Reflection.MethodInfo, $throwOnBindFailure: boolean):Function;
            public static CreateDelegate($type: System.Type, $firstArgument: any, $method: System.Reflection.MethodInfo):Function;
            public static CreateDelegate($type: System.Type, $method: System.Reflection.MethodInfo, $throwOnBindFailure: boolean):Function;
            public static CreateDelegate($type: System.Type, $method: System.Reflection.MethodInfo):Function;
            public static CreateDelegate($type: System.Type, $target: any, $method: string):Function;
            public static CreateDelegate($type: System.Type, $target: System.Type, $method: string, $ignoreCase: boolean, $throwOnBindFailure: boolean):Function;
            public static CreateDelegate($type: System.Type, $target: System.Type, $method: string):Function;
            public static CreateDelegate($type: System.Type, $target: System.Type, $method: string, $ignoreCase: boolean):Function;
            public static CreateDelegate($type: System.Type, $target: any, $method: string, $ignoreCase: boolean, $throwOnBindFailure: boolean):Function;
            public static CreateDelegate($type: System.Type, $target: any, $method: string, $ignoreCase: boolean):Function;
            public DynamicInvoke(...args: any[]):any;
            public Clone():any;
            public GetObjectData($info: System.Runtime.Serialization.SerializationInfo, $context: System.Runtime.Serialization.StreamingContext):void;
            public GetInvocationList():System.Array$1<Function>;
            public static Combine($a: Function, $b: Function):Function;
            public static Combine(...delegates: Function[]):Function;
            public static Remove($source: Function, $value: Function):Function;
            public static RemoveAll($source: Function, $value: Function):Function;
            public static op_Equality($d1: Function, $d2: Function):boolean;
            public static op_Inequality($d1: Function, $d2: Function):boolean;
            
        }
        type Predicate$1<T> = (obj: T) => boolean;
        type Action$1<T> = (obj: T) => void;
        type Comparison$1<T> = (x: T, y: T) => number;
        class Type extends System.Reflection.MemberInfo {
            public static FilterAttribute: System.Reflection.MemberFilter;
            public static FilterName: System.Reflection.MemberFilter;
            public static FilterNameIgnoreCase: System.Reflection.MemberFilter;
            public static Missing: any;
            public static Delimiter: number;
            public static EmptyTypes: System.Array$1<System.Type>;
            public MemberType: System.Reflection.MemberTypes;
            public DeclaringType: System.Type;
            public DeclaringMethod: System.Reflection.MethodBase;
            public ReflectedType: System.Type;
            public StructLayoutAttribute: System.Runtime.InteropServices.StructLayoutAttribute;
            public GUID: System.Guid;
            public static DefaultBinder: System.Reflection.Binder;
            public Module: System.Reflection.Module;
            public Assembly: System.Reflection.Assembly;
            public TypeHandle: System.RuntimeTypeHandle;
            public FullName: string;
            public Namespace: string;
            public AssemblyQualifiedName: string;
            public BaseType: System.Type;
            public TypeInitializer: System.Reflection.ConstructorInfo;
            public IsNested: boolean;
            public Attributes: System.Reflection.TypeAttributes;
            public GenericParameterAttributes: System.Reflection.GenericParameterAttributes;
            public IsVisible: boolean;
            public IsNotPublic: boolean;
            public IsPublic: boolean;
            public IsNestedPublic: boolean;
            public IsNestedPrivate: boolean;
            public IsNestedFamily: boolean;
            public IsNestedAssembly: boolean;
            public IsNestedFamANDAssem: boolean;
            public IsNestedFamORAssem: boolean;
            public IsAutoLayout: boolean;
            public IsLayoutSequential: boolean;
            public IsExplicitLayout: boolean;
            public IsClass: boolean;
            public IsInterface: boolean;
            public IsValueType: boolean;
            public IsAbstract: boolean;
            public IsSealed: boolean;
            public IsEnum: boolean;
            public IsSpecialName: boolean;
            public IsImport: boolean;
            public IsSerializable: boolean;
            public IsAnsiClass: boolean;
            public IsUnicodeClass: boolean;
            public IsAutoClass: boolean;
            public IsArray: boolean;
            public IsGenericType: boolean;
            public IsGenericTypeDefinition: boolean;
            public IsConstructedGenericType: boolean;
            public IsGenericParameter: boolean;
            public GenericParameterPosition: number;
            public ContainsGenericParameters: boolean;
            public IsByRef: boolean;
            public IsPointer: boolean;
            public IsPrimitive: boolean;
            public IsCOMObject: boolean;
            public HasElementType: boolean;
            public IsContextful: boolean;
            public IsMarshalByRef: boolean;
            public GenericTypeArguments: System.Array$1<System.Type>;
            public IsSecurityCritical: boolean;
            public IsSecuritySafeCritical: boolean;
            public IsSecurityTransparent: boolean;
            public UnderlyingSystemType: System.Type;
            public static GetType($typeName: string, $assemblyResolver: System.Func$2<System.Reflection.AssemblyName, System.Reflection.Assembly>, $typeResolver: System.Func$4<System.Reflection.Assembly, string, boolean, System.Type>):System.Type;
            public static GetType($typeName: string, $assemblyResolver: System.Func$2<System.Reflection.AssemblyName, System.Reflection.Assembly>, $typeResolver: System.Func$4<System.Reflection.Assembly, string, boolean, System.Type>, $throwOnError: boolean):System.Type;
            public static GetType($typeName: string, $assemblyResolver: System.Func$2<System.Reflection.AssemblyName, System.Reflection.Assembly>, $typeResolver: System.Func$4<System.Reflection.Assembly, string, boolean, System.Type>, $throwOnError: boolean, $ignoreCase: boolean):System.Type;
            public MakePointerType():System.Type;
            public MakeByRefType():System.Type;
            public MakeArrayType():System.Type;
            public MakeArrayType($rank: number):System.Type;
            public static GetTypeFromProgID($progID: string):System.Type;
            public static GetTypeFromProgID($progID: string, $throwOnError: boolean):System.Type;
            public static GetTypeFromProgID($progID: string, $server: string):System.Type;
            public static GetTypeFromProgID($progID: string, $server: string, $throwOnError: boolean):System.Type;
            public static GetTypeFromCLSID($clsid: System.Guid):System.Type;
            public static GetTypeFromCLSID($clsid: System.Guid, $throwOnError: boolean):System.Type;
            public static GetTypeFromCLSID($clsid: System.Guid, $server: string):System.Type;
            public static GetTypeFromCLSID($clsid: System.Guid, $server: string, $throwOnError: boolean):System.Type;
            public static GetTypeCode($type: System.Type):System.TypeCode;
            public InvokeMember($name: string, $invokeAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $target: any, $args: System.Array$1<any>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>, $culture: System.Globalization.CultureInfo, $namedParameters: System.Array$1<string>):any;
            public InvokeMember($name: string, $invokeAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $target: any, $args: System.Array$1<any>, $culture: System.Globalization.CultureInfo):any;
            public InvokeMember($name: string, $invokeAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $target: any, $args: System.Array$1<any>):any;
            public static GetTypeHandle($o: any):System.RuntimeTypeHandle;
            public GetArrayRank():number;
            public GetConstructor($bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $callConvention: System.Reflection.CallingConventions, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.ConstructorInfo;
            public GetConstructor($bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.ConstructorInfo;
            public GetConstructor($types: System.Array$1<System.Type>):System.Reflection.ConstructorInfo;
            public GetConstructors():System.Array$1<System.Reflection.ConstructorInfo>;
            public GetConstructors($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.ConstructorInfo>;
            public GetMethod($name: string, $bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $callConvention: System.Reflection.CallingConventions, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.MethodInfo;
            public GetMethod($name: string, $bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.MethodInfo;
            public GetMethod($name: string, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.MethodInfo;
            public GetMethod($name: string, $types: System.Array$1<System.Type>):System.Reflection.MethodInfo;
            public GetMethod($name: string, $bindingAttr: System.Reflection.BindingFlags):System.Reflection.MethodInfo;
            public GetMethod($name: string):System.Reflection.MethodInfo;
            public GetMethods():System.Array$1<System.Reflection.MethodInfo>;
            public GetMethods($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.MethodInfo>;
            public GetField($name: string, $bindingAttr: System.Reflection.BindingFlags):System.Reflection.FieldInfo;
            public GetField($name: string):System.Reflection.FieldInfo;
            public GetFields():System.Array$1<System.Reflection.FieldInfo>;
            public GetFields($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.FieldInfo>;
            public GetInterface($name: string):System.Type;
            public GetInterface($name: string, $ignoreCase: boolean):System.Type;
            public GetInterfaces():System.Array$1<System.Type>;
            public FindInterfaces($filter: System.Reflection.TypeFilter, $filterCriteria: any):System.Array$1<System.Type>;
            public GetEvent($name: string):System.Reflection.EventInfo;
            public GetEvent($name: string, $bindingAttr: System.Reflection.BindingFlags):System.Reflection.EventInfo;
            public GetEvents():System.Array$1<System.Reflection.EventInfo>;
            public GetEvents($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.EventInfo>;
            public GetProperty($name: string, $bindingAttr: System.Reflection.BindingFlags, $binder: System.Reflection.Binder, $returnType: System.Type, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.PropertyInfo;
            public GetProperty($name: string, $returnType: System.Type, $types: System.Array$1<System.Type>, $modifiers: System.Array$1<System.Reflection.ParameterModifier>):System.Reflection.PropertyInfo;
            public GetProperty($name: string, $bindingAttr: System.Reflection.BindingFlags):System.Reflection.PropertyInfo;
            public GetProperty($name: string, $returnType: System.Type, $types: System.Array$1<System.Type>):System.Reflection.PropertyInfo;
            public GetProperty($name: string, $types: System.Array$1<System.Type>):System.Reflection.PropertyInfo;
            public GetProperty($name: string, $returnType: System.Type):System.Reflection.PropertyInfo;
            public GetProperty($name: string):System.Reflection.PropertyInfo;
            public GetProperties($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.PropertyInfo>;
            public GetProperties():System.Array$1<System.Reflection.PropertyInfo>;
            public GetNestedTypes():System.Array$1<System.Type>;
            public GetNestedTypes($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Type>;
            public GetNestedType($name: string):System.Type;
            public GetNestedType($name: string, $bindingAttr: System.Reflection.BindingFlags):System.Type;
            public GetMember($name: string):System.Array$1<System.Reflection.MemberInfo>;
            public GetMember($name: string, $bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.MemberInfo>;
            public GetMember($name: string, $type: System.Reflection.MemberTypes, $bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.MemberInfo>;
            public GetMembers():System.Array$1<System.Reflection.MemberInfo>;
            public GetMembers($bindingAttr: System.Reflection.BindingFlags):System.Array$1<System.Reflection.MemberInfo>;
            public GetDefaultMembers():System.Array$1<System.Reflection.MemberInfo>;
            public FindMembers($memberType: System.Reflection.MemberTypes, $bindingAttr: System.Reflection.BindingFlags, $filter: System.Reflection.MemberFilter, $filterCriteria: any):System.Array$1<System.Reflection.MemberInfo>;
            public GetGenericParameterConstraints():System.Array$1<System.Type>;
            public MakeGenericType(...typeArguments: System.Type[]):System.Type;
            public GetElementType():System.Type;
            public GetGenericArguments():System.Array$1<System.Type>;
            public GetGenericTypeDefinition():System.Type;
            public GetEnumNames():System.Array$1<string>;
            public GetEnumValues():System.Array;
            public GetEnumUnderlyingType():System.Type;
            public IsEnumDefined($value: any):boolean;
            public GetEnumName($value: any):string;
            public IsSubclassOf($c: System.Type):boolean;
            public IsInstanceOfType($o: any):boolean;
            public IsAssignableFrom($c: System.Type):boolean;
            public IsEquivalentTo($other: System.Type):boolean;
            public static GetTypeArray($args: System.Array$1<any>):System.Array$1<System.Type>;
            public Equals($o: any):boolean;
            public Equals($o: System.Type):boolean;
            public static op_Equality($left: System.Type, $right: System.Type):boolean;
            public static op_Inequality($left: System.Type, $right: System.Type):boolean;
            public GetInterfaceMap($interfaceType: System.Type):System.Reflection.InterfaceMapping;
            public GetType():System.Type;
            public static GetType($typeName: string):System.Type;
            public static GetType($typeName: string, $throwOnError: boolean):System.Type;
            public static GetType($typeName: string, $throwOnError: boolean, $ignoreCase: boolean):System.Type;
            public static ReflectionOnlyGetType($typeName: string, $throwIfNotFound: boolean, $ignoreCase: boolean):System.Type;
            public static GetTypeFromHandle($handle: System.RuntimeTypeHandle):System.Type;
            public GetType():System.Type;
            
        }
        class String extends System.Object {
            
        }
        class Array extends System.Object {
            public LongLength: bigint;
            public IsFixedSize: boolean;
            public IsReadOnly: boolean;
            public IsSynchronized: boolean;
            public SyncRoot: any;
            public Length: number;
            public Rank: number;
            public static CreateInstance($elementType: System.Type, ...lengths: bigint[]):System.Array;
            public CopyTo($array: System.Array, $index: number):void;
            public Clone():any;
            public static BinarySearch($array: System.Array, $value: any):number;
            public static Copy($sourceArray: System.Array, $destinationArray: System.Array, $length: bigint):void;
            public static Copy($sourceArray: System.Array, $sourceIndex: bigint, $destinationArray: System.Array, $destinationIndex: bigint, $length: bigint):void;
            public CopyTo($array: System.Array, $index: bigint):void;
            public GetLongLength($dimension: number):bigint;
            public GetValue($index: bigint):any;
            public GetValue($index1: bigint, $index2: bigint):any;
            public GetValue($index1: bigint, $index2: bigint, $index3: bigint):any;
            public GetValue(...indices: bigint[]):any;
            public static BinarySearch($array: System.Array, $index: number, $length: number, $value: any):number;
            public static BinarySearch($array: System.Array, $value: any, $comparer: System.Collections.IComparer):number;
            public static BinarySearch($array: System.Array, $index: number, $length: number, $value: any, $comparer: System.Collections.IComparer):number;
            public static IndexOf($array: System.Array, $value: any):number;
            public static IndexOf($array: System.Array, $value: any, $startIndex: number):number;
            public static IndexOf($array: System.Array, $value: any, $startIndex: number, $count: number):number;
            public static LastIndexOf($array: System.Array, $value: any):number;
            public static LastIndexOf($array: System.Array, $value: any, $startIndex: number):number;
            public static LastIndexOf($array: System.Array, $value: any, $startIndex: number, $count: number):number;
            public static Reverse($array: System.Array):void;
            public static Reverse($array: System.Array, $index: number, $length: number):void;
            public SetValue($value: any, $index: bigint):void;
            public SetValue($value: any, $index1: bigint, $index2: bigint):void;
            public SetValue($value: any, $index1: bigint, $index2: bigint, $index3: bigint):void;
            public SetValue($value: any, ...indices: bigint[]):void;
            public static Sort($array: System.Array):void;
            public static Sort($array: System.Array, $index: number, $length: number):void;
            public static Sort($array: System.Array, $comparer: System.Collections.IComparer):void;
            public static Sort($array: System.Array, $index: number, $length: number, $comparer: System.Collections.IComparer):void;
            public static Sort($keys: System.Array, $items: System.Array):void;
            public static Sort($keys: System.Array, $items: System.Array, $comparer: System.Collections.IComparer):void;
            public static Sort($keys: System.Array, $items: System.Array, $index: number, $length: number):void;
            public static Sort($keys: System.Array, $items: System.Array, $index: number, $length: number, $comparer: System.Collections.IComparer):void;
            public GetEnumerator():System.Collections.IEnumerator;
            public GetLength($dimension: number):number;
            public GetLowerBound($dimension: number):number;
            public GetValue(...indices: number[]):any;
            public SetValue($value: any, ...indices: number[]):void;
            public GetUpperBound($dimension: number):number;
            public GetValue($index: number):any;
            public GetValue($index1: number, $index2: number):any;
            public GetValue($index1: number, $index2: number, $index3: number):any;
            public SetValue($value: any, $index: number):void;
            public SetValue($value: any, $index1: number, $index2: number):void;
            public SetValue($value: any, $index1: number, $index2: number, $index3: number):void;
            public static CreateInstance($elementType: System.Type, $length: number):System.Array;
            public static CreateInstance($elementType: System.Type, $length1: number, $length2: number):System.Array;
            public static CreateInstance($elementType: System.Type, $length1: number, $length2: number, $length3: number):System.Array;
            public static CreateInstance($elementType: System.Type, ...lengths: number[]):System.Array;
            public static CreateInstance($elementType: System.Type, $lengths: System.Array$1<number>, $lowerBounds: System.Array$1<number>):System.Array;
            public static Clear($array: System.Array, $index: number, $length: number):void;
            public static Copy($sourceArray: System.Array, $destinationArray: System.Array, $length: number):void;
            public static Copy($sourceArray: System.Array, $sourceIndex: number, $destinationArray: System.Array, $destinationIndex: number, $length: number):void;
            public static ConstrainedCopy($sourceArray: System.Array, $sourceIndex: number, $destinationArray: System.Array, $destinationIndex: number, $length: number):void;
            public Initialize():void;
            
        }
        class Int64 extends System.ValueType {
            
        }
        class Char extends System.ValueType {
            
        }
        class Enum extends System.ValueType {
            
        }
        type Func$2<T,TResult> = (arg: T) => TResult;
        type Func$4<T1,T2,T3,TResult> = (arg1: T1, arg2: T2, arg3: T3) => TResult;
        class Attribute extends System.Object {
            
        }
        class Guid extends System.ValueType {
            
        }
        enum TypeCode { Empty = 0, Object = 1, DBNull = 2, Boolean = 3, Char = 4, SByte = 5, Byte = 6, Int16 = 7, UInt16 = 8, Int32 = 9, UInt32 = 10, Int64 = 11, UInt64 = 12, Single = 13, Double = 14, Decimal = 15, DateTime = 16, String = 18 }
        class RuntimeTypeHandle extends System.ValueType {
            
        }
        class MarshalByRefObject extends System.Object {
            
        }
        class DateTime extends System.ValueType {
            
        }
        class Byte extends System.ValueType {
            
        }
        class Single extends System.ValueType {
            
        }
        interface Single {
            FormattedString($fractionDigits?: number):string;
            
        }
        
        class UInt32 extends System.ValueType {
            
        }
        class UInt64 extends System.ValueType {
            
        }
        class Double extends System.ValueType {
            
        }
        class IntPtr extends System.ValueType {
            
        }
        type Func$1<TResult> = () => TResult;
        type Action = () => void;
        var Action: {new (func: () => void): Action;}
        class Exception extends System.Object {
            
        }
        class UInt16 extends System.ValueType {
            
        }
        type Action$2<T1,T2> = (arg1: T1, arg2: T2) => void;
        type Action$3<T1,T2,T3> = (arg1: T1, arg2: T2, arg3: T3) => void;
        class Nullable$1<T> extends System.ValueType {
            
        }
        class Int16 extends System.ValueType {
            
        }
        
    }
    namespace System.Collections.Generic {
        interface IList$1<T> {
            get_Item($index: number):T;
            set_Item($index: number, $value: T):void;
            IndexOf($item: T):number;
            Insert($index: number, $item: T):void;
            RemoveAt($index: number):void;
            
        }
        class List$1<T> extends System.Object {
            public Capacity: number;
            public Count: number;
            public constructor();
            public constructor($capacity: number);
            public constructor($collection: System.Collections.Generic.IEnumerable$1<T>);
            public get_Item($index: number):T;
            public set_Item($index: number, $value: T):void;
            public Add($item: T):void;
            public AddRange($collection: System.Collections.Generic.IEnumerable$1<T>):void;
            public AsReadOnly():System.Collections.ObjectModel.ReadOnlyCollection$1<T>;
            public BinarySearch($index: number, $count: number, $item: T, $comparer: System.Collections.Generic.IComparer$1<T>):number;
            public BinarySearch($item: T):number;
            public BinarySearch($item: T, $comparer: System.Collections.Generic.IComparer$1<T>):number;
            public Clear():void;
            public Contains($item: T):boolean;
            public CopyTo($array: System.Array$1<T>):void;
            public CopyTo($index: number, $array: System.Array$1<T>, $arrayIndex: number, $count: number):void;
            public CopyTo($array: System.Array$1<T>, $arrayIndex: number):void;
            public Exists($match: System.Predicate$1<T>):boolean;
            public Find($match: System.Predicate$1<T>):T;
            public FindAll($match: System.Predicate$1<T>):System.Collections.Generic.List$1<T>;
            public FindIndex($match: System.Predicate$1<T>):number;
            public FindIndex($startIndex: number, $match: System.Predicate$1<T>):number;
            public FindIndex($startIndex: number, $count: number, $match: System.Predicate$1<T>):number;
            public FindLast($match: System.Predicate$1<T>):T;
            public FindLastIndex($match: System.Predicate$1<T>):number;
            public FindLastIndex($startIndex: number, $match: System.Predicate$1<T>):number;
            public FindLastIndex($startIndex: number, $count: number, $match: System.Predicate$1<T>):number;
            public ForEach($action: System.Action$1<T>):void;
            public GetEnumerator():System.Collections.Generic.List$1.Enumerator<T>;
            public GetRange($index: number, $count: number):System.Collections.Generic.List$1<T>;
            public IndexOf($item: T):number;
            public IndexOf($item: T, $index: number):number;
            public IndexOf($item: T, $index: number, $count: number):number;
            public Insert($index: number, $item: T):void;
            public InsertRange($index: number, $collection: System.Collections.Generic.IEnumerable$1<T>):void;
            public LastIndexOf($item: T):number;
            public LastIndexOf($item: T, $index: number):number;
            public LastIndexOf($item: T, $index: number, $count: number):number;
            public Remove($item: T):boolean;
            public RemoveAll($match: System.Predicate$1<T>):number;
            public RemoveAt($index: number):void;
            public RemoveRange($index: number, $count: number):void;
            public Reverse():void;
            public Reverse($index: number, $count: number):void;
            public Sort():void;
            public Sort($comparer: System.Collections.Generic.IComparer$1<T>):void;
            public Sort($index: number, $count: number, $comparer: System.Collections.Generic.IComparer$1<T>):void;
            public Sort($comparison: System.Comparison$1<T>):void;
            public ToArray():System.Array$1<T>;
            public TrimExcess():void;
            public TrueForAll($match: System.Predicate$1<T>):boolean;
            
        }
        interface IEnumerable$1<T> {
            
        }
        interface IComparer$1<T> {
            
        }
        class Dictionary$2<TKey,TValue> extends System.Object {
            public Comparer: System.Collections.Generic.IEqualityComparer$1<TKey>;
            public Count: number;
            public Keys: System.Collections.Generic.Dictionary$2.KeyCollection<TKey, TValue>;
            public Values: System.Collections.Generic.Dictionary$2.ValueCollection<TKey, TValue>;
            public constructor();
            public constructor($capacity: number);
            public constructor($comparer: System.Collections.Generic.IEqualityComparer$1<TKey>);
            public constructor($capacity: number, $comparer: System.Collections.Generic.IEqualityComparer$1<TKey>);
            public get_Item($key: TKey):TValue;
            public set_Item($key: TKey, $value: TValue):void;
            public Add($key: TKey, $value: TValue):void;
            public Clear():void;
            public ContainsKey($key: TKey):boolean;
            public ContainsValue($value: TValue):boolean;
            public GetEnumerator():System.Collections.Generic.Dictionary$2.Enumerator<TKey, TValue>;
            public GetObjectData($info: System.Runtime.Serialization.SerializationInfo, $context: System.Runtime.Serialization.StreamingContext):void;
            public OnDeserialization($sender: any):void;
            public Remove($key: TKey):boolean;
            public TryGetValue($key: TKey, $value: $Ref<TValue>):boolean;
            
        }
        interface IEqualityComparer$1<T> {
            
        }
        interface IDictionary$2<TKey,TValue> {
            Keys: System.Collections.Generic.ICollection$1<TKey>;
            Values: System.Collections.Generic.ICollection$1<TValue>;
            get_Item($key: TKey):TValue;
            set_Item($key: TKey, $value: TValue):void;
            ContainsKey($key: TKey):boolean;
            Add($key: TKey, $value: TValue):void;
            Remove($key: TKey):boolean;
            TryGetValue($key: TKey, $value: $Ref<TValue>):boolean;
            
        }
        class KeyValuePair$2<TKey,TValue> extends System.ValueType {
            
        }
        interface ICollection$1<T> {
            
        }
        interface IEnumerator$1<T> {
            
        }
        
    }
    namespace System.Collections.ObjectModel {
        class ReadOnlyCollection$1<T> extends System.Object {
            
        }
        
    }
    namespace System.Collections.Generic.List$1 {
        class Enumerator<T> extends System.ValueType {
            
        }
        
    }
    namespace System.Collections.Generic.Dictionary$2 {
        class KeyCollection<TKey,TValue> extends System.Object {
            
        }
        class ValueCollection<TKey,TValue> extends System.Object {
            
        }
        class Enumerator<TKey,TValue> extends System.ValueType {
            
        }
        
    }
    namespace System.Runtime.Serialization {
        class SerializationInfo extends System.Object {
            
        }
        class StreamingContext extends System.ValueType {
            
        }
        
    }
    namespace System.Reflection {
        class MethodInfo extends System.Reflection.MethodBase {
            
        }
        class MethodBase extends System.Reflection.MemberInfo {
            
        }
        class MemberInfo extends System.Object {
            
        }
        type MemberFilter = (m: System.Reflection.MemberInfo, filterCriteria: any) => boolean;
        var MemberFilter: {new (func: (m: System.Reflection.MemberInfo, filterCriteria: any) => boolean): MemberFilter;}
        enum MemberTypes { Constructor = 1, Event = 2, Field = 4, Method = 8, Property = 16, TypeInfo = 32, Custom = 64, NestedType = 128, All = 191 }
        class AssemblyName extends System.Object {
            
        }
        class Assembly extends System.Object {
            
        }
        class Binder extends System.Object {
            
        }
        enum BindingFlags { Default = 0, IgnoreCase = 1, DeclaredOnly = 2, Instance = 4, Static = 8, Public = 16, NonPublic = 32, FlattenHierarchy = 64, InvokeMethod = 256, CreateInstance = 512, GetField = 1024, SetField = 2048, GetProperty = 4096, SetProperty = 8192, PutDispProperty = 16384, PutRefDispProperty = 32768, ExactBinding = 65536, SuppressChangeType = 131072, OptionalParamBinding = 262144, IgnoreReturn = 16777216 }
        class ParameterModifier extends System.ValueType {
            
        }
        class Module extends System.Object {
            
        }
        class ConstructorInfo extends System.Reflection.MethodBase {
            
        }
        enum CallingConventions { Standard = 1, VarArgs = 2, Any = 3, HasThis = 32, ExplicitThis = 64 }
        class FieldInfo extends System.Reflection.MemberInfo {
            
        }
        type TypeFilter = (m: System.Type, filterCriteria: any) => boolean;
        var TypeFilter: {new (func: (m: System.Type, filterCriteria: any) => boolean): TypeFilter;}
        class EventInfo extends System.Reflection.MemberInfo {
            
        }
        class PropertyInfo extends System.Reflection.MemberInfo {
            
        }
        enum TypeAttributes { VisibilityMask = 7, NotPublic = 0, Public = 1, NestedPublic = 2, NestedPrivate = 3, NestedFamily = 4, NestedAssembly = 5, NestedFamANDAssem = 6, NestedFamORAssem = 7, LayoutMask = 24, AutoLayout = 0, SequentialLayout = 8, ExplicitLayout = 16, ClassSemanticsMask = 32, Class = 0, Interface = 32, Abstract = 128, Sealed = 256, SpecialName = 1024, Import = 4096, Serializable = 8192, WindowsRuntime = 16384, StringFormatMask = 196608, AnsiClass = 0, UnicodeClass = 65536, AutoClass = 131072, CustomFormatClass = 196608, CustomFormatMask = 12582912, BeforeFieldInit = 1048576, ReservedMask = 264192, RTSpecialName = 2048, HasSecurity = 262144 }
        enum GenericParameterAttributes { None = 0, VarianceMask = 3, Covariant = 1, Contravariant = 2, SpecialConstraintMask = 28, ReferenceTypeConstraint = 4, NotNullableValueTypeConstraint = 8, DefaultConstructorConstraint = 16 }
        class InterfaceMapping extends System.ValueType {
            
        }
        
    }
    namespace System.Collections {
        interface IComparer {
            
        }
        interface IEnumerator {
            
        }
        interface IList {
            
        }
        interface IDictionary {
            
        }
        class Hashtable extends System.Object {
            
        }
        
    }
    namespace System.Runtime.InteropServices {
        class StructLayoutAttribute extends System.Attribute {
            
        }
        
    }
    namespace System.Globalization {
        class CultureInfo extends System.Object {
            
        }
        
    }
    namespace System.IO {
        class File extends System.Object {
            public static AppendAllText($path: string, $contents: string):void;
            public static AppendAllText($path: string, $contents: string, $encoding: System.Text.Encoding):void;
            public static AppendText($path: string):System.IO.StreamWriter;
            public static Copy($sourceFileName: string, $destFileName: string):void;
            public static Copy($sourceFileName: string, $destFileName: string, $overwrite: boolean):void;
            public static Create($path: string):System.IO.FileStream;
            public static Create($path: string, $bufferSize: number):System.IO.FileStream;
            public static Create($path: string, $bufferSize: number, $options: System.IO.FileOptions):System.IO.FileStream;
            public static Create($path: string, $bufferSize: number, $options: System.IO.FileOptions, $fileSecurity: System.Security.AccessControl.FileSecurity):System.IO.FileStream;
            public static CreateText($path: string):System.IO.StreamWriter;
            public static Delete($path: string):void;
            public static Exists($path: string):boolean;
            public static GetAccessControl($path: string):System.Security.AccessControl.FileSecurity;
            public static GetAccessControl($path: string, $includeSections: System.Security.AccessControl.AccessControlSections):System.Security.AccessControl.FileSecurity;
            public static GetAttributes($path: string):System.IO.FileAttributes;
            public static GetCreationTime($path: string):Date;
            public static GetCreationTimeUtc($path: string):Date;
            public static GetLastAccessTime($path: string):Date;
            public static GetLastAccessTimeUtc($path: string):Date;
            public static GetLastWriteTime($path: string):Date;
            public static GetLastWriteTimeUtc($path: string):Date;
            public static Move($sourceFileName: string, $destFileName: string):void;
            public static Open($path: string, $mode: System.IO.FileMode):System.IO.FileStream;
            public static Open($path: string, $mode: System.IO.FileMode, $access: System.IO.FileAccess):System.IO.FileStream;
            public static Open($path: string, $mode: System.IO.FileMode, $access: System.IO.FileAccess, $share: System.IO.FileShare):System.IO.FileStream;
            public static OpenRead($path: string):System.IO.FileStream;
            public static OpenText($path: string):System.IO.StreamReader;
            public static OpenWrite($path: string):System.IO.FileStream;
            public static Replace($sourceFileName: string, $destinationFileName: string, $destinationBackupFileName: string):void;
            public static Replace($sourceFileName: string, $destinationFileName: string, $destinationBackupFileName: string, $ignoreMetadataErrors: boolean):void;
            public static SetAccessControl($path: string, $fileSecurity: System.Security.AccessControl.FileSecurity):void;
            public static SetAttributes($path: string, $fileAttributes: System.IO.FileAttributes):void;
            public static SetCreationTime($path: string, $creationTime: Date):void;
            public static SetCreationTimeUtc($path: string, $creationTimeUtc: Date):void;
            public static SetLastAccessTime($path: string, $lastAccessTime: Date):void;
            public static SetLastAccessTimeUtc($path: string, $lastAccessTimeUtc: Date):void;
            public static SetLastWriteTime($path: string, $lastWriteTime: Date):void;
            public static SetLastWriteTimeUtc($path: string, $lastWriteTimeUtc: Date):void;
            public static ReadAllBytes($path: string):System.Array$1<number>;
            public static ReadAllLines($path: string):System.Array$1<string>;
            public static ReadAllLines($path: string, $encoding: System.Text.Encoding):System.Array$1<string>;
            public static ReadAllText($path: string):string;
            public static ReadAllText($path: string, $encoding: System.Text.Encoding):string;
            public static WriteAllBytes($path: string, $bytes: System.Array$1<number>):void;
            public static WriteAllLines($path: string, $contents: System.Array$1<string>):void;
            public static WriteAllLines($path: string, $contents: System.Array$1<string>, $encoding: System.Text.Encoding):void;
            public static WriteAllText($path: string, $contents: string):void;
            public static WriteAllText($path: string, $contents: string, $encoding: System.Text.Encoding):void;
            public static Encrypt($path: string):void;
            public static Decrypt($path: string):void;
            public static ReadLines($path: string):System.Collections.Generic.IEnumerable$1<string>;
            public static ReadLines($path: string, $encoding: System.Text.Encoding):System.Collections.Generic.IEnumerable$1<string>;
            public static AppendAllLines($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>):void;
            public static AppendAllLines($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>, $encoding: System.Text.Encoding):void;
            public static WriteAllLines($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>):void;
            public static WriteAllLines($path: string, $contents: System.Collections.Generic.IEnumerable$1<string>, $encoding: System.Text.Encoding):void;
            
        }
        class StreamWriter extends System.IO.TextWriter {
            
        }
        class TextWriter extends System.MarshalByRefObject {
            
        }
        class FileStream extends System.IO.Stream {
            
        }
        class Stream extends System.MarshalByRefObject {
            
        }
        enum FileOptions { None = 0, Encrypted = 16384, DeleteOnClose = 67108864, SequentialScan = 134217728, RandomAccess = 268435456, Asynchronous = 1073741824, WriteThrough = -2147483648 }
        enum FileAttributes { Archive = 32, Compressed = 2048, Device = 64, Directory = 16, Encrypted = 16384, Hidden = 2, Normal = 128, NotContentIndexed = 8192, Offline = 4096, ReadOnly = 1, ReparsePoint = 1024, SparseFile = 512, System = 4, Temporary = 256, IntegrityStream = 32768, NoScrubData = 131072 }
        enum FileMode { CreateNew = 1, Create = 2, Open = 3, OpenOrCreate = 4, Truncate = 5, Append = 6 }
        enum FileAccess { Read = 1, Write = 2, ReadWrite = 3 }
        enum FileShare { None = 0, Read = 1, Write = 2, ReadWrite = 3, Delete = 4, Inheritable = 16 }
        class StreamReader extends System.IO.TextReader {
            
        }
        class TextReader extends System.MarshalByRefObject {
            
        }
        class Directory extends System.Object {
            public static GetFiles($path: string):System.Array$1<string>;
            public static GetFiles($path: string, $searchPattern: string):System.Array$1<string>;
            public static GetFiles($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption):System.Array$1<string>;
            public static GetDirectories($path: string):System.Array$1<string>;
            public static GetDirectories($path: string, $searchPattern: string):System.Array$1<string>;
            public static GetDirectories($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption):System.Array$1<string>;
            public static GetFileSystemEntries($path: string):System.Array$1<string>;
            public static GetFileSystemEntries($path: string, $searchPattern: string):System.Array$1<string>;
            public static GetFileSystemEntries($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption):System.Array$1<string>;
            public static EnumerateDirectories($path: string):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateDirectories($path: string, $searchPattern: string):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateDirectories($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateFiles($path: string):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateFiles($path: string, $searchPattern: string):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateFiles($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateFileSystemEntries($path: string):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateFileSystemEntries($path: string, $searchPattern: string):System.Collections.Generic.IEnumerable$1<string>;
            public static EnumerateFileSystemEntries($path: string, $searchPattern: string, $searchOption: System.IO.SearchOption):System.Collections.Generic.IEnumerable$1<string>;
            public static GetDirectoryRoot($path: string):string;
            public static CreateDirectory($path: string):System.IO.DirectoryInfo;
            public static CreateDirectory($path: string, $directorySecurity: System.Security.AccessControl.DirectorySecurity):System.IO.DirectoryInfo;
            public static Delete($path: string):void;
            public static Delete($path: string, $recursive: boolean):void;
            public static Exists($path: string):boolean;
            public static GetLastAccessTime($path: string):Date;
            public static GetLastAccessTimeUtc($path: string):Date;
            public static GetLastWriteTime($path: string):Date;
            public static GetLastWriteTimeUtc($path: string):Date;
            public static GetCreationTime($path: string):Date;
            public static GetCreationTimeUtc($path: string):Date;
            public static GetCurrentDirectory():string;
            public static GetLogicalDrives():System.Array$1<string>;
            public static GetParent($path: string):System.IO.DirectoryInfo;
            public static Move($sourceDirName: string, $destDirName: string):void;
            public static SetAccessControl($path: string, $directorySecurity: System.Security.AccessControl.DirectorySecurity):void;
            public static SetCreationTime($path: string, $creationTime: Date):void;
            public static SetCreationTimeUtc($path: string, $creationTimeUtc: Date):void;
            public static SetCurrentDirectory($path: string):void;
            public static SetLastAccessTime($path: string, $lastAccessTime: Date):void;
            public static SetLastAccessTimeUtc($path: string, $lastAccessTimeUtc: Date):void;
            public static SetLastWriteTime($path: string, $lastWriteTime: Date):void;
            public static SetLastWriteTimeUtc($path: string, $lastWriteTimeUtc: Date):void;
            public static GetAccessControl($path: string, $includeSections: System.Security.AccessControl.AccessControlSections):System.Security.AccessControl.DirectorySecurity;
            public static GetAccessControl($path: string):System.Security.AccessControl.DirectorySecurity;
            
        }
        enum SearchOption { TopDirectoryOnly = 0, AllDirectories = 1 }
        class DirectoryInfo extends System.IO.FileSystemInfo {
            public Exists: boolean;
            public Name: string;
            public Parent: System.IO.DirectoryInfo;
            public Root: System.IO.DirectoryInfo;
            public constructor($path: string);
            public Create():void;
            public CreateSubdirectory($path: string):System.IO.DirectoryInfo;
            public GetFiles():System.Array$1<System.IO.FileInfo>;
            public GetFiles($searchPattern: string):System.Array$1<System.IO.FileInfo>;
            public GetDirectories():System.Array$1<System.IO.DirectoryInfo>;
            public GetDirectories($searchPattern: string):System.Array$1<System.IO.DirectoryInfo>;
            public GetFileSystemInfos():System.Array$1<System.IO.FileSystemInfo>;
            public GetFileSystemInfos($searchPattern: string):System.Array$1<System.IO.FileSystemInfo>;
            public GetFileSystemInfos($searchPattern: string, $searchOption: System.IO.SearchOption):System.Array$1<System.IO.FileSystemInfo>;
            public Delete():void;
            public Delete($recursive: boolean):void;
            public MoveTo($destDirName: string):void;
            public GetDirectories($searchPattern: string, $searchOption: System.IO.SearchOption):System.Array$1<System.IO.DirectoryInfo>;
            public GetFiles($searchPattern: string, $searchOption: System.IO.SearchOption):System.Array$1<System.IO.FileInfo>;
            public Create($directorySecurity: System.Security.AccessControl.DirectorySecurity):void;
            public CreateSubdirectory($path: string, $directorySecurity: System.Security.AccessControl.DirectorySecurity):System.IO.DirectoryInfo;
            public GetAccessControl():System.Security.AccessControl.DirectorySecurity;
            public GetAccessControl($includeSections: System.Security.AccessControl.AccessControlSections):System.Security.AccessControl.DirectorySecurity;
            public SetAccessControl($directorySecurity: System.Security.AccessControl.DirectorySecurity):void;
            public EnumerateDirectories():System.Collections.Generic.IEnumerable$1<System.IO.DirectoryInfo>;
            public EnumerateDirectories($searchPattern: string):System.Collections.Generic.IEnumerable$1<System.IO.DirectoryInfo>;
            public EnumerateDirectories($searchPattern: string, $searchOption: System.IO.SearchOption):System.Collections.Generic.IEnumerable$1<System.IO.DirectoryInfo>;
            public EnumerateFiles():System.Collections.Generic.IEnumerable$1<System.IO.FileInfo>;
            public EnumerateFiles($searchPattern: string):System.Collections.Generic.IEnumerable$1<System.IO.FileInfo>;
            public EnumerateFiles($searchPattern: string, $searchOption: System.IO.SearchOption):System.Collections.Generic.IEnumerable$1<System.IO.FileInfo>;
            public EnumerateFileSystemInfos():System.Collections.Generic.IEnumerable$1<System.IO.FileSystemInfo>;
            public EnumerateFileSystemInfos($searchPattern: string):System.Collections.Generic.IEnumerable$1<System.IO.FileSystemInfo>;
            public EnumerateFileSystemInfos($searchPattern: string, $searchOption: System.IO.SearchOption):System.Collections.Generic.IEnumerable$1<System.IO.FileSystemInfo>;
            
        }
        class FileSystemInfo extends System.MarshalByRefObject {
            
        }
        class FileInfo extends System.IO.FileSystemInfo {
            public Name: string;
            public Length: bigint;
            public DirectoryName: string;
            public Directory: System.IO.DirectoryInfo;
            public IsReadOnly: boolean;
            public Exists: boolean;
            public constructor($fileName: string);
            public GetAccessControl():System.Security.AccessControl.FileSecurity;
            public GetAccessControl($includeSections: System.Security.AccessControl.AccessControlSections):System.Security.AccessControl.FileSecurity;
            public SetAccessControl($fileSecurity: System.Security.AccessControl.FileSecurity):void;
            public OpenText():System.IO.StreamReader;
            public CreateText():System.IO.StreamWriter;
            public AppendText():System.IO.StreamWriter;
            public CopyTo($destFileName: string):System.IO.FileInfo;
            public CopyTo($destFileName: string, $overwrite: boolean):System.IO.FileInfo;
            public Create():System.IO.FileStream;
            public Delete():void;
            public Decrypt():void;
            public Encrypt():void;
            public Open($mode: System.IO.FileMode):System.IO.FileStream;
            public Open($mode: System.IO.FileMode, $access: System.IO.FileAccess):System.IO.FileStream;
            public Open($mode: System.IO.FileMode, $access: System.IO.FileAccess, $share: System.IO.FileShare):System.IO.FileStream;
            public OpenRead():System.IO.FileStream;
            public OpenWrite():System.IO.FileStream;
            public MoveTo($destFileName: string):void;
            public Replace($destinationFileName: string, $destinationBackupFileName: string):System.IO.FileInfo;
            public Replace($destinationFileName: string, $destinationBackupFileName: string, $ignoreMetadataErrors: boolean):System.IO.FileInfo;
            
        }
        class Path extends System.Object {
            public static AltDirectorySeparatorChar: number;
            public static DirectorySeparatorChar: number;
            public static PathSeparator: number;
            public static VolumeSeparatorChar: number;
            public static ChangeExtension($path: string, $extension: string):string;
            public static Combine($path1: string, $path2: string):string;
            public static GetDirectoryName($path: string):string;
            public static GetExtension($path: string):string;
            public static GetFileName($path: string):string;
            public static GetFileNameWithoutExtension($path: string):string;
            public static GetFullPath($path: string):string;
            public static GetPathRoot($path: string):string;
            public static GetTempFileName():string;
            public static GetTempPath():string;
            public static HasExtension($path: string):boolean;
            public static IsPathRooted($path: string):boolean;
            public static GetInvalidFileNameChars():System.Array$1<number>;
            public static GetInvalidPathChars():System.Array$1<number>;
            public static GetRandomFileName():string;
            public static Combine(...paths: string[]):string;
            public static Combine($path1: string, $path2: string, $path3: string):string;
            public static Combine($path1: string, $path2: string, $path3: string, $path4: string):string;
            
        }
        
    }
    namespace System.Text {
        class Encoding extends System.Object {
            
        }
        class StringBuilder extends System.Object {
            
        }
        
    }
    namespace System.Security.AccessControl {
        class FileSecurity extends System.Security.AccessControl.FileSystemSecurity {
            
        }
        class FileSystemSecurity extends System.Security.AccessControl.NativeObjectSecurity {
            
        }
        class NativeObjectSecurity extends System.Security.AccessControl.CommonObjectSecurity {
            
        }
        class CommonObjectSecurity extends System.Security.AccessControl.ObjectSecurity {
            
        }
        class ObjectSecurity extends System.Object {
            
        }
        enum AccessControlSections { None = 0, Audit = 1, Access = 2, Owner = 4, Group = 8, All = 15 }
        class DirectorySecurity extends System.Security.AccessControl.FileSystemSecurity {
            
        }
        
    }
    namespace UnityEngine {
        class Object extends System.Object {
            public name: string;
            public hideFlags: UnityEngine.HideFlags;
            public constructor();
            public GetInstanceID():number;
            public static op_Implicit($exists: UnityEngine.Object):boolean;
            public static Instantiate($original: UnityEngine.Object, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $parent: UnityEngine.Transform):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $parent: UnityEngine.Transform):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $parent: UnityEngine.Transform, $instantiateInWorldSpace: boolean):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $parent: UnityEngine.Transform):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $parent: UnityEngine.Transform):UnityEngine.Object;
            public static Instantiate($original: UnityEngine.Object, $parent: UnityEngine.Transform, $worldPositionStays: boolean):UnityEngine.Object;
            public static Destroy($obj: UnityEngine.Object, $t: number):void;
            public static Destroy($obj: UnityEngine.Object):void;
            public static DestroyImmediate($obj: UnityEngine.Object, $allowDestroyingAssets: boolean):void;
            public static DestroyImmediate($obj: UnityEngine.Object):void;
            public static FindObjectsOfType($type: System.Type):System.Array$1<UnityEngine.Object>;
            public static DontDestroyOnLoad($target: UnityEngine.Object):void;
            public static FindObjectOfType($type: System.Type):UnityEngine.Object;
            public static op_Equality($x: UnityEngine.Object, $y: UnityEngine.Object):boolean;
            public static op_Inequality($x: UnityEngine.Object, $y: UnityEngine.Object):boolean;
            
        }
        class Vector3 extends System.ValueType {
            public static kEpsilon: number;
            public static kEpsilonNormalSqrt: number;
            public x: number;
            public y: number;
            public z: number;
            public normalized: UnityEngine.Vector3;
            public magnitude: number;
            public sqrMagnitude: number;
            public static zero: UnityEngine.Vector3;
            public static one: UnityEngine.Vector3;
            public static forward: UnityEngine.Vector3;
            public static back: UnityEngine.Vector3;
            public static up: UnityEngine.Vector3;
            public static down: UnityEngine.Vector3;
            public static left: UnityEngine.Vector3;
            public static right: UnityEngine.Vector3;
            public static positiveInfinity: UnityEngine.Vector3;
            public static negativeInfinity: UnityEngine.Vector3;
            public constructor($x: number, $y: number, $z: number);
            public constructor($x: number, $y: number);
            public static Slerp($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number):UnityEngine.Vector3;
            public static SlerpUnclamped($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number):UnityEngine.Vector3;
            public static OrthoNormalize($normal: $Ref<UnityEngine.Vector3>, $tangent: $Ref<UnityEngine.Vector3>):void;
            public static OrthoNormalize($normal: $Ref<UnityEngine.Vector3>, $tangent: $Ref<UnityEngine.Vector3>, $binormal: $Ref<UnityEngine.Vector3>):void;
            public static RotateTowards($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $maxRadiansDelta: number, $maxMagnitudeDelta: number):UnityEngine.Vector3;
            public static Lerp($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number):UnityEngine.Vector3;
            public static LerpUnclamped($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $t: number):UnityEngine.Vector3;
            public static MoveTowards($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $maxDistanceDelta: number):UnityEngine.Vector3;
            public static SmoothDamp($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $currentVelocity: $Ref<UnityEngine.Vector3>, $smoothTime: number, $maxSpeed: number):UnityEngine.Vector3;
            public static SmoothDamp($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $currentVelocity: $Ref<UnityEngine.Vector3>, $smoothTime: number):UnityEngine.Vector3;
            public static SmoothDamp($current: UnityEngine.Vector3, $target: UnityEngine.Vector3, $currentVelocity: $Ref<UnityEngine.Vector3>, $smoothTime: number, $maxSpeed: number, $deltaTime: number):UnityEngine.Vector3;
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public Set($newX: number, $newY: number, $newZ: number):void;
            public static Scale($a: UnityEngine.Vector3, $b: UnityEngine.Vector3):UnityEngine.Vector3;
            public Scale($scale: UnityEngine.Vector3):void;
            public static Cross($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3):UnityEngine.Vector3;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Vector3):boolean;
            public static Reflect($inDirection: UnityEngine.Vector3, $inNormal: UnityEngine.Vector3):UnityEngine.Vector3;
            public static Normalize($value: UnityEngine.Vector3):UnityEngine.Vector3;
            public Normalize():void;
            public static Dot($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3):number;
            public static Project($vector: UnityEngine.Vector3, $onNormal: UnityEngine.Vector3):UnityEngine.Vector3;
            public static ProjectOnPlane($vector: UnityEngine.Vector3, $planeNormal: UnityEngine.Vector3):UnityEngine.Vector3;
            public static Angle($from: UnityEngine.Vector3, $to: UnityEngine.Vector3):number;
            public static SignedAngle($from: UnityEngine.Vector3, $to: UnityEngine.Vector3, $axis: UnityEngine.Vector3):number;
            public static Distance($a: UnityEngine.Vector3, $b: UnityEngine.Vector3):number;
            public static ClampMagnitude($vector: UnityEngine.Vector3, $maxLength: number):UnityEngine.Vector3;
            public static Magnitude($vector: UnityEngine.Vector3):number;
            public static SqrMagnitude($vector: UnityEngine.Vector3):number;
            public static Min($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3):UnityEngine.Vector3;
            public static Max($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3):UnityEngine.Vector3;
            public static op_Addition($a: UnityEngine.Vector3, $b: UnityEngine.Vector3):UnityEngine.Vector3;
            public static op_Subtraction($a: UnityEngine.Vector3, $b: UnityEngine.Vector3):UnityEngine.Vector3;
            public static op_UnaryNegation($a: UnityEngine.Vector3):UnityEngine.Vector3;
            public static op_Multiply($a: UnityEngine.Vector3, $d: number):UnityEngine.Vector3;
            public static op_Multiply($d: number, $a: UnityEngine.Vector3):UnityEngine.Vector3;
            public static op_Division($a: UnityEngine.Vector3, $d: number):UnityEngine.Vector3;
            public static op_Equality($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3):boolean;
            public static op_Inequality($lhs: UnityEngine.Vector3, $rhs: UnityEngine.Vector3):boolean;
            public ToString():string;
            public ToString($format: string):string;
            
        }
        class Quaternion extends System.ValueType {
            public x: number;
            public y: number;
            public z: number;
            public w: number;
            public static kEpsilon: number;
            public static identity: UnityEngine.Quaternion;
            public eulerAngles: UnityEngine.Vector3;
            public normalized: UnityEngine.Quaternion;
            public constructor($x: number, $y: number, $z: number, $w: number);
            public static FromToRotation($fromDirection: UnityEngine.Vector3, $toDirection: UnityEngine.Vector3):UnityEngine.Quaternion;
            public static Inverse($rotation: UnityEngine.Quaternion):UnityEngine.Quaternion;
            public static Slerp($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number):UnityEngine.Quaternion;
            public static SlerpUnclamped($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number):UnityEngine.Quaternion;
            public static Lerp($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number):UnityEngine.Quaternion;
            public static LerpUnclamped($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion, $t: number):UnityEngine.Quaternion;
            public static AngleAxis($angle: number, $axis: UnityEngine.Vector3):UnityEngine.Quaternion;
            public static LookRotation($forward: UnityEngine.Vector3, $upwards: UnityEngine.Vector3):UnityEngine.Quaternion;
            public static LookRotation($forward: UnityEngine.Vector3):UnityEngine.Quaternion;
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public Set($newX: number, $newY: number, $newZ: number, $newW: number):void;
            public static op_Multiply($lhs: UnityEngine.Quaternion, $rhs: UnityEngine.Quaternion):UnityEngine.Quaternion;
            public static op_Multiply($rotation: UnityEngine.Quaternion, $point: UnityEngine.Vector3):UnityEngine.Vector3;
            public static op_Equality($lhs: UnityEngine.Quaternion, $rhs: UnityEngine.Quaternion):boolean;
            public static op_Inequality($lhs: UnityEngine.Quaternion, $rhs: UnityEngine.Quaternion):boolean;
            public static Dot($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion):number;
            public SetLookRotation($view: UnityEngine.Vector3):void;
            public SetLookRotation($view: UnityEngine.Vector3, $up: UnityEngine.Vector3):void;
            public static Angle($a: UnityEngine.Quaternion, $b: UnityEngine.Quaternion):number;
            public static Euler($x: number, $y: number, $z: number):UnityEngine.Quaternion;
            public static Euler($euler: UnityEngine.Vector3):UnityEngine.Quaternion;
            public ToAngleAxis($angle: $Ref<number>, $axis: $Ref<UnityEngine.Vector3>):void;
            public SetFromToRotation($fromDirection: UnityEngine.Vector3, $toDirection: UnityEngine.Vector3):void;
            public static RotateTowards($from: UnityEngine.Quaternion, $to: UnityEngine.Quaternion, $maxDegreesDelta: number):UnityEngine.Quaternion;
            public static Normalize($q: UnityEngine.Quaternion):UnityEngine.Quaternion;
            public Normalize():void;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Quaternion):boolean;
            public ToString():string;
            public ToString($format: string):string;
            
        }
        class Transform extends UnityEngine.Component {
            public position: UnityEngine.Vector3;
            public localPosition: UnityEngine.Vector3;
            public eulerAngles: UnityEngine.Vector3;
            public localEulerAngles: UnityEngine.Vector3;
            public right: UnityEngine.Vector3;
            public up: UnityEngine.Vector3;
            public forward: UnityEngine.Vector3;
            public rotation: UnityEngine.Quaternion;
            public localRotation: UnityEngine.Quaternion;
            public localScale: UnityEngine.Vector3;
            public parent: UnityEngine.Transform;
            public worldToLocalMatrix: UnityEngine.Matrix4x4;
            public localToWorldMatrix: UnityEngine.Matrix4x4;
            public root: UnityEngine.Transform;
            public childCount: number;
            public lossyScale: UnityEngine.Vector3;
            public hasChanged: boolean;
            public hierarchyCapacity: number;
            public hierarchyCount: number;
            public SetParent($p: UnityEngine.Transform):void;
            public SetParent($parent: UnityEngine.Transform, $worldPositionStays: boolean):void;
            public SetPositionAndRotation($position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion):void;
            public Translate($translation: UnityEngine.Vector3, $relativeTo: UnityEngine.Space):void;
            public Translate($translation: UnityEngine.Vector3):void;
            public Translate($x: number, $y: number, $z: number, $relativeTo: UnityEngine.Space):void;
            public Translate($x: number, $y: number, $z: number):void;
            public Translate($translation: UnityEngine.Vector3, $relativeTo: UnityEngine.Transform):void;
            public Translate($x: number, $y: number, $z: number, $relativeTo: UnityEngine.Transform):void;
            public Rotate($eulers: UnityEngine.Vector3, $relativeTo: UnityEngine.Space):void;
            public Rotate($eulers: UnityEngine.Vector3):void;
            public Rotate($xAngle: number, $yAngle: number, $zAngle: number, $relativeTo: UnityEngine.Space):void;
            public Rotate($xAngle: number, $yAngle: number, $zAngle: number):void;
            public Rotate($axis: UnityEngine.Vector3, $angle: number, $relativeTo: UnityEngine.Space):void;
            public Rotate($axis: UnityEngine.Vector3, $angle: number):void;
            public RotateAround($point: UnityEngine.Vector3, $axis: UnityEngine.Vector3, $angle: number):void;
            public LookAt($target: UnityEngine.Transform, $worldUp: UnityEngine.Vector3):void;
            public LookAt($target: UnityEngine.Transform):void;
            public LookAt($worldPosition: UnityEngine.Vector3, $worldUp: UnityEngine.Vector3):void;
            public LookAt($worldPosition: UnityEngine.Vector3):void;
            public TransformDirection($direction: UnityEngine.Vector3):UnityEngine.Vector3;
            public TransformDirection($x: number, $y: number, $z: number):UnityEngine.Vector3;
            public InverseTransformDirection($direction: UnityEngine.Vector3):UnityEngine.Vector3;
            public InverseTransformDirection($x: number, $y: number, $z: number):UnityEngine.Vector3;
            public TransformVector($vector: UnityEngine.Vector3):UnityEngine.Vector3;
            public TransformVector($x: number, $y: number, $z: number):UnityEngine.Vector3;
            public InverseTransformVector($vector: UnityEngine.Vector3):UnityEngine.Vector3;
            public InverseTransformVector($x: number, $y: number, $z: number):UnityEngine.Vector3;
            public TransformPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public TransformPoint($x: number, $y: number, $z: number):UnityEngine.Vector3;
            public InverseTransformPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public InverseTransformPoint($x: number, $y: number, $z: number):UnityEngine.Vector3;
            public DetachChildren():void;
            public SetAsFirstSibling():void;
            public SetAsLastSibling():void;
            public SetSiblingIndex($index: number):void;
            public GetSiblingIndex():number;
            public Find($n: string):UnityEngine.Transform;
            public IsChildOf($parent: UnityEngine.Transform):boolean;
            public GetEnumerator():System.Collections.IEnumerator;
            public GetChild($index: number):UnityEngine.Transform;
            
        }
        class Component extends UnityEngine.Object {
            public transform: UnityEngine.Transform;
            public gameObject: UnityEngine.GameObject;
            public tag: string;
            public constructor();
            public GetComponent($type: System.Type):UnityEngine.Component;
            public GetComponent($type: string):UnityEngine.Component;
            public GetComponentInChildren($t: System.Type, $includeInactive: boolean):UnityEngine.Component;
            public GetComponentInChildren($t: System.Type):UnityEngine.Component;
            public GetComponentsInChildren($t: System.Type, $includeInactive: boolean):System.Array$1<UnityEngine.Component>;
            public GetComponentsInChildren($t: System.Type):System.Array$1<UnityEngine.Component>;
            public GetComponentInParent($t: System.Type):UnityEngine.Component;
            public GetComponentsInParent($t: System.Type, $includeInactive: boolean):System.Array$1<UnityEngine.Component>;
            public GetComponentsInParent($t: System.Type):System.Array$1<UnityEngine.Component>;
            public GetComponents($type: System.Type):System.Array$1<UnityEngine.Component>;
            public GetComponents($type: System.Type, $results: System.Collections.Generic.List$1<UnityEngine.Component>):void;
            public CompareTag($tag: string):boolean;
            public SendMessageUpwards($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions):void;
            public SendMessageUpwards($methodName: string, $value: any):void;
            public SendMessageUpwards($methodName: string):void;
            public SendMessageUpwards($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            public SendMessage($methodName: string, $value: any):void;
            public SendMessage($methodName: string):void;
            public SendMessage($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions):void;
            public SendMessage($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            public BroadcastMessage($methodName: string, $parameter: any, $options: UnityEngine.SendMessageOptions):void;
            public BroadcastMessage($methodName: string, $parameter: any):void;
            public BroadcastMessage($methodName: string):void;
            public BroadcastMessage($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            
        }
        enum HideFlags { None = 0, HideInHierarchy = 1, HideInInspector = 2, DontSaveInEditor = 4, NotEditable = 8, DontSaveInBuild = 16, DontUnloadUnusedAsset = 32, DontSave = 52, HideAndDontSave = 61 }
        class GameObject extends UnityEngine.Object {
            public transform: UnityEngine.Transform;
            public layer: number;
            public activeSelf: boolean;
            public activeInHierarchy: boolean;
            public isStatic: boolean;
            public tag: string;
            public scene: UnityEngine.SceneManagement.Scene;
            public gameObject: UnityEngine.GameObject;
            public constructor($name: string);
            public constructor();
            public constructor($name: string, ...components: System.Type[]);
            public static CreatePrimitive($type: UnityEngine.PrimitiveType):UnityEngine.GameObject;
            public GetComponent($type: System.Type):UnityEngine.Component;
            public GetComponent($type: string):UnityEngine.Component;
            public GetComponentInChildren($type: System.Type, $includeInactive: boolean):UnityEngine.Component;
            public GetComponentInChildren($type: System.Type):UnityEngine.Component;
            public GetComponentInParent($type: System.Type):UnityEngine.Component;
            public GetComponents($type: System.Type):System.Array$1<UnityEngine.Component>;
            public GetComponents($type: System.Type, $results: System.Collections.Generic.List$1<UnityEngine.Component>):void;
            public GetComponentsInChildren($type: System.Type):System.Array$1<UnityEngine.Component>;
            public GetComponentsInChildren($type: System.Type, $includeInactive: boolean):System.Array$1<UnityEngine.Component>;
            public GetComponentsInParent($type: System.Type):System.Array$1<UnityEngine.Component>;
            public GetComponentsInParent($type: System.Type, $includeInactive: boolean):System.Array$1<UnityEngine.Component>;
            public static FindWithTag($tag: string):UnityEngine.GameObject;
            public SendMessageUpwards($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            public SendMessage($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            public BroadcastMessage($methodName: string, $options: UnityEngine.SendMessageOptions):void;
            public AddComponent($componentType: System.Type):UnityEngine.Component;
            public SetActive($value: boolean):void;
            public CompareTag($tag: string):boolean;
            public static FindGameObjectWithTag($tag: string):UnityEngine.GameObject;
            public static FindGameObjectsWithTag($tag: string):System.Array$1<UnityEngine.GameObject>;
            public SendMessageUpwards($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions):void;
            public SendMessageUpwards($methodName: string, $value: any):void;
            public SendMessageUpwards($methodName: string):void;
            public SendMessage($methodName: string, $value: any, $options: UnityEngine.SendMessageOptions):void;
            public SendMessage($methodName: string, $value: any):void;
            public SendMessage($methodName: string):void;
            public BroadcastMessage($methodName: string, $parameter: any, $options: UnityEngine.SendMessageOptions):void;
            public BroadcastMessage($methodName: string, $parameter: any):void;
            public BroadcastMessage($methodName: string):void;
            public static Find($name: string):UnityEngine.GameObject;
            
        }
        enum PrimitiveType { Sphere = 0, Capsule = 1, Cylinder = 2, Cube = 3, Plane = 4, Quad = 5 }
        enum SendMessageOptions { RequireReceiver = 0, DontRequireReceiver = 1 }
        class Behaviour extends UnityEngine.Component {
            public enabled: boolean;
            public isActiveAndEnabled: boolean;
            public constructor();
            
        }
        class Matrix4x4 extends System.ValueType {
            public m00: number;
            public m10: number;
            public m20: number;
            public m30: number;
            public m01: number;
            public m11: number;
            public m21: number;
            public m31: number;
            public m02: number;
            public m12: number;
            public m22: number;
            public m32: number;
            public m03: number;
            public m13: number;
            public m23: number;
            public m33: number;
            public rotation: UnityEngine.Quaternion;
            public lossyScale: UnityEngine.Vector3;
            public isIdentity: boolean;
            public determinant: number;
            public decomposeProjection: UnityEngine.FrustumPlanes;
            public inverse: UnityEngine.Matrix4x4;
            public transpose: UnityEngine.Matrix4x4;
            public static zero: UnityEngine.Matrix4x4;
            public static identity: UnityEngine.Matrix4x4;
            public constructor($column0: UnityEngine.Vector4, $column1: UnityEngine.Vector4, $column2: UnityEngine.Vector4, $column3: UnityEngine.Vector4);
            public ValidTRS():boolean;
            public static Determinant($m: UnityEngine.Matrix4x4):number;
            public static TRS($pos: UnityEngine.Vector3, $q: UnityEngine.Quaternion, $s: UnityEngine.Vector3):UnityEngine.Matrix4x4;
            public SetTRS($pos: UnityEngine.Vector3, $q: UnityEngine.Quaternion, $s: UnityEngine.Vector3):void;
            public static Inverse($m: UnityEngine.Matrix4x4):UnityEngine.Matrix4x4;
            public static Transpose($m: UnityEngine.Matrix4x4):UnityEngine.Matrix4x4;
            public static Ortho($left: number, $right: number, $bottom: number, $top: number, $zNear: number, $zFar: number):UnityEngine.Matrix4x4;
            public static Perspective($fov: number, $aspect: number, $zNear: number, $zFar: number):UnityEngine.Matrix4x4;
            public static LookAt($from: UnityEngine.Vector3, $to: UnityEngine.Vector3, $up: UnityEngine.Vector3):UnityEngine.Matrix4x4;
            public static Frustum($left: number, $right: number, $bottom: number, $top: number, $zNear: number, $zFar: number):UnityEngine.Matrix4x4;
            public static Frustum($fp: UnityEngine.FrustumPlanes):UnityEngine.Matrix4x4;
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Matrix4x4):boolean;
            public static op_Multiply($lhs: UnityEngine.Matrix4x4, $rhs: UnityEngine.Matrix4x4):UnityEngine.Matrix4x4;
            public static op_Multiply($lhs: UnityEngine.Matrix4x4, $vector: UnityEngine.Vector4):UnityEngine.Vector4;
            public static op_Equality($lhs: UnityEngine.Matrix4x4, $rhs: UnityEngine.Matrix4x4):boolean;
            public static op_Inequality($lhs: UnityEngine.Matrix4x4, $rhs: UnityEngine.Matrix4x4):boolean;
            public GetColumn($index: number):UnityEngine.Vector4;
            public GetRow($index: number):UnityEngine.Vector4;
            public SetColumn($index: number, $column: UnityEngine.Vector4):void;
            public SetRow($index: number, $row: UnityEngine.Vector4):void;
            public MultiplyPoint($point: UnityEngine.Vector3):UnityEngine.Vector3;
            public MultiplyPoint3x4($point: UnityEngine.Vector3):UnityEngine.Vector3;
            public MultiplyVector($vector: UnityEngine.Vector3):UnityEngine.Vector3;
            public TransformPlane($plane: UnityEngine.Plane):UnityEngine.Plane;
            public static Scale($vector: UnityEngine.Vector3):UnityEngine.Matrix4x4;
            public static Translate($vector: UnityEngine.Vector3):UnityEngine.Matrix4x4;
            public static Rotate($q: UnityEngine.Quaternion):UnityEngine.Matrix4x4;
            public ToString():string;
            public ToString($format: string):string;
            
        }
        enum Space { World = 0, Self = 1 }
        class Animator extends UnityEngine.Behaviour {
            public isOptimizable: boolean;
            public isHuman: boolean;
            public hasRootMotion: boolean;
            public humanScale: number;
            public isInitialized: boolean;
            public deltaPosition: UnityEngine.Vector3;
            public deltaRotation: UnityEngine.Quaternion;
            public velocity: UnityEngine.Vector3;
            public angularVelocity: UnityEngine.Vector3;
            public rootPosition: UnityEngine.Vector3;
            public rootRotation: UnityEngine.Quaternion;
            public applyRootMotion: boolean;
            public updateMode: UnityEngine.AnimatorUpdateMode;
            public hasTransformHierarchy: boolean;
            public gravityWeight: number;
            public bodyPosition: UnityEngine.Vector3;
            public bodyRotation: UnityEngine.Quaternion;
            public stabilizeFeet: boolean;
            public layerCount: number;
            public parameters: System.Array$1<UnityEngine.AnimatorControllerParameter>;
            public parameterCount: number;
            public feetPivotActive: number;
            public pivotWeight: number;
            public pivotPosition: UnityEngine.Vector3;
            public isMatchingTarget: boolean;
            public speed: number;
            public targetPosition: UnityEngine.Vector3;
            public targetRotation: UnityEngine.Quaternion;
            public cullingMode: UnityEngine.AnimatorCullingMode;
            public playbackTime: number;
            public recorderStartTime: number;
            public recorderStopTime: number;
            public recorderMode: UnityEngine.AnimatorRecorderMode;
            public runtimeAnimatorController: UnityEngine.RuntimeAnimatorController;
            public hasBoundPlayables: boolean;
            public avatar: UnityEngine.Avatar;
            public playableGraph: UnityEngine.Playables.PlayableGraph;
            public layersAffectMassCenter: boolean;
            public leftFeetBottomHeight: number;
            public rightFeetBottomHeight: number;
            public logWarnings: boolean;
            public fireEvents: boolean;
            public keepAnimatorControllerStateOnDisable: boolean;
            public constructor();
            public GetFloat($name: string):number;
            public GetFloat($id: number):number;
            public SetFloat($name: string, $value: number):void;
            public SetFloat($name: string, $value: number, $dampTime: number, $deltaTime: number):void;
            public SetFloat($id: number, $value: number):void;
            public SetFloat($id: number, $value: number, $dampTime: number, $deltaTime: number):void;
            public GetBool($name: string):boolean;
            public GetBool($id: number):boolean;
            public SetBool($name: string, $value: boolean):void;
            public SetBool($id: number, $value: boolean):void;
            public GetInteger($name: string):number;
            public GetInteger($id: number):number;
            public SetInteger($name: string, $value: number):void;
            public SetInteger($id: number, $value: number):void;
            public SetTrigger($name: string):void;
            public SetTrigger($id: number):void;
            public ResetTrigger($name: string):void;
            public ResetTrigger($id: number):void;
            public IsParameterControlledByCurve($name: string):boolean;
            public IsParameterControlledByCurve($id: number):boolean;
            public GetIKPosition($goal: UnityEngine.AvatarIKGoal):UnityEngine.Vector3;
            public SetIKPosition($goal: UnityEngine.AvatarIKGoal, $goalPosition: UnityEngine.Vector3):void;
            public GetIKRotation($goal: UnityEngine.AvatarIKGoal):UnityEngine.Quaternion;
            public SetIKRotation($goal: UnityEngine.AvatarIKGoal, $goalRotation: UnityEngine.Quaternion):void;
            public GetIKPositionWeight($goal: UnityEngine.AvatarIKGoal):number;
            public SetIKPositionWeight($goal: UnityEngine.AvatarIKGoal, $value: number):void;
            public GetIKRotationWeight($goal: UnityEngine.AvatarIKGoal):number;
            public SetIKRotationWeight($goal: UnityEngine.AvatarIKGoal, $value: number):void;
            public GetIKHintPosition($hint: UnityEngine.AvatarIKHint):UnityEngine.Vector3;
            public SetIKHintPosition($hint: UnityEngine.AvatarIKHint, $hintPosition: UnityEngine.Vector3):void;
            public GetIKHintPositionWeight($hint: UnityEngine.AvatarIKHint):number;
            public SetIKHintPositionWeight($hint: UnityEngine.AvatarIKHint, $value: number):void;
            public SetLookAtPosition($lookAtPosition: UnityEngine.Vector3):void;
            public SetLookAtWeight($weight: number):void;
            public SetLookAtWeight($weight: number, $bodyWeight: number):void;
            public SetLookAtWeight($weight: number, $bodyWeight: number, $headWeight: number):void;
            public SetLookAtWeight($weight: number, $bodyWeight: number, $headWeight: number, $eyesWeight: number):void;
            public SetLookAtWeight($weight: number, $bodyWeight: number, $headWeight: number, $eyesWeight: number, $clampWeight: number):void;
            public SetBoneLocalRotation($humanBoneId: UnityEngine.HumanBodyBones, $rotation: UnityEngine.Quaternion):void;
            public GetBehaviours($fullPathHash: number, $layerIndex: number):System.Array$1<UnityEngine.StateMachineBehaviour>;
            public GetLayerName($layerIndex: number):string;
            public GetLayerIndex($layerName: string):number;
            public GetLayerWeight($layerIndex: number):number;
            public SetLayerWeight($layerIndex: number, $weight: number):void;
            public GetCurrentAnimatorStateInfo($layerIndex: number):UnityEngine.AnimatorStateInfo;
            public GetNextAnimatorStateInfo($layerIndex: number):UnityEngine.AnimatorStateInfo;
            public GetAnimatorTransitionInfo($layerIndex: number):UnityEngine.AnimatorTransitionInfo;
            public GetCurrentAnimatorClipInfoCount($layerIndex: number):number;
            public GetNextAnimatorClipInfoCount($layerIndex: number):number;
            public GetCurrentAnimatorClipInfo($layerIndex: number):System.Array$1<UnityEngine.AnimatorClipInfo>;
            public GetNextAnimatorClipInfo($layerIndex: number):System.Array$1<UnityEngine.AnimatorClipInfo>;
            public GetCurrentAnimatorClipInfo($layerIndex: number, $clips: System.Collections.Generic.List$1<UnityEngine.AnimatorClipInfo>):void;
            public GetNextAnimatorClipInfo($layerIndex: number, $clips: System.Collections.Generic.List$1<UnityEngine.AnimatorClipInfo>):void;
            public IsInTransition($layerIndex: number):boolean;
            public GetParameter($index: number):UnityEngine.AnimatorControllerParameter;
            public MatchTarget($matchPosition: UnityEngine.Vector3, $matchRotation: UnityEngine.Quaternion, $targetBodyPart: UnityEngine.AvatarTarget, $weightMask: UnityEngine.MatchTargetWeightMask, $startNormalizedTime: number):void;
            public MatchTarget($matchPosition: UnityEngine.Vector3, $matchRotation: UnityEngine.Quaternion, $targetBodyPart: UnityEngine.AvatarTarget, $weightMask: UnityEngine.MatchTargetWeightMask, $startNormalizedTime: number, $targetNormalizedTime: number):void;
            public InterruptMatchTarget():void;
            public InterruptMatchTarget($completeMatch: boolean):void;
            public CrossFadeInFixedTime($stateName: string, $fixedTransitionDuration: number):void;
            public CrossFadeInFixedTime($stateName: string, $fixedTransitionDuration: number, $layer: number):void;
            public CrossFadeInFixedTime($stateName: string, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number):void;
            public CrossFadeInFixedTime($stateName: string, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number, $normalizedTransitionTime: number):void;
            public CrossFadeInFixedTime($stateHashName: number, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number):void;
            public CrossFadeInFixedTime($stateHashName: number, $fixedTransitionDuration: number, $layer: number):void;
            public CrossFadeInFixedTime($stateHashName: number, $fixedTransitionDuration: number):void;
            public CrossFadeInFixedTime($stateHashName: number, $fixedTransitionDuration: number, $layer: number, $fixedTimeOffset: number, $normalizedTransitionTime: number):void;
            public WriteDefaultValues():void;
            public CrossFade($stateName: string, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number):void;
            public CrossFade($stateName: string, $normalizedTransitionDuration: number, $layer: number):void;
            public CrossFade($stateName: string, $normalizedTransitionDuration: number):void;
            public CrossFade($stateName: string, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number, $normalizedTransitionTime: number):void;
            public CrossFade($stateHashName: number, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number, $normalizedTransitionTime: number):void;
            public CrossFade($stateHashName: number, $normalizedTransitionDuration: number, $layer: number, $normalizedTimeOffset: number):void;
            public CrossFade($stateHashName: number, $normalizedTransitionDuration: number, $layer: number):void;
            public CrossFade($stateHashName: number, $normalizedTransitionDuration: number):void;
            public PlayInFixedTime($stateName: string, $layer: number):void;
            public PlayInFixedTime($stateName: string):void;
            public PlayInFixedTime($stateName: string, $layer: number, $fixedTime: number):void;
            public PlayInFixedTime($stateNameHash: number, $layer: number, $fixedTime: number):void;
            public PlayInFixedTime($stateNameHash: number, $layer: number):void;
            public PlayInFixedTime($stateNameHash: number):void;
            public Play($stateName: string, $layer: number):void;
            public Play($stateName: string):void;
            public Play($stateName: string, $layer: number, $normalizedTime: number):void;
            public Play($stateNameHash: number, $layer: number, $normalizedTime: number):void;
            public Play($stateNameHash: number, $layer: number):void;
            public Play($stateNameHash: number):void;
            public SetTarget($targetIndex: UnityEngine.AvatarTarget, $targetNormalizedTime: number):void;
            public GetBoneTransform($humanBoneId: UnityEngine.HumanBodyBones):UnityEngine.Transform;
            public StartPlayback():void;
            public StopPlayback():void;
            public StartRecording($frameCount: number):void;
            public StopRecording():void;
            public HasState($layerIndex: number, $stateID: number):boolean;
            public static StringToHash($name: string):number;
            public Update($deltaTime: number):void;
            public Rebind():void;
            public ApplyBuiltinRootMotion():void;
            
        }
        class AnimationInfo extends System.ValueType {
            
        }
        enum AnimatorUpdateMode { Normal = 0, AnimatePhysics = 1, UnscaledTime = 2 }
        enum AvatarIKGoal { LeftFoot = 0, RightFoot = 1, LeftHand = 2, RightHand = 3 }
        enum AvatarIKHint { LeftKnee = 0, RightKnee = 1, LeftElbow = 2, RightElbow = 3 }
        enum HumanBodyBones { Hips = 0, LeftUpperLeg = 1, RightUpperLeg = 2, LeftLowerLeg = 3, RightLowerLeg = 4, LeftFoot = 5, RightFoot = 6, Spine = 7, Chest = 8, UpperChest = 54, Neck = 9, Head = 10, LeftShoulder = 11, RightShoulder = 12, LeftUpperArm = 13, RightUpperArm = 14, LeftLowerArm = 15, RightLowerArm = 16, LeftHand = 17, RightHand = 18, LeftToes = 19, RightToes = 20, LeftEye = 21, RightEye = 22, Jaw = 23, LeftThumbProximal = 24, LeftThumbIntermediate = 25, LeftThumbDistal = 26, LeftIndexProximal = 27, LeftIndexIntermediate = 28, LeftIndexDistal = 29, LeftMiddleProximal = 30, LeftMiddleIntermediate = 31, LeftMiddleDistal = 32, LeftRingProximal = 33, LeftRingIntermediate = 34, LeftRingDistal = 35, LeftLittleProximal = 36, LeftLittleIntermediate = 37, LeftLittleDistal = 38, RightThumbProximal = 39, RightThumbIntermediate = 40, RightThumbDistal = 41, RightIndexProximal = 42, RightIndexIntermediate = 43, RightIndexDistal = 44, RightMiddleProximal = 45, RightMiddleIntermediate = 46, RightMiddleDistal = 47, RightRingProximal = 48, RightRingIntermediate = 49, RightRingDistal = 50, RightLittleProximal = 51, RightLittleIntermediate = 52, RightLittleDistal = 53, LastBone = 55 }
        class StateMachineBehaviour extends UnityEngine.ScriptableObject {
            
        }
        class ScriptableObject extends UnityEngine.Object {
            public constructor();
            public static CreateInstance($className: string):UnityEngine.ScriptableObject;
            public static CreateInstance($type: System.Type):UnityEngine.ScriptableObject;
            
        }
        class AnimatorStateInfo extends System.ValueType {
            
        }
        class AnimatorTransitionInfo extends System.ValueType {
            
        }
        class AnimatorClipInfo extends System.ValueType {
            
        }
        class AnimatorControllerParameter extends System.Object {
            
        }
        enum AvatarTarget { Root = 0, Body = 1, LeftFoot = 2, RightFoot = 3, LeftHand = 4, RightHand = 5 }
        class MatchTargetWeightMask extends System.ValueType {
            
        }
        enum AnimatorCullingMode { AlwaysAnimate = 0, CullUpdateTransforms = 1, CullCompletely = 2, BasedOnRenderers = 1 }
        enum AnimatorRecorderMode { Offline = 0, Playback = 1, Record = 2 }
        class RuntimeAnimatorController extends UnityEngine.Object {
            
        }
        class Avatar extends UnityEngine.Object {
            
        }
        class AssetBundle extends UnityEngine.Object {
            public isStreamedSceneAssetBundle: boolean;
            public static UnloadAllAssetBundles($unloadAllObjects: boolean):void;
            public static GetAllLoadedAssetBundles():System.Collections.Generic.IEnumerable$1<UnityEngine.AssetBundle>;
            public static LoadFromFileAsync($path: string):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromFileAsync($path: string, $crc: number):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromFileAsync($path: string, $crc: number, $offset: bigint):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromFile($path: string):UnityEngine.AssetBundle;
            public static LoadFromFile($path: string, $crc: number):UnityEngine.AssetBundle;
            public static LoadFromFile($path: string, $crc: number, $offset: bigint):UnityEngine.AssetBundle;
            public static LoadFromMemoryAsync($binary: System.Array$1<number>):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromMemoryAsync($binary: System.Array$1<number>, $crc: number):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromMemory($binary: System.Array$1<number>):UnityEngine.AssetBundle;
            public static LoadFromMemory($binary: System.Array$1<number>, $crc: number):UnityEngine.AssetBundle;
            public static LoadFromStreamAsync($stream: System.IO.Stream, $crc: number, $managedReadBufferSize: number):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromStreamAsync($stream: System.IO.Stream, $crc: number):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromStreamAsync($stream: System.IO.Stream):UnityEngine.AssetBundleCreateRequest;
            public static LoadFromStream($stream: System.IO.Stream, $crc: number, $managedReadBufferSize: number):UnityEngine.AssetBundle;
            public static LoadFromStream($stream: System.IO.Stream, $crc: number):UnityEngine.AssetBundle;
            public static LoadFromStream($stream: System.IO.Stream):UnityEngine.AssetBundle;
            public static SetAssetBundleDecryptKey($password: string):void;
            public Contains($name: string):boolean;
            public LoadAsset($name: string):UnityEngine.Object;
            public LoadAsset($name: string, $type: System.Type):UnityEngine.Object;
            public LoadAssetAsync($name: string):UnityEngine.AssetBundleRequest;
            public LoadAssetAsync($name: string, $type: System.Type):UnityEngine.AssetBundleRequest;
            public LoadAssetWithSubAssets($name: string):System.Array$1<UnityEngine.Object>;
            public LoadAssetWithSubAssets($name: string, $type: System.Type):System.Array$1<UnityEngine.Object>;
            public LoadAssetWithSubAssetsAsync($name: string):UnityEngine.AssetBundleRequest;
            public LoadAssetWithSubAssetsAsync($name: string, $type: System.Type):UnityEngine.AssetBundleRequest;
            public LoadAllAssets():System.Array$1<UnityEngine.Object>;
            public LoadAllAssets($type: System.Type):System.Array$1<UnityEngine.Object>;
            public LoadAllAssetsAsync():UnityEngine.AssetBundleRequest;
            public LoadAllAssetsAsync($type: System.Type):UnityEngine.AssetBundleRequest;
            public Unload($unloadAllLoadedObjects: boolean):void;
            public GetAllAssetNames():System.Array$1<string>;
            public GetAllScenePaths():System.Array$1<string>;
            public static RecompressAssetBundleAsync($inputPath: string, $outputPath: string, $method: UnityEngine.BuildCompression, $expectedCRC?: number, $priority?: UnityEngine.ThreadPriority):UnityEngine.AssetBundleRecompressOperation;
            
        }
        class AssetBundleCreateRequest extends UnityEngine.AsyncOperation {
            public assetBundle: UnityEngine.AssetBundle;
            public constructor();
            
        }
        class AsyncOperation extends UnityEngine.YieldInstruction {
            public isDone: boolean;
            public progress: number;
            public priority: number;
            public allowSceneActivation: boolean;
            public constructor();
            public add_completed($value: System.Action$1<UnityEngine.AsyncOperation>):void;
            public remove_completed($value: System.Action$1<UnityEngine.AsyncOperation>):void;
            
        }
        class YieldInstruction extends System.Object {
            
        }
        class AssetBundleRequest extends UnityEngine.AsyncOperation {
            public asset: UnityEngine.Object;
            public allAssets: System.Array$1<UnityEngine.Object>;
            public constructor();
            
        }
        class AssetBundleRecompressOperation extends UnityEngine.AsyncOperation {
            
        }
        class BuildCompression extends System.ValueType {
            
        }
        enum ThreadPriority { Low = 0, BelowNormal = 1, Normal = 2, High = 4 }
        class AudioClip extends UnityEngine.Object {
            public length: number;
            public samples: number;
            public channels: number;
            public frequency: number;
            public loadType: UnityEngine.AudioClipLoadType;
            public preloadAudioData: boolean;
            public ambisonic: boolean;
            public loadState: UnityEngine.AudioDataLoadState;
            public loadInBackground: boolean;
            public LoadAudioData():boolean;
            public UnloadAudioData():boolean;
            public GetData($data: System.Array$1<number>, $offsetSamples: number):boolean;
            public SetData($data: System.Array$1<number>, $offsetSamples: number):boolean;
            public static Create($name: string, $lengthSamples: number, $channels: number, $frequency: number, $stream: boolean):UnityEngine.AudioClip;
            public static Create($name: string, $lengthSamples: number, $channels: number, $frequency: number, $stream: boolean, $pcmreadercallback: UnityEngine.AudioClip.PCMReaderCallback):UnityEngine.AudioClip;
            public static Create($name: string, $lengthSamples: number, $channels: number, $frequency: number, $stream: boolean, $pcmreadercallback: UnityEngine.AudioClip.PCMReaderCallback, $pcmsetpositioncallback: UnityEngine.AudioClip.PCMSetPositionCallback):UnityEngine.AudioClip;
            
        }
        enum AudioClipLoadType { DecompressOnLoad = 0, CompressedInMemory = 1, Streaming = 2 }
        enum AudioDataLoadState { Unloaded = 0, Loading = 1, Loaded = 2, Failed = 3 }
        class AudioListener extends UnityEngine.AudioBehaviour {
            public static volume: number;
            public static pause: boolean;
            public velocityUpdateMode: UnityEngine.AudioVelocityUpdateMode;
            public constructor();
            public static GetOutputData($samples: System.Array$1<number>, $channel: number):void;
            public static GetSpectrumData($samples: System.Array$1<number>, $channel: number, $window: UnityEngine.FFTWindow):void;
            
        }
        class AudioBehaviour extends UnityEngine.Behaviour {
            
        }
        enum AudioVelocityUpdateMode { Auto = 0, Fixed = 1, Dynamic = 2 }
        enum FFTWindow { Rectangular = 0, Triangle = 1, Hamming = 2, Hanning = 3, Blackman = 4, BlackmanHarris = 5 }
        class AudioSource extends UnityEngine.AudioBehaviour {
            public volume: number;
            public pitch: number;
            public time: number;
            public timeSamples: number;
            public clip: UnityEngine.AudioClip;
            public outputAudioMixerGroup: UnityEngine.Audio.AudioMixerGroup;
            public isPlaying: boolean;
            public isVirtual: boolean;
            public loop: boolean;
            public ignoreListenerVolume: boolean;
            public playOnAwake: boolean;
            public ignoreListenerPause: boolean;
            public velocityUpdateMode: UnityEngine.AudioVelocityUpdateMode;
            public panStereo: number;
            public spatialBlend: number;
            public spatialize: boolean;
            public spatializePostEffects: boolean;
            public reverbZoneMix: number;
            public bypassEffects: boolean;
            public bypassListenerEffects: boolean;
            public bypassReverbZones: boolean;
            public dopplerLevel: number;
            public spread: number;
            public priority: number;
            public mute: boolean;
            public minDistance: number;
            public maxDistance: number;
            public rolloffMode: UnityEngine.AudioRolloffMode;
            public constructor();
            public Play($delay: bigint):void;
            public Play():void;
            public PlayDelayed($delay: number):void;
            public PlayScheduled($time: number):void;
            public SetScheduledStartTime($time: number):void;
            public SetScheduledEndTime($time: number):void;
            public Stop():void;
            public Pause():void;
            public UnPause():void;
            public PlayOneShot($clip: UnityEngine.AudioClip):void;
            public PlayOneShot($clip: UnityEngine.AudioClip, $volumeScale: number):void;
            public static PlayClipAtPoint($clip: UnityEngine.AudioClip, $position: UnityEngine.Vector3):void;
            public static PlayClipAtPoint($clip: UnityEngine.AudioClip, $position: UnityEngine.Vector3, $volume: number):void;
            public SetCustomCurve($type: UnityEngine.AudioSourceCurveType, $curve: UnityEngine.AnimationCurve):void;
            public GetCustomCurve($type: UnityEngine.AudioSourceCurveType):UnityEngine.AnimationCurve;
            public GetOutputData($samples: System.Array$1<number>, $channel: number):void;
            public GetSpectrumData($samples: System.Array$1<number>, $channel: number, $window: UnityEngine.FFTWindow):void;
            public SetSpatializerFloat($index: number, $value: number):boolean;
            public GetSpatializerFloat($index: number, $value: $Ref<number>):boolean;
            public SetAmbisonicDecoderFloat($index: number, $value: number):boolean;
            public GetAmbisonicDecoderFloat($index: number, $value: $Ref<number>):boolean;
            
        }
        enum AudioSourceCurveType { CustomRolloff = 0, SpatialBlend = 1, ReverbZoneMix = 2, Spread = 3 }
        class AnimationCurve extends System.Object {
            
        }
        enum AudioRolloffMode { Logarithmic = 0, Linear = 1, Custom = 2 }
        class Texture extends UnityEngine.Object {
            public static masterTextureLimit: number;
            public static anisotropicFiltering: UnityEngine.AnisotropicFiltering;
            public width: number;
            public height: number;
            public dimension: UnityEngine.Rendering.TextureDimension;
            public isReadable: boolean;
            public wrapMode: UnityEngine.TextureWrapMode;
            public wrapModeU: UnityEngine.TextureWrapMode;
            public wrapModeV: UnityEngine.TextureWrapMode;
            public wrapModeW: UnityEngine.TextureWrapMode;
            public filterMode: UnityEngine.FilterMode;
            public anisoLevel: number;
            public mipMapBias: number;
            public texelSize: UnityEngine.Vector2;
            public updateCount: number;
            public imageContentsHash: UnityEngine.Hash128;
            public static totalTextureMemory: bigint;
            public static desiredTextureMemory: bigint;
            public static targetTextureMemory: bigint;
            public static currentTextureMemory: bigint;
            public static nonStreamingTextureMemory: bigint;
            public static streamingMipmapUploadCount: bigint;
            public static streamingRendererCount: bigint;
            public static streamingTextureCount: bigint;
            public static nonStreamingTextureCount: bigint;
            public static streamingTexturePendingLoadCount: bigint;
            public static streamingTextureLoadingCount: bigint;
            public static streamingTextureForceLoadAll: boolean;
            public static streamingTextureDiscardUnusedMips: boolean;
            public static SetGlobalAnisotropicFilteringLimits($forcedMin: number, $globalMax: number):void;
            public GetNativeTexturePtr():System.IntPtr;
            public IncrementUpdateCount():void;
            public static SetStreamingTextureMaterialDebugProperties():void;
            
        }
        enum AnisotropicFiltering { Disable = 0, Enable = 1, ForceEnable = 2 }
        enum TextureWrapMode { Repeat = 0, Clamp = 1, Mirror = 2, MirrorOnce = 3 }
        enum FilterMode { Point = 0, Bilinear = 1, Trilinear = 2 }
        class Vector2 extends System.ValueType {
            public x: number;
            public y: number;
            public static kEpsilon: number;
            public static kEpsilonNormalSqrt: number;
            public normalized: UnityEngine.Vector2;
            public magnitude: number;
            public sqrMagnitude: number;
            public static zero: UnityEngine.Vector2;
            public static one: UnityEngine.Vector2;
            public static up: UnityEngine.Vector2;
            public static down: UnityEngine.Vector2;
            public static left: UnityEngine.Vector2;
            public static right: UnityEngine.Vector2;
            public static positiveInfinity: UnityEngine.Vector2;
            public static negativeInfinity: UnityEngine.Vector2;
            public constructor($x: number, $y: number);
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public Set($newX: number, $newY: number):void;
            public static Lerp($a: UnityEngine.Vector2, $b: UnityEngine.Vector2, $t: number):UnityEngine.Vector2;
            public static LerpUnclamped($a: UnityEngine.Vector2, $b: UnityEngine.Vector2, $t: number):UnityEngine.Vector2;
            public static MoveTowards($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $maxDistanceDelta: number):UnityEngine.Vector2;
            public static Scale($a: UnityEngine.Vector2, $b: UnityEngine.Vector2):UnityEngine.Vector2;
            public Scale($scale: UnityEngine.Vector2):void;
            public Normalize():void;
            public ToString():string;
            public ToString($format: string):string;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Vector2):boolean;
            public static Reflect($inDirection: UnityEngine.Vector2, $inNormal: UnityEngine.Vector2):UnityEngine.Vector2;
            public static Perpendicular($inDirection: UnityEngine.Vector2):UnityEngine.Vector2;
            public static Dot($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2):number;
            public static Angle($from: UnityEngine.Vector2, $to: UnityEngine.Vector2):number;
            public static SignedAngle($from: UnityEngine.Vector2, $to: UnityEngine.Vector2):number;
            public static Distance($a: UnityEngine.Vector2, $b: UnityEngine.Vector2):number;
            public static ClampMagnitude($vector: UnityEngine.Vector2, $maxLength: number):UnityEngine.Vector2;
            public static SqrMagnitude($a: UnityEngine.Vector2):number;
            public SqrMagnitude():number;
            public static Min($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2):UnityEngine.Vector2;
            public static Max($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2):UnityEngine.Vector2;
            public static SmoothDamp($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $currentVelocity: $Ref<UnityEngine.Vector2>, $smoothTime: number, $maxSpeed: number):UnityEngine.Vector2;
            public static SmoothDamp($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $currentVelocity: $Ref<UnityEngine.Vector2>, $smoothTime: number):UnityEngine.Vector2;
            public static SmoothDamp($current: UnityEngine.Vector2, $target: UnityEngine.Vector2, $currentVelocity: $Ref<UnityEngine.Vector2>, $smoothTime: number, $maxSpeed: number, $deltaTime: number):UnityEngine.Vector2;
            public static op_Addition($a: UnityEngine.Vector2, $b: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_Subtraction($a: UnityEngine.Vector2, $b: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_Multiply($a: UnityEngine.Vector2, $b: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_Division($a: UnityEngine.Vector2, $b: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_UnaryNegation($a: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_Multiply($a: UnityEngine.Vector2, $d: number):UnityEngine.Vector2;
            public static op_Multiply($d: number, $a: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_Division($a: UnityEngine.Vector2, $d: number):UnityEngine.Vector2;
            public static op_Equality($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2):boolean;
            public static op_Inequality($lhs: UnityEngine.Vector2, $rhs: UnityEngine.Vector2):boolean;
            public static op_Implicit($v: UnityEngine.Vector3):UnityEngine.Vector2;
            public static op_Implicit($v: UnityEngine.Vector2):UnityEngine.Vector3;
            
        }
        class Hash128 extends System.ValueType {
            
        }
        class Color extends System.ValueType {
            public r: number;
            public g: number;
            public b: number;
            public a: number;
            public static red: UnityEngine.Color;
            public static green: UnityEngine.Color;
            public static blue: UnityEngine.Color;
            public static white: UnityEngine.Color;
            public static black: UnityEngine.Color;
            public static yellow: UnityEngine.Color;
            public static cyan: UnityEngine.Color;
            public static magenta: UnityEngine.Color;
            public static gray: UnityEngine.Color;
            public static grey: UnityEngine.Color;
            public static clear: UnityEngine.Color;
            public grayscale: number;
            public linear: UnityEngine.Color;
            public gamma: UnityEngine.Color;
            public maxColorComponent: number;
            public constructor($r: number, $g: number, $b: number, $a: number);
            public constructor($r: number, $g: number, $b: number);
            public ToString():string;
            public ToString($format: string):string;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Color):boolean;
            public static op_Addition($a: UnityEngine.Color, $b: UnityEngine.Color):UnityEngine.Color;
            public static op_Subtraction($a: UnityEngine.Color, $b: UnityEngine.Color):UnityEngine.Color;
            public static op_Multiply($a: UnityEngine.Color, $b: UnityEngine.Color):UnityEngine.Color;
            public static op_Multiply($a: UnityEngine.Color, $b: number):UnityEngine.Color;
            public static op_Multiply($b: number, $a: UnityEngine.Color):UnityEngine.Color;
            public static op_Division($a: UnityEngine.Color, $b: number):UnityEngine.Color;
            public static op_Equality($lhs: UnityEngine.Color, $rhs: UnityEngine.Color):boolean;
            public static op_Inequality($lhs: UnityEngine.Color, $rhs: UnityEngine.Color):boolean;
            public static Lerp($a: UnityEngine.Color, $b: UnityEngine.Color, $t: number):UnityEngine.Color;
            public static LerpUnclamped($a: UnityEngine.Color, $b: UnityEngine.Color, $t: number):UnityEngine.Color;
            public static op_Implicit($c: UnityEngine.Color):UnityEngine.Vector4;
            public static op_Implicit($v: UnityEngine.Vector4):UnityEngine.Color;
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public static RGBToHSV($rgbColor: UnityEngine.Color, $H: $Ref<number>, $S: $Ref<number>, $V: $Ref<number>):void;
            public static HSVToRGB($H: number, $S: number, $V: number):UnityEngine.Color;
            public static HSVToRGB($H: number, $S: number, $V: number, $hdr: boolean):UnityEngine.Color;
            
        }
        class Vector4 extends System.ValueType {
            public static kEpsilon: number;
            public x: number;
            public y: number;
            public z: number;
            public w: number;
            public normalized: UnityEngine.Vector4;
            public magnitude: number;
            public sqrMagnitude: number;
            public static zero: UnityEngine.Vector4;
            public static one: UnityEngine.Vector4;
            public static positiveInfinity: UnityEngine.Vector4;
            public static negativeInfinity: UnityEngine.Vector4;
            public constructor($x: number, $y: number, $z: number, $w: number);
            public constructor($x: number, $y: number, $z: number);
            public constructor($x: number, $y: number);
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public Set($newX: number, $newY: number, $newZ: number, $newW: number):void;
            public static Lerp($a: UnityEngine.Vector4, $b: UnityEngine.Vector4, $t: number):UnityEngine.Vector4;
            public static LerpUnclamped($a: UnityEngine.Vector4, $b: UnityEngine.Vector4, $t: number):UnityEngine.Vector4;
            public static MoveTowards($current: UnityEngine.Vector4, $target: UnityEngine.Vector4, $maxDistanceDelta: number):UnityEngine.Vector4;
            public static Scale($a: UnityEngine.Vector4, $b: UnityEngine.Vector4):UnityEngine.Vector4;
            public Scale($scale: UnityEngine.Vector4):void;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Vector4):boolean;
            public static Normalize($a: UnityEngine.Vector4):UnityEngine.Vector4;
            public Normalize():void;
            public static Dot($a: UnityEngine.Vector4, $b: UnityEngine.Vector4):number;
            public static Project($a: UnityEngine.Vector4, $b: UnityEngine.Vector4):UnityEngine.Vector4;
            public static Distance($a: UnityEngine.Vector4, $b: UnityEngine.Vector4):number;
            public static Magnitude($a: UnityEngine.Vector4):number;
            public static Min($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4):UnityEngine.Vector4;
            public static Max($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4):UnityEngine.Vector4;
            public static op_Addition($a: UnityEngine.Vector4, $b: UnityEngine.Vector4):UnityEngine.Vector4;
            public static op_Subtraction($a: UnityEngine.Vector4, $b: UnityEngine.Vector4):UnityEngine.Vector4;
            public static op_UnaryNegation($a: UnityEngine.Vector4):UnityEngine.Vector4;
            public static op_Multiply($a: UnityEngine.Vector4, $d: number):UnityEngine.Vector4;
            public static op_Multiply($d: number, $a: UnityEngine.Vector4):UnityEngine.Vector4;
            public static op_Division($a: UnityEngine.Vector4, $d: number):UnityEngine.Vector4;
            public static op_Equality($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4):boolean;
            public static op_Inequality($lhs: UnityEngine.Vector4, $rhs: UnityEngine.Vector4):boolean;
            public static op_Implicit($v: UnityEngine.Vector3):UnityEngine.Vector4;
            public static op_Implicit($v: UnityEngine.Vector4):UnityEngine.Vector3;
            public static op_Implicit($v: UnityEngine.Vector2):UnityEngine.Vector4;
            public static op_Implicit($v: UnityEngine.Vector4):UnityEngine.Vector2;
            public ToString():string;
            public ToString($format: string):string;
            public static SqrMagnitude($a: UnityEngine.Vector4):number;
            public SqrMagnitude():number;
            
        }
        class Touch extends System.ValueType {
            public fingerId: number;
            public position: UnityEngine.Vector2;
            public rawPosition: UnityEngine.Vector2;
            public deltaPosition: UnityEngine.Vector2;
            public deltaTime: number;
            public tapCount: number;
            public phase: UnityEngine.TouchPhase;
            public pressure: number;
            public maximumPossiblePressure: number;
            public type: UnityEngine.TouchType;
            public altitudeAngle: number;
            public azimuthAngle: number;
            public radius: number;
            public radiusVariance: number;
            
        }
        enum TouchPhase { Began = 0, Moved = 1, Stationary = 2, Ended = 3, Canceled = 4 }
        enum TouchType { Direct = 0, Indirect = 1, Stylus = 2 }
        class Application extends System.Object {
            public static isPlaying: boolean;
            public static isFocused: boolean;
            public static platform: UnityEngine.RuntimePlatform;
            public static buildGUID: string;
            public static isMobilePlatform: boolean;
            public static isConsolePlatform: boolean;
            public static runInBackground: boolean;
            public static isBatchMode: boolean;
            public static dataPath: string;
            public static streamingAssetsPath: string;
            public static persistentDataPath: string;
            public static temporaryCachePath: string;
            public static absoluteURL: string;
            public static unityVersion: string;
            public static version: string;
            public static installerName: string;
            public static identifier: string;
            public static installMode: UnityEngine.ApplicationInstallMode;
            public static sandboxType: UnityEngine.ApplicationSandboxType;
            public static productName: string;
            public static companyName: string;
            public static cloudProjectId: string;
            public static targetFrameRate: number;
            public static systemLanguage: UnityEngine.SystemLanguage;
            public static consoleLogPath: string;
            public static backgroundLoadingPriority: UnityEngine.ThreadPriority;
            public static internetReachability: UnityEngine.NetworkReachability;
            public static genuine: boolean;
            public static genuineCheckAvailable: boolean;
            public static isEditor: boolean;
            public constructor();
            public static Quit($exitCode: number):void;
            public static Quit():void;
            public static Unload():void;
            public static CanStreamedLevelBeLoaded($levelIndex: number):boolean;
            public static CanStreamedLevelBeLoaded($levelName: string):boolean;
            public static IsPlaying($obj: UnityEngine.Object):boolean;
            public static GetBuildTags():System.Array$1<string>;
            public static SetBuildTags($buildTags: System.Array$1<string>):void;
            public static HasProLicense():boolean;
            public static RequestAdvertisingIdentifierAsync($delegateMethod: UnityEngine.Application.AdvertisingIdentifierCallback):boolean;
            public static OpenURL($url: string):void;
            public static GetStackTraceLogType($logType: UnityEngine.LogType):UnityEngine.StackTraceLogType;
            public static SetStackTraceLogType($logType: UnityEngine.LogType, $stackTraceType: UnityEngine.StackTraceLogType):void;
            public static RequestUserAuthorization($mode: UnityEngine.UserAuthorization):UnityEngine.AsyncOperation;
            public static HasUserAuthorization($mode: UnityEngine.UserAuthorization):boolean;
            public static add_lowMemory($value: UnityEngine.Application.LowMemoryCallback):void;
            public static remove_lowMemory($value: UnityEngine.Application.LowMemoryCallback):void;
            public static add_logMessageReceived($value: UnityEngine.Application.LogCallback):void;
            public static remove_logMessageReceived($value: UnityEngine.Application.LogCallback):void;
            public static add_logMessageReceivedThreaded($value: UnityEngine.Application.LogCallback):void;
            public static remove_logMessageReceivedThreaded($value: UnityEngine.Application.LogCallback):void;
            public static add_onBeforeRender($value: UnityEngine.Events.UnityAction):void;
            public static remove_onBeforeRender($value: UnityEngine.Events.UnityAction):void;
            public static add_focusChanged($value: System.Action$1<boolean>):void;
            public static remove_focusChanged($value: System.Action$1<boolean>):void;
            public static add_wantsToQuit($value: System.Func$1<boolean>):void;
            public static remove_wantsToQuit($value: System.Func$1<boolean>):void;
            public static add_quitting($value: System.Action):void;
            public static remove_quitting($value: System.Action):void;
            
        }
        enum RuntimePlatform { OSXEditor = 0, OSXPlayer = 1, WindowsPlayer = 2, OSXWebPlayer = 3, OSXDashboardPlayer = 4, WindowsWebPlayer = 5, WindowsEditor = 7, IPhonePlayer = 8, XBOX360 = 10, PS3 = 9, Android = 11, NaCl = 12, FlashPlayer = 15, LinuxPlayer = 13, LinuxEditor = 16, WebGLPlayer = 17, MetroPlayerX86 = 18, WSAPlayerX86 = 18, MetroPlayerX64 = 19, WSAPlayerX64 = 19, MetroPlayerARM = 20, WSAPlayerARM = 20, WP8Player = 21, BB10Player = 22, BlackBerryPlayer = 22, TizenPlayer = 23, PSP2 = 24, PS4 = 25, PSM = 26, XboxOne = 27, SamsungTVPlayer = 28, WiiU = 30, tvOS = 31, Switch = 32, Lumin = 33 }
        enum ApplicationInstallMode { Unknown = 0, Store = 1, DeveloperBuild = 2, Adhoc = 3, Enterprise = 4, Editor = 5 }
        enum ApplicationSandboxType { Unknown = 0, NotSandboxed = 1, Sandboxed = 2, SandboxBroken = 3 }
        enum SystemLanguage { Afrikaans = 0, Arabic = 1, Basque = 2, Belarusian = 3, Bulgarian = 4, Catalan = 5, Chinese = 6, Czech = 7, Danish = 8, Dutch = 9, English = 10, Estonian = 11, Faroese = 12, Finnish = 13, French = 14, German = 15, Greek = 16, Hebrew = 17, Hugarian = 18, Icelandic = 19, Indonesian = 20, Italian = 21, Japanese = 22, Korean = 23, Latvian = 24, Lithuanian = 25, Norwegian = 26, Polish = 27, Portuguese = 28, Romanian = 29, Russian = 30, SerboCroatian = 31, Slovak = 32, Slovenian = 33, Spanish = 34, Swedish = 35, Thai = 36, Turkish = 37, Ukrainian = 38, Vietnamese = 39, ChineseSimplified = 40, ChineseTraditional = 41, Unknown = 42, Hungarian = 18 }
        enum StackTraceLogType { None = 0, ScriptOnly = 1, Full = 2 }
        enum LogType { Error = 0, Assert = 1, Warning = 2, Log = 3, Exception = 4 }
        enum NetworkReachability { NotReachable = 0, ReachableViaCarrierDataNetwork = 1, ReachableViaLocalAreaNetwork = 2 }
        enum UserAuthorization { WebCam = 1, Microphone = 2 }
        class Material extends UnityEngine.Object {
            public shader: UnityEngine.Shader;
            public color: UnityEngine.Color;
            public mainTexture: UnityEngine.Texture;
            public mainTextureOffset: UnityEngine.Vector2;
            public mainTextureScale: UnityEngine.Vector2;
            public renderQueue: number;
            public globalIlluminationFlags: UnityEngine.MaterialGlobalIlluminationFlags;
            public doubleSidedGI: boolean;
            public enableInstancing: boolean;
            public passCount: number;
            public shaderKeywords: System.Array$1<string>;
            public constructor($shader: UnityEngine.Shader);
            public constructor($source: UnityEngine.Material);
            public HasProperty($nameID: number):boolean;
            public HasProperty($name: string):boolean;
            public EnableKeyword($keyword: string):void;
            public DisableKeyword($keyword: string):void;
            public IsKeywordEnabled($keyword: string):boolean;
            public SetShaderPassEnabled($passName: string, $enabled: boolean):void;
            public GetShaderPassEnabled($passName: string):boolean;
            public GetPassName($pass: number):string;
            public FindPass($passName: string):number;
            public SetOverrideTag($tag: string, $val: string):void;
            public GetTag($tag: string, $searchFallbacks: boolean, $defaultValue: string):string;
            public GetTag($tag: string, $searchFallbacks: boolean):string;
            public Lerp($start: UnityEngine.Material, $end: UnityEngine.Material, $t: number):void;
            public SetPass($pass: number):boolean;
            public CopyPropertiesFromMaterial($mat: UnityEngine.Material):void;
            public GetTexturePropertyNames():System.Array$1<string>;
            public GetTexturePropertyNameIDs():System.Array$1<number>;
            public GetTexturePropertyNames($outNames: System.Collections.Generic.List$1<string>):void;
            public GetTexturePropertyNameIDs($outNames: System.Collections.Generic.List$1<number>):void;
            public SetFloat($name: string, $value: number):void;
            public SetFloat($nameID: number, $value: number):void;
            public SetInt($name: string, $value: number):void;
            public SetInt($nameID: number, $value: number):void;
            public SetColor($name: string, $value: UnityEngine.Color):void;
            public SetColor($nameID: number, $value: UnityEngine.Color):void;
            public SetVector($name: string, $value: UnityEngine.Vector4):void;
            public SetVector($nameID: number, $value: UnityEngine.Vector4):void;
            public SetMatrix($name: string, $value: UnityEngine.Matrix4x4):void;
            public SetMatrix($nameID: number, $value: UnityEngine.Matrix4x4):void;
            public SetTexture($name: string, $value: UnityEngine.Texture):void;
            public SetTexture($nameID: number, $value: UnityEngine.Texture):void;
            public SetBuffer($name: string, $value: UnityEngine.ComputeBuffer):void;
            public SetBuffer($nameID: number, $value: UnityEngine.ComputeBuffer):void;
            public SetFloatArray($name: string, $values: System.Collections.Generic.List$1<number>):void;
            public SetFloatArray($nameID: number, $values: System.Collections.Generic.List$1<number>):void;
            public SetFloatArray($name: string, $values: System.Array$1<number>):void;
            public SetFloatArray($nameID: number, $values: System.Array$1<number>):void;
            public SetColorArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Color>):void;
            public SetColorArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Color>):void;
            public SetColorArray($name: string, $values: System.Array$1<UnityEngine.Color>):void;
            public SetColorArray($nameID: number, $values: System.Array$1<UnityEngine.Color>):void;
            public SetVectorArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public SetVectorArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public SetVectorArray($name: string, $values: System.Array$1<UnityEngine.Vector4>):void;
            public SetVectorArray($nameID: number, $values: System.Array$1<UnityEngine.Vector4>):void;
            public SetMatrixArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public SetMatrixArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public SetMatrixArray($name: string, $values: System.Array$1<UnityEngine.Matrix4x4>):void;
            public SetMatrixArray($nameID: number, $values: System.Array$1<UnityEngine.Matrix4x4>):void;
            public GetFloat($name: string):number;
            public GetFloat($nameID: number):number;
            public GetInt($name: string):number;
            public GetInt($nameID: number):number;
            public GetColor($name: string):UnityEngine.Color;
            public GetColor($nameID: number):UnityEngine.Color;
            public GetVector($name: string):UnityEngine.Vector4;
            public GetVector($nameID: number):UnityEngine.Vector4;
            public GetMatrix($name: string):UnityEngine.Matrix4x4;
            public GetMatrix($nameID: number):UnityEngine.Matrix4x4;
            public GetTexture($name: string):UnityEngine.Texture;
            public GetTexture($nameID: number):UnityEngine.Texture;
            public GetFloatArray($name: string):System.Array$1<number>;
            public GetFloatArray($nameID: number):System.Array$1<number>;
            public GetColorArray($name: string):System.Array$1<UnityEngine.Color>;
            public GetColorArray($nameID: number):System.Array$1<UnityEngine.Color>;
            public GetVectorArray($name: string):System.Array$1<UnityEngine.Vector4>;
            public GetVectorArray($nameID: number):System.Array$1<UnityEngine.Vector4>;
            public GetMatrixArray($name: string):System.Array$1<UnityEngine.Matrix4x4>;
            public GetMatrixArray($nameID: number):System.Array$1<UnityEngine.Matrix4x4>;
            public GetFloatArray($name: string, $values: System.Collections.Generic.List$1<number>):void;
            public GetFloatArray($nameID: number, $values: System.Collections.Generic.List$1<number>):void;
            public GetColorArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Color>):void;
            public GetColorArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Color>):void;
            public GetVectorArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public GetVectorArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public GetMatrixArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public GetMatrixArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public SetTextureOffset($name: string, $value: UnityEngine.Vector2):void;
            public SetTextureOffset($nameID: number, $value: UnityEngine.Vector2):void;
            public SetTextureScale($name: string, $value: UnityEngine.Vector2):void;
            public SetTextureScale($nameID: number, $value: UnityEngine.Vector2):void;
            public GetTextureOffset($name: string):UnityEngine.Vector2;
            public GetTextureOffset($nameID: number):UnityEngine.Vector2;
            public GetTextureScale($name: string):UnityEngine.Vector2;
            public GetTextureScale($nameID: number):UnityEngine.Vector2;
            
        }
        class Shader extends UnityEngine.Object {
            public maximumLOD: number;
            public static globalMaximumLOD: number;
            public isSupported: boolean;
            public static globalRenderPipeline: string;
            public renderQueue: number;
            public static Find($name: string):UnityEngine.Shader;
            public static EnableKeyword($keyword: string):void;
            public static DisableKeyword($keyword: string):void;
            public static IsKeywordEnabled($keyword: string):boolean;
            public static WarmupAllShaders():void;
            public static PropertyToID($name: string):number;
            public static SetGlobalFloat($name: string, $value: number):void;
            public static SetGlobalFloat($nameID: number, $value: number):void;
            public static SetGlobalInt($name: string, $value: number):void;
            public static SetGlobalInt($nameID: number, $value: number):void;
            public static SetGlobalVector($name: string, $value: UnityEngine.Vector4):void;
            public static SetGlobalVector($nameID: number, $value: UnityEngine.Vector4):void;
            public static SetGlobalColor($name: string, $value: UnityEngine.Color):void;
            public static SetGlobalColor($nameID: number, $value: UnityEngine.Color):void;
            public static SetGlobalMatrix($name: string, $value: UnityEngine.Matrix4x4):void;
            public static SetGlobalMatrix($nameID: number, $value: UnityEngine.Matrix4x4):void;
            public static SetGlobalTexture($name: string, $value: UnityEngine.Texture):void;
            public static SetGlobalTexture($nameID: number, $value: UnityEngine.Texture):void;
            public static SetGlobalBuffer($name: string, $value: UnityEngine.ComputeBuffer):void;
            public static SetGlobalBuffer($nameID: number, $value: UnityEngine.ComputeBuffer):void;
            public static SetGlobalFloatArray($name: string, $values: System.Collections.Generic.List$1<number>):void;
            public static SetGlobalFloatArray($nameID: number, $values: System.Collections.Generic.List$1<number>):void;
            public static SetGlobalFloatArray($name: string, $values: System.Array$1<number>):void;
            public static SetGlobalFloatArray($nameID: number, $values: System.Array$1<number>):void;
            public static SetGlobalVectorArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public static SetGlobalVectorArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public static SetGlobalVectorArray($name: string, $values: System.Array$1<UnityEngine.Vector4>):void;
            public static SetGlobalVectorArray($nameID: number, $values: System.Array$1<UnityEngine.Vector4>):void;
            public static SetGlobalMatrixArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public static SetGlobalMatrixArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public static SetGlobalMatrixArray($name: string, $values: System.Array$1<UnityEngine.Matrix4x4>):void;
            public static SetGlobalMatrixArray($nameID: number, $values: System.Array$1<UnityEngine.Matrix4x4>):void;
            public static GetGlobalFloat($name: string):number;
            public static GetGlobalFloat($nameID: number):number;
            public static GetGlobalInt($name: string):number;
            public static GetGlobalInt($nameID: number):number;
            public static GetGlobalVector($name: string):UnityEngine.Vector4;
            public static GetGlobalVector($nameID: number):UnityEngine.Vector4;
            public static GetGlobalColor($name: string):UnityEngine.Color;
            public static GetGlobalColor($nameID: number):UnityEngine.Color;
            public static GetGlobalMatrix($name: string):UnityEngine.Matrix4x4;
            public static GetGlobalMatrix($nameID: number):UnityEngine.Matrix4x4;
            public static GetGlobalTexture($name: string):UnityEngine.Texture;
            public static GetGlobalTexture($nameID: number):UnityEngine.Texture;
            public static GetGlobalFloatArray($name: string):System.Array$1<number>;
            public static GetGlobalFloatArray($nameID: number):System.Array$1<number>;
            public static GetGlobalVectorArray($name: string):System.Array$1<UnityEngine.Vector4>;
            public static GetGlobalVectorArray($nameID: number):System.Array$1<UnityEngine.Vector4>;
            public static GetGlobalMatrixArray($name: string):System.Array$1<UnityEngine.Matrix4x4>;
            public static GetGlobalMatrixArray($nameID: number):System.Array$1<UnityEngine.Matrix4x4>;
            public static GetGlobalFloatArray($name: string, $values: System.Collections.Generic.List$1<number>):void;
            public static GetGlobalFloatArray($nameID: number, $values: System.Collections.Generic.List$1<number>):void;
            public static GetGlobalVectorArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public static GetGlobalVectorArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public static GetGlobalMatrixArray($name: string, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public static GetGlobalMatrixArray($nameID: number, $values: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            
        }
        enum MaterialGlobalIlluminationFlags { None = 0, RealtimeEmissive = 1, BakedEmissive = 2, EmissiveIsBlack = 4, AnyEmissive = 3 }
        class ComputeBuffer extends System.Object {
            
        }
        class Ray extends System.ValueType {
            public origin: UnityEngine.Vector3;
            public direction: UnityEngine.Vector3;
            public constructor($origin: UnityEngine.Vector3, $direction: UnityEngine.Vector3);
            public GetPoint($distance: number):UnityEngine.Vector3;
            public ToString():string;
            public ToString($format: string):string;
            
        }
        class Camera extends UnityEngine.Behaviour {
            public static onPreCull: UnityEngine.Camera.CameraCallback;
            public static onPreRender: UnityEngine.Camera.CameraCallback;
            public static onPostRender: UnityEngine.Camera.CameraCallback;
            public nearClipPlane: number;
            public farClipPlane: number;
            public fieldOfView: number;
            public renderingPath: UnityEngine.RenderingPath;
            public actualRenderingPath: UnityEngine.RenderingPath;
            public allowHDR: boolean;
            public allowMSAA: boolean;
            public allowDynamicResolution: boolean;
            public forceIntoRenderTexture: boolean;
            public orthographicSize: number;
            public orthographic: boolean;
            public opaqueSortMode: UnityEngine.Rendering.OpaqueSortMode;
            public transparencySortMode: UnityEngine.TransparencySortMode;
            public transparencySortAxis: UnityEngine.Vector3;
            public depth: number;
            public aspect: number;
            public velocity: UnityEngine.Vector3;
            public cullingMask: number;
            public eventMask: number;
            public layerCullSpherical: boolean;
            public cameraType: UnityEngine.CameraType;
            public layerCullDistances: System.Array$1<number>;
            public useOcclusionCulling: boolean;
            public cullingMatrix: UnityEngine.Matrix4x4;
            public backgroundColor: UnityEngine.Color;
            public clearFlags: UnityEngine.CameraClearFlags;
            public depthTextureMode: UnityEngine.DepthTextureMode;
            public clearStencilAfterLightingPass: boolean;
            public usePhysicalProperties: boolean;
            public sensorSize: UnityEngine.Vector2;
            public lensShift: UnityEngine.Vector2;
            public focalLength: number;
            public gateFit: UnityEngine.Camera.GateFitMode;
            public rect: UnityEngine.Rect;
            public pixelRect: UnityEngine.Rect;
            public pixelWidth: number;
            public pixelHeight: number;
            public scaledPixelWidth: number;
            public scaledPixelHeight: number;
            public targetTexture: UnityEngine.RenderTexture;
            public activeTexture: UnityEngine.RenderTexture;
            public targetDisplay: number;
            public cameraToWorldMatrix: UnityEngine.Matrix4x4;
            public worldToCameraMatrix: UnityEngine.Matrix4x4;
            public projectionMatrix: UnityEngine.Matrix4x4;
            public nonJitteredProjectionMatrix: UnityEngine.Matrix4x4;
            public useJitteredProjectionMatrixForTransparentRendering: boolean;
            public previousViewProjectionMatrix: UnityEngine.Matrix4x4;
            public static main: UnityEngine.Camera;
            public static current: UnityEngine.Camera;
            public scene: UnityEngine.SceneManagement.Scene;
            public stereoEnabled: boolean;
            public stereoSeparation: number;
            public stereoConvergence: number;
            public areVRStereoViewMatricesWithinSingleCullTolerance: boolean;
            public stereoTargetEye: UnityEngine.StereoTargetEyeMask;
            public stereoActiveEye: UnityEngine.Camera.MonoOrStereoscopicEye;
            public static allCamerasCount: number;
            public static allCameras: System.Array$1<UnityEngine.Camera>;
            public commandBufferCount: number;
            public constructor();
            public Reset():void;
            public ResetTransparencySortSettings():void;
            public ResetAspect():void;
            public ResetCullingMatrix():void;
            public SetReplacementShader($shader: UnityEngine.Shader, $replacementTag: string):void;
            public ResetReplacementShader():void;
            public SetTargetBuffers($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer):void;
            public SetTargetBuffers($colorBuffer: System.Array$1<UnityEngine.RenderBuffer>, $depthBuffer: UnityEngine.RenderBuffer):void;
            public ResetWorldToCameraMatrix():void;
            public ResetProjectionMatrix():void;
            public CalculateObliqueMatrix($clipPlane: UnityEngine.Vector4):UnityEngine.Matrix4x4;
            public WorldToScreenPoint($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye):UnityEngine.Vector3;
            public WorldToViewportPoint($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye):UnityEngine.Vector3;
            public ViewportToWorldPoint($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye):UnityEngine.Vector3;
            public ScreenToWorldPoint($position: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye):UnityEngine.Vector3;
            public WorldToScreenPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public WorldToViewportPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public ViewportToWorldPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public ScreenToWorldPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public ScreenToViewportPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public ViewportToScreenPoint($position: UnityEngine.Vector3):UnityEngine.Vector3;
            public ViewportPointToRay($pos: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye):UnityEngine.Ray;
            public ViewportPointToRay($pos: UnityEngine.Vector3):UnityEngine.Ray;
            public ScreenPointToRay($pos: UnityEngine.Vector3, $eye: UnityEngine.Camera.MonoOrStereoscopicEye):UnityEngine.Ray;
            public ScreenPointToRay($pos: UnityEngine.Vector3):UnityEngine.Ray;
            public CalculateFrustumCorners($viewport: UnityEngine.Rect, $z: number, $eye: UnityEngine.Camera.MonoOrStereoscopicEye, $outCorners: System.Array$1<UnityEngine.Vector3>):void;
            public static CalculateProjectionMatrixFromPhysicalProperties($output: $Ref<UnityEngine.Matrix4x4>, $focalLength: number, $sensorSize: UnityEngine.Vector2, $lensShift: UnityEngine.Vector2, $nearClip: number, $farClip: number, $gateFitParameters?: UnityEngine.Camera.GateFitParameters):void;
            public static FocalLengthToFOV($focalLength: number, $sensorSize: number):number;
            public static FOVToFocalLength($fov: number, $sensorSize: number):number;
            public GetStereoNonJitteredProjectionMatrix($eye: UnityEngine.Camera.StereoscopicEye):UnityEngine.Matrix4x4;
            public GetStereoViewMatrix($eye: UnityEngine.Camera.StereoscopicEye):UnityEngine.Matrix4x4;
            public CopyStereoDeviceProjectionMatrixToNonJittered($eye: UnityEngine.Camera.StereoscopicEye):void;
            public GetStereoProjectionMatrix($eye: UnityEngine.Camera.StereoscopicEye):UnityEngine.Matrix4x4;
            public SetStereoProjectionMatrix($eye: UnityEngine.Camera.StereoscopicEye, $matrix: UnityEngine.Matrix4x4):void;
            public ResetStereoProjectionMatrices():void;
            public SetStereoViewMatrix($eye: UnityEngine.Camera.StereoscopicEye, $matrix: UnityEngine.Matrix4x4):void;
            public ResetStereoViewMatrices():void;
            public static GetAllCameras($cameras: System.Array$1<UnityEngine.Camera>):number;
            public RenderToCubemap($cubemap: UnityEngine.Cubemap, $faceMask: number):boolean;
            public RenderToCubemap($cubemap: UnityEngine.Cubemap):boolean;
            public RenderToCubemap($cubemap: UnityEngine.RenderTexture, $faceMask: number):boolean;
            public RenderToCubemap($cubemap: UnityEngine.RenderTexture):boolean;
            public RenderToCubemap($cubemap: UnityEngine.RenderTexture, $faceMask: number, $stereoEye: UnityEngine.Camera.MonoOrStereoscopicEye):boolean;
            public Render():void;
            public RenderWithShader($shader: UnityEngine.Shader, $replacementTag: string):void;
            public RenderDontRestore():void;
            public static SetupCurrent($cur: UnityEngine.Camera):void;
            public CopyFrom($other: UnityEngine.Camera):void;
            public RemoveCommandBuffers($evt: UnityEngine.Rendering.CameraEvent):void;
            public RemoveAllCommandBuffers():void;
            public AddCommandBuffer($evt: UnityEngine.Rendering.CameraEvent, $buffer: UnityEngine.Rendering.CommandBuffer):void;
            public AddCommandBufferAsync($evt: UnityEngine.Rendering.CameraEvent, $buffer: UnityEngine.Rendering.CommandBuffer, $queueType: UnityEngine.Rendering.ComputeQueueType):void;
            public RemoveCommandBuffer($evt: UnityEngine.Rendering.CameraEvent, $buffer: UnityEngine.Rendering.CommandBuffer):void;
            public GetCommandBuffers($evt: UnityEngine.Rendering.CameraEvent):System.Array$1<UnityEngine.Rendering.CommandBuffer>;
            
        }
        enum RenderingPath { UsePlayerSettings = -1, VertexLit = 0, Forward = 1, DeferredLighting = 2, DeferredShading = 3 }
        enum TransparencySortMode { Default = 0, Perspective = 1, Orthographic = 2, CustomAxis = 3 }
        enum CameraType { Game = 1, SceneView = 2, Preview = 4, VR = 8, Reflection = 16 }
        enum CameraClearFlags { Skybox = 1, Color = 2, SolidColor = 2, Depth = 3, Nothing = 4 }
        enum DepthTextureMode { None = 0, Depth = 1, DepthNormals = 2, MotionVectors = 4 }
        class Rect extends System.ValueType {
            public static zero: UnityEngine.Rect;
            public x: number;
            public y: number;
            public position: UnityEngine.Vector2;
            public center: UnityEngine.Vector2;
            public min: UnityEngine.Vector2;
            public max: UnityEngine.Vector2;
            public width: number;
            public height: number;
            public size: UnityEngine.Vector2;
            public xMin: number;
            public yMin: number;
            public xMax: number;
            public yMax: number;
            public constructor($x: number, $y: number, $width: number, $height: number);
            public constructor($position: UnityEngine.Vector2, $size: UnityEngine.Vector2);
            public constructor($source: UnityEngine.Rect);
            public static MinMaxRect($xmin: number, $ymin: number, $xmax: number, $ymax: number):UnityEngine.Rect;
            public Set($x: number, $y: number, $width: number, $height: number):void;
            public Contains($point: UnityEngine.Vector2):boolean;
            public Contains($point: UnityEngine.Vector3):boolean;
            public Contains($point: UnityEngine.Vector3, $allowInverse: boolean):boolean;
            public Overlaps($other: UnityEngine.Rect):boolean;
            public Overlaps($other: UnityEngine.Rect, $allowInverse: boolean):boolean;
            public static NormalizedToPoint($rectangle: UnityEngine.Rect, $normalizedRectCoordinates: UnityEngine.Vector2):UnityEngine.Vector2;
            public static PointToNormalized($rectangle: UnityEngine.Rect, $point: UnityEngine.Vector2):UnityEngine.Vector2;
            public static op_Inequality($lhs: UnityEngine.Rect, $rhs: UnityEngine.Rect):boolean;
            public static op_Equality($lhs: UnityEngine.Rect, $rhs: UnityEngine.Rect):boolean;
            public Equals($other: any):boolean;
            public Equals($other: UnityEngine.Rect):boolean;
            public ToString():string;
            public ToString($format: string):string;
            
        }
        class RenderTexture extends UnityEngine.Texture {
            public width: number;
            public height: number;
            public dimension: UnityEngine.Rendering.TextureDimension;
            public useMipMap: boolean;
            public sRGB: boolean;
            public format: UnityEngine.RenderTextureFormat;
            public vrUsage: UnityEngine.VRTextureUsage;
            public memorylessMode: UnityEngine.RenderTextureMemoryless;
            public autoGenerateMips: boolean;
            public volumeDepth: number;
            public antiAliasing: number;
            public bindTextureMS: boolean;
            public enableRandomWrite: boolean;
            public useDynamicScale: boolean;
            public isPowerOfTwo: boolean;
            public static active: UnityEngine.RenderTexture;
            public colorBuffer: UnityEngine.RenderBuffer;
            public depthBuffer: UnityEngine.RenderBuffer;
            public depth: number;
            public descriptor: UnityEngine.RenderTextureDescriptor;
            public constructor($desc: UnityEngine.RenderTextureDescriptor);
            public constructor($textureToCopy: UnityEngine.RenderTexture);
            public constructor($width: number, $height: number, $depth: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat);
            public constructor($width: number, $height: number, $depth: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite);
            public constructor($width: number, $height: number, $depth: number, $format: UnityEngine.RenderTextureFormat);
            public constructor($width: number, $height: number, $depth: number);
            public GetNativeDepthBufferPtr():System.IntPtr;
            public DiscardContents($discardColor: boolean, $discardDepth: boolean):void;
            public MarkRestoreExpected():void;
            public DiscardContents():void;
            public ResolveAntiAliasedSurface():void;
            public ResolveAntiAliasedSurface($target: UnityEngine.RenderTexture):void;
            public SetGlobalShaderProperty($propertyName: string):void;
            public Create():boolean;
            public Release():void;
            public IsCreated():boolean;
            public GenerateMips():void;
            public ConvertToEquirect($equirect: UnityEngine.RenderTexture, $eye?: UnityEngine.Camera.MonoOrStereoscopicEye):void;
            public static SupportsStencil($rt: UnityEngine.RenderTexture):boolean;
            public static ReleaseTemporary($temp: UnityEngine.RenderTexture):void;
            public static GetTemporary($desc: UnityEngine.RenderTextureDescriptor):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number, $memorylessMode: UnityEngine.RenderTextureMemoryless, $vrUsage: UnityEngine.VRTextureUsage, $useDynamicScale: boolean):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number, $memorylessMode: UnityEngine.RenderTextureMemoryless, $vrUsage: UnityEngine.VRTextureUsage):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number, $memorylessMode: UnityEngine.RenderTextureMemoryless):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite, $antiAliasing: number):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat, $readWrite: UnityEngine.RenderTextureReadWrite):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number, $format: UnityEngine.RenderTextureFormat):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number, $depthBuffer: number):UnityEngine.RenderTexture;
            public static GetTemporary($width: number, $height: number):UnityEngine.RenderTexture;
            
        }
        class RenderBuffer extends System.ValueType {
            
        }
        enum StereoTargetEyeMask { None = 0, Left = 1, Right = 2, Both = 3 }
        class Cubemap extends UnityEngine.Texture {
            
        }
        class FrustumPlanes extends System.ValueType {
            
        }
        class Plane extends System.ValueType {
            public normal: UnityEngine.Vector3;
            public distance: number;
            public flipped: UnityEngine.Plane;
            public constructor($inNormal: UnityEngine.Vector3, $inPoint: UnityEngine.Vector3);
            public constructor($inNormal: UnityEngine.Vector3, $d: number);
            public constructor($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $c: UnityEngine.Vector3);
            public SetNormalAndPosition($inNormal: UnityEngine.Vector3, $inPoint: UnityEngine.Vector3):void;
            public Set3Points($a: UnityEngine.Vector3, $b: UnityEngine.Vector3, $c: UnityEngine.Vector3):void;
            public Flip():void;
            public Translate($translation: UnityEngine.Vector3):void;
            public static Translate($plane: UnityEngine.Plane, $translation: UnityEngine.Vector3):UnityEngine.Plane;
            public ClosestPointOnPlane($point: UnityEngine.Vector3):UnityEngine.Vector3;
            public GetDistanceToPoint($point: UnityEngine.Vector3):number;
            public GetSide($point: UnityEngine.Vector3):boolean;
            public SameSide($inPt0: UnityEngine.Vector3, $inPt1: UnityEngine.Vector3):boolean;
            public Raycast($ray: UnityEngine.Ray, $enter: $Ref<number>):boolean;
            public ToString():string;
            public ToString($format: string):string;
            
        }
        enum TexGenMode { None = 0, SphereMap = 1, Object = 2, EyeLinear = 3, CubeReflect = 4, CubeNormal = 5 }
        enum RenderTextureFormat { ARGB32 = 0, Depth = 1, ARGBHalf = 2, Shadowmap = 3, RGB565 = 4, ARGB4444 = 5, ARGB1555 = 6, Default = 7, ARGB2101010 = 8, DefaultHDR = 9, ARGB64 = 10, ARGBFloat = 11, RGFloat = 12, RGHalf = 13, RFloat = 14, RHalf = 15, R8 = 16, ARGBInt = 17, RGInt = 18, RInt = 19, BGRA32 = 20, RGB111110Float = 22, RG32 = 23, RGBAUShort = 24, RG16 = 25, BGRA10101010_XR = 26, BGR101010_XR = 27, R16 = 28 }
        enum VRTextureUsage { None = 0, OneEye = 1, TwoEyes = 2 }
        enum RenderTextureMemoryless { None = 0, Color = 1, Depth = 2, MSAA = 4 }
        class RenderTextureDescriptor extends System.ValueType {
            
        }
        enum RenderTextureReadWrite { Default = 0, Linear = 1, sRGB = 2 }
        class Debug extends System.Object {
            public static unityLogger: UnityEngine.ILogger;
            public static developerConsoleVisible: boolean;
            public static isDebugBuild: boolean;
            public constructor();
            public static DrawLine($start: UnityEngine.Vector3, $end: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number):void;
            public static DrawLine($start: UnityEngine.Vector3, $end: UnityEngine.Vector3, $color: UnityEngine.Color):void;
            public static DrawLine($start: UnityEngine.Vector3, $end: UnityEngine.Vector3):void;
            public static DrawLine($start: UnityEngine.Vector3, $end: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number, $depthTest: boolean):void;
            public static DrawRay($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number):void;
            public static DrawRay($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3, $color: UnityEngine.Color):void;
            public static DrawRay($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3):void;
            public static DrawRay($start: UnityEngine.Vector3, $dir: UnityEngine.Vector3, $color: UnityEngine.Color, $duration: number, $depthTest: boolean):void;
            public static Break():void;
            public static DebugBreak():void;
            public static Log($message: any):void;
            public static Log($message: any, $context: UnityEngine.Object):void;
            public static LogFormat($format: string, ...args: any[]):void;
            public static LogFormat($context: UnityEngine.Object, $format: string, ...args: any[]):void;
            public static LogError($message: any):void;
            public static LogError($message: any, $context: UnityEngine.Object):void;
            public static LogErrorFormat($format: string, ...args: any[]):void;
            public static LogErrorFormat($context: UnityEngine.Object, $format: string, ...args: any[]):void;
            public static ClearDeveloperConsole():void;
            public static LogException($exception: System.Exception):void;
            public static LogException($exception: System.Exception, $context: UnityEngine.Object):void;
            public static LogWarning($message: any):void;
            public static LogWarning($message: any, $context: UnityEngine.Object):void;
            public static LogWarningFormat($format: string, ...args: any[]):void;
            public static LogWarningFormat($context: UnityEngine.Object, $format: string, ...args: any[]):void;
            public static Assert($condition: boolean):void;
            public static Assert($condition: boolean, $context: UnityEngine.Object):void;
            public static Assert($condition: boolean, $message: any):void;
            public static Assert($condition: boolean, $message: string):void;
            public static Assert($condition: boolean, $message: any, $context: UnityEngine.Object):void;
            public static Assert($condition: boolean, $message: string, $context: UnityEngine.Object):void;
            public static AssertFormat($condition: boolean, $format: string, ...args: any[]):void;
            public static AssertFormat($condition: boolean, $context: UnityEngine.Object, $format: string, ...args: any[]):void;
            public static LogAssertion($message: any):void;
            public static LogAssertion($message: any, $context: UnityEngine.Object):void;
            public static LogAssertionFormat($format: string, ...args: any[]):void;
            public static LogAssertionFormat($context: UnityEngine.Object, $format: string, ...args: any[]):void;
            
        }
        interface ILogger {
            
        }
        class Display extends System.Object {
            public static displays: System.Array$1<UnityEngine.Display>;
            public renderingWidth: number;
            public renderingHeight: number;
            public systemWidth: number;
            public systemHeight: number;
            public colorBuffer: UnityEngine.RenderBuffer;
            public depthBuffer: UnityEngine.RenderBuffer;
            public active: boolean;
            public static main: UnityEngine.Display;
            public Activate():void;
            public Activate($width: number, $height: number, $refreshRate: number):void;
            public SetParams($width: number, $height: number, $x: number, $y: number):void;
            public SetRenderingResolution($w: number, $h: number):void;
            public static RelativeMouseAt($inputMouseCoordinates: UnityEngine.Vector3):UnityEngine.Vector3;
            public static add_onDisplaysUpdated($value: UnityEngine.Display.DisplaysUpdatedDelegate):void;
            public static remove_onDisplaysUpdated($value: UnityEngine.Display.DisplaysUpdatedDelegate):void;
            
        }
        class Gradient extends System.Object {
            public colorKeys: System.Array$1<UnityEngine.GradientColorKey>;
            public alphaKeys: System.Array$1<UnityEngine.GradientAlphaKey>;
            public mode: UnityEngine.GradientMode;
            public constructor();
            public Evaluate($time: number):UnityEngine.Color;
            public SetKeys($colorKeys: System.Array$1<UnityEngine.GradientColorKey>, $alphaKeys: System.Array$1<UnityEngine.GradientAlphaKey>):void;
            public Equals($o: any):boolean;
            public Equals($other: UnityEngine.Gradient):boolean;
            
        }
        class GradientColorKey extends System.ValueType {
            
        }
        class GradientAlphaKey extends System.ValueType {
            
        }
        enum GradientMode { Blend = 0, Fixed = 1 }
        class Screen extends System.Object {
            public static width: number;
            public static height: number;
            public static dpi: number;
            public static orientation: UnityEngine.ScreenOrientation;
            public static sleepTimeout: number;
            public static autorotateToPortrait: boolean;
            public static autorotateToPortraitUpsideDown: boolean;
            public static autorotateToLandscapeLeft: boolean;
            public static autorotateToLandscapeRight: boolean;
            public static currentResolution: UnityEngine.Resolution;
            public static fullScreen: boolean;
            public static fullScreenMode: UnityEngine.FullScreenMode;
            public static safeArea: UnityEngine.Rect;
            public static resolutions: System.Array$1<UnityEngine.Resolution>;
            public constructor();
            public static SetResolution($width: number, $height: number, $fullscreenMode: UnityEngine.FullScreenMode, $preferredRefreshRate: number):void;
            public static SetResolution($width: number, $height: number, $fullscreenMode: UnityEngine.FullScreenMode):void;
            public static SetResolution($width: number, $height: number, $fullscreen: boolean, $preferredRefreshRate: number):void;
            public static SetResolution($width: number, $height: number, $fullscreen: boolean):void;
            
        }
        enum ScreenOrientation { Unknown = 0, Portrait = 1, PortraitUpsideDown = 2, LandscapeLeft = 3, LandscapeRight = 4, AutoRotation = 5, Landscape = 3 }
        class Resolution extends System.ValueType {
            
        }
        enum FullScreenMode { ExclusiveFullScreen = 0, FullScreenWindow = 1, MaximizedWindow = 2, Windowed = 3 }
        class Graphics extends System.Object {
            public static activeColorGamut: UnityEngine.ColorGamut;
            public static activeTier: UnityEngine.Rendering.GraphicsTier;
            public static activeColorBuffer: UnityEngine.RenderBuffer;
            public static activeDepthBuffer: UnityEngine.RenderBuffer;
            public constructor();
            public static ClearRandomWriteTargets():void;
            public static ExecuteCommandBuffer($buffer: UnityEngine.Rendering.CommandBuffer):void;
            public static ExecuteCommandBufferAsync($buffer: UnityEngine.Rendering.CommandBuffer, $queueType: UnityEngine.Rendering.ComputeQueueType):void;
            public static SetRenderTarget($rt: UnityEngine.RenderTexture, $mipLevel: number, $face: UnityEngine.CubemapFace, $depthSlice: number):void;
            public static SetRenderTarget($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer, $mipLevel: number, $face: UnityEngine.CubemapFace, $depthSlice: number):void;
            public static SetRenderTarget($colorBuffers: System.Array$1<UnityEngine.RenderBuffer>, $depthBuffer: UnityEngine.RenderBuffer):void;
            public static SetRenderTarget($setup: UnityEngine.RenderTargetSetup):void;
            public static SetRandomWriteTarget($index: number, $uav: UnityEngine.RenderTexture):void;
            public static SetRandomWriteTarget($index: number, $uav: UnityEngine.ComputeBuffer, $preserveCounterValue: boolean):void;
            public static CopyTexture($src: UnityEngine.Texture, $dst: UnityEngine.Texture):void;
            public static CopyTexture($src: UnityEngine.Texture, $srcElement: number, $dst: UnityEngine.Texture, $dstElement: number):void;
            public static CopyTexture($src: UnityEngine.Texture, $srcElement: number, $srcMip: number, $dst: UnityEngine.Texture, $dstElement: number, $dstMip: number):void;
            public static CopyTexture($src: UnityEngine.Texture, $srcElement: number, $srcMip: number, $srcX: number, $srcY: number, $srcWidth: number, $srcHeight: number, $dst: UnityEngine.Texture, $dstElement: number, $dstMip: number, $dstX: number, $dstY: number):void;
            public static ConvertTexture($src: UnityEngine.Texture, $dst: UnityEngine.Texture):boolean;
            public static ConvertTexture($src: UnityEngine.Texture, $srcElement: number, $dst: UnityEngine.Texture, $dstElement: number):boolean;
            public static CreateGPUFence($stage: UnityEngine.Rendering.SynchronisationStage):UnityEngine.Rendering.GPUFence;
            public static WaitOnGPUFence($fence: UnityEngine.Rendering.GPUFence, $stage: UnityEngine.Rendering.SynchronisationStage):void;
            public static CreateGPUFence():UnityEngine.Rendering.GPUFence;
            public static WaitOnGPUFence($fence: UnityEngine.Rendering.GPUFence):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $color: UnityEngine.Color, $mat: UnityEngine.Material, $pass: number):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material, $pass: number):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material, $pass: number):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $mat: UnityEngine.Material, $pass: number):void;
            public static DrawMeshNow($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $materialIndex: number):void;
            public static DrawMeshNow($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $materialIndex: number):void;
            public static DrawMeshNow($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion):void;
            public static DrawMeshNow($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean, $useLightProbes: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $useLightProbes: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean, $useLightProbes: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage, $lightProbeProxyVolume: UnityEngine.LightProbeProxyVolume):void;
            public static DrawProcedural($topology: UnityEngine.MeshTopology, $vertexCount: number, $instanceCount: number):void;
            public static DrawProceduralIndirect($topology: UnityEngine.MeshTopology, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number):void;
            public static Blit($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture):void;
            public static Blit($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $scale: UnityEngine.Vector2, $offset: UnityEngine.Vector2):void;
            public static Blit($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $mat: UnityEngine.Material, $pass: number):void;
            public static Blit($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $mat: UnityEngine.Material):void;
            public static Blit($source: UnityEngine.Texture, $mat: UnityEngine.Material, $pass: number):void;
            public static Blit($source: UnityEngine.Texture, $mat: UnityEngine.Material):void;
            public static BlitMultiTap($source: UnityEngine.Texture, $dest: UnityEngine.RenderTexture, $mat: UnityEngine.Material, ...offsets: UnityEngine.Vector2[]):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $position: UnityEngine.Vector3, $rotation: UnityEngine.Quaternion, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: boolean, $receiveShadows: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $useLightProbes: boolean):void;
            public static DrawMesh($mesh: UnityEngine.Mesh, $matrix: UnityEngine.Matrix4x4, $material: UnityEngine.Material, $layer: number, $camera: UnityEngine.Camera, $submeshIndex: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $probeAnchor: UnityEngine.Transform, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Array$1<UnityEngine.Matrix4x4>, $count: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera):void;
            public static DrawMeshInstanced($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $matrices: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera):void;
            public static DrawMeshInstancedIndirect($mesh: UnityEngine.Mesh, $submeshIndex: number, $material: UnityEngine.Material, $bounds: UnityEngine.Bounds, $bufferWithArgs: UnityEngine.ComputeBuffer, $argsOffset: number, $properties: UnityEngine.MaterialPropertyBlock, $castShadows: UnityEngine.Rendering.ShadowCastingMode, $receiveShadows: boolean, $layer: number, $camera: UnityEngine.Camera, $lightProbeUsage: UnityEngine.Rendering.LightProbeUsage):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $color: UnityEngine.Color, $mat: UnityEngine.Material):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $color: UnityEngine.Color):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $sourceRect: UnityEngine.Rect, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number, $mat: UnityEngine.Material):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $leftBorder: number, $rightBorder: number, $topBorder: number, $bottomBorder: number):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture, $mat: UnityEngine.Material):void;
            public static DrawTexture($screenRect: UnityEngine.Rect, $texture: UnityEngine.Texture):void;
            public static DrawProcedural($topology: UnityEngine.MeshTopology, $vertexCount: number):void;
            public static DrawProceduralIndirect($topology: UnityEngine.MeshTopology, $bufferWithArgs: UnityEngine.ComputeBuffer):void;
            public static SetRenderTarget($rt: UnityEngine.RenderTexture):void;
            public static SetRenderTarget($rt: UnityEngine.RenderTexture, $mipLevel: number):void;
            public static SetRenderTarget($rt: UnityEngine.RenderTexture, $mipLevel: number, $face: UnityEngine.CubemapFace):void;
            public static SetRenderTarget($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer):void;
            public static SetRenderTarget($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer, $mipLevel: number):void;
            public static SetRenderTarget($colorBuffer: UnityEngine.RenderBuffer, $depthBuffer: UnityEngine.RenderBuffer, $mipLevel: number, $face: UnityEngine.CubemapFace):void;
            public static SetRandomWriteTarget($index: number, $uav: UnityEngine.ComputeBuffer):void;
            
        }
        enum ColorGamut { sRGB = 0, Rec709 = 1, Rec2020 = 2, DisplayP3 = 3, HDR10 = 4, DolbyHDR = 5 }
        enum CubemapFace { Unknown = -1, PositiveX = 0, NegativeX = 1, PositiveY = 2, NegativeY = 3, PositiveZ = 4, NegativeZ = 5 }
        class RenderTargetSetup extends System.ValueType {
            
        }
        class Mesh extends UnityEngine.Object {
            public indexFormat: UnityEngine.Rendering.IndexFormat;
            public vertexBufferCount: number;
            public blendShapeCount: number;
            public boneWeights: System.Array$1<UnityEngine.BoneWeight>;
            public bindposes: System.Array$1<UnityEngine.Matrix4x4>;
            public isReadable: boolean;
            public vertexCount: number;
            public subMeshCount: number;
            public bounds: UnityEngine.Bounds;
            public vertices: System.Array$1<UnityEngine.Vector3>;
            public normals: System.Array$1<UnityEngine.Vector3>;
            public tangents: System.Array$1<UnityEngine.Vector4>;
            public uv: System.Array$1<UnityEngine.Vector2>;
            public uv2: System.Array$1<UnityEngine.Vector2>;
            public uv3: System.Array$1<UnityEngine.Vector2>;
            public uv4: System.Array$1<UnityEngine.Vector2>;
            public uv5: System.Array$1<UnityEngine.Vector2>;
            public uv6: System.Array$1<UnityEngine.Vector2>;
            public uv7: System.Array$1<UnityEngine.Vector2>;
            public uv8: System.Array$1<UnityEngine.Vector2>;
            public colors: System.Array$1<UnityEngine.Color>;
            public colors32: System.Array$1<UnityEngine.Color32>;
            public triangles: System.Array$1<number>;
            public constructor();
            public GetNativeVertexBufferPtr($index: number):System.IntPtr;
            public GetNativeIndexBufferPtr():System.IntPtr;
            public ClearBlendShapes():void;
            public GetBlendShapeName($shapeIndex: number):string;
            public GetBlendShapeIndex($blendShapeName: string):number;
            public GetBlendShapeFrameCount($shapeIndex: number):number;
            public GetBlendShapeFrameWeight($shapeIndex: number, $frameIndex: number):number;
            public GetBlendShapeFrameVertices($shapeIndex: number, $frameIndex: number, $deltaVertices: System.Array$1<UnityEngine.Vector3>, $deltaNormals: System.Array$1<UnityEngine.Vector3>, $deltaTangents: System.Array$1<UnityEngine.Vector3>):void;
            public AddBlendShapeFrame($shapeName: string, $frameWeight: number, $deltaVertices: System.Array$1<UnityEngine.Vector3>, $deltaNormals: System.Array$1<UnityEngine.Vector3>, $deltaTangents: System.Array$1<UnityEngine.Vector3>):void;
            public GetUVDistributionMetric($uvSetIndex: number):number;
            public GetVertices($vertices: System.Collections.Generic.List$1<UnityEngine.Vector3>):void;
            public SetVertices($inVertices: System.Collections.Generic.List$1<UnityEngine.Vector3>):void;
            public GetNormals($normals: System.Collections.Generic.List$1<UnityEngine.Vector3>):void;
            public SetNormals($inNormals: System.Collections.Generic.List$1<UnityEngine.Vector3>):void;
            public GetTangents($tangents: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public SetTangents($inTangents: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public GetColors($colors: System.Collections.Generic.List$1<UnityEngine.Color>):void;
            public SetColors($inColors: System.Collections.Generic.List$1<UnityEngine.Color>):void;
            public GetColors($colors: System.Collections.Generic.List$1<UnityEngine.Color32>):void;
            public SetColors($inColors: System.Collections.Generic.List$1<UnityEngine.Color32>):void;
            public SetUVs($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector2>):void;
            public SetUVs($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector3>):void;
            public SetUVs($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public GetUVs($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector2>):void;
            public GetUVs($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector3>):void;
            public GetUVs($channel: number, $uvs: System.Collections.Generic.List$1<UnityEngine.Vector4>):void;
            public GetTriangles($submesh: number):System.Array$1<number>;
            public GetTriangles($submesh: number, $applyBaseVertex: boolean):System.Array$1<number>;
            public GetTriangles($triangles: System.Collections.Generic.List$1<number>, $submesh: number):void;
            public GetTriangles($triangles: System.Collections.Generic.List$1<number>, $submesh: number, $applyBaseVertex: boolean):void;
            public GetIndices($submesh: number):System.Array$1<number>;
            public GetIndices($submesh: number, $applyBaseVertex: boolean):System.Array$1<number>;
            public GetIndices($indices: System.Collections.Generic.List$1<number>, $submesh: number):void;
            public GetIndices($indices: System.Collections.Generic.List$1<number>, $submesh: number, $applyBaseVertex: boolean):void;
            public GetIndexStart($submesh: number):number;
            public GetIndexCount($submesh: number):number;
            public GetBaseVertex($submesh: number):number;
            public SetTriangles($triangles: System.Array$1<number>, $submesh: number):void;
            public SetTriangles($triangles: System.Array$1<number>, $submesh: number, $calculateBounds: boolean):void;
            public SetTriangles($triangles: System.Array$1<number>, $submesh: number, $calculateBounds: boolean, $baseVertex: number):void;
            public SetTriangles($triangles: System.Collections.Generic.List$1<number>, $submesh: number):void;
            public SetTriangles($triangles: System.Collections.Generic.List$1<number>, $submesh: number, $calculateBounds: boolean):void;
            public SetTriangles($triangles: System.Collections.Generic.List$1<number>, $submesh: number, $calculateBounds: boolean, $baseVertex: number):void;
            public SetIndices($indices: System.Array$1<number>, $topology: UnityEngine.MeshTopology, $submesh: number):void;
            public SetIndices($indices: System.Array$1<number>, $topology: UnityEngine.MeshTopology, $submesh: number, $calculateBounds: boolean):void;
            public SetIndices($indices: System.Array$1<number>, $topology: UnityEngine.MeshTopology, $submesh: number, $calculateBounds: boolean, $baseVertex: number):void;
            public GetBindposes($bindposes: System.Collections.Generic.List$1<UnityEngine.Matrix4x4>):void;
            public GetBoneWeights($boneWeights: System.Collections.Generic.List$1<UnityEngine.BoneWeight>):void;
            public Clear($keepVertexLayout: boolean):void;
            public Clear():void;
            public RecalculateBounds():void;
            public RecalculateNormals():void;
            public RecalculateTangents():void;
            public MarkDynamic():void;
            public UploadMeshData($markNoLongerReadable: boolean):void;
            public GetTopology($submesh: number):UnityEngine.MeshTopology;
            public CombineMeshes($combine: System.Array$1<UnityEngine.CombineInstance>, $mergeSubMeshes: boolean, $useMatrices: boolean, $hasLightmapData: boolean):void;
            public CombineMeshes($combine: System.Array$1<UnityEngine.CombineInstance>, $mergeSubMeshes: boolean, $useMatrices: boolean):void;
            public CombineMeshes($combine: System.Array$1<UnityEngine.CombineInstance>, $mergeSubMeshes: boolean):void;
            public CombineMeshes($combine: System.Array$1<UnityEngine.CombineInstance>):void;
            
        }
        class MaterialPropertyBlock extends System.Object {
            
        }
        class LightProbeProxyVolume extends UnityEngine.Behaviour {
            
        }
        class Bounds extends System.ValueType {
            
        }
        enum MeshTopology { Triangles = 0, Quads = 2, Lines = 3, LineStrip = 4, Points = 5 }
        class BoneWeight extends System.ValueType {
            
        }
        class Color32 extends System.ValueType {
            
        }
        class CombineInstance extends System.ValueType {
            
        }
        class GL extends System.Object {
            public static TRIANGLES: number;
            public static TRIANGLE_STRIP: number;
            public static QUADS: number;
            public static LINES: number;
            public static LINE_STRIP: number;
            public static wireframe: boolean;
            public static sRGBWrite: boolean;
            public static invertCulling: boolean;
            public static modelview: UnityEngine.Matrix4x4;
            public constructor();
            public static Vertex3($x: number, $y: number, $z: number):void;
            public static Vertex($v: UnityEngine.Vector3):void;
            public static TexCoord3($x: number, $y: number, $z: number):void;
            public static TexCoord($v: UnityEngine.Vector3):void;
            public static TexCoord2($x: number, $y: number):void;
            public static MultiTexCoord3($unit: number, $x: number, $y: number, $z: number):void;
            public static MultiTexCoord($unit: number, $v: UnityEngine.Vector3):void;
            public static MultiTexCoord2($unit: number, $x: number, $y: number):void;
            public static Color($c: UnityEngine.Color):void;
            public static Flush():void;
            public static RenderTargetBarrier():void;
            public static MultMatrix($m: UnityEngine.Matrix4x4):void;
            public static PushMatrix():void;
            public static PopMatrix():void;
            public static LoadIdentity():void;
            public static LoadOrtho():void;
            public static LoadPixelMatrix():void;
            public static LoadProjectionMatrix($mat: UnityEngine.Matrix4x4):void;
            public static InvalidateState():void;
            public static GetGPUProjectionMatrix($proj: UnityEngine.Matrix4x4, $renderIntoTexture: boolean):UnityEngine.Matrix4x4;
            public static LoadPixelMatrix($left: number, $right: number, $bottom: number, $top: number):void;
            public static IssuePluginEvent($callback: System.IntPtr, $eventID: number):void;
            public static Begin($mode: number):void;
            public static End():void;
            public static Clear($clearDepth: boolean, $clearColor: boolean, $backgroundColor: UnityEngine.Color, $depth: number):void;
            public static Clear($clearDepth: boolean, $clearColor: boolean, $backgroundColor: UnityEngine.Color):void;
            public static Viewport($pixelRect: UnityEngine.Rect):void;
            public static ClearWithSkybox($clearDepth: boolean, $camera: UnityEngine.Camera):void;
            
        }
        class Texture2D extends UnityEngine.Texture {
            public mipmapCount: number;
            public format: UnityEngine.TextureFormat;
            public static whiteTexture: UnityEngine.Texture2D;
            public static blackTexture: UnityEngine.Texture2D;
            public isReadable: boolean;
            public streamingMipmaps: boolean;
            public streamingMipmapsPriority: number;
            public requestedMipmapLevel: number;
            public desiredMipmapLevel: number;
            public loadingMipmapLevel: number;
            public loadedMipmapLevel: number;
            public alphaIsTransparency: boolean;
            public constructor($width: number, $height: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat, $flags: UnityEngine.Experimental.Rendering.TextureCreationFlags);
            public constructor($width: number, $height: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean, $linear: boolean);
            public constructor($width: number, $height: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean);
            public constructor($width: number, $height: number);
            public Compress($highQuality: boolean):void;
            public ClearRequestedMipmapLevel():void;
            public IsRequestedMipmapLevelLoaded():boolean;
            public UpdateExternalTexture($nativeTex: System.IntPtr):void;
            public GetRawTextureData():System.Array$1<number>;
            public GetPixels($x: number, $y: number, $blockWidth: number, $blockHeight: number, $miplevel: number):System.Array$1<UnityEngine.Color>;
            public GetPixels($x: number, $y: number, $blockWidth: number, $blockHeight: number):System.Array$1<UnityEngine.Color>;
            public GetPixels32($miplevel: number):System.Array$1<UnityEngine.Color32>;
            public GetPixels32():System.Array$1<UnityEngine.Color32>;
            public PackTextures($textures: System.Array$1<UnityEngine.Texture2D>, $padding: number, $maximumAtlasSize: number, $makeNoLongerReadable: boolean):System.Array$1<UnityEngine.Rect>;
            public PackTextures($textures: System.Array$1<UnityEngine.Texture2D>, $padding: number, $maximumAtlasSize: number):System.Array$1<UnityEngine.Rect>;
            public PackTextures($textures: System.Array$1<UnityEngine.Texture2D>, $padding: number):System.Array$1<UnityEngine.Rect>;
            public static CreateExternalTexture($width: number, $height: number, $format: UnityEngine.TextureFormat, $mipChain: boolean, $linear: boolean, $nativeTex: System.IntPtr):UnityEngine.Texture2D;
            public SetPixel($x: number, $y: number, $color: UnityEngine.Color):void;
            public SetPixels($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color>, $miplevel: number):void;
            public SetPixels($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color>):void;
            public SetPixels($colors: System.Array$1<UnityEngine.Color>, $miplevel: number):void;
            public SetPixels($colors: System.Array$1<UnityEngine.Color>):void;
            public GetPixel($x: number, $y: number):UnityEngine.Color;
            public GetPixelBilinear($x: number, $y: number):UnityEngine.Color;
            public LoadRawTextureData($data: System.IntPtr, $size: number):void;
            public LoadRawTextureData($data: System.Array$1<number>):void;
            public Apply($updateMipmaps: boolean, $makeNoLongerReadable: boolean):void;
            public Apply($updateMipmaps: boolean):void;
            public Apply():void;
            public Resize($width: number, $height: number):boolean;
            public Resize($width: number, $height: number, $format: UnityEngine.TextureFormat, $hasMipMap: boolean):boolean;
            public ReadPixels($source: UnityEngine.Rect, $destX: number, $destY: number, $recalculateMipMaps: boolean):void;
            public ReadPixels($source: UnityEngine.Rect, $destX: number, $destY: number):void;
            public static GenerateAtlas($sizes: System.Array$1<UnityEngine.Vector2>, $padding: number, $atlasSize: number, $results: System.Collections.Generic.List$1<UnityEngine.Rect>):boolean;
            public SetPixels32($colors: System.Array$1<UnityEngine.Color32>, $miplevel: number):void;
            public SetPixels32($colors: System.Array$1<UnityEngine.Color32>):void;
            public SetPixels32($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color32>, $miplevel: number):void;
            public SetPixels32($x: number, $y: number, $blockWidth: number, $blockHeight: number, $colors: System.Array$1<UnityEngine.Color32>):void;
            public GetPixels($miplevel: number):System.Array$1<UnityEngine.Color>;
            public GetPixels():System.Array$1<UnityEngine.Color>;
            
        }
        enum TextureFormat { Alpha8 = 1, ARGB4444 = 2, RGB24 = 3, RGBA32 = 4, ARGB32 = 5, RGB565 = 7, R16 = 9, DXT1 = 10, DXT5 = 12, RGBA4444 = 13, BGRA32 = 14, RHalf = 15, RGHalf = 16, RGBAHalf = 17, RFloat = 18, RGFloat = 19, RGBAFloat = 20, YUY2 = 21, RGB9e5Float = 22, BC4 = 26, BC5 = 27, BC6H = 24, BC7 = 25, DXT1Crunched = 28, DXT5Crunched = 29, PVRTC_RGB2 = 30, PVRTC_RGBA2 = 31, PVRTC_RGB4 = 32, PVRTC_RGBA4 = 33, ETC_RGB4 = 34, ATC_RGB4 = -127, ATC_RGBA8 = -127, EAC_R = 41, EAC_R_SIGNED = 42, EAC_RG = 43, EAC_RG_SIGNED = 44, ETC2_RGB = 45, ETC2_RGBA1 = 46, ETC2_RGBA8 = 47, ASTC_RGB_4x4 = 48, ASTC_RGB_5x5 = 49, ASTC_RGB_6x6 = 50, ASTC_RGB_8x8 = 51, ASTC_RGB_10x10 = 52, ASTC_RGB_12x12 = 53, ASTC_RGBA_4x4 = 54, ASTC_RGBA_5x5 = 55, ASTC_RGBA_6x6 = 56, ASTC_RGBA_8x8 = 57, ASTC_RGBA_10x10 = 58, ASTC_RGBA_12x12 = 59, ETC_RGB4_3DS = 60, ETC_RGBA8_3DS = 61, RG16 = 62, R8 = 63, ETC_RGB4Crunched = 64, ETC2_RGBA8Crunched = 65, PVRTC_2BPP_RGB = -127, PVRTC_2BPP_RGBA = -127, PVRTC_4BPP_RGB = -127, PVRTC_4BPP_RGBA = -127 }
        class QualitySettings extends UnityEngine.Object {
            public static pixelLightCount: number;
            public static shadows: UnityEngine.ShadowQuality;
            public static shadowProjection: UnityEngine.ShadowProjection;
            public static shadowCascades: number;
            public static shadowDistance: number;
            public static shadowResolution: UnityEngine.ShadowResolution;
            public static shadowmaskMode: UnityEngine.ShadowmaskMode;
            public static shadowNearPlaneOffset: number;
            public static shadowCascade2Split: number;
            public static shadowCascade4Split: UnityEngine.Vector3;
            public static lodBias: number;
            public static anisotropicFiltering: UnityEngine.AnisotropicFiltering;
            public static masterTextureLimit: number;
            public static maximumLODLevel: number;
            public static particleRaycastBudget: number;
            public static softParticles: boolean;
            public static softVegetation: boolean;
            public static vSyncCount: number;
            public static antiAliasing: number;
            public static asyncUploadTimeSlice: number;
            public static asyncUploadBufferSize: number;
            public static asyncUploadPersistentBuffer: boolean;
            public static realtimeReflectionProbes: boolean;
            public static billboardsFaceCameraPosition: boolean;
            public static resolutionScalingFixedDPIFactor: number;
            public static blendWeights: UnityEngine.BlendWeights;
            public static streamingMipmapsActive: boolean;
            public static streamingMipmapsMemoryBudget: number;
            public static streamingMipmapsRenderersPerFrame: number;
            public static streamingMipmapsMaxLevelReduction: number;
            public static streamingMipmapsAddAllCameras: boolean;
            public static streamingMipmapsMaxFileIORequests: number;
            public static maxQueuedFrames: number;
            public static names: System.Array$1<string>;
            public static desiredColorSpace: UnityEngine.ColorSpace;
            public static activeColorSpace: UnityEngine.ColorSpace;
            public static IncreaseLevel($applyExpensiveChanges: boolean):void;
            public static DecreaseLevel($applyExpensiveChanges: boolean):void;
            public static SetQualityLevel($index: number):void;
            public static IncreaseLevel():void;
            public static DecreaseLevel():void;
            public static GetQualityLevel():number;
            public static SetQualityLevel($index: number, $applyExpensiveChanges: boolean):void;
            
        }
        enum QualityLevel { Fastest = 0, Fast = 1, Simple = 2, Good = 3, Beautiful = 4, Fantastic = 5 }
        enum ShadowQuality { Disable = 0, HardOnly = 1, All = 2 }
        enum ShadowProjection { CloseFit = 0, StableFit = 1 }
        enum ShadowResolution { Low = 0, Medium = 1, High = 2, VeryHigh = 3 }
        enum ShadowmaskMode { Shadowmask = 0, DistanceShadowmask = 1 }
        enum BlendWeights { OneBone = 1, TwoBones = 2, FourBones = 4 }
        enum ColorSpace { Uninitialized = -1, Gamma = 0, Linear = 1 }
        class MeshFilter extends UnityEngine.Component {
            public sharedMesh: UnityEngine.Mesh;
            public mesh: UnityEngine.Mesh;
            public constructor();
            
        }
        class Input extends System.Object {
            public static simulateMouseWithTouches: boolean;
            public static anyKey: boolean;
            public static anyKeyDown: boolean;
            public static inputString: string;
            public static mousePosition: UnityEngine.Vector3;
            public static mouseScrollDelta: UnityEngine.Vector2;
            public static imeCompositionMode: UnityEngine.IMECompositionMode;
            public static compositionString: string;
            public static imeIsSelected: boolean;
            public static compositionCursorPos: UnityEngine.Vector2;
            public static mousePresent: boolean;
            public static touchCount: number;
            public static touchPressureSupported: boolean;
            public static stylusTouchSupported: boolean;
            public static touchSupported: boolean;
            public static multiTouchEnabled: boolean;
            public static deviceOrientation: UnityEngine.DeviceOrientation;
            public static acceleration: UnityEngine.Vector3;
            public static compensateSensors: boolean;
            public static accelerationEventCount: number;
            public static backButtonLeavesApp: boolean;
            public static location: UnityEngine.LocationService;
            public static compass: UnityEngine.Compass;
            public static gyro: UnityEngine.Gyroscope;
            public static touches: System.Array$1<UnityEngine.Touch>;
            public static accelerationEvents: System.Array$1<UnityEngine.AccelerationEvent>;
            public constructor();
            public static GetAxis($axisName: string):number;
            public static GetAxisRaw($axisName: string):number;
            public static GetButton($buttonName: string):boolean;
            public static GetButtonDown($buttonName: string):boolean;
            public static GetButtonUp($buttonName: string):boolean;
            public static GetMouseButton($button: number):boolean;
            public static GetMouseButtonDown($button: number):boolean;
            public static GetMouseButtonUp($button: number):boolean;
            public static ResetInputAxes():void;
            public static IsJoystickPreconfigured($joystickName: string):boolean;
            public static GetJoystickNames():System.Array$1<string>;
            public static GetTouch($index: number):UnityEngine.Touch;
            public static GetAccelerationEvent($index: number):UnityEngine.AccelerationEvent;
            public static GetKey($key: UnityEngine.KeyCode):boolean;
            public static GetKey($name: string):boolean;
            public static GetKeyUp($key: UnityEngine.KeyCode):boolean;
            public static GetKeyUp($name: string):boolean;
            public static GetKeyDown($key: UnityEngine.KeyCode):boolean;
            public static GetKeyDown($name: string):boolean;
            
        }
        class AccelerationEvent extends System.ValueType {
            
        }
        enum KeyCode { None = 0, Backspace = 8, Delete = 127, Tab = 9, Clear = 12, Return = 13, Pause = 19, Escape = 27, Space = 32, Keypad0 = 256, Keypad1 = 257, Keypad2 = 258, Keypad3 = 259, Keypad4 = 260, Keypad5 = 261, Keypad6 = 262, Keypad7 = 263, Keypad8 = 264, Keypad9 = 265, KeypadPeriod = 266, KeypadDivide = 267, KeypadMultiply = 268, KeypadMinus = 269, KeypadPlus = 270, KeypadEnter = 271, KeypadEquals = 272, UpArrow = 273, DownArrow = 274, RightArrow = 275, LeftArrow = 276, Insert = 277, Home = 278, End = 279, PageUp = 280, PageDown = 281, F1 = 282, F2 = 283, F3 = 284, F4 = 285, F5 = 286, F6 = 287, F7 = 288, F8 = 289, F9 = 290, F10 = 291, F11 = 292, F12 = 293, F13 = 294, F14 = 295, F15 = 296, Alpha0 = 48, Alpha1 = 49, Alpha2 = 50, Alpha3 = 51, Alpha4 = 52, Alpha5 = 53, Alpha6 = 54, Alpha7 = 55, Alpha8 = 56, Alpha9 = 57, Exclaim = 33, DoubleQuote = 34, Hash = 35, Dollar = 36, Percent = 37, Ampersand = 38, Quote = 39, LeftParen = 40, RightParen = 41, Asterisk = 42, Plus = 43, Comma = 44, Minus = 45, Period = 46, Slash = 47, Colon = 58, Semicolon = 59, Less = 60, Equals = 61, Greater = 62, Question = 63, At = 64, LeftBracket = 91, Backslash = 92, RightBracket = 93, Caret = 94, Underscore = 95, BackQuote = 96, A = 97, B = 98, C = 99, D = 100, E = 101, F = 102, G = 103, H = 104, I = 105, J = 106, K = 107, L = 108, M = 109, N = 110, O = 111, P = 112, Q = 113, R = 114, S = 115, T = 116, U = 117, V = 118, W = 119, X = 120, Y = 121, Z = 122, LeftCurlyBracket = 123, Pipe = 124, RightCurlyBracket = 125, Tilde = 126, Numlock = 300, CapsLock = 301, ScrollLock = 302, RightShift = 303, LeftShift = 304, RightControl = 305, LeftControl = 306, RightAlt = 307, LeftAlt = 308, LeftCommand = 310, LeftApple = 310, LeftWindows = 311, RightCommand = 309, RightApple = 309, RightWindows = 312, AltGr = 313, Help = 315, Print = 316, SysReq = 317, Break = 318, Menu = 319, Mouse0 = 323, Mouse1 = 324, Mouse2 = 325, Mouse3 = 326, Mouse4 = 327, Mouse5 = 328, Mouse6 = 329, JoystickButton0 = 330, JoystickButton1 = 331, JoystickButton2 = 332, JoystickButton3 = 333, JoystickButton4 = 334, JoystickButton5 = 335, JoystickButton6 = 336, JoystickButton7 = 337, JoystickButton8 = 338, JoystickButton9 = 339, JoystickButton10 = 340, JoystickButton11 = 341, JoystickButton12 = 342, JoystickButton13 = 343, JoystickButton14 = 344, JoystickButton15 = 345, JoystickButton16 = 346, JoystickButton17 = 347, JoystickButton18 = 348, JoystickButton19 = 349, Joystick1Button0 = 350, Joystick1Button1 = 351, Joystick1Button2 = 352, Joystick1Button3 = 353, Joystick1Button4 = 354, Joystick1Button5 = 355, Joystick1Button6 = 356, Joystick1Button7 = 357, Joystick1Button8 = 358, Joystick1Button9 = 359, Joystick1Button10 = 360, Joystick1Button11 = 361, Joystick1Button12 = 362, Joystick1Button13 = 363, Joystick1Button14 = 364, Joystick1Button15 = 365, Joystick1Button16 = 366, Joystick1Button17 = 367, Joystick1Button18 = 368, Joystick1Button19 = 369, Joystick2Button0 = 370, Joystick2Button1 = 371, Joystick2Button2 = 372, Joystick2Button3 = 373, Joystick2Button4 = 374, Joystick2Button5 = 375, Joystick2Button6 = 376, Joystick2Button7 = 377, Joystick2Button8 = 378, Joystick2Button9 = 379, Joystick2Button10 = 380, Joystick2Button11 = 381, Joystick2Button12 = 382, Joystick2Button13 = 383, Joystick2Button14 = 384, Joystick2Button15 = 385, Joystick2Button16 = 386, Joystick2Button17 = 387, Joystick2Button18 = 388, Joystick2Button19 = 389, Joystick3Button0 = 390, Joystick3Button1 = 391, Joystick3Button2 = 392, Joystick3Button3 = 393, Joystick3Button4 = 394, Joystick3Button5 = 395, Joystick3Button6 = 396, Joystick3Button7 = 397, Joystick3Button8 = 398, Joystick3Button9 = 399, Joystick3Button10 = 400, Joystick3Button11 = 401, Joystick3Button12 = 402, Joystick3Button13 = 403, Joystick3Button14 = 404, Joystick3Button15 = 405, Joystick3Button16 = 406, Joystick3Button17 = 407, Joystick3Button18 = 408, Joystick3Button19 = 409, Joystick4Button0 = 410, Joystick4Button1 = 411, Joystick4Button2 = 412, Joystick4Button3 = 413, Joystick4Button4 = 414, Joystick4Button5 = 415, Joystick4Button6 = 416, Joystick4Button7 = 417, Joystick4Button8 = 418, Joystick4Button9 = 419, Joystick4Button10 = 420, Joystick4Button11 = 421, Joystick4Button12 = 422, Joystick4Button13 = 423, Joystick4Button14 = 424, Joystick4Button15 = 425, Joystick4Button16 = 426, Joystick4Button17 = 427, Joystick4Button18 = 428, Joystick4Button19 = 429, Joystick5Button0 = 430, Joystick5Button1 = 431, Joystick5Button2 = 432, Joystick5Button3 = 433, Joystick5Button4 = 434, Joystick5Button5 = 435, Joystick5Button6 = 436, Joystick5Button7 = 437, Joystick5Button8 = 438, Joystick5Button9 = 439, Joystick5Button10 = 440, Joystick5Button11 = 441, Joystick5Button12 = 442, Joystick5Button13 = 443, Joystick5Button14 = 444, Joystick5Button15 = 445, Joystick5Button16 = 446, Joystick5Button17 = 447, Joystick5Button18 = 448, Joystick5Button19 = 449, Joystick6Button0 = 450, Joystick6Button1 = 451, Joystick6Button2 = 452, Joystick6Button3 = 453, Joystick6Button4 = 454, Joystick6Button5 = 455, Joystick6Button6 = 456, Joystick6Button7 = 457, Joystick6Button8 = 458, Joystick6Button9 = 459, Joystick6Button10 = 460, Joystick6Button11 = 461, Joystick6Button12 = 462, Joystick6Button13 = 463, Joystick6Button14 = 464, Joystick6Button15 = 465, Joystick6Button16 = 466, Joystick6Button17 = 467, Joystick6Button18 = 468, Joystick6Button19 = 469, Joystick7Button0 = 470, Joystick7Button1 = 471, Joystick7Button2 = 472, Joystick7Button3 = 473, Joystick7Button4 = 474, Joystick7Button5 = 475, Joystick7Button6 = 476, Joystick7Button7 = 477, Joystick7Button8 = 478, Joystick7Button9 = 479, Joystick7Button10 = 480, Joystick7Button11 = 481, Joystick7Button12 = 482, Joystick7Button13 = 483, Joystick7Button14 = 484, Joystick7Button15 = 485, Joystick7Button16 = 486, Joystick7Button17 = 487, Joystick7Button18 = 488, Joystick7Button19 = 489, Joystick8Button0 = 490, Joystick8Button1 = 491, Joystick8Button2 = 492, Joystick8Button3 = 493, Joystick8Button4 = 494, Joystick8Button5 = 495, Joystick8Button6 = 496, Joystick8Button7 = 497, Joystick8Button8 = 498, Joystick8Button9 = 499, Joystick8Button10 = 500, Joystick8Button11 = 501, Joystick8Button12 = 502, Joystick8Button13 = 503, Joystick8Button14 = 504, Joystick8Button15 = 505, Joystick8Button16 = 506, Joystick8Button17 = 507, Joystick8Button18 = 508, Joystick8Button19 = 509 }
        enum IMECompositionMode { Auto = 0, On = 1, Off = 2 }
        enum DeviceOrientation { Unknown = 0, Portrait = 1, PortraitUpsideDown = 2, LandscapeLeft = 3, LandscapeRight = 4, FaceUp = 5, FaceDown = 6 }
        class LocationService extends System.Object {
            
        }
        class Compass extends System.Object {
            
        }
        class Gyroscope extends System.Object {
            
        }
        class LayerMask extends System.ValueType {
            public value: number;
            public static op_Implicit($mask: UnityEngine.LayerMask):number;
            public static op_Implicit($intVal: number):UnityEngine.LayerMask;
            public static LayerToName($layer: number):string;
            public static NameToLayer($layerName: string):number;
            public static GetMask(...layerNames: string[]):number;
            
        }
        class Mathf extends System.ValueType {
            public static PI: number;
            public static Infinity: number;
            public static NegativeInfinity: number;
            public static Deg2Rad: number;
            public static Rad2Deg: number;
            public static Epsilon: number;
            public static ClosestPowerOfTwo($value: number):number;
            public static IsPowerOfTwo($value: number):boolean;
            public static NextPowerOfTwo($value: number):number;
            public static GammaToLinearSpace($value: number):number;
            public static LinearToGammaSpace($value: number):number;
            public static CorrelatedColorTemperatureToRGB($kelvin: number):UnityEngine.Color;
            public static FloatToHalf($val: number):number;
            public static HalfToFloat($val: number):number;
            public static PerlinNoise($x: number, $y: number):number;
            public static Sin($f: number):number;
            public static Cos($f: number):number;
            public static Tan($f: number):number;
            public static Asin($f: number):number;
            public static Acos($f: number):number;
            public static Atan($f: number):number;
            public static Atan2($y: number, $x: number):number;
            public static Sqrt($f: number):number;
            public static Abs($f: number):number;
            public static Abs($value: number):number;
            public static Min($a: number, $b: number):number;
            public static Min(...values: number[]):number;
            public static Min($a: number, $b: number):number;
            public static Min(...values: number[]):number;
            public static Max($a: number, $b: number):number;
            public static Max(...values: number[]):number;
            public static Max($a: number, $b: number):number;
            public static Max(...values: number[]):number;
            public static Pow($f: number, $p: number):number;
            public static Exp($power: number):number;
            public static Log($f: number, $p: number):number;
            public static Log($f: number):number;
            public static Log10($f: number):number;
            public static Ceil($f: number):number;
            public static Floor($f: number):number;
            public static Round($f: number):number;
            public static CeilToInt($f: number):number;
            public static FloorToInt($f: number):number;
            public static RoundToInt($f: number):number;
            public static Sign($f: number):number;
            public static Clamp($value: number, $min: number, $max: number):number;
            public static Clamp($value: number, $min: number, $max: number):number;
            public static Clamp01($value: number):number;
            public static Lerp($a: number, $b: number, $t: number):number;
            public static LerpUnclamped($a: number, $b: number, $t: number):number;
            public static LerpAngle($a: number, $b: number, $t: number):number;
            public static MoveTowards($current: number, $target: number, $maxDelta: number):number;
            public static MoveTowardsAngle($current: number, $target: number, $maxDelta: number):number;
            public static SmoothStep($from: number, $to: number, $t: number):number;
            public static Gamma($value: number, $absmax: number, $gamma: number):number;
            public static Approximately($a: number, $b: number):boolean;
            public static SmoothDamp($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number):number;
            public static SmoothDamp($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number):number;
            public static SmoothDamp($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number, $deltaTime: number):number;
            public static SmoothDampAngle($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number):number;
            public static SmoothDampAngle($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number):number;
            public static SmoothDampAngle($current: number, $target: number, $currentVelocity: $Ref<number>, $smoothTime: number, $maxSpeed: number, $deltaTime: number):number;
            public static Repeat($t: number, $length: number):number;
            public static PingPong($t: number, $length: number):number;
            public static InverseLerp($a: number, $b: number, $value: number):number;
            public static DeltaAngle($current: number, $target: number):number;
            
        }
        class MonoBehaviour extends UnityEngine.Behaviour {
            public useGUILayout: boolean;
            public runInEditMode: boolean;
            public constructor();
            public IsInvoking():boolean;
            public CancelInvoke():void;
            public Invoke($methodName: string, $time: number):void;
            public InvokeRepeating($methodName: string, $time: number, $repeatRate: number):void;
            public CancelInvoke($methodName: string):void;
            public IsInvoking($methodName: string):boolean;
            public StartCoroutine($methodName: string):UnityEngine.Coroutine;
            public StartCoroutine($methodName: string, $value: any):UnityEngine.Coroutine;
            public StartCoroutine($routine: System.Collections.IEnumerator):UnityEngine.Coroutine;
            public StopCoroutine($routine: System.Collections.IEnumerator):void;
            public StopCoroutine($routine: UnityEngine.Coroutine):void;
            public StopCoroutine($methodName: string):void;
            public StopAllCoroutines():void;
            public static print($message: any):void;
            
        }
        class Coroutine extends UnityEngine.YieldInstruction {
            
        }
        class PlayerPrefs extends System.Object {
            public constructor();
            public static SetInt($key: string, $value: number):void;
            public static GetInt($key: string, $defaultValue: number):number;
            public static GetInt($key: string):number;
            public static SetFloat($key: string, $value: number):void;
            public static GetFloat($key: string, $defaultValue: number):number;
            public static GetFloat($key: string):number;
            public static SetString($key: string, $value: string):void;
            public static GetString($key: string, $defaultValue: string):string;
            public static GetString($key: string):string;
            public static HasKey($key: string):boolean;
            public static DeleteKey($key: string):void;
            public static DeleteAll():void;
            public static Save():void;
            
        }
        class Random extends System.Object {
            public static state: UnityEngine.Random.State;
            public static value: number;
            public static insideUnitSphere: UnityEngine.Vector3;
            public static insideUnitCircle: UnityEngine.Vector2;
            public static onUnitSphere: UnityEngine.Vector3;
            public static rotation: UnityEngine.Quaternion;
            public static rotationUniform: UnityEngine.Quaternion;
            public constructor();
            public static InitState($seed: number):void;
            public static Range($min: number, $max: number):number;
            public static Range($min: number, $max: number):number;
            public static ColorHSV():UnityEngine.Color;
            public static ColorHSV($hueMin: number, $hueMax: number):UnityEngine.Color;
            public static ColorHSV($hueMin: number, $hueMax: number, $saturationMin: number, $saturationMax: number):UnityEngine.Color;
            public static ColorHSV($hueMin: number, $hueMax: number, $saturationMin: number, $saturationMax: number, $valueMin: number, $valueMax: number):UnityEngine.Color;
            public static ColorHSV($hueMin: number, $hueMax: number, $saturationMin: number, $saturationMax: number, $valueMin: number, $valueMax: number, $alphaMin: number, $alphaMax: number):UnityEngine.Color;
            
        }
        class Resources extends System.Object {
            public constructor();
            public static FindObjectsOfTypeAll($type: System.Type):System.Array$1<UnityEngine.Object>;
            public static Load($path: string):UnityEngine.Object;
            public static Load($path: string, $systemTypeInstance: System.Type):UnityEngine.Object;
            public static LoadAsync($path: string):UnityEngine.ResourceRequest;
            public static LoadAsync($path: string, $type: System.Type):UnityEngine.ResourceRequest;
            public static LoadAll($path: string, $systemTypeInstance: System.Type):System.Array$1<UnityEngine.Object>;
            public static LoadAll($path: string):System.Array$1<UnityEngine.Object>;
            public static GetBuiltinResource($type: System.Type, $path: string):UnityEngine.Object;
            public static UnloadAsset($assetToUnload: UnityEngine.Object):void;
            public static UnloadUnusedAssets():UnityEngine.AsyncOperation;
            
        }
        class ResourceRequest extends UnityEngine.AsyncOperation {
            
        }
        class SystemInfo extends System.Object {
            public static unsupportedIdentifier: string;
            public static batteryLevel: number;
            public static batteryStatus: UnityEngine.BatteryStatus;
            public static operatingSystem: string;
            public static operatingSystemFamily: UnityEngine.OperatingSystemFamily;
            public static processorType: string;
            public static processorFrequency: number;
            public static processorCount: number;
            public static systemMemorySize: number;
            public static deviceUniqueIdentifier: string;
            public static deviceName: string;
            public static deviceModel: string;
            public static supportsAccelerometer: boolean;
            public static supportsGyroscope: boolean;
            public static supportsLocationService: boolean;
            public static supportsVibration: boolean;
            public static supportsAudio: boolean;
            public static deviceType: UnityEngine.DeviceType;
            public static graphicsMemorySize: number;
            public static graphicsDeviceName: string;
            public static graphicsDeviceVendor: string;
            public static graphicsDeviceID: number;
            public static graphicsDeviceVendorID: number;
            public static graphicsDeviceType: UnityEngine.Rendering.GraphicsDeviceType;
            public static graphicsUVStartsAtTop: boolean;
            public static graphicsDeviceVersion: string;
            public static graphicsShaderLevel: number;
            public static graphicsMultiThreaded: boolean;
            public static hasHiddenSurfaceRemovalOnGPU: boolean;
            public static hasDynamicUniformArrayIndexingInFragmentShaders: boolean;
            public static supportsShadows: boolean;
            public static supportsRawShadowDepthSampling: boolean;
            public static supportsMotionVectors: boolean;
            public static supportsRenderToCubemap: boolean;
            public static supportsImageEffects: boolean;
            public static supports3DTextures: boolean;
            public static supports2DArrayTextures: boolean;
            public static supports3DRenderTextures: boolean;
            public static supportsCubemapArrayTextures: boolean;
            public static copyTextureSupport: UnityEngine.Rendering.CopyTextureSupport;
            public static supportsComputeShaders: boolean;
            public static supportsInstancing: boolean;
            public static supportsHardwareQuadTopology: boolean;
            public static supports32bitsIndexBuffer: boolean;
            public static supportsSparseTextures: boolean;
            public static supportedRenderTargetCount: number;
            public static supportsSeparatedRenderTargetsBlend: boolean;
            public static supportsMultisampledTextures: number;
            public static supportsMultisampleAutoResolve: boolean;
            public static supportsTextureWrapMirrorOnce: number;
            public static usesReversedZBuffer: boolean;
            public static npotSupport: UnityEngine.NPOTSupport;
            public static maxTextureSize: number;
            public static maxCubemapSize: number;
            public static supportsAsyncCompute: boolean;
            public static supportsGPUFence: boolean;
            public static supportsAsyncGPUReadback: boolean;
            public static supportsMipStreaming: boolean;
            public constructor();
            public static SupportsRenderTextureFormat($format: UnityEngine.RenderTextureFormat):boolean;
            public static SupportsBlendingOnRenderTextureFormat($format: UnityEngine.RenderTextureFormat):boolean;
            public static SupportsTextureFormat($format: UnityEngine.TextureFormat):boolean;
            public static IsFormatSupported($format: UnityEngine.Experimental.Rendering.GraphicsFormat, $usage: UnityEngine.Experimental.Rendering.FormatUsage):boolean;
            
        }
        enum BatteryStatus { Unknown = 0, Charging = 1, Discharging = 2, NotCharging = 3, Full = 4 }
        enum OperatingSystemFamily { Other = 0, MacOSX = 1, Windows = 2, Linux = 3 }
        enum DeviceType { Unknown = 0, Handheld = 1, Console = 2, Desktop = 3 }
        enum NPOTSupport { None = 0, Restricted = 1, Full = 2 }
        class TextAsset extends UnityEngine.Object {
            public text: string;
            public bytes: System.Array$1<number>;
            public constructor();
            public constructor($text: string);
            
        }
        class Texture3D extends UnityEngine.Texture {
            public depth: number;
            public format: UnityEngine.TextureFormat;
            public isReadable: boolean;
            public constructor($width: number, $height: number, $depth: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat, $flags: UnityEngine.Experimental.Rendering.TextureCreationFlags);
            public constructor($width: number, $height: number, $depth: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean);
            public GetPixels($miplevel: number):System.Array$1<UnityEngine.Color>;
            public GetPixels():System.Array$1<UnityEngine.Color>;
            public GetPixels32($miplevel: number):System.Array$1<UnityEngine.Color32>;
            public GetPixels32():System.Array$1<UnityEngine.Color32>;
            public SetPixels($colors: System.Array$1<UnityEngine.Color>, $miplevel: number):void;
            public SetPixels($colors: System.Array$1<UnityEngine.Color>):void;
            public SetPixels32($colors: System.Array$1<UnityEngine.Color32>, $miplevel: number):void;
            public SetPixels32($colors: System.Array$1<UnityEngine.Color32>):void;
            public Apply($updateMipmaps: boolean, $makeNoLongerReadable: boolean):void;
            public Apply($updateMipmaps: boolean):void;
            public Apply():void;
            
        }
        class Texture2DArray extends UnityEngine.Texture {
            public depth: number;
            public format: UnityEngine.TextureFormat;
            public isReadable: boolean;
            public constructor($width: number, $height: number, $depth: number, $format: UnityEngine.Experimental.Rendering.GraphicsFormat, $flags: UnityEngine.Experimental.Rendering.TextureCreationFlags);
            public constructor($width: number, $height: number, $depth: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean, $linear: boolean);
            public constructor($width: number, $height: number, $depth: number, $textureFormat: UnityEngine.TextureFormat, $mipChain: boolean);
            public GetPixels($arrayElement: number, $miplevel: number):System.Array$1<UnityEngine.Color>;
            public GetPixels($arrayElement: number):System.Array$1<UnityEngine.Color>;
            public GetPixels32($arrayElement: number, $miplevel: number):System.Array$1<UnityEngine.Color32>;
            public GetPixels32($arrayElement: number):System.Array$1<UnityEngine.Color32>;
            public SetPixels($colors: System.Array$1<UnityEngine.Color>, $arrayElement: number, $miplevel: number):void;
            public SetPixels($colors: System.Array$1<UnityEngine.Color>, $arrayElement: number):void;
            public SetPixels32($colors: System.Array$1<UnityEngine.Color32>, $arrayElement: number, $miplevel: number):void;
            public SetPixels32($colors: System.Array$1<UnityEngine.Color32>, $arrayElement: number):void;
            public Apply($updateMipmaps: boolean, $makeNoLongerReadable: boolean):void;
            public Apply($updateMipmaps: boolean):void;
            public Apply():void;
            
        }
        class Time extends System.Object {
            public static time: number;
            public static timeSinceLevelLoad: number;
            public static deltaTime: number;
            public static fixedTime: number;
            public static unscaledTime: number;
            public static fixedUnscaledTime: number;
            public static unscaledDeltaTime: number;
            public static fixedUnscaledDeltaTime: number;
            public static fixedDeltaTime: number;
            public static maximumDeltaTime: number;
            public static smoothDeltaTime: number;
            public static maximumParticleDeltaTime: number;
            public static timeScale: number;
            public static frameCount: number;
            public static renderedFrameCount: number;
            public static realtimeSinceStartup: number;
            public static captureFramerate: number;
            public static inFixedTimeStep: boolean;
            public constructor();
            
        }
        class Font extends UnityEngine.Object {
            public material: UnityEngine.Material;
            public fontNames: System.Array$1<string>;
            public dynamic: boolean;
            public ascent: number;
            public fontSize: number;
            public characterInfo: System.Array$1<UnityEngine.CharacterInfo>;
            public lineHeight: number;
            public constructor();
            public constructor($name: string);
            public static add_textureRebuilt($value: System.Action$1<UnityEngine.Font>):void;
            public static remove_textureRebuilt($value: System.Action$1<UnityEngine.Font>):void;
            public static CreateDynamicFontFromOSFont($fontname: string, $size: number):UnityEngine.Font;
            public static CreateDynamicFontFromOSFont($fontnames: System.Array$1<string>, $size: number):UnityEngine.Font;
            public static GetMaxVertsForString($str: string):number;
            public HasCharacter($c: number):boolean;
            public static GetOSInstalledFontNames():System.Array$1<string>;
            public static GetPathsToOSFonts():System.Array$1<string>;
            public GetCharacterInfo($ch: number, $info: $Ref<UnityEngine.CharacterInfo>, $size: number, $style: UnityEngine.FontStyle):boolean;
            public GetCharacterInfo($ch: number, $info: $Ref<UnityEngine.CharacterInfo>, $size: number):boolean;
            public GetCharacterInfo($ch: number, $info: $Ref<UnityEngine.CharacterInfo>):boolean;
            public RequestCharactersInTexture($characters: string, $size: number, $style: UnityEngine.FontStyle):void;
            public RequestCharactersInTexture($characters: string, $size: number):void;
            public RequestCharactersInTexture($characters: string):void;
            
        }
        class CharacterInfo extends System.ValueType {
            
        }
        enum FontStyle { Normal = 0, Bold = 1, Italic = 2, BoldAndItalic = 3 }
        class ParticleSystem extends UnityEngine.Component {
            public isPlaying: boolean;
            public isEmitting: boolean;
            public isStopped: boolean;
            public isPaused: boolean;
            public particleCount: number;
            public time: number;
            public randomSeed: number;
            public useAutoRandomSeed: boolean;
            public proceduralSimulationSupported: boolean;
            public main: UnityEngine.ParticleSystem.MainModule;
            public emission: UnityEngine.ParticleSystem.EmissionModule;
            public shape: UnityEngine.ParticleSystem.ShapeModule;
            public velocityOverLifetime: UnityEngine.ParticleSystem.VelocityOverLifetimeModule;
            public limitVelocityOverLifetime: UnityEngine.ParticleSystem.LimitVelocityOverLifetimeModule;
            public inheritVelocity: UnityEngine.ParticleSystem.InheritVelocityModule;
            public forceOverLifetime: UnityEngine.ParticleSystem.ForceOverLifetimeModule;
            public colorOverLifetime: UnityEngine.ParticleSystem.ColorOverLifetimeModule;
            public colorBySpeed: UnityEngine.ParticleSystem.ColorBySpeedModule;
            public sizeOverLifetime: UnityEngine.ParticleSystem.SizeOverLifetimeModule;
            public sizeBySpeed: UnityEngine.ParticleSystem.SizeBySpeedModule;
            public rotationOverLifetime: UnityEngine.ParticleSystem.RotationOverLifetimeModule;
            public rotationBySpeed: UnityEngine.ParticleSystem.RotationBySpeedModule;
            public externalForces: UnityEngine.ParticleSystem.ExternalForcesModule;
            public noise: UnityEngine.ParticleSystem.NoiseModule;
            public collision: UnityEngine.ParticleSystem.CollisionModule;
            public trigger: UnityEngine.ParticleSystem.TriggerModule;
            public subEmitters: UnityEngine.ParticleSystem.SubEmittersModule;
            public textureSheetAnimation: UnityEngine.ParticleSystem.TextureSheetAnimationModule;
            public lights: UnityEngine.ParticleSystem.LightsModule;
            public trails: UnityEngine.ParticleSystem.TrailModule;
            public customData: UnityEngine.ParticleSystem.CustomDataModule;
            public constructor();
            public SetCustomParticleData($customData: System.Collections.Generic.List$1<UnityEngine.Vector4>, $streamIndex: UnityEngine.ParticleSystemCustomData):void;
            public GetCustomParticleData($customData: System.Collections.Generic.List$1<UnityEngine.Vector4>, $streamIndex: UnityEngine.ParticleSystemCustomData):number;
            public TriggerSubEmitter($subEmitterIndex: number):void;
            public TriggerSubEmitter($subEmitterIndex: number, $particle: $Ref<UnityEngine.ParticleSystem.Particle>):void;
            public TriggerSubEmitter($subEmitterIndex: number, $particles: System.Collections.Generic.List$1<UnityEngine.ParticleSystem.Particle>):void;
            public SetParticles($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number):void;
            public SetParticles($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number):void;
            public SetParticles($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>):void;
            public SetParticles($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number):void;
            public SetParticles($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number):void;
            public SetParticles($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>):void;
            public GetParticles($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number):number;
            public GetParticles($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>, $size: number):number;
            public GetParticles($particles: System.Array$1<UnityEngine.ParticleSystem.Particle>):number;
            public GetParticles($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number, $offset: number):number;
            public GetParticles($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>, $size: number):number;
            public GetParticles($particles: Unity.Collections.NativeArray$1<UnityEngine.ParticleSystem.Particle>):number;
            public Simulate($t: number, $withChildren: boolean, $restart: boolean, $fixedTimeStep: boolean):void;
            public Simulate($t: number, $withChildren: boolean, $restart: boolean):void;
            public Simulate($t: number, $withChildren: boolean):void;
            public Simulate($t: number):void;
            public Play($withChildren: boolean):void;
            public Play():void;
            public Pause($withChildren: boolean):void;
            public Pause():void;
            public Stop($withChildren: boolean, $stopBehavior: UnityEngine.ParticleSystemStopBehavior):void;
            public Stop($withChildren: boolean):void;
            public Stop():void;
            public Clear($withChildren: boolean):void;
            public Clear():void;
            public IsAlive($withChildren: boolean):boolean;
            public IsAlive():boolean;
            public Emit($count: number):void;
            public Emit($emitParams: UnityEngine.ParticleSystem.EmitParams, $count: number):void;
            public static ResetPreMappedBufferMemory():void;
            
        }
        enum ParticleSystemCustomData { Custom1 = 0, Custom2 = 1 }
        enum ParticleSystemSimulationSpace { Local = 0, World = 1, Custom = 2 }
        enum ParticleSystemScalingMode { Hierarchy = 0, Local = 1, Shape = 2 }
        enum ParticleSystemStopBehavior { StopEmittingAndClear = 0, StopEmitting = 1 }
        enum RenderMode { ScreenSpaceOverlay = 0, ScreenSpaceCamera = 1, WorldSpace = 2 }
        class Collider extends UnityEngine.Component {
            
        }
        class RaycastHit extends System.ValueType {
            
        }
        class MeshCollider extends UnityEngine.Collider {
            
        }
        class MeshRenderer extends UnityEngine.Renderer {
            
        }
        class Renderer extends UnityEngine.Component {
            
        }
        class Sprite extends UnityEngine.Object {
            
        }
        enum EventModifiers { None = 0, Shift = 1, Control = 2, Alt = 4, Command = 8, Numeric = 16, CapsLock = 32, FunctionKey = 64 }
        
    }
    namespace UnityEngine.SceneManagement {
        class Scene extends System.ValueType {
            
        }
        
    }
    namespace UnityEngine.Playables {
        class PlayableGraph extends System.ValueType {
            
        }
        
    }
    namespace UnityEngine.AudioClip {
        type PCMReaderCallback = (data: System.Array$1<number>) => void;
        var PCMReaderCallback: {new (func: (data: System.Array$1<number>) => void): PCMReaderCallback;}
        type PCMSetPositionCallback = (position: number) => void;
        var PCMSetPositionCallback: {new (func: (position: number) => void): PCMSetPositionCallback;}
        
    }
    namespace UnityEngine.Audio {
        class AudioMixerGroup extends UnityEngine.Object {
            
        }
        
    }
    namespace UnityEngine.Rendering {
        enum TextureDimension { Unknown = -1, None = 0, Any = 1, Tex2D = 2, Tex3D = 3, Cube = 4, Tex2DArray = 5, CubeArray = 6 }
        enum OpaqueSortMode { Default = 0, FrontToBack = 1, NoDistanceSort = 2 }
        enum CameraEvent { BeforeDepthTexture = 0, AfterDepthTexture = 1, BeforeDepthNormalsTexture = 2, AfterDepthNormalsTexture = 3, BeforeGBuffer = 4, AfterGBuffer = 5, BeforeLighting = 6, AfterLighting = 7, BeforeFinalPass = 8, AfterFinalPass = 9, BeforeForwardOpaque = 10, AfterForwardOpaque = 11, BeforeImageEffectsOpaque = 12, AfterImageEffectsOpaque = 13, BeforeSkybox = 14, AfterSkybox = 15, BeforeForwardAlpha = 16, AfterForwardAlpha = 17, BeforeImageEffects = 18, AfterImageEffects = 19, AfterEverything = 20, BeforeReflections = 21, AfterReflections = 22, BeforeHaloAndLensFlares = 23, AfterHaloAndLensFlares = 24 }
        class CommandBuffer extends System.Object {
            
        }
        enum ComputeQueueType { Default = 0, Background = 1, Urgent = 2 }
        enum ShaderHardwareTier { Tier1 = 0, Tier2 = 1, Tier3 = 2 }
        enum GraphicsTier { Tier1 = 0, Tier2 = 1, Tier3 = 2 }
        class GPUFence extends System.ValueType {
            
        }
        enum SynchronisationStage { VertexProcessing = 0, PixelProcessing = 1 }
        enum ShadowCastingMode { Off = 0, On = 1, TwoSided = 2, ShadowsOnly = 3 }
        enum LightProbeUsage { Off = 0, BlendProbes = 1, UseProxyVolume = 2, CustomProvided = 4 }
        enum IndexFormat { UInt16 = 0, UInt32 = 1 }
        enum GraphicsDeviceType { OpenGL2 = 0, Direct3D9 = 1, Direct3D11 = 2, PlayStation3 = 3, Null = 4, Xbox360 = 6, OpenGLES2 = 8, OpenGLES3 = 11, PlayStationVita = 12, PlayStation4 = 13, XboxOne = 14, PlayStationMobile = 15, Metal = 16, OpenGLCore = 17, Direct3D12 = 18, N3DS = 19, Vulkan = 21, Switch = 22, XboxOneD3D12 = 23 }
        enum CopyTextureSupport { None = 0, Basic = 1, Copy3D = 2, DifferentTypes = 4, TextureToRT = 8, RTToTexture = 16 }
        enum BlendMode { Zero = 0, One = 1, DstColor = 2, SrcColor = 3, OneMinusDstColor = 4, SrcAlpha = 5, OneMinusSrcColor = 6, DstAlpha = 7, OneMinusDstAlpha = 8, SrcAlphaSaturate = 9, OneMinusSrcAlpha = 10 }
        
    }
    namespace UnityEngine.Application {
        type AdvertisingIdentifierCallback = (advertisingId: string, trackingEnabled: boolean, errorMsg: string) => void;
        var AdvertisingIdentifierCallback: {new (func: (advertisingId: string, trackingEnabled: boolean, errorMsg: string) => void): AdvertisingIdentifierCallback;}
        type LowMemoryCallback = () => void;
        var LowMemoryCallback: {new (func: () => void): LowMemoryCallback;}
        type LogCallback = (condition: string, stackTrace: string, type: UnityEngine.LogType) => void;
        var LogCallback: {new (func: (condition: string, stackTrace: string, type: UnityEngine.LogType) => void): LogCallback;}
        
    }
    namespace UnityEngine.Events {
        type UnityAction = () => void;
        var UnityAction: {new (func: () => void): UnityAction;}
        
    }
    namespace UnityEngine.Camera {
        type CameraCallback = (cam: UnityEngine.Camera) => void;
        var CameraCallback: {new (func: (cam: UnityEngine.Camera) => void): CameraCallback;}
        enum GateFitMode { Vertical = 1, Horizontal = 2, Fill = 3, Overscan = 4, None = 0 }
        enum MonoOrStereoscopicEye { Left = 0, Right = 1, Mono = 2 }
        class GateFitParameters extends System.ValueType {
            
        }
        enum StereoscopicEye { Left = 0, Right = 1 }
        
    }
    namespace UnityEngine.Experimental.Rendering {
        enum GraphicsFormat { None = 0, R8_SRGB = 1, R8G8_SRGB = 2, R8G8B8_SRGB = 3, R8G8B8A8_SRGB = 4, R8_UNorm = 5, R8G8_UNorm = 6, R8G8B8_UNorm = 7, R8G8B8A8_UNorm = 8, R8_SNorm = 9, R8G8_SNorm = 10, R8G8B8_SNorm = 11, R8G8B8A8_SNorm = 12, R8_UInt = 13, R8G8_UInt = 14, R8G8B8_UInt = 15, R8G8B8A8_UInt = 16, R8_SInt = 17, R8G8_SInt = 18, R8G8B8_SInt = 19, R8G8B8A8_SInt = 20, R16_UNorm = 21, R16G16_UNorm = 22, R16G16B16_UNorm = 23, R16G16B16A16_UNorm = 24, R16_SNorm = 25, R16G16_SNorm = 26, R16G16B16_SNorm = 27, R16G16B16A16_SNorm = 28, R16_UInt = 29, R16G16_UInt = 30, R16G16B16_UInt = 31, R16G16B16A16_UInt = 32, R16_SInt = 33, R16G16_SInt = 34, R16G16B16_SInt = 35, R16G16B16A16_SInt = 36, R32_UInt = 37, R32G32_UInt = 38, R32G32B32_UInt = 39, R32G32B32A32_UInt = 40, R32_SInt = 41, R32G32_SInt = 42, R32G32B32_SInt = 43, R32G32B32A32_SInt = 44, R16_SFloat = 45, R16G16_SFloat = 46, R16G16B16_SFloat = 47, R16G16B16A16_SFloat = 48, R32_SFloat = 49, R32G32_SFloat = 50, R32G32B32_SFloat = 51, R32G32B32A32_SFloat = 52, B8G8R8_SRGB = 56, B8G8R8A8_SRGB = 57, B8G8R8_UNorm = 58, B8G8R8A8_UNorm = 59, B8G8R8_SNorm = 60, B8G8R8A8_SNorm = 61, B8G8R8_UInt = 62, B8G8R8A8_UInt = 63, B8G8R8_SInt = 64, B8G8R8A8_SInt = 65, R4G4B4A4_UNormPack16 = 66, B4G4R4A4_UNormPack16 = 67, R5G6B5_UNormPack16 = 68, B5G6R5_UNormPack16 = 69, R5G5B5A1_UNormPack16 = 70, B5G5R5A1_UNormPack16 = 71, A1R5G5B5_UNormPack16 = 72, E5B9G9R9_UFloatPack32 = 73, B10G11R11_UFloatPack32 = 74, A2B10G10R10_UNormPack32 = 75, A2B10G10R10_UIntPack32 = 76, A2B10G10R10_SIntPack32 = 77, A2R10G10B10_UNormPack32 = 78, A2R10G10B10_UIntPack32 = 79, A2R10G10B10_SIntPack32 = 80, A2R10G10B10_XRSRGBPack32 = 81, A2R10G10B10_XRUNormPack32 = 82, R10G10B10_XRSRGBPack32 = 83, R10G10B10_XRUNormPack32 = 84, A10R10G10B10_XRSRGBPack32 = 85, A10R10G10B10_XRUNormPack32 = 86, D16_UNorm = 90, D24_UNorm = 91, D24_UNorm_S8_UInt = 92, D32_SFloat = 93, D32_SFloat_S8_Uint = 94, S8_Uint = 95, RGB_DXT1_SRGB = 96, RGBA_DXT1_SRGB = 96, RGB_DXT1_UNorm = 97, RGBA_DXT1_UNorm = 97, RGBA_DXT3_SRGB = 98, RGBA_DXT3_UNorm = 99, RGBA_DXT5_SRGB = 100, RGBA_DXT5_UNorm = 101, R_BC4_UNorm = 102, R_BC4_SNorm = 103, RG_BC5_UNorm = 104, RG_BC5_SNorm = 105, RGB_BC6H_UFloat = 106, RGB_BC6H_SFloat = 107, RGBA_BC7_SRGB = 108, RGBA_BC7_UNorm = 109, RGB_PVRTC_2Bpp_SRGB = 110, RGB_PVRTC_2Bpp_UNorm = 111, RGB_PVRTC_4Bpp_SRGB = 112, RGB_PVRTC_4Bpp_UNorm = 113, RGBA_PVRTC_2Bpp_SRGB = 114, RGBA_PVRTC_2Bpp_UNorm = 115, RGBA_PVRTC_4Bpp_SRGB = 116, RGBA_PVRTC_4Bpp_UNorm = 117, RGB_ETC_UNorm = 118, RGB_ETC2_SRGB = 119, RGB_ETC2_UNorm = 120, RGB_A1_ETC2_SRGB = 121, RGB_A1_ETC2_UNorm = 122, RGBA_ETC2_SRGB = 123, RGBA_ETC2_UNorm = 124, R_EAC_UNorm = 125, R_EAC_SNorm = 126, RG_EAC_UNorm = 127, RG_EAC_SNorm = 128, RGBA_ASTC4X4_SRGB = 129, RGBA_ASTC4X4_UNorm = 130, RGBA_ASTC5X5_SRGB = 131, RGBA_ASTC5X5_UNorm = 132, RGBA_ASTC6X6_SRGB = 133, RGBA_ASTC6X6_UNorm = 134, RGBA_ASTC8X8_SRGB = 135, RGBA_ASTC8X8_UNorm = 136, RGBA_ASTC10X10_SRGB = 137, RGBA_ASTC10X10_UNorm = 138, RGBA_ASTC12X12_SRGB = 139, RGBA_ASTC12X12_UNorm = 140 }
        enum TextureCreationFlags { None = 0, MipChain = 1, Crunch = 64 }
        enum FormatUsage { Sample = 0, Linear = 1, Render = 3, Blend = 4, LoadStore = 8, MSAA2x = 9, MSAA4x = 10, MSAA8x = 11 }
        
    }
    namespace UnityEngine.Display {
        type DisplaysUpdatedDelegate = () => void;
        var DisplaysUpdatedDelegate: {new (func: () => void): DisplaysUpdatedDelegate;}
        
    }
    namespace Unity.Collections {
        class NativeArray$1<T> extends System.ValueType {
            
        }
        
    }
    namespace UnityEngine.Random {
        class State extends System.ValueType {
            
        }
        
    }
    namespace UnityEngine.Font {
        type FontTextureRebuildCallback = () => void;
        var FontTextureRebuildCallback: {new (func: () => void): FontTextureRebuildCallback;}
        
    }
    namespace UnityEngine.ParticleSystem {
        class Particle extends System.ValueType {
            
        }
        class MainModule extends System.ValueType {
            
        }
        class EmissionModule extends System.ValueType {
            
        }
        class ShapeModule extends System.ValueType {
            
        }
        class VelocityOverLifetimeModule extends System.ValueType {
            
        }
        class LimitVelocityOverLifetimeModule extends System.ValueType {
            
        }
        class InheritVelocityModule extends System.ValueType {
            
        }
        class ForceOverLifetimeModule extends System.ValueType {
            
        }
        class ColorOverLifetimeModule extends System.ValueType {
            
        }
        class ColorBySpeedModule extends System.ValueType {
            
        }
        class SizeOverLifetimeModule extends System.ValueType {
            
        }
        class SizeBySpeedModule extends System.ValueType {
            
        }
        class RotationOverLifetimeModule extends System.ValueType {
            
        }
        class RotationBySpeedModule extends System.ValueType {
            
        }
        class ExternalForcesModule extends System.ValueType {
            
        }
        class NoiseModule extends System.ValueType {
            
        }
        class CollisionModule extends System.ValueType {
            
        }
        class TriggerModule extends System.ValueType {
            
        }
        class SubEmittersModule extends System.ValueType {
            
        }
        class TextureSheetAnimationModule extends System.ValueType {
            
        }
        class LightsModule extends System.ValueType {
            
        }
        class TrailModule extends System.ValueType {
            
        }
        class CustomDataModule extends System.ValueType {
            
        }
        class EmitParams extends System.ValueType {
            
        }
        
    }
    namespace FairyEditor.Dialog {
        class DialogBase extends FairyGUI.Window {
            public __actionHandler: System.Action;
            public __cancelHandler: System.Action;
            public constructor();
            public Center($restraint: boolean):void;
            public ActionHandler():void;
            public CancelHandler():void;
            public Center():void;
            public Center($restraint: boolean):void;
            
        }
        
    }
    namespace FairyGUI {
        class Window extends FairyGUI.GComponent {
            public bringToFontOnClick: boolean;
            public __onInit: System.Action;
            public __onShown: System.Action;
            public __onHide: System.Action;
            public __doShowAnimation: System.Action;
            public __doHideAnimation: System.Action;
            public contentPane: FairyGUI.GComponent;
            public frame: FairyGUI.GComponent;
            public closeButton: FairyGUI.GObject;
            public dragArea: FairyGUI.GObject;
            public contentArea: FairyGUI.GObject;
            public modalWaitingPane: FairyGUI.GObject;
            public isShowing: boolean;
            public isTop: boolean;
            public modal: boolean;
            public modalWaiting: boolean;
            public constructor();
            public AddUISource($source: FairyGUI.IUISource):void;
            public Show():void;
            public ShowOn($r: FairyGUI.GRoot):void;
            public Hide():void;
            public HideImmediately():void;
            public CenterOn($r: FairyGUI.GRoot, $restraint: boolean):void;
            public ToggleStatus():void;
            public BringToFront():void;
            public ShowModalWait():void;
            public ShowModalWait($requestingCmd: number):void;
            public CloseModalWait():boolean;
            public CloseModalWait($requestingCmd: number):boolean;
            public Init():void;
            
        }
        class GComponent extends FairyGUI.GObject {
            public __onConstruct: System.Action;
            public __onDispose: System.Action;
            public rootContainer: FairyGUI.Container;
            public container: FairyGUI.Container;
            public scrollPane: FairyGUI.ScrollPane;
            public onDrop: FairyGUI.EventListener;
            public fairyBatching: boolean;
            public opaque: boolean;
            public margin: FairyGUI.Margin;
            public childrenRenderOrder: FairyGUI.ChildrenRenderOrder;
            public apexIndex: number;
            public tabStopChildren: boolean;
            public numChildren: number;
            public Controllers: System.Collections.Generic.List$1<FairyGUI.Controller>;
            public clipSoftness: UnityEngine.Vector2;
            public mask: FairyGUI.DisplayObject;
            public reversedMask: boolean;
            public baseUserData: string;
            public viewWidth: number;
            public viewHeight: number;
            public constructor();
            public InvalidateBatchingState($childChanged: boolean):void;
            public AddChild($child: FairyGUI.GObject):FairyGUI.GObject;
            public AddChildAt($child: FairyGUI.GObject, $index: number):FairyGUI.GObject;
            public RemoveChild($child: FairyGUI.GObject):FairyGUI.GObject;
            public RemoveChild($child: FairyGUI.GObject, $dispose: boolean):FairyGUI.GObject;
            public RemoveChildAt($index: number):FairyGUI.GObject;
            public RemoveChildAt($index: number, $dispose: boolean):FairyGUI.GObject;
            public RemoveChildren():void;
            public RemoveChildren($beginIndex: number, $endIndex: number, $dispose: boolean):void;
            public GetChildAt($index: number):FairyGUI.GObject;
            public GetChild($name: string):FairyGUI.GObject;
            public GetChildByPath($path: string):FairyGUI.GObject;
            public GetVisibleChild($name: string):FairyGUI.GObject;
            public GetChildInGroup($group: FairyGUI.GGroup, $name: string):FairyGUI.GObject;
            public GetChildren():System.Array$1<FairyGUI.GObject>;
            public GetChildIndex($child: FairyGUI.GObject):number;
            public SetChildIndex($child: FairyGUI.GObject, $index: number):void;
            public SetChildIndexBefore($child: FairyGUI.GObject, $index: number):number;
            public SwapChildren($child1: FairyGUI.GObject, $child2: FairyGUI.GObject):void;
            public SwapChildrenAt($index1: number, $index2: number):void;
            public IsAncestorOf($obj: FairyGUI.GObject):boolean;
            public ChangeChildrenOrder($objs: System.Collections.Generic.IList$1<FairyGUI.GObject>):void;
            public AddController($controller: FairyGUI.Controller):void;
            public GetControllerAt($index: number):FairyGUI.Controller;
            public GetController($name: string):FairyGUI.Controller;
            public RemoveController($c: FairyGUI.Controller):void;
            public GetTransitionAt($index: number):FairyGUI.Transition;
            public GetTransition($name: string):FairyGUI.Transition;
            public IsChildInView($child: FairyGUI.GObject):boolean;
            public GetFirstChildInView():number;
            public SetBoundsChangedFlag():void;
            public EnsureBoundsCorrect():void;
            public ConstructFromXML($xml: FairyGUI.Utils.XML):void;
            public InvalidateBatchingState():void;
            
        }
        class GObject extends FairyGUI.EventDispatcher {
            public name: string;
            public data: any;
            public sourceWidth: number;
            public sourceHeight: number;
            public initWidth: number;
            public initHeight: number;
            public minWidth: number;
            public maxWidth: number;
            public minHeight: number;
            public maxHeight: number;
            public dragBounds: System.Nullable$1<UnityEngine.Rect>;
            public packageItem: FairyGUI.PackageItem;
            public id: string;
            public relations: FairyGUI.Relations;
            public parent: FairyGUI.GComponent;
            public displayObject: FairyGUI.DisplayObject;
            public static draggingObject: FairyGUI.GObject;
            public onClick: FairyGUI.EventListener;
            public onRightClick: FairyGUI.EventListener;
            public onTouchBegin: FairyGUI.EventListener;
            public onTouchMove: FairyGUI.EventListener;
            public onTouchEnd: FairyGUI.EventListener;
            public onRollOver: FairyGUI.EventListener;
            public onRollOut: FairyGUI.EventListener;
            public onAddedToStage: FairyGUI.EventListener;
            public onRemovedFromStage: FairyGUI.EventListener;
            public onKeyDown: FairyGUI.EventListener;
            public onClickLink: FairyGUI.EventListener;
            public onPositionChanged: FairyGUI.EventListener;
            public onSizeChanged: FairyGUI.EventListener;
            public onDragStart: FairyGUI.EventListener;
            public onDragMove: FairyGUI.EventListener;
            public onDragEnd: FairyGUI.EventListener;
            public onGearStop: FairyGUI.EventListener;
            public onFocusIn: FairyGUI.EventListener;
            public onFocusOut: FairyGUI.EventListener;
            public x: number;
            public y: number;
            public z: number;
            public xy: UnityEngine.Vector2;
            public position: UnityEngine.Vector3;
            public width: number;
            public height: number;
            public size: UnityEngine.Vector2;
            public actualWidth: number;
            public actualHeight: number;
            public xMin: number;
            public yMin: number;
            public scaleX: number;
            public scaleY: number;
            public scale: UnityEngine.Vector2;
            public skew: UnityEngine.Vector2;
            public pivotX: number;
            public pivotY: number;
            public pivot: UnityEngine.Vector2;
            public pivotAsAnchor: boolean;
            public touchable: boolean;
            public grayed: boolean;
            public enabled: boolean;
            public rotation: number;
            public rotationX: number;
            public rotationY: number;
            public alpha: number;
            public visible: boolean;
            public sortingOrder: number;
            public focusable: boolean;
            public tabStop: boolean;
            public focused: boolean;
            public tooltips: string;
            public cursor: string;
            public filter: FairyGUI.IFilter;
            public blendMode: FairyGUI.BlendMode;
            public gameObjectName: string;
            public inContainer: boolean;
            public onStage: boolean;
            public resourceURL: string;
            public gearXY: FairyGUI.GearXY;
            public gearSize: FairyGUI.GearSize;
            public gearLook: FairyGUI.GearLook;
            public group: FairyGUI.GGroup;
            public root: FairyGUI.GRoot;
            public text: string;
            public icon: string;
            public draggable: boolean;
            public dragging: boolean;
            public isDisposed: boolean;
            public asImage: FairyGUI.GImage;
            public asCom: FairyGUI.GComponent;
            public asButton: FairyGUI.GButton;
            public asLabel: FairyGUI.GLabel;
            public asProgress: FairyGUI.GProgressBar;
            public asSlider: FairyGUI.GSlider;
            public asComboBox: FairyGUI.GComboBox;
            public asTextField: FairyGUI.GTextField;
            public asRichTextField: FairyGUI.GRichTextField;
            public asTextInput: FairyGUI.GTextInput;
            public asLoader: FairyGUI.GLoader;
            public asLoader3D: FairyGUI.GLoader3D;
            public asList: FairyGUI.GList;
            public asGraph: FairyGUI.GGraph;
            public asGroup: FairyGUI.GGroup;
            public asMovieClip: FairyGUI.GMovieClip;
            public asTree: FairyGUI.GTree;
            public treeNode: FairyGUI.GTreeNode;
            public constructor();
            public SetXY($xv: number, $yv: number):void;
            public SetXY($xv: number, $yv: number, $topLeftValue: boolean):void;
            public SetPosition($xv: number, $yv: number, $zv: number):void;
            public Center():void;
            public Center($restraint: boolean):void;
            public MakeFullScreen():void;
            public SetSize($wv: number, $hv: number):void;
            public SetSize($wv: number, $hv: number, $ignorePivot: boolean):void;
            public SetScale($wv: number, $hv: number):void;
            public SetPivot($xv: number, $yv: number):void;
            public SetPivot($xv: number, $yv: number, $asAnchor: boolean):void;
            public RequestFocus():void;
            public RequestFocus($byKey: boolean):void;
            public SetHome($obj: FairyGUI.GObject):void;
            public GetGear($index: number):FairyGUI.GearBase;
            public InvalidateBatchingState():void;
            public HandleControllerChanged($c: FairyGUI.Controller):void;
            public AddRelation($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType):void;
            public AddRelation($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType, $usePercent: boolean):void;
            public RemoveRelation($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType):void;
            public RemoveFromParent():void;
            public StartDrag():void;
            public StartDrag($touchId: number):void;
            public StopDrag():void;
            public LocalToGlobal($pt: UnityEngine.Vector2):UnityEngine.Vector2;
            public GlobalToLocal($pt: UnityEngine.Vector2):UnityEngine.Vector2;
            public LocalToGlobal($rect: UnityEngine.Rect):UnityEngine.Rect;
            public GlobalToLocal($rect: UnityEngine.Rect):UnityEngine.Rect;
            public LocalToRoot($pt: UnityEngine.Vector2, $r: FairyGUI.GRoot):UnityEngine.Vector2;
            public RootToLocal($pt: UnityEngine.Vector2, $r: FairyGUI.GRoot):UnityEngine.Vector2;
            public WorldToLocal($pt: UnityEngine.Vector3):UnityEngine.Vector2;
            public WorldToLocal($pt: UnityEngine.Vector3, $camera: UnityEngine.Camera):UnityEngine.Vector2;
            public TransformPoint($pt: UnityEngine.Vector2, $targetSpace: FairyGUI.GObject):UnityEngine.Vector2;
            public TransformRect($rect: UnityEngine.Rect, $targetSpace: FairyGUI.GObject):UnityEngine.Rect;
            public Dispose():void;
            public ConstructFromResource():void;
            public Setup_BeforeAdd($buffer: FairyGUI.Utils.ByteBuffer, $beginPos: number):void;
            public Setup_AfterAdd($buffer: FairyGUI.Utils.ByteBuffer, $beginPos: number):void;
            public TweenMove($endValue: UnityEngine.Vector2, $duration: number):FairyGUI.GTweener;
            public TweenMoveX($endValue: number, $duration: number):FairyGUI.GTweener;
            public TweenMoveY($endValue: number, $duration: number):FairyGUI.GTweener;
            public TweenScale($endValue: UnityEngine.Vector2, $duration: number):FairyGUI.GTweener;
            public TweenScaleX($endValue: number, $duration: number):FairyGUI.GTweener;
            public TweenScaleY($endValue: number, $duration: number):FairyGUI.GTweener;
            public TweenResize($endValue: UnityEngine.Vector2, $duration: number):FairyGUI.GTweener;
            public TweenFade($endValue: number, $duration: number):FairyGUI.GTweener;
            public TweenRotate($endValue: number, $duration: number):FairyGUI.GTweener;
            
        }
        class EventDispatcher extends System.Object {
            public constructor();
            public AddEventListener($strType: string, $callback: FairyGUI.EventCallback1):void;
            public AddEventListener($strType: string, $callback: FairyGUI.EventCallback0):void;
            public RemoveEventListener($strType: string, $callback: FairyGUI.EventCallback1):void;
            public RemoveEventListener($strType: string, $callback: FairyGUI.EventCallback0):void;
            public AddCapture($strType: string, $callback: FairyGUI.EventCallback1):void;
            public RemoveCapture($strType: string, $callback: FairyGUI.EventCallback1):void;
            public RemoveEventListeners():void;
            public RemoveEventListeners($strType: string):void;
            public hasEventListeners($strType: string):boolean;
            public isDispatching($strType: string):boolean;
            public DispatchEvent($strType: string):boolean;
            public DispatchEvent($strType: string, $data: any):boolean;
            public DispatchEvent($strType: string, $data: any, $initiator: any):boolean;
            public DispatchEvent($context: FairyGUI.EventContext):boolean;
            public BubbleEvent($strType: string, $data: any):boolean;
            public BroadcastEvent($strType: string, $data: any):boolean;
            
        }
        class GRoot extends FairyGUI.GComponent {
            public static contentScaleFactor: number;
            public static contentScaleLevel: number;
            public static inst: FairyGUI.GRoot;
            public modalLayer: FairyGUI.GGraph;
            public hasModalWindow: boolean;
            public modalWaiting: boolean;
            public touchTarget: FairyGUI.GObject;
            public hasAnyPopup: boolean;
            public focus: FairyGUI.GObject;
            public soundVolume: number;
            public constructor();
            public SetContentScaleFactor($designResolutionX: number, $designResolutionY: number):void;
            public SetContentScaleFactor($designResolutionX: number, $designResolutionY: number, $screenMatchMode: FairyGUI.UIContentScaler.ScreenMatchMode):void;
            public SetContentScaleFactor($constantScaleFactor: number):void;
            public ApplyContentScaleFactor():void;
            public ShowWindow($win: FairyGUI.Window):void;
            public HideWindow($win: FairyGUI.Window):void;
            public HideWindowImmediately($win: FairyGUI.Window):void;
            public HideWindowImmediately($win: FairyGUI.Window, $dispose: boolean):void;
            public BringToFront($win: FairyGUI.Window):void;
            public ShowModalWait():void;
            public CloseModalWait():void;
            public CloseAllExceptModals():void;
            public CloseAllWindows():void;
            public GetTopWindow():FairyGUI.Window;
            public DisplayObjectToGObject($obj: FairyGUI.DisplayObject):FairyGUI.GObject;
            public ShowPopup($popup: FairyGUI.GObject):void;
            public ShowPopup($popup: FairyGUI.GObject, $target: FairyGUI.GObject):void;
            public ShowPopup($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection):void;
            public ShowPopup($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection, $closeUntilUpEvent: boolean):void;
            public GetPoupPosition($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection):UnityEngine.Vector2;
            public TogglePopup($popup: FairyGUI.GObject):void;
            public TogglePopup($popup: FairyGUI.GObject, $target: FairyGUI.GObject):void;
            public TogglePopup($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection):void;
            public TogglePopup($popup: FairyGUI.GObject, $target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection, $closeUntilUpEvent: boolean):void;
            public HidePopup():void;
            public HidePopup($popup: FairyGUI.GObject):void;
            public ShowTooltips($msg: string):void;
            public ShowTooltips($msg: string, $delay: number):void;
            public ShowTooltipsWin($tooltipWin: FairyGUI.GObject):void;
            public ShowTooltipsWin($tooltipWin: FairyGUI.GObject, $delay: number):void;
            public HideTooltips():void;
            public EnableSound():void;
            public DisableSound():void;
            public PlayOneShotSound($clip: UnityEngine.AudioClip, $volumeScale: number):void;
            public PlayOneShotSound($clip: UnityEngine.AudioClip):void;
            
        }
        type EventCallback1 = (context: FairyGUI.EventContext) => void;
        var EventCallback1: {new (func: (context: FairyGUI.EventContext) => void): EventCallback1;}
        class EventContext extends System.Object {
            public type: string;
            public data: any;
            public sender: FairyGUI.EventDispatcher;
            public initiator: any;
            public inputEvent: FairyGUI.InputEvent;
            public isDefaultPrevented: boolean;
            public constructor();
            public StopPropagation():void;
            public PreventDefault():void;
            public CaptureTouch():void;
            
        }
        class GLoader extends FairyGUI.GObject {
            public showErrorSign: boolean;
            public url: string;
            public icon: string;
            public align: FairyGUI.AlignType;
            public verticalAlign: FairyGUI.VertAlignType;
            public fill: FairyGUI.FillType;
            public shrinkOnly: boolean;
            public autoSize: boolean;
            public playing: boolean;
            public frame: number;
            public timeScale: number;
            public ignoreEngineTimeScale: boolean;
            public material: UnityEngine.Material;
            public shader: string;
            public color: UnityEngine.Color;
            public fillMethod: FairyGUI.FillMethod;
            public fillOrigin: number;
            public fillClockwise: boolean;
            public fillAmount: number;
            public image: FairyGUI.Image;
            public movieClip: FairyGUI.MovieClip;
            public component: FairyGUI.GComponent;
            public texture: FairyGUI.NTexture;
            public filter: FairyGUI.IFilter;
            public blendMode: FairyGUI.BlendMode;
            public constructor();
            public Advance($time: number):void;
            
        }
        class Image extends FairyGUI.DisplayObject {
            public texture: FairyGUI.NTexture;
            public textureScale: UnityEngine.Vector2;
            public color: UnityEngine.Color;
            public fillMethod: FairyGUI.FillMethod;
            public fillOrigin: number;
            public fillClockwise: boolean;
            public fillAmount: number;
            public scale9Grid: System.Nullable$1<UnityEngine.Rect>;
            public scaleByTile: boolean;
            public tileGridIndice: number;
            public constructor();
            public constructor($texture: FairyGUI.NTexture);
            public SetNativeSize():void;
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public SliceFill($vb: FairyGUI.VertexBuffer):void;
            
        }
        class DisplayObject extends FairyGUI.EventDispatcher {
            public name: string;
            public gOwner: FairyGUI.GObject;
            public id: number;
            public parent: FairyGUI.Container;
            public gameObject: UnityEngine.GameObject;
            public cachedTransform: UnityEngine.Transform;
            public graphics: FairyGUI.NGraphics;
            public paintingGraphics: FairyGUI.NGraphics;
            public onClick: FairyGUI.EventListener;
            public onRightClick: FairyGUI.EventListener;
            public onTouchBegin: FairyGUI.EventListener;
            public onTouchMove: FairyGUI.EventListener;
            public onTouchEnd: FairyGUI.EventListener;
            public onRollOver: FairyGUI.EventListener;
            public onRollOut: FairyGUI.EventListener;
            public onMouseWheel: FairyGUI.EventListener;
            public onAddedToStage: FairyGUI.EventListener;
            public onRemovedFromStage: FairyGUI.EventListener;
            public onKeyDown: FairyGUI.EventListener;
            public onClickLink: FairyGUI.EventListener;
            public onFocusIn: FairyGUI.EventListener;
            public onFocusOut: FairyGUI.EventListener;
            public alpha: number;
            public grayed: boolean;
            public visible: boolean;
            public x: number;
            public y: number;
            public z: number;
            public xy: UnityEngine.Vector2;
            public position: UnityEngine.Vector3;
            public pixelPerfect: boolean;
            public width: number;
            public height: number;
            public size: UnityEngine.Vector2;
            public scaleX: number;
            public scaleY: number;
            public scale: UnityEngine.Vector2;
            public rotation: number;
            public rotationX: number;
            public rotationY: number;
            public skew: UnityEngine.Vector2;
            public perspective: boolean;
            public focalLength: number;
            public pivot: UnityEngine.Vector2;
            public location: UnityEngine.Vector3;
            public material: UnityEngine.Material;
            public shader: string;
            public renderingOrder: number;
            public layer: number;
            public focusable: boolean;
            public tabStop: boolean;
            public focused: boolean;
            public cursor: string;
            public isDisposed: boolean;
            public topmost: FairyGUI.Container;
            public stage: FairyGUI.Stage;
            public worldSpaceContainer: FairyGUI.Container;
            public touchable: boolean;
            public touchDisabled: boolean;
            public paintingMode: boolean;
            public cacheAsBitmap: boolean;
            public filter: FairyGUI.IFilter;
            public blendMode: FairyGUI.BlendMode;
            public home: UnityEngine.Transform;
            public constructor();
            public add_onPaint($value: System.Action):void;
            public remove_onPaint($value: System.Action):void;
            public SetXY($xv: number, $yv: number):void;
            public SetPosition($xv: number, $yv: number, $zv: number):void;
            public SetSize($wv: number, $hv: number):void;
            public EnsureSizeCorrect():void;
            public SetScale($xv: number, $yv: number):void;
            public EnterPaintingMode():void;
            public EnterPaintingMode($requestorId: number, $extend: System.Nullable$1<FairyGUI.Margin>):void;
            public EnterPaintingMode($requestorId: number, $extend: System.Nullable$1<FairyGUI.Margin>, $scale: number):void;
            public LeavePaintingMode($requestorId: number):void;
            public GetScreenShot($extend: System.Nullable$1<FairyGUI.Margin>, $scale: number):UnityEngine.Texture2D;
            public GetBounds($targetSpace: FairyGUI.DisplayObject):UnityEngine.Rect;
            public GlobalToLocal($point: UnityEngine.Vector2):UnityEngine.Vector2;
            public LocalToGlobal($point: UnityEngine.Vector2):UnityEngine.Vector2;
            public WorldToLocal($worldPoint: UnityEngine.Vector3, $direction: UnityEngine.Vector3):UnityEngine.Vector3;
            public LocalToWorld($localPoint: UnityEngine.Vector3):UnityEngine.Vector3;
            public TransformPoint($point: UnityEngine.Vector2, $targetSpace: FairyGUI.DisplayObject):UnityEngine.Vector2;
            public TransformRect($rect: UnityEngine.Rect, $targetSpace: FairyGUI.DisplayObject):UnityEngine.Rect;
            public RemoveFromParent():void;
            public InvalidateBatchingState():void;
            public Update($context: FairyGUI.UpdateContext):void;
            public Dispose():void;
            
        }
        class EventListener extends System.Object {
            public type: string;
            public isEmpty: boolean;
            public isDispatching: boolean;
            public constructor($owner: FairyGUI.EventDispatcher, $type: string);
            public AddCapture($callback: FairyGUI.EventCallback1):void;
            public RemoveCapture($callback: FairyGUI.EventCallback1):void;
            public Add($callback: FairyGUI.EventCallback1):void;
            public Remove($callback: FairyGUI.EventCallback1):void;
            public Add($callback: FairyGUI.EventCallback0):void;
            public Remove($callback: FairyGUI.EventCallback0):void;
            public Set($callback: FairyGUI.EventCallback1):void;
            public Set($callback: FairyGUI.EventCallback0):void;
            public Clear():void;
            public Call():boolean;
            public Call($data: any):boolean;
            public BubbleCall($data: any):boolean;
            public BubbleCall():boolean;
            public BroadcastCall($data: any):boolean;
            public BroadcastCall():boolean;
            
        }
        class NTexture extends System.Object {
            public uvRect: UnityEngine.Rect;
            public rotated: boolean;
            public refCount: number;
            public lastActive: number;
            public destroyMethod: FairyGUI.DestroyMethod;
            public static Empty: FairyGUI.NTexture;
            public width: number;
            public height: number;
            public offset: UnityEngine.Vector2;
            public originalSize: UnityEngine.Vector2;
            public root: FairyGUI.NTexture;
            public disposed: boolean;
            public nativeTexture: UnityEngine.Texture;
            public alphaTexture: UnityEngine.Texture;
            public constructor($texture: UnityEngine.Texture);
            public constructor($texture: UnityEngine.Texture, $alphaTexture: UnityEngine.Texture, $xScale: number, $yScale: number);
            public constructor($texture: UnityEngine.Texture, $region: UnityEngine.Rect);
            public constructor($root: FairyGUI.NTexture, $region: UnityEngine.Rect, $rotated: boolean);
            public constructor($root: FairyGUI.NTexture, $region: UnityEngine.Rect, $rotated: boolean, $originalSize: UnityEngine.Vector2, $offset: UnityEngine.Vector2);
            public constructor($sprite: UnityEngine.Sprite);
            public static add_CustomDestroyMethod($value: System.Action$1<UnityEngine.Texture>):void;
            public static remove_CustomDestroyMethod($value: System.Action$1<UnityEngine.Texture>):void;
            public add_onSizeChanged($value: System.Action$1<FairyGUI.NTexture>):void;
            public remove_onSizeChanged($value: System.Action$1<FairyGUI.NTexture>):void;
            public add_onRelease($value: System.Action$1<FairyGUI.NTexture>):void;
            public remove_onRelease($value: System.Action$1<FairyGUI.NTexture>):void;
            public static DisposeEmpty():void;
            public GetDrawRect($drawRect: UnityEngine.Rect):UnityEngine.Rect;
            public GetUV($uv: System.Array$1<UnityEngine.Vector2>):void;
            public GetMaterialManager($shaderName: string):FairyGUI.MaterialManager;
            public Unload():void;
            public Unload($destroyMaterials: boolean):void;
            public Reload($nativeTexture: UnityEngine.Texture, $alphaTexture: UnityEngine.Texture):void;
            public AddRef():void;
            public ReleaseRef():void;
            public Dispose():void;
            
        }
        class BitmapFont extends FairyGUI.BaseFont {
            public size: number;
            public resizable: boolean;
            public hasChannel: boolean;
            public constructor();
            public AddChar($ch: number, $glyph: FairyGUI.BitmapFont.BMGlyph):void;
            
        }
        class BaseFont extends System.Object {
            public name: string;
            public mainTexture: FairyGUI.NTexture;
            public canTint: boolean;
            public customBold: boolean;
            public customBoldAndItalic: boolean;
            public customOutline: boolean;
            public shader: string;
            public keepCrisp: boolean;
            public version: number;
            public constructor();
            public UpdateGraphics($graphics: FairyGUI.NGraphics):void;
            public SetFormat($format: FairyGUI.TextFormat, $fontSizeScale: number):void;
            public PrepareCharacters($text: string):void;
            public GetGlyph($ch: number, $width: $Ref<number>, $height: $Ref<number>, $baseline: $Ref<number>):boolean;
            public DrawGlyph($x: number, $y: number, $vertList: System.Collections.Generic.List$1<UnityEngine.Vector3>, $uvList: System.Collections.Generic.List$1<UnityEngine.Vector2>, $uv2List: System.Collections.Generic.List$1<UnityEngine.Vector2>, $colList: System.Collections.Generic.List$1<UnityEngine.Color32>):number;
            public DrawLine($x: number, $y: number, $width: number, $fontSize: number, $type: number, $vertList: System.Collections.Generic.List$1<UnityEngine.Vector3>, $uvList: System.Collections.Generic.List$1<UnityEngine.Vector2>, $uv2List: System.Collections.Generic.List$1<UnityEngine.Vector2>, $colList: System.Collections.Generic.List$1<UnityEngine.Color32>):number;
            public HasCharacter($ch: number):boolean;
            public GetLineHeight($size: number):number;
            public Dispose():void;
            
        }
        enum AlignType { Left = 0, Center = 1, Right = 2 }
        enum VertAlignType { Top = 0, Middle = 1, Bottom = 2 }
        enum AutoSizeType { None = 0, Both = 1, Height = 2, Shrink = 3 }
        enum FlipType { None = 0, Horizontal = 1, Vertical = 2, Both = 3 }
        enum FillMethod { None = 0, Horizontal = 1, Vertical = 2, Radial90 = 3, Radial180 = 4, Radial360 = 5 }
        enum EaseType { Linear = 0, SineIn = 1, SineOut = 2, SineInOut = 3, QuadIn = 4, QuadOut = 5, QuadInOut = 6, CubicIn = 7, CubicOut = 8, CubicInOut = 9, QuartIn = 10, QuartOut = 11, QuartInOut = 12, QuintIn = 13, QuintOut = 14, QuintInOut = 15, ExpoIn = 16, ExpoOut = 17, ExpoInOut = 18, CircIn = 19, CircOut = 20, CircInOut = 21, ElasticIn = 22, ElasticOut = 23, ElasticInOut = 24, BackIn = 25, BackOut = 26, BackInOut = 27, BounceIn = 28, BounceOut = 29, BounceInOut = 30, Custom = 31 }
        class CustomEase extends System.Object {
            public constructor($pointDensity?: number);
            public Create($pathPoints: System.Collections.Generic.IEnumerable$1<FairyGUI.GPathPoint>):void;
            public Evaluate($time: number):number;
            
        }
        class GPathPoint extends System.ValueType {
            public pos: UnityEngine.Vector3;
            public control1: UnityEngine.Vector3;
            public control2: UnityEngine.Vector3;
            public curveType: FairyGUI.GPathPoint.CurveType;
            public smooth: boolean;
            public constructor($pos: UnityEngine.Vector3);
            public constructor($pos: UnityEngine.Vector3, $control: UnityEngine.Vector3);
            public constructor($pos: UnityEngine.Vector3, $control1: UnityEngine.Vector3, $control2: UnityEngine.Vector3);
            public constructor($pos: UnityEngine.Vector3, $curveType: FairyGUI.GPathPoint.CurveType);
            
        }
        class Container extends FairyGUI.DisplayObject {
            public renderMode: UnityEngine.RenderMode;
            public renderCamera: UnityEngine.Camera;
            public opaque: boolean;
            public clipSoftness: System.Nullable$1<UnityEngine.Vector4>;
            public hitArea: FairyGUI.IHitTest;
            public touchChildren: boolean;
            public reversedMask: boolean;
            public numChildren: number;
            public clipRect: System.Nullable$1<UnityEngine.Rect>;
            public mask: FairyGUI.DisplayObject;
            public fairyBatching: boolean;
            public tabStopChildren: boolean;
            public constructor();
            public constructor($gameObjectName: string);
            public constructor($attachTarget: UnityEngine.GameObject);
            public add_onUpdate($value: System.Action):void;
            public remove_onUpdate($value: System.Action):void;
            public AddChild($child: FairyGUI.DisplayObject):FairyGUI.DisplayObject;
            public AddChildAt($child: FairyGUI.DisplayObject, $index: number):FairyGUI.DisplayObject;
            public Contains($child: FairyGUI.DisplayObject):boolean;
            public GetChildAt($index: number):FairyGUI.DisplayObject;
            public GetChild($name: string):FairyGUI.DisplayObject;
            public GetChildren():System.Array$1<FairyGUI.DisplayObject>;
            public GetChildIndex($child: FairyGUI.DisplayObject):number;
            public RemoveChild($child: FairyGUI.DisplayObject):FairyGUI.DisplayObject;
            public RemoveChild($child: FairyGUI.DisplayObject, $dispose: boolean):FairyGUI.DisplayObject;
            public RemoveChildAt($index: number):FairyGUI.DisplayObject;
            public RemoveChildAt($index: number, $dispose: boolean):FairyGUI.DisplayObject;
            public RemoveChildren():void;
            public RemoveChildren($beginIndex: number, $endIndex: number, $dispose: boolean):void;
            public SetChildIndex($child: FairyGUI.DisplayObject, $index: number):void;
            public SwapChildren($child1: FairyGUI.DisplayObject, $child2: FairyGUI.DisplayObject):void;
            public SwapChildrenAt($index1: number, $index2: number):void;
            public ChangeChildrenOrder($indice: System.Collections.Generic.IList$1<number>, $objs: System.Collections.Generic.IList$1<FairyGUI.DisplayObject>):void;
            public GetDescendants($backward: boolean):System.Collections.Generic.IEnumerator$1<FairyGUI.DisplayObject>;
            public CreateGraphics():void;
            public GetRenderCamera():UnityEngine.Camera;
            public HitTest($stagePoint: UnityEngine.Vector2, $forTouch: boolean):FairyGUI.DisplayObject;
            public IsAncestorOf($obj: FairyGUI.DisplayObject):boolean;
            public InvalidateBatchingState($childrenChanged: boolean):void;
            public SetChildrenLayer($value: number):void;
            public InvalidateBatchingState():void;
            
        }
        class TextFormat extends System.Object {
            public size: number;
            public font: string;
            public color: UnityEngine.Color;
            public lineSpacing: number;
            public letterSpacing: number;
            public bold: boolean;
            public underline: boolean;
            public italic: boolean;
            public strikethrough: boolean;
            public gradientColor: System.Array$1<UnityEngine.Color32>;
            public align: FairyGUI.AlignType;
            public specialStyle: FairyGUI.TextFormat.SpecialStyle;
            public outline: number;
            public outlineColor: UnityEngine.Color;
            public shadowOffset: UnityEngine.Vector2;
            public shadowColor: UnityEngine.Color;
            public faceDilate: number;
            public outlineSoftness: number;
            public underlaySoftness: number;
            public constructor();
            public SetColor($value: number):void;
            public EqualStyle($aFormat: FairyGUI.TextFormat):boolean;
            public CopyFrom($source: FairyGUI.TextFormat):void;
            public FillVertexColors($vertexColors: System.Array$1<UnityEngine.Color32>):void;
            
        }
        class GTweener extends System.Object {
            public delay: number;
            public duration: number;
            public repeat: number;
            public target: any;
            public userData: any;
            public startValue: FairyGUI.TweenValue;
            public endValue: FairyGUI.TweenValue;
            public value: FairyGUI.TweenValue;
            public deltaValue: FairyGUI.TweenValue;
            public normalizedTime: number;
            public completed: boolean;
            public allCompleted: boolean;
            public constructor();
            public SetDelay($value: number):FairyGUI.GTweener;
            public SetDuration($value: number):FairyGUI.GTweener;
            public SetBreakpoint($value: number):FairyGUI.GTweener;
            public SetEase($value: FairyGUI.EaseType):FairyGUI.GTweener;
            public SetEase($value: FairyGUI.EaseType, $customEase: FairyGUI.CustomEase):FairyGUI.GTweener;
            public SetEasePeriod($value: number):FairyGUI.GTweener;
            public SetEaseOvershootOrAmplitude($value: number):FairyGUI.GTweener;
            public SetRepeat($times: number, $yoyo?: boolean):FairyGUI.GTweener;
            public SetTimeScale($value: number):FairyGUI.GTweener;
            public SetIgnoreEngineTimeScale($value: boolean):FairyGUI.GTweener;
            public SetSnapping($value: boolean):FairyGUI.GTweener;
            public SetPath($value: FairyGUI.GPath):FairyGUI.GTweener;
            public SetTarget($value: any):FairyGUI.GTweener;
            public SetTarget($value: any, $propType: FairyGUI.TweenPropType):FairyGUI.GTweener;
            public SetUserData($value: any):FairyGUI.GTweener;
            public OnUpdate($callback: FairyGUI.GTweenCallback):FairyGUI.GTweener;
            public OnStart($callback: FairyGUI.GTweenCallback):FairyGUI.GTweener;
            public OnComplete($callback: FairyGUI.GTweenCallback):FairyGUI.GTweener;
            public OnUpdate($callback: FairyGUI.GTweenCallback1):FairyGUI.GTweener;
            public OnStart($callback: FairyGUI.GTweenCallback1):FairyGUI.GTweener;
            public OnComplete($callback: FairyGUI.GTweenCallback1):FairyGUI.GTweener;
            public SetListener($value: FairyGUI.ITweenListener):FairyGUI.GTweener;
            public SetPaused($paused: boolean):FairyGUI.GTweener;
            public Seek($time: number):void;
            public Kill($complete?: boolean):void;
            
        }
        class GPath extends System.Object {
            public length: number;
            public segmentCount: number;
            public constructor();
            public Create($pt1: FairyGUI.GPathPoint, $pt2: FairyGUI.GPathPoint):void;
            public Create($pt1: FairyGUI.GPathPoint, $pt2: FairyGUI.GPathPoint, $pt3: FairyGUI.GPathPoint):void;
            public Create($pt1: FairyGUI.GPathPoint, $pt2: FairyGUI.GPathPoint, $pt3: FairyGUI.GPathPoint, $pt4: FairyGUI.GPathPoint):void;
            public Create($points: System.Collections.Generic.IEnumerable$1<FairyGUI.GPathPoint>):void;
            public Clear():void;
            public GetPointAt($t: number):UnityEngine.Vector3;
            public GetSegmentLength($segmentIndex: number):number;
            public GetPointsInSegment($segmentIndex: number, $t0: number, $t1: number, $points: System.Collections.Generic.List$1<UnityEngine.Vector3>, $ts?: System.Collections.Generic.List$1<number>, $pointDensity?: number):void;
            public GetAllPoints($points: System.Collections.Generic.List$1<UnityEngine.Vector3>, $pointDensity?: number):void;
            
        }
        class RichTextField extends FairyGUI.Container {
            public htmlPageContext: FairyGUI.Utils.IHtmlPageContext;
            public htmlParseOptions: FairyGUI.Utils.HtmlParseOptions;
            public emojies: System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>;
            public textField: FairyGUI.TextField;
            public text: string;
            public htmlText: string;
            public textFormat: FairyGUI.TextFormat;
            public htmlElementCount: number;
            public constructor();
            public GetHtmlElement($name: string):FairyGUI.Utils.HtmlElement;
            public GetHtmlElementAt($index: number):FairyGUI.Utils.HtmlElement;
            public ShowHtmlObject($index: number, $show: boolean):void;
            
        }
        class InputEvent extends System.Object {
            public x: number;
            public y: number;
            public keyCode: UnityEngine.KeyCode;
            public character: number;
            public modifiers: UnityEngine.EventModifiers;
            public mouseWheelDelta: number;
            public touchId: number;
            public button: number;
            public clickCount: number;
            public holdTime: number;
            public position: UnityEngine.Vector2;
            public isDoubleClick: boolean;
            public ctrlOrCmd: boolean;
            public ctrl: boolean;
            public shift: boolean;
            public alt: boolean;
            public command: boolean;
            public constructor();
            
        }
        class GComboBox extends FairyGUI.GComponent {
            public visibleItemCount: number;
            public dropdown: FairyGUI.GComponent;
            public sound: FairyGUI.NAudioClip;
            public soundVolumeScale: number;
            public onChanged: FairyGUI.EventListener;
            public icon: string;
            public title: string;
            public text: string;
            public titleColor: UnityEngine.Color;
            public titleFontSize: number;
            public items: System.Array$1<string>;
            public icons: System.Array$1<string>;
            public values: System.Array$1<string>;
            public itemList: System.Collections.Generic.List$1<string>;
            public valueList: System.Collections.Generic.List$1<string>;
            public iconList: System.Collections.Generic.List$1<string>;
            public selectedIndex: number;
            public selectionController: FairyGUI.Controller;
            public value: string;
            public popupDirection: FairyGUI.PopupDirection;
            public constructor();
            public ApplyListChange():void;
            public GetTextField():FairyGUI.GTextField;
            public UpdateDropdownList():void;
            
        }
        class Shape extends FairyGUI.DisplayObject {
            public color: UnityEngine.Color;
            public isEmpty: boolean;
            public constructor();
            public DrawRect($lineSize: number, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color):void;
            public DrawRect($lineSize: number, $colors: System.Array$1<UnityEngine.Color32>):void;
            public DrawRoundRect($lineSize: number, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color, $topLeftRadius: number, $topRightRadius: number, $bottomLeftRadius: number, $bottomRightRadius: number):void;
            public DrawEllipse($fillColor: UnityEngine.Color):void;
            public DrawEllipse($lineSize: number, $centerColor: UnityEngine.Color, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color, $startDegree: number, $endDegree: number):void;
            public DrawPolygon($points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color):void;
            public DrawPolygon($points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $colors: System.Array$1<UnityEngine.Color32>):void;
            public DrawPolygon($points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color, $lineSize: number, $lineColor: UnityEngine.Color):void;
            public DrawRegularPolygon($sides: number, $lineSize: number, $centerColor: UnityEngine.Color, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color, $rotation: number, $distances: System.Array$1<number>):void;
            public Clear():void;
            
        }
        class VertexBuffer extends System.Object {
            public contentRect: UnityEngine.Rect;
            public uvRect: UnityEngine.Rect;
            public vertexColor: UnityEngine.Color32;
            public textureSize: UnityEngine.Vector2;
            public vertices: System.Collections.Generic.List$1<UnityEngine.Vector3>;
            public colors: System.Collections.Generic.List$1<UnityEngine.Color32>;
            public uvs: System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public uvs2: System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public triangles: System.Collections.Generic.List$1<number>;
            public static NormalizedUV: System.Array$1<UnityEngine.Vector2>;
            public static NormalizedPosition: System.Array$1<UnityEngine.Vector2>;
            public currentVertCount: number;
            public static Begin():FairyGUI.VertexBuffer;
            public static Begin($source: FairyGUI.VertexBuffer):FairyGUI.VertexBuffer;
            public End():void;
            public Clear():void;
            public AddVert($position: UnityEngine.Vector3):void;
            public AddVert($position: UnityEngine.Vector3, $color: UnityEngine.Color32):void;
            public AddVert($position: UnityEngine.Vector3, $color: UnityEngine.Color32, $uv: UnityEngine.Vector2):void;
            public AddQuad($vertRect: UnityEngine.Rect):void;
            public AddQuad($vertRect: UnityEngine.Rect, $color: UnityEngine.Color32):void;
            public AddQuad($vertRect: UnityEngine.Rect, $color: UnityEngine.Color32, $uvRect: UnityEngine.Rect):void;
            public RepeatColors($value: System.Array$1<UnityEngine.Color32>, $startIndex: number, $count: number):void;
            public AddTriangle($idx0: number, $idx1: number, $idx2: number):void;
            public AddTriangles($idxList: System.Array$1<number>, $startVertexIndex?: number):void;
            public AddTriangles($startVertexIndex?: number):void;
            public GetPosition($index: number):UnityEngine.Vector3;
            public GetUVAtPosition($position: UnityEngine.Vector2, $usePercent: boolean):UnityEngine.Vector2;
            public Append($vb: FairyGUI.VertexBuffer):void;
            public Insert($vb: FairyGUI.VertexBuffer):void;
            
        }
        class LineMesh extends System.Object {
            public path: FairyGUI.GPath;
            public lineWidth: number;
            public lineWidthCurve: UnityEngine.AnimationCurve;
            public gradient: UnityEngine.Gradient;
            public roundEdge: boolean;
            public fillStart: number;
            public fillEnd: number;
            public pointDensity: number;
            public repeatFill: boolean;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class StraightLineMesh extends System.Object {
            public color: UnityEngine.Color;
            public origin: UnityEngine.Vector3;
            public end: UnityEngine.Vector3;
            public lineWidth: number;
            public repeatFill: boolean;
            public constructor();
            public constructor($lineWidth: number, $color: UnityEngine.Color, $repeatFill: boolean);
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        enum PopupDirection { Auto = 0, Up = 1, Down = 2 }
        class GTree extends FairyGUI.GList {
            public treeNodeRender: FairyGUI.GTree.TreeNodeRenderDelegate;
            public treeNodeWillExpand: FairyGUI.GTree.TreeNodeWillExpandDelegate;
            public rootNode: FairyGUI.GTreeNode;
            public indent: number;
            public clickToExpand: number;
            public constructor();
            public GetSelectedNode():FairyGUI.GTreeNode;
            public GetSelectedNodes():System.Collections.Generic.List$1<FairyGUI.GTreeNode>;
            public GetSelectedNodes($result: System.Collections.Generic.List$1<FairyGUI.GTreeNode>):System.Collections.Generic.List$1<FairyGUI.GTreeNode>;
            public SelectNode($node: FairyGUI.GTreeNode):void;
            public SelectNode($node: FairyGUI.GTreeNode, $scrollItToView: boolean):void;
            public UnselectNode($node: FairyGUI.GTreeNode):void;
            public ExpandAll():void;
            public ExpandAll($folderNode: FairyGUI.GTreeNode):void;
            public CollapseAll():void;
            public CollapseAll($folderNode: FairyGUI.GTreeNode):void;
            
        }
        class GList extends FairyGUI.GComponent {
            public defaultItem: string;
            public foldInvisibleItems: boolean;
            public selectionMode: FairyGUI.ListSelectionMode;
            public itemRenderer: FairyGUI.ListItemRenderer;
            public itemProvider: FairyGUI.ListItemProvider;
            public scrollItemToViewOnClick: boolean;
            public onClickItem: FairyGUI.EventListener;
            public onRightClickItem: FairyGUI.EventListener;
            public layout: FairyGUI.ListLayoutType;
            public lineCount: number;
            public columnCount: number;
            public lineGap: number;
            public columnGap: number;
            public align: FairyGUI.AlignType;
            public verticalAlign: FairyGUI.VertAlignType;
            public autoResizeItem: boolean;
            public defaultItemSize: UnityEngine.Vector2;
            public itemPool: FairyGUI.GObjectPool;
            public selectedIndex: number;
            public selectionController: FairyGUI.Controller;
            public touchItem: FairyGUI.GObject;
            public isVirtual: boolean;
            public numItems: number;
            public constructor();
            public GetFromPool($url: string):FairyGUI.GObject;
            public AddItemFromPool():FairyGUI.GObject;
            public AddItemFromPool($url: string):FairyGUI.GObject;
            public RemoveChildToPoolAt($index: number):void;
            public RemoveChildToPool($child: FairyGUI.GObject):void;
            public RemoveChildrenToPool():void;
            public RemoveChildrenToPool($beginIndex: number, $endIndex: number):void;
            public GetSelection():System.Collections.Generic.List$1<number>;
            public GetSelection($result: System.Collections.Generic.List$1<number>):System.Collections.Generic.List$1<number>;
            public AddSelection($index: number, $scrollItToView: boolean):void;
            public RemoveSelection($index: number):void;
            public ClearSelection():void;
            public SelectAll():void;
            public SelectNone():void;
            public SelectReverse():void;
            public EnableSelectionFocusEvents($enabled: boolean):void;
            public EnableArrowKeyNavigation($enabled: boolean):void;
            public HandleArrowKey($dir: number):number;
            public ResizeToFit():void;
            public ResizeToFit($itemCount: number):void;
            public ResizeToFit($itemCount: number, $minSize: number):void;
            public ScrollToView($index: number):void;
            public ScrollToView($index: number, $ani: boolean):void;
            public ScrollToView($index: number, $ani: boolean, $setFirst: boolean):void;
            public ChildIndexToItemIndex($index: number):number;
            public ItemIndexToChildIndex($index: number):number;
            public SetVirtual():void;
            public SetVirtualAndLoop():void;
            public RefreshVirtualList():void;
            
        }
        class GTreeNode extends System.Object {
            public data: any;
            public parent: FairyGUI.GTreeNode;
            public tree: FairyGUI.GTree;
            public cell: FairyGUI.GComponent;
            public level: number;
            public expanded: boolean;
            public isFolder: boolean;
            public text: string;
            public icon: string;
            public numChildren: number;
            public constructor($hasChild: boolean);
            public constructor($hasChild: boolean, $resURL: string);
            public ExpandToRoot():void;
            public AddChild($child: FairyGUI.GTreeNode):FairyGUI.GTreeNode;
            public AddChildAt($child: FairyGUI.GTreeNode, $index: number):FairyGUI.GTreeNode;
            public RemoveChild($child: FairyGUI.GTreeNode):FairyGUI.GTreeNode;
            public RemoveChildAt($index: number):FairyGUI.GTreeNode;
            public RemoveChildren($beginIndex?: number, $endIndex?: number):void;
            public GetChildAt($index: number):FairyGUI.GTreeNode;
            public GetChildIndex($child: FairyGUI.GTreeNode):number;
            public GetPrevSibling():FairyGUI.GTreeNode;
            public GetNextSibling():FairyGUI.GTreeNode;
            public SetChildIndex($child: FairyGUI.GTreeNode, $index: number):void;
            public SwapChildren($child1: FairyGUI.GTreeNode, $child2: FairyGUI.GTreeNode):void;
            public SwapChildrenAt($index1: number, $index2: number):void;
            
        }
        class GLabel extends FairyGUI.GComponent {
            public icon: string;
            public title: string;
            public text: string;
            public editable: boolean;
            public titleColor: UnityEngine.Color;
            public titleFontSize: number;
            public color: UnityEngine.Color;
            public constructor();
            public GetTextField():FairyGUI.GTextField;
            
        }
        class GButton extends FairyGUI.GComponent {
            public sound: FairyGUI.NAudioClip;
            public soundVolumeScale: number;
            public changeStateOnClick: boolean;
            public linkedPopup: FairyGUI.GObject;
            public static UP: string;
            public static DOWN: string;
            public static OVER: string;
            public static SELECTED_OVER: string;
            public static DISABLED: string;
            public static SELECTED_DISABLED: string;
            public onChanged: FairyGUI.EventListener;
            public icon: string;
            public title: string;
            public text: string;
            public selectedIcon: string;
            public selectedTitle: string;
            public titleColor: UnityEngine.Color;
            public color: UnityEngine.Color;
            public titleFontSize: number;
            public selected: boolean;
            public mode: FairyGUI.ButtonMode;
            public relatedController: FairyGUI.Controller;
            public relatedPageId: string;
            public constructor();
            public FireClick($downEffect: boolean, $clickCall?: boolean):void;
            public GetTextField():FairyGUI.GTextField;
            
        }
        class GTextField extends FairyGUI.GObject {
            public text: string;
            public templateVars: System.Collections.Generic.Dictionary$2<string, string>;
            public textFormat: FairyGUI.TextFormat;
            public color: UnityEngine.Color;
            public align: FairyGUI.AlignType;
            public verticalAlign: FairyGUI.VertAlignType;
            public singleLine: boolean;
            public stroke: number;
            public strokeColor: UnityEngine.Color;
            public shadowOffset: UnityEngine.Vector2;
            public UBBEnabled: boolean;
            public autoSize: FairyGUI.AutoSizeType;
            public textWidth: number;
            public textHeight: number;
            public constructor();
            public SetVar($name: string, $value: string):FairyGUI.GTextField;
            public FlushVars():void;
            public HasCharacter($ch: number):boolean;
            
        }
        enum GroupLayoutType { None = 0, Horizontal = 1, Vertical = 2 }
        class BlendModeUtils extends System.Object {
            public static Factors: System.Array$1<FairyGUI.BlendModeUtils.BlendFactor>;
            public constructor();
            public static Apply($mat: UnityEngine.Material, $blendMode: FairyGUI.BlendMode):void;
            public static Override($blendMode: FairyGUI.BlendMode, $srcFactor: UnityEngine.Rendering.BlendMode, $dstFactor: UnityEngine.Rendering.BlendMode):void;
            
        }
        enum BlendMode { Normal = 0, None = 1, Add = 2, Multiply = 3, Screen = 4, Erase = 5, Mask = 6, Below = 7, Off = 8, One_OneMinusSrcAlpha = 9, Custom1 = 10, Custom2 = 11, Custom3 = 12 }
        class CaptureCamera extends UnityEngine.MonoBehaviour {
            public cachedTransform: UnityEngine.Transform;
            public cachedCamera: UnityEngine.Camera;
            public static Name: string;
            public static LayerName: string;
            public static HiddenLayerName: string;
            public static layer: number;
            public static hiddenLayer: number;
            public constructor();
            public static CheckMain():void;
            public static CreateRenderTexture($width: number, $height: number, $stencilSupport: boolean):UnityEngine.RenderTexture;
            public static Capture($target: FairyGUI.DisplayObject, $texture: UnityEngine.RenderTexture, $contentHeight: number, $offset: UnityEngine.Vector2):void;
            
        }
        interface IHitTest {
            HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class UpdateContext extends System.Object {
            public clipped: boolean;
            public clipInfo: FairyGUI.UpdateContext.ClipInfo;
            public renderingOrder: number;
            public batchingDepth: number;
            public rectMaskDepth: number;
            public stencilReferenceValue: number;
            public stencilCompareValue: number;
            public alpha: number;
            public grayed: boolean;
            public static current: FairyGUI.UpdateContext;
            public static working: boolean;
            public constructor();
            public static add_OnBegin($value: System.Action):void;
            public static remove_OnBegin($value: System.Action):void;
            public static add_OnEnd($value: System.Action):void;
            public static remove_OnEnd($value: System.Action):void;
            public Begin():void;
            public End():void;
            public EnterClipping($clipId: number, $clipRect: UnityEngine.Rect, $softness: System.Nullable$1<UnityEngine.Vector4>):void;
            public EnterClipping($clipId: number, $reversedMask: boolean):void;
            public LeaveClipping():void;
            public EnterPaintingMode():void;
            public LeavePaintingMode():void;
            public ApplyClippingProperties($mat: UnityEngine.Material, $isStdMaterial: boolean):void;
            public ApplyAlphaMaskProperties($mat: UnityEngine.Material, $erasing: boolean):void;
            
        }
        class NGraphics extends System.Object {
            public blendMode: FairyGUI.BlendMode;
            public dontClip: boolean;
            public gameObject: UnityEngine.GameObject;
            public meshFilter: UnityEngine.MeshFilter;
            public meshRenderer: UnityEngine.MeshRenderer;
            public mesh: UnityEngine.Mesh;
            public meshFactory: FairyGUI.IMeshFactory;
            public contentRect: UnityEngine.Rect;
            public flip: FairyGUI.FlipType;
            public texture: FairyGUI.NTexture;
            public shader: string;
            public material: UnityEngine.Material;
            public materialKeywords: System.Array$1<string>;
            public enabled: boolean;
            public sortingOrder: number;
            public color: UnityEngine.Color;
            public vertexMatrix: FairyGUI.NGraphics.VertexMatrix;
            public materialPropertyBlock: UnityEngine.MaterialPropertyBlock;
            public constructor($gameObject: UnityEngine.GameObject);
            public add_meshModifier($value: System.Action):void;
            public remove_meshModifier($value: System.Action):void;
            public SetShaderAndTexture($shader: string, $texture: FairyGUI.NTexture):void;
            public SetMaterial($material: UnityEngine.Material):void;
            public ToggleKeyword($keyword: string, $enabled: boolean):void;
            public Tint():void;
            public SetMeshDirty():void;
            public UpdateMesh():boolean;
            public Dispose():void;
            public Update($context: FairyGUI.UpdateContext, $alpha: number, $grayed: boolean):void;
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class Stage extends FairyGUI.Container {
            public soundVolume: number;
            public static inst: FairyGUI.Stage;
            public static touchScreen: boolean;
            public static keyboardInput: boolean;
            public static isTouchOnUI: boolean;
            public static devicePixelRatio: number;
            public onStageResized: FairyGUI.EventListener;
            public touchTarget: FairyGUI.DisplayObject;
            public focus: FairyGUI.DisplayObject;
            public touchPosition: UnityEngine.Vector2;
            public touchCount: number;
            public keyboard: FairyGUI.IKeyboard;
            public activeCursor: string;
            public constructor();
            public add_beforeUpdate($value: System.Action):void;
            public remove_beforeUpdate($value: System.Action):void;
            public add_afterUpdate($value: System.Action):void;
            public remove_afterUpdate($value: System.Action):void;
            public static Instantiate():void;
            public SetFous($newFocus: FairyGUI.DisplayObject, $byKey?: boolean):void;
            public DoKeyNavigate($backward: boolean):void;
            public GetTouchPosition($touchId: number):UnityEngine.Vector2;
            public GetTouchTarget($touchId: number):FairyGUI.DisplayObject;
            public GetAllTouch($result: System.Array$1<number>):System.Array$1<number>;
            public ResetInputState():void;
            public CancelClick($touchId: number):void;
            public EnableSound():void;
            public DisableSound():void;
            public PlayOneShotSound($clip: UnityEngine.AudioClip, $volumeScale: number):void;
            public PlayOneShotSound($clip: UnityEngine.AudioClip):void;
            public OpenKeyboard($text: string, $autocorrection: boolean, $multiline: boolean, $secure: boolean, $alert: boolean, $textPlaceholder: string, $keyboardType: number, $hideInput: boolean):void;
            public CloseKeyboard():void;
            public InputString($value: string):void;
            public SetCustomInput($screenPos: UnityEngine.Vector2, $buttonDown: boolean):void;
            public SetCustomInput($screenPos: UnityEngine.Vector2, $buttonDown: boolean, $buttonUp: boolean):void;
            public SetCustomInput($hit: $Ref<UnityEngine.RaycastHit>, $buttonDown: boolean):void;
            public SetCustomInput($hit: $Ref<UnityEngine.RaycastHit>, $buttonDown: boolean, $buttonUp: boolean):void;
            public ForceUpdate():void;
            public ApplyPanelOrder($target: FairyGUI.Container):void;
            public SortWorldSpacePanelsByZOrder($panelSortingOrder: number):void;
            public MonitorTexture($texture: FairyGUI.NTexture):void;
            public AddTouchMonitor($touchId: number, $target: FairyGUI.EventDispatcher):void;
            public RemoveTouchMonitor($target: FairyGUI.EventDispatcher):void;
            public IsTouchMonitoring($target: FairyGUI.EventDispatcher):boolean;
            public RegisterCursor($cursorName: string, $texture: UnityEngine.Texture2D, $hotspot: UnityEngine.Vector2):void;
            
        }
        class Margin extends System.ValueType {
            public left: number;
            public right: number;
            public top: number;
            public bottom: number;
            
        }
        interface IFilter {
            target: FairyGUI.DisplayObject;
            Update():void;
            Dispose():void;
            
        }
        class DisplayObjectInfo extends UnityEngine.MonoBehaviour {
            public displayObject: FairyGUI.DisplayObject;
            public constructor();
            
        }
        class GoWrapper extends FairyGUI.DisplayObject {
            public wrapTarget: UnityEngine.GameObject;
            public renderingOrder: number;
            public constructor();
            public constructor($go: UnityEngine.GameObject);
            public add_onUpdate($value: System.Action$1<FairyGUI.UpdateContext>):void;
            public remove_onUpdate($value: System.Action$1<FairyGUI.UpdateContext>):void;
            public SetWrapTarget($target: UnityEngine.GameObject, $cloneMaterial: boolean):void;
            public CacheRenderers():void;
            
        }
        class ColliderHitTest extends System.Object {
            public collider: UnityEngine.Collider;
            public constructor();
            public HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class HitTestContext extends System.Object {
            public static screenPoint: UnityEngine.Vector3;
            public static worldPoint: UnityEngine.Vector3;
            public static direction: UnityEngine.Vector3;
            public static forTouch: boolean;
            public static camera: UnityEngine.Camera;
            public static layerMask: number;
            public static maxDistance: number;
            public static cachedMainCamera: UnityEngine.Camera;
            public constructor();
            public static GetRaycastHitFromCache($camera: UnityEngine.Camera, $hit: $Ref<UnityEngine.RaycastHit>):boolean;
            public static CacheRaycastHit($camera: UnityEngine.Camera, $hit: $Ref<UnityEngine.RaycastHit>):void;
            public static ClearRaycastHitCache():void;
            
        }
        class MeshColliderHitTest extends FairyGUI.ColliderHitTest {
            public lastHit: UnityEngine.Vector2;
            public constructor($collider: UnityEngine.MeshCollider);
            
        }
        class PixelHitTestData extends System.Object {
            public pixelWidth: number;
            public scale: number;
            public pixels: System.Array$1<number>;
            public pixelsLength: number;
            public pixelsOffset: number;
            public constructor();
            public Load($ba: FairyGUI.Utils.ByteBuffer):void;
            
        }
        class PixelHitTest extends System.Object {
            public offsetX: number;
            public offsetY: number;
            public sourceWidth: number;
            public sourceHeight: number;
            public constructor($data: FairyGUI.PixelHitTestData, $offsetX: number, $offsetY: number, $sourceWidth: number, $sourceHeight: number);
            public HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class RectHitTest extends System.Object {
            public rect: UnityEngine.Rect;
            public constructor();
            public HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class ShapeHitTest extends System.Object {
            public shape: FairyGUI.DisplayObject;
            public constructor($obj: FairyGUI.DisplayObject);
            public HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class MaterialManager extends System.Object {
            public firstMaterialInFrame: boolean;
            public add_onCreateNewMaterial($value: System.Action$1<UnityEngine.Material>):void;
            public remove_onCreateNewMaterial($value: System.Action$1<UnityEngine.Material>):void;
            public GetFlagsByKeywords($keywords: System.Collections.Generic.IList$1<string>):number;
            public GetMaterial($flags: number, $blendMode: FairyGUI.BlendMode, $group: number):UnityEngine.Material;
            public DestroyMaterials():void;
            public RefreshMaterials():void;
            
        }
        class CompositeMesh extends System.Object {
            public elements: System.Collections.Generic.List$1<FairyGUI.IMeshFactory>;
            public activeIndex: number;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public HitTest($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2):boolean;
            
        }
        interface IMeshFactory {
            OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class EllipseMesh extends System.Object {
            public drawRect: System.Nullable$1<UnityEngine.Rect>;
            public lineWidth: number;
            public lineColor: UnityEngine.Color32;
            public centerColor: System.Nullable$1<UnityEngine.Color32>;
            public fillColor: System.Nullable$1<UnityEngine.Color32>;
            public startDegree: number;
            public endDegreee: number;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public HitTest($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2):boolean;
            
        }
        class FillMesh extends System.Object {
            public method: FairyGUI.FillMethod;
            public origin: number;
            public amount: number;
            public clockwise: boolean;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class PlaneMesh extends System.Object {
            public gridSize: number;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class PolygonMesh extends System.Object {
            public points: System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public texcoords: System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public lineWidth: number;
            public lineColor: UnityEngine.Color32;
            public fillColor: System.Nullable$1<UnityEngine.Color32>;
            public colors: System.Array$1<UnityEngine.Color32>;
            public usePercentPositions: boolean;
            public constructor();
            public Add($point: UnityEngine.Vector2):void;
            public Add($point: UnityEngine.Vector2, $texcoord: UnityEngine.Vector2):void;
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public HitTest($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2):boolean;
            
        }
        class RectMesh extends System.Object {
            public drawRect: System.Nullable$1<UnityEngine.Rect>;
            public lineWidth: number;
            public lineColor: UnityEngine.Color32;
            public fillColor: System.Nullable$1<UnityEngine.Color32>;
            public colors: System.Array$1<UnityEngine.Color32>;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public HitTest($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2):boolean;
            
        }
        class RegularPolygonMesh extends System.Object {
            public drawRect: System.Nullable$1<UnityEngine.Rect>;
            public sides: number;
            public lineWidth: number;
            public lineColor: UnityEngine.Color32;
            public centerColor: System.Nullable$1<UnityEngine.Color32>;
            public fillColor: System.Nullable$1<UnityEngine.Color32>;
            public distances: System.Array$1<number>;
            public rotation: number;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public HitTest($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2):boolean;
            
        }
        class RoundedRectMesh extends System.Object {
            public drawRect: System.Nullable$1<UnityEngine.Rect>;
            public lineWidth: number;
            public lineColor: UnityEngine.Color32;
            public fillColor: System.Nullable$1<UnityEngine.Color32>;
            public topLeftRadius: number;
            public topRightRadius: number;
            public bottomLeftRadius: number;
            public bottomRightRadius: number;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            public HitTest($contentRect: UnityEngine.Rect, $point: UnityEngine.Vector2):boolean;
            
        }
        class MovieClip extends FairyGUI.Image {
            public interval: number;
            public swing: boolean;
            public repeatDelay: number;
            public timeScale: number;
            public ignoreEngineTimeScale: boolean;
            public onPlayEnd: FairyGUI.EventListener;
            public frames: System.Array$1<FairyGUI.MovieClip.Frame>;
            public playing: boolean;
            public frame: number;
            public constructor();
            public Rewind():void;
            public SyncStatus($anotherMc: FairyGUI.MovieClip):void;
            public Advance($time: number):void;
            public SetPlaySettings():void;
            public SetPlaySettings($start: number, $end: number, $times: number, $endAt: number):void;
            
        }
        class NAudioClip extends System.Object {
            public static CustomDestroyMethod: System.Action$1<UnityEngine.AudioClip>;
            public destroyMethod: FairyGUI.DestroyMethod;
            public nativeClip: UnityEngine.AudioClip;
            public constructor($audioClip: UnityEngine.AudioClip);
            public Unload():void;
            public Reload($audioClip: UnityEngine.AudioClip):void;
            
        }
        enum DestroyMethod { Destroy = 0, Unload = 1, None = 2, ReleaseTemp = 3, Custom = 4 }
        class ShaderConfig extends System.Object {
            public static Get: FairyGUI.ShaderConfig.GetFunction;
            public static imageShader: string;
            public static textShader: string;
            public static bmFontShader: string;
            public static TMPFontShader: string;
            public static ID_ClipBox: number;
            public static ID_ClipSoftness: number;
            public static ID_AlphaTex: number;
            public static ID_StencilComp: number;
            public static ID_Stencil: number;
            public static ID_StencilOp: number;
            public static ID_StencilReadMask: number;
            public static ID_ColorMask: number;
            public static ID_ColorMatrix: number;
            public static ID_ColorOffset: number;
            public static ID_BlendSrcFactor: number;
            public static ID_BlendDstFactor: number;
            public static ID_ColorOption: number;
            public static ID_Stencil2: number;
            public static GetShader($name: string):UnityEngine.Shader;
            
        }
        interface IKeyboard {
            done: boolean;
            supportsCaret: boolean;
            GetInput():string;
            Open($text: string, $autocorrection: boolean, $multiline: boolean, $secure: boolean, $alert: boolean, $textPlaceholder: string, $keyboardType: number, $hideInput: boolean):void;
            Close():void;
            
        }
        class StageCamera extends UnityEngine.MonoBehaviour {
            public constantSize: boolean;
            public unitsPerPixel: number;
            public cachedTransform: UnityEngine.Transform;
            public cachedCamera: UnityEngine.Camera;
            public static main: UnityEngine.Camera;
            public static screenSizeVer: number;
            public static Name: string;
            public static LayerName: string;
            public static DefaultCameraSize: number;
            public static DefaultUnitsPerPixel: number;
            public constructor();
            public ApplyModifiedProperties():void;
            public static CheckMainCamera():void;
            public static CheckCaptureCamera():void;
            public static CreateCamera($name: string, $cullingMask: number):UnityEngine.Camera;
            
        }
        class StageEngine extends UnityEngine.MonoBehaviour {
            public ObjectsOnStage: number;
            public GraphicsOnStage: number;
            public static beingQuit: boolean;
            public constructor();
            
        }
        class Stats extends System.Object {
            public static ObjectCount: number;
            public static GraphicsCount: number;
            public static LatestObjectCreation: number;
            public static LatestGraphicsCreation: number;
            public constructor();
            
        }
        class DynamicFont extends FairyGUI.BaseFont {
            public nativeFont: UnityEngine.Font;
            public constructor();
            public constructor($name: string, $font: UnityEngine.Font);
            
        }
        class Emoji extends System.Object {
            public url: string;
            public width: number;
            public height: number;
            public constructor($url: string, $width: number, $height: number);
            public constructor($url: string);
            
        }
        class FontManager extends System.Object {
            public static sFontFactory: System.Collections.Generic.Dictionary$2<string, FairyGUI.BaseFont>;
            public constructor();
            public static RegisterFont($font: FairyGUI.BaseFont, $alias?: string):void;
            public static UnregisterFont($font: FairyGUI.BaseFont):void;
            public static GetFont($name: string):FairyGUI.BaseFont;
            public static Clear():void;
            
        }
        class InputTextField extends FairyGUI.RichTextField {
            public static onCopy: System.Action$2<FairyGUI.InputTextField, string>;
            public static onPaste: System.Action$1<FairyGUI.InputTextField>;
            public static contextMenu: FairyGUI.PopupMenu;
            public maxLength: number;
            public keyboardInput: boolean;
            public keyboardType: number;
            public hideInput: boolean;
            public disableIME: boolean;
            public mouseWheelEnabled: boolean;
            public onChanged: FairyGUI.EventListener;
            public onSubmit: FairyGUI.EventListener;
            public text: string;
            public textFormat: FairyGUI.TextFormat;
            public restrict: string;
            public caretPosition: number;
            public selectionBeginIndex: number;
            public selectionEndIndex: number;
            public promptText: string;
            public displayAsPassword: boolean;
            public editable: boolean;
            public border: number;
            public corner: number;
            public borderColor: UnityEngine.Color;
            public backgroundColor: UnityEngine.Color;
            public constructor();
            public SetSelection($start: number, $length: number):void;
            public ReplaceSelection($value: string):void;
            public ReplaceText($value: string):void;
            public GetSelection():string;
            
        }
        class PopupMenu extends FairyGUI.EventDispatcher {
            public visibleItemCount: number;
            public hideOnClickItem: boolean;
            public autoSize: boolean;
            public onPopup: FairyGUI.EventListener;
            public onClose: FairyGUI.EventListener;
            public itemCount: number;
            public contentPane: FairyGUI.GComponent;
            public list: FairyGUI.GList;
            public constructor();
            public constructor($resourceURL: string);
            public AddItem($caption: string, $callback: FairyGUI.EventCallback0):FairyGUI.GButton;
            public AddItem($caption: string, $callback: FairyGUI.EventCallback1):FairyGUI.GButton;
            public AddItemAt($caption: string, $index: number, $callback: FairyGUI.EventCallback1):FairyGUI.GButton;
            public AddItemAt($caption: string, $index: number, $callback: FairyGUI.EventCallback0):FairyGUI.GButton;
            public AddSeperator():void;
            public AddSeperator($index: number):void;
            public GetItemName($index: number):string;
            public SetItemText($name: string, $caption: string):void;
            public SetItemVisible($name: string, $visible: boolean):void;
            public SetItemGrayed($name: string, $grayed: boolean):void;
            public SetItemCheckable($name: string, $checkable: boolean):void;
            public SetItemChecked($name: string, $check: boolean):void;
            public IsItemChecked($name: string):boolean;
            public RemoveItem($name: string):void;
            public ClearItems():void;
            public Dispose():void;
            public Show():void;
            public Show($target: FairyGUI.GObject):void;
            public Show($target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection):void;
            public Show($target: FairyGUI.GObject, $dir: FairyGUI.PopupDirection, $parentMenu: FairyGUI.PopupMenu):void;
            public Hide():void;
            
        }
        class TextField extends FairyGUI.DisplayObject {
            public textFormat: FairyGUI.TextFormat;
            public align: FairyGUI.AlignType;
            public verticalAlign: FairyGUI.VertAlignType;
            public text: string;
            public htmlText: string;
            public parsedText: string;
            public autoSize: FairyGUI.AutoSizeType;
            public wordWrap: boolean;
            public singleLine: boolean;
            public stroke: number;
            public strokeColor: UnityEngine.Color;
            public shadowOffset: UnityEngine.Vector2;
            public textWidth: number;
            public textHeight: number;
            public maxWidth: number;
            public htmlElements: System.Collections.Generic.List$1<FairyGUI.Utils.HtmlElement>;
            public lines: System.Collections.Generic.List$1<FairyGUI.TextField.LineInfo>;
            public charPositions: System.Collections.Generic.List$1<FairyGUI.TextField.CharPosition>;
            public richTextField: FairyGUI.RichTextField;
            public constructor();
            public EnableCharPositionSupport():void;
            public ApplyFormat():void;
            public Redraw():boolean;
            public HasCharacter($ch: number):boolean;
            public GetLinesShape($startLine: number, $startCharX: number, $endLine: number, $endCharX: number, $clipped: boolean, $resultRects: System.Collections.Generic.List$1<UnityEngine.Rect>):void;
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class RTLSupport extends System.Object {
            public static BaseDirection: FairyGUI.RTLSupport.DirectionType;
            public constructor();
            public static IsArabicLetter($ch: number):boolean;
            public static ConvertNumber($strNumber: string):string;
            public static ContainsArabicLetters($text: string):boolean;
            public static DetectTextDirection($text: string):FairyGUI.RTLSupport.DirectionType;
            public static DoMapping($input: string):string;
            public static ConvertLineL($source: string):string;
            public static ConvertLineR($source: string):string;
            
        }
        class SelectionShape extends FairyGUI.DisplayObject {
            public rects: System.Collections.Generic.List$1<UnityEngine.Rect>;
            public color: UnityEngine.Color;
            public constructor();
            public Refresh():void;
            public Clear():void;
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class TouchScreenKeyboard extends System.Object {
            public done: boolean;
            public supportsCaret: boolean;
            public constructor();
            public GetInput():string;
            public Open($text: string, $autocorrection: boolean, $multiline: boolean, $secure: boolean, $alert: boolean, $textPlaceholder: string, $keyboardType: number, $hideInput: boolean):void;
            public Close():void;
            
        }
        class TypingEffect extends System.Object {
            public constructor($textField: FairyGUI.TextField);
            public constructor($textField: FairyGUI.GTextField);
            public Start():void;
            public Print():boolean;
            public Print($interval: number):System.Collections.IEnumerator;
            public PrintAll($interval: number):void;
            public Cancel():void;
            
        }
        type EventCallback0 = () => void;
        var EventCallback0: {new (func: () => void): EventCallback0;}
        interface IEventDispatcher {
            AddEventListener($strType: string, $callback: FairyGUI.EventCallback0):void;
            AddEventListener($strType: string, $callback: FairyGUI.EventCallback1):void;
            RemoveEventListener($strType: string, $callback: FairyGUI.EventCallback0):void;
            RemoveEventListener($strType: string, $callback: FairyGUI.EventCallback1):void;
            DispatchEvent($context: FairyGUI.EventContext):boolean;
            DispatchEvent($strType: string):boolean;
            DispatchEvent($strType: string, $data: any):boolean;
            DispatchEvent($strType: string, $data: any, $initiator: any):boolean;
            
        }
        class GLoader3D extends FairyGUI.GObject {
            public armatureComponent: DragonBones.UnityArmatureComponent;
            public spineAnimation: Spine.Unity.SkeletonAnimation;
            public url: string;
            public icon: string;
            public align: FairyGUI.AlignType;
            public verticalAlign: FairyGUI.VertAlignType;
            public fill: FairyGUI.FillType;
            public shrinkOnly: boolean;
            public autoSize: boolean;
            public playing: boolean;
            public frame: number;
            public timeScale: number;
            public ignoreEngineTimeScale: boolean;
            public loop: boolean;
            public animationName: string;
            public skinName: string;
            public material: UnityEngine.Material;
            public shader: string;
            public color: UnityEngine.Color;
            public wrapTarget: UnityEngine.GameObject;
            public filter: FairyGUI.IFilter;
            public blendMode: FairyGUI.BlendMode;
            public constructor();
            public SetDragonBones($asset: DragonBones.DragonBonesData, $width: number, $height: number, $anchor: UnityEngine.Vector2):void;
            public SetSpine($asset: Spine.Unity.SkeletonDataAsset, $width: number, $height: number, $anchor: UnityEngine.Vector2):void;
            public Advance($time: number):void;
            public SetWrapTarget($gameObject: UnityEngine.GameObject, $cloneMaterial: boolean, $width: number, $height: number):void;
            
        }
        enum FillType { None = 0, Scale = 1, ScaleMatchHeight = 2, ScaleMatchWidth = 3, ScaleFree = 4, ScaleNoBorder = 5 }
        class ExternalFont extends FairyGUI.BaseFont {
            public samplePointSize: number;
            public renderMode: UnityEngine.TextCore.LowLevel.GlyphRenderMode;
            public constructor();
            public Load($file: string):void;
            
        }
        class ExternalTMPFont extends FairyGUI.TMPFont {
            public constructor();
            public Load($file: string, $samplePointSize: number):void;
            
        }
        class TMPFont extends FairyGUI.BaseFont {
            public fontAsset: TMPro.TMP_FontAsset;
            public fontWeight: TMPro.FontWeight;
            public constructor();
            
        }
        class BlurFilter extends System.Object {
            public blurSize: number;
            public target: FairyGUI.DisplayObject;
            public constructor();
            public Dispose():void;
            public Update():void;
            
        }
        class ColorFilter extends System.Object {
            public target: FairyGUI.DisplayObject;
            public constructor();
            public Dispose():void;
            public Update():void;
            public Invert():void;
            public AdjustSaturation($sat: number):void;
            public AdjustContrast($value: number):void;
            public AdjustBrightness($value: number):void;
            public AdjustHue($value: number):void;
            public Tint($color: UnityEngine.Color, $amount?: number):void;
            public Reset():void;
            public ConcatValues(...values: number[]):void;
            
        }
        class LongPressGesture extends FairyGUI.EventDispatcher {
            public trigger: number;
            public interval: number;
            public once: boolean;
            public holdRangeRadius: number;
            public static TRIGGER: number;
            public static INTERVAL: number;
            public host: FairyGUI.GObject;
            public onBegin: FairyGUI.EventListener;
            public onEnd: FairyGUI.EventListener;
            public onAction: FairyGUI.EventListener;
            public constructor($host: FairyGUI.GObject);
            public Dispose():void;
            public Enable($value: boolean):void;
            public Cancel():void;
            
        }
        class PinchGesture extends FairyGUI.EventDispatcher {
            public scale: number;
            public delta: number;
            public host: FairyGUI.GObject;
            public onBegin: FairyGUI.EventListener;
            public onEnd: FairyGUI.EventListener;
            public onAction: FairyGUI.EventListener;
            public constructor($host: FairyGUI.GObject);
            public Dispose():void;
            public Enable($value: boolean):void;
            
        }
        class RotationGesture extends FairyGUI.EventDispatcher {
            public rotation: number;
            public delta: number;
            public snapping: boolean;
            public host: FairyGUI.GObject;
            public onBegin: FairyGUI.EventListener;
            public onEnd: FairyGUI.EventListener;
            public onAction: FairyGUI.EventListener;
            public constructor($host: FairyGUI.GObject);
            public Dispose():void;
            public Enable($value: boolean):void;
            
        }
        class SwipeGesture extends FairyGUI.EventDispatcher {
            public velocity: UnityEngine.Vector2;
            public position: UnityEngine.Vector2;
            public delta: UnityEngine.Vector2;
            public actionDistance: number;
            public snapping: boolean;
            public static ACTION_DISTANCE: number;
            public host: FairyGUI.GObject;
            public onBegin: FairyGUI.EventListener;
            public onEnd: FairyGUI.EventListener;
            public onMove: FairyGUI.EventListener;
            public onAction: FairyGUI.EventListener;
            public constructor($host: FairyGUI.GObject);
            public Dispose():void;
            public Enable($value: boolean):void;
            
        }
        class EaseManager extends System.Object {
            public static Evaluate($easeType: FairyGUI.EaseType, $time: number, $duration: number, $overshootOrAmplitude?: number, $period?: number, $customEase?: FairyGUI.CustomEase):number;
            
        }
        class GTween extends System.Object {
            public static catchCallbackExceptions: boolean;
            public constructor();
            public static To($startValue: number, $endValue: number, $duration: number):FairyGUI.GTweener;
            public static To($startValue: UnityEngine.Vector2, $endValue: UnityEngine.Vector2, $duration: number):FairyGUI.GTweener;
            public static To($startValue: UnityEngine.Vector3, $endValue: UnityEngine.Vector3, $duration: number):FairyGUI.GTweener;
            public static To($startValue: UnityEngine.Vector4, $endValue: UnityEngine.Vector4, $duration: number):FairyGUI.GTweener;
            public static To($startValue: UnityEngine.Color, $endValue: UnityEngine.Color, $duration: number):FairyGUI.GTweener;
            public static ToDouble($startValue: number, $endValue: number, $duration: number):FairyGUI.GTweener;
            public static DelayedCall($delay: number):FairyGUI.GTweener;
            public static Shake($startValue: UnityEngine.Vector3, $amplitude: number, $duration: number):FairyGUI.GTweener;
            public static IsTweening($target: any):boolean;
            public static IsTweening($target: any, $propType: FairyGUI.TweenPropType):boolean;
            public static Kill($target: any):void;
            public static Kill($target: any, $complete: boolean):void;
            public static Kill($target: any, $propType: FairyGUI.TweenPropType, $complete: boolean):void;
            public static GetTween($target: any):FairyGUI.GTweener;
            public static GetTween($target: any, $propType: FairyGUI.TweenPropType):FairyGUI.GTweener;
            public static Clean():void;
            
        }
        enum TweenPropType { None = 0, X = 1, Y = 2, Z = 3, XY = 4, Position = 5, Width = 6, Height = 7, Size = 8, ScaleX = 9, ScaleY = 10, Scale = 11, Rotation = 12, RotationX = 13, RotationY = 14, Alpha = 15, Progress = 16 }
        interface ITweenListener {
            OnTweenStart($tweener: FairyGUI.GTweener):void;
            OnTweenUpdate($tweener: FairyGUI.GTweener):void;
            OnTweenComplete($tweener: FairyGUI.GTweener):void;
            
        }
        type GTweenCallback = () => void;
        var GTweenCallback: {new (func: () => void): GTweenCallback;}
        type GTweenCallback1 = (tweener: FairyGUI.GTweener) => void;
        var GTweenCallback1: {new (func: (tweener: FairyGUI.GTweener) => void): GTweenCallback1;}
        class TweenValue extends System.Object {
            public x: number;
            public y: number;
            public z: number;
            public w: number;
            public d: number;
            public vec2: UnityEngine.Vector2;
            public vec3: UnityEngine.Vector3;
            public vec4: UnityEngine.Vector4;
            public color: UnityEngine.Color;
            public constructor();
            public get_Item($index: number):number;
            public set_Item($index: number, $value: number):void;
            public SetZero():void;
            
        }
        class ChangePageAction extends FairyGUI.ControllerAction {
            public objectId: string;
            public controllerName: string;
            public targetPage: string;
            public constructor();
            
        }
        class ControllerAction extends System.Object {
            public fromPage: System.Array$1<string>;
            public toPage: System.Array$1<string>;
            public constructor();
            public static CreateAction($type: FairyGUI.ControllerAction.ActionType):FairyGUI.ControllerAction;
            public Run($controller: FairyGUI.Controller, $prevPage: string, $curPage: string):void;
            public Setup($buffer: FairyGUI.Utils.ByteBuffer):void;
            
        }
        class Controller extends FairyGUI.EventDispatcher {
            public name: string;
            public onChanged: FairyGUI.EventListener;
            public selectedIndex: number;
            public selectedPage: string;
            public previsousIndex: number;
            public previousPage: string;
            public pageCount: number;
            public constructor();
            public Dispose():void;
            public SetSelectedIndex($value: number):void;
            public SetSelectedPage($value: string):void;
            public GetPageName($index: number):string;
            public GetPageId($index: number):string;
            public GetPageIdByName($aName: string):string;
            public AddPage($name: string):void;
            public AddPageAt($name: string, $index: number):void;
            public RemovePage($name: string):void;
            public RemovePageAt($index: number):void;
            public ClearPages():void;
            public HasPage($aName: string):boolean;
            public RunActions():void;
            public Setup($buffer: FairyGUI.Utils.ByteBuffer):void;
            
        }
        class PlayTransitionAction extends FairyGUI.ControllerAction {
            public transitionName: string;
            public playTimes: number;
            public delay: number;
            public stopOnExit: boolean;
            public constructor();
            
        }
        class AsyncCreationHelper extends System.Object {
            public constructor();
            public static CreateObject($item: FairyGUI.PackageItem, $callback: FairyGUI.UIPackage.CreateObjectCallback):void;
            
        }
        class PackageItem extends System.Object {
            public owner: FairyGUI.UIPackage;
            public type: FairyGUI.PackageItemType;
            public objectType: FairyGUI.ObjectType;
            public id: string;
            public name: string;
            public width: number;
            public height: number;
            public file: string;
            public exported: boolean;
            public texture: FairyGUI.NTexture;
            public rawData: FairyGUI.Utils.ByteBuffer;
            public branches: System.Array$1<string>;
            public highResolution: System.Array$1<string>;
            public scale9Grid: System.Nullable$1<UnityEngine.Rect>;
            public scaleByTile: boolean;
            public tileGridIndice: number;
            public pixelHitTestData: FairyGUI.PixelHitTestData;
            public interval: number;
            public repeatDelay: number;
            public swing: boolean;
            public frames: System.Array$1<FairyGUI.MovieClip.Frame>;
            public translated: boolean;
            public extensionCreator: FairyGUI.UIObjectFactory.GComponentCreator;
            public bitmapFont: FairyGUI.BitmapFont;
            public audioClip: FairyGUI.NAudioClip;
            public skeletonAnchor: UnityEngine.Vector2;
            public skeletonAsset: any;
            public constructor();
            public Load():any;
            public getBranch():FairyGUI.PackageItem;
            public getHighResolution():FairyGUI.PackageItem;
            
        }
        class DragDropManager extends System.Object {
            public static inst: FairyGUI.DragDropManager;
            public dragAgent: FairyGUI.GLoader;
            public dragging: boolean;
            public constructor();
            public StartDrag($source: FairyGUI.GObject, $icon: string, $sourceData: any, $touchPointID?: number):void;
            public Cancel():void;
            
        }
        interface EMRenderTarget {
            EM_sortingOrder: number;
            EM_BeforeUpdate():void;
            EM_Update($context: FairyGUI.UpdateContext):void;
            EM_Reload():void;
            
        }
        class EMRenderSupport extends System.Object {
            public static orderChanged: boolean;
            public static packageListReady: boolean;
            public static hasTarget: boolean;
            public constructor();
            public static Add($value: FairyGUI.EMRenderTarget):void;
            public static Remove($value: FairyGUI.EMRenderTarget):void;
            public static Update():void;
            public static Reload():void;
            
        }
        enum ButtonMode { Common = 0, Check = 1, Radio = 2 }
        class ScrollPane extends FairyGUI.EventDispatcher {
            public static TWEEN_TIME_GO: number;
            public static TWEEN_TIME_DEFAULT: number;
            public static PULL_RATIO: number;
            public static draggingPane: FairyGUI.ScrollPane;
            public onScroll: FairyGUI.EventListener;
            public onScrollEnd: FairyGUI.EventListener;
            public onPullDownRelease: FairyGUI.EventListener;
            public onPullUpRelease: FairyGUI.EventListener;
            public owner: FairyGUI.GComponent;
            public hzScrollBar: FairyGUI.GScrollBar;
            public vtScrollBar: FairyGUI.GScrollBar;
            public header: FairyGUI.GComponent;
            public footer: FairyGUI.GComponent;
            public bouncebackEffect: boolean;
            public touchEffect: boolean;
            public inertiaDisabled: boolean;
            public softnessOnTopOrLeftSide: boolean;
            public scrollStep: number;
            public snapToItem: boolean;
            public pageMode: boolean;
            public pageController: FairyGUI.Controller;
            public mouseWheelEnabled: boolean;
            public decelerationRate: number;
            public isDragged: boolean;
            public percX: number;
            public percY: number;
            public posX: number;
            public posY: number;
            public isBottomMost: boolean;
            public isRightMost: boolean;
            public currentPageX: number;
            public currentPageY: number;
            public scrollingPosX: number;
            public scrollingPosY: number;
            public contentWidth: number;
            public contentHeight: number;
            public viewWidth: number;
            public viewHeight: number;
            public constructor($owner: FairyGUI.GComponent);
            public Setup($buffer: FairyGUI.Utils.ByteBuffer):void;
            public Dispose():void;
            public SetPercX($value: number, $ani: boolean):void;
            public SetPercY($value: number, $ani: boolean):void;
            public SetPosX($value: number, $ani: boolean):void;
            public SetPosY($value: number, $ani: boolean):void;
            public SetCurrentPageX($value: number, $ani: boolean):void;
            public SetCurrentPageY($value: number, $ani: boolean):void;
            public ScrollTop():void;
            public ScrollTop($ani: boolean):void;
            public ScrollBottom():void;
            public ScrollBottom($ani: boolean):void;
            public ScrollUp():void;
            public ScrollUp($ratio: number, $ani: boolean):void;
            public ScrollDown():void;
            public ScrollDown($ratio: number, $ani: boolean):void;
            public ScrollLeft():void;
            public ScrollLeft($ratio: number, $ani: boolean):void;
            public ScrollRight():void;
            public ScrollRight($ratio: number, $ani: boolean):void;
            public ScrollToView($obj: FairyGUI.GObject):void;
            public ScrollToView($obj: FairyGUI.GObject, $ani: boolean):void;
            public ScrollToView($obj: FairyGUI.GObject, $ani: boolean, $setFirst: boolean):void;
            public ScrollToView($rect: UnityEngine.Rect, $ani: boolean, $setFirst: boolean):void;
            public IsChildInView($obj: FairyGUI.GObject):boolean;
            public CancelDragging():void;
            public LockHeader($size: number):void;
            public LockFooter($size: number):void;
            public UpdateScrollBarVisible():void;
            
        }
        enum ChildrenRenderOrder { Ascent = 0, Descent = 1, Arch = 2 }
        class GGroup extends FairyGUI.GObject {
            public layout: FairyGUI.GroupLayoutType;
            public lineGap: number;
            public columnGap: number;
            public excludeInvisibles: boolean;
            public autoSizeDisabled: boolean;
            public mainGridMinSize: number;
            public mainGridIndex: number;
            public constructor();
            public SetBoundsChangedFlag($positionChangedOnly?: boolean):void;
            public EnsureBoundsCorrect():void;
            
        }
        class Transition extends System.Object {
            public invalidateBatchingEveryFrame: boolean;
            public name: string;
            public playing: boolean;
            public timeScale: number;
            public ignoreEngineTimeScale: boolean;
            public constructor($owner: FairyGUI.GComponent);
            public Play():void;
            public Play($onComplete: FairyGUI.PlayCompleteCallback):void;
            public Play($times: number, $delay: number, $onComplete: FairyGUI.PlayCompleteCallback):void;
            public Play($times: number, $delay: number, $startTime: number, $endTime: number, $onComplete: FairyGUI.PlayCompleteCallback):void;
            public PlayReverse():void;
            public PlayReverse($onComplete: FairyGUI.PlayCompleteCallback):void;
            public PlayReverse($times: number, $delay: number, $onComplete: FairyGUI.PlayCompleteCallback):void;
            public ChangePlayTimes($value: number):void;
            public SetAutoPlay($autoPlay: boolean, $times: number, $delay: number):void;
            public Stop():void;
            public Stop($setToComplete: boolean, $processCallback: boolean):void;
            public SetPaused($paused: boolean):void;
            public Dispose():void;
            public SetValue($label: string, ...aParams: any[]):void;
            public SetHook($label: string, $callback: FairyGUI.TransitionHook):void;
            public ClearHooks():void;
            public SetTarget($label: string, $newTarget: FairyGUI.GObject):void;
            public SetDuration($label: string, $value: number):void;
            public GetLabelTime($label: string):number;
            public OnTweenStart($tweener: FairyGUI.GTweener):void;
            public OnTweenUpdate($tweener: FairyGUI.GTweener):void;
            public OnTweenComplete($tweener: FairyGUI.GTweener):void;
            public Setup($buffer: FairyGUI.Utils.ByteBuffer):void;
            
        }
        class GearAnimation extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            
        }
        class GearBase extends System.Object {
            public static disableAllTweenEffect: boolean;
            public controller: FairyGUI.Controller;
            public tweenConfig: FairyGUI.GearTweenConfig;
            public Dispose():void;
            public Setup($buffer: FairyGUI.Utils.ByteBuffer):void;
            public UpdateFromRelations($dx: number, $dy: number):void;
            public Apply():void;
            public UpdateState():void;
            
        }
        class GearTweenConfig extends System.Object {
            public tween: boolean;
            public easeType: FairyGUI.EaseType;
            public customEase: FairyGUI.CustomEase;
            public duration: number;
            public delay: number;
            public constructor();
            
        }
        class GearColor extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            public OnTweenStart($tweener: FairyGUI.GTweener):void;
            public OnTweenUpdate($tweener: FairyGUI.GTweener):void;
            public OnTweenComplete($tweener: FairyGUI.GTweener):void;
            
        }
        class GearDisplay extends FairyGUI.GearBase {
            public pages: System.Array$1<string>;
            public connected: boolean;
            public constructor($owner: FairyGUI.GObject);
            public AddLock():number;
            public ReleaseLock($token: number):void;
            
        }
        class GearDisplay2 extends FairyGUI.GearBase {
            public condition: number;
            public pages: System.Array$1<string>;
            public constructor($owner: FairyGUI.GObject);
            public Evaluate($connected: boolean):boolean;
            
        }
        class GearFontSize extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            
        }
        class GearIcon extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            
        }
        class GearLook extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            public OnTweenStart($tweener: FairyGUI.GTweener):void;
            public OnTweenUpdate($tweener: FairyGUI.GTweener):void;
            public OnTweenComplete($tweener: FairyGUI.GTweener):void;
            
        }
        class GearSize extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            public OnTweenStart($tweener: FairyGUI.GTweener):void;
            public OnTweenUpdate($tweener: FairyGUI.GTweener):void;
            public OnTweenComplete($tweener: FairyGUI.GTweener):void;
            
        }
        class GearText extends FairyGUI.GearBase {
            public constructor($owner: FairyGUI.GObject);
            
        }
        class GearXY extends FairyGUI.GearBase {
            public positionsInPercent: boolean;
            public constructor($owner: FairyGUI.GObject);
            public AddExtStatus($pageId: string, $buffer: FairyGUI.Utils.ByteBuffer):void;
            public OnTweenStart($tweener: FairyGUI.GTweener):void;
            public OnTweenUpdate($tweener: FairyGUI.GTweener):void;
            public OnTweenComplete($tweener: FairyGUI.GTweener):void;
            
        }
        interface IAnimationGear {
            playing: boolean;
            frame: number;
            timeScale: number;
            ignoreEngineTimeScale: boolean;
            Advance($time: number):void;
            
        }
        interface IColorGear {
            color: UnityEngine.Color;
            
        }
        interface ITextColorGear {
            strokeColor: UnityEngine.Color;
            
        }
        class GGraph extends FairyGUI.GObject {
            public color: UnityEngine.Color;
            public shape: FairyGUI.Shape;
            public constructor();
            public ReplaceMe($target: FairyGUI.GObject):void;
            public AddBeforeMe($target: FairyGUI.GObject):void;
            public AddAfterMe($target: FairyGUI.GObject):void;
            public SetNativeObject($obj: FairyGUI.DisplayObject):void;
            public DrawRect($aWidth: number, $aHeight: number, $lineSize: number, $lineColor: UnityEngine.Color, $fillColor: UnityEngine.Color):void;
            public DrawRoundRect($aWidth: number, $aHeight: number, $fillColor: UnityEngine.Color, $corner: System.Array$1<number>):void;
            public DrawEllipse($aWidth: number, $aHeight: number, $fillColor: UnityEngine.Color):void;
            public DrawPolygon($aWidth: number, $aHeight: number, $points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color):void;
            public DrawPolygon($aWidth: number, $aHeight: number, $points: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $fillColor: UnityEngine.Color, $lineSize: number, $lineColor: UnityEngine.Color):void;
            
        }
        class GImage extends FairyGUI.GObject {
            public color: UnityEngine.Color;
            public flip: FairyGUI.FlipType;
            public fillMethod: FairyGUI.FillMethod;
            public fillOrigin: number;
            public fillClockwise: boolean;
            public fillAmount: number;
            public texture: FairyGUI.NTexture;
            public material: UnityEngine.Material;
            public shader: string;
            public constructor();
            
        }
        enum ListSelectionMode { Single = 0, Multiple = 1, Multiple_SingleClick = 2, None = 3 }
        type ListItemRenderer = (index: number, item: FairyGUI.GObject) => void;
        var ListItemRenderer: {new (func: (index: number, item: FairyGUI.GObject) => void): ListItemRenderer;}
        type ListItemProvider = (index: number) => string;
        var ListItemProvider: {new (func: (index: number) => string): ListItemProvider;}
        enum ListLayoutType { SingleColumn = 0, SingleRow = 1, FlowHorizontal = 2, FlowVertical = 3, Pagination = 4 }
        class GObjectPool extends System.Object {
            public initCallback: FairyGUI.GObjectPool.InitCallbackDelegate;
            public count: number;
            public constructor($manager: UnityEngine.Transform);
            public Clear():void;
            public GetObject($url: string):FairyGUI.GObject;
            public ReturnObject($obj: FairyGUI.GObject):void;
            
        }
        class GMovieClip extends FairyGUI.GObject {
            public onPlayEnd: FairyGUI.EventListener;
            public playing: boolean;
            public frame: number;
            public color: UnityEngine.Color;
            public flip: FairyGUI.FlipType;
            public material: UnityEngine.Material;
            public shader: string;
            public timeScale: number;
            public ignoreEngineTimeScale: boolean;
            public constructor();
            public Rewind():void;
            public SyncStatus($anotherMc: FairyGUI.GMovieClip):void;
            public Advance($time: number):void;
            public SetPlaySettings($start: number, $end: number, $times: number, $endAt: number):void;
            
        }
        class Relations extends System.Object {
            public handling: FairyGUI.GObject;
            public isEmpty: boolean;
            public constructor($owner: FairyGUI.GObject);
            public Add($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType):void;
            public Add($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType, $usePercent: boolean):void;
            public Remove($target: FairyGUI.GObject, $relationType: FairyGUI.RelationType):void;
            public Contains($target: FairyGUI.GObject):boolean;
            public ClearFor($target: FairyGUI.GObject):void;
            public ClearAll():void;
            public CopyFrom($source: FairyGUI.Relations):void;
            public Dispose():void;
            public OnOwnerSizeChanged($dWidth: number, $dHeight: number, $applyPivot: boolean):void;
            public Setup($buffer: FairyGUI.Utils.ByteBuffer, $parentToChild: boolean):void;
            
        }
        enum RelationType { Left_Left = 0, Left_Center = 1, Left_Right = 2, Center_Center = 3, Right_Left = 4, Right_Center = 5, Right_Right = 6, Top_Top = 7, Top_Middle = 8, Top_Bottom = 9, Middle_Middle = 10, Bottom_Top = 11, Bottom_Middle = 12, Bottom_Bottom = 13, Width = 14, Height = 15, LeftExt_Left = 16, LeftExt_Right = 17, RightExt_Left = 18, RightExt_Right = 19, TopExt_Top = 20, TopExt_Bottom = 21, BottomExt_Top = 22, BottomExt_Bottom = 23, Size = 24 }
        class GProgressBar extends FairyGUI.GComponent {
            public titleType: FairyGUI.ProgressTitleType;
            public min: number;
            public max: number;
            public value: number;
            public reverse: boolean;
            public constructor();
            public TweenValue($value: number, $duration: number):FairyGUI.GTweener;
            public Update($newValue: number):void;
            
        }
        class GSlider extends FairyGUI.GComponent {
            public changeOnClick: boolean;
            public canDrag: boolean;
            public onChanged: FairyGUI.EventListener;
            public onGripTouchEnd: FairyGUI.EventListener;
            public titleType: FairyGUI.ProgressTitleType;
            public min: number;
            public max: number;
            public value: number;
            public wholeNumbers: boolean;
            public constructor();
            
        }
        class GRichTextField extends FairyGUI.GTextField {
            public richTextField: FairyGUI.RichTextField;
            public emojies: System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>;
            public constructor();
            
        }
        class GTextInput extends FairyGUI.GTextField {
            public inputTextField: FairyGUI.InputTextField;
            public onChanged: FairyGUI.EventListener;
            public onSubmit: FairyGUI.EventListener;
            public editable: boolean;
            public hideInput: boolean;
            public maxLength: number;
            public restrict: string;
            public displayAsPassword: boolean;
            public caretPosition: number;
            public promptText: string;
            public keyboardInput: boolean;
            public keyboardType: number;
            public disableIME: boolean;
            public emojies: System.Collections.Generic.Dictionary$2<number, FairyGUI.Emoji>;
            public border: number;
            public corner: number;
            public borderColor: UnityEngine.Color;
            public backgroundColor: UnityEngine.Color;
            public mouseWheelEnabled: boolean;
            public constructor();
            public SetSelection($start: number, $length: number):void;
            public ReplaceSelection($value: string):void;
            
        }
        enum ProgressTitleType { Percent = 0, ValueAndMax = 1, Value = 2, Max = 3 }
        class GScrollBar extends FairyGUI.GComponent {
            public minSize: number;
            public gripDragging: boolean;
            public constructor();
            public SetScrollPane($target: FairyGUI.ScrollPane, $vertical: boolean):void;
            public SetDisplayPerc($value: number):void;
            public setScrollPerc($value: number):void;
            
        }
        interface IUISource {
            fileName: string;
            loaded: boolean;
            Load($callback: FairyGUI.UILoadCallback):void;
            
        }
        type UILoadCallback = () => void;
        var UILoadCallback: {new (func: () => void): UILoadCallback;}
        class UIPackage extends System.Object {
            public static unloadBundleByFGUI: boolean;
            public static URL_PREFIX: string;
            public id: string;
            public name: string;
            public static branch: string;
            public assetPath: string;
            public customId: string;
            public resBundle: UnityEngine.AssetBundle;
            public dependencies: System.Array$1<System.Collections.Generic.Dictionary$2<string, string>>;
            public constructor();
            public static add_onReleaseResource($value: System.Action$1<FairyGUI.PackageItem>):void;
            public static remove_onReleaseResource($value: System.Action$1<FairyGUI.PackageItem>):void;
            public static GetVar($key: string):string;
            public static SetVar($key: string, $value: string):void;
            public static GetById($id: string):FairyGUI.UIPackage;
            public static GetByName($name: string):FairyGUI.UIPackage;
            public static AddPackage($bundle: UnityEngine.AssetBundle):FairyGUI.UIPackage;
            public static AddPackage($desc: UnityEngine.AssetBundle, $res: UnityEngine.AssetBundle):FairyGUI.UIPackage;
            public static AddPackage($desc: UnityEngine.AssetBundle, $res: UnityEngine.AssetBundle, $mainAssetName: string):FairyGUI.UIPackage;
            public static AddPackage($descFilePath: string):FairyGUI.UIPackage;
            public static AddPackage($assetPath: string, $loadFunc: FairyGUI.UIPackage.LoadResource):FairyGUI.UIPackage;
            public static AddPackage($descData: System.Array$1<number>, $assetNamePrefix: string, $loadFunc: FairyGUI.UIPackage.LoadResource):FairyGUI.UIPackage;
            public static AddPackage($descData: System.Array$1<number>, $assetNamePrefix: string, $loadFunc: FairyGUI.UIPackage.LoadResourceAsync):FairyGUI.UIPackage;
            public static RemovePackage($packageIdOrName: string):void;
            public static RemoveAllPackages():void;
            public static GetPackages():System.Collections.Generic.List$1<FairyGUI.UIPackage>;
            public static CreateObject($pkgName: string, $resName: string):FairyGUI.GObject;
            public static CreateObject($pkgName: string, $resName: string, $userClass: System.Type):FairyGUI.GObject;
            public static CreateObjectFromURL($url: string):FairyGUI.GObject;
            public static CreateObjectFromURL($url: string, $userClass: System.Type):FairyGUI.GObject;
            public static CreateObjectAsync($pkgName: string, $resName: string, $callback: FairyGUI.UIPackage.CreateObjectCallback):void;
            public static CreateObjectFromURL($url: string, $callback: FairyGUI.UIPackage.CreateObjectCallback):void;
            public static GetItemAsset($pkgName: string, $resName: string):any;
            public static GetItemAssetByURL($url: string):any;
            public static GetItemURL($pkgName: string, $resName: string):string;
            public static GetItemByURL($url: string):FairyGUI.PackageItem;
            public static NormalizeURL($url: string):string;
            public static SetStringsSource($source: FairyGUI.Utils.XML):void;
            public LoadAllAssets():void;
            public UnloadAssets():void;
            public ReloadAssets():void;
            public ReloadAssets($resBundle: UnityEngine.AssetBundle):void;
            public CreateObject($resName: string):FairyGUI.GObject;
            public CreateObject($resName: string, $userClass: System.Type):FairyGUI.GObject;
            public CreateObjectAsync($resName: string, $callback: FairyGUI.UIPackage.CreateObjectCallback):void;
            public GetItemAsset($resName: string):any;
            public GetItems():System.Collections.Generic.List$1<FairyGUI.PackageItem>;
            public GetItem($itemId: string):FairyGUI.PackageItem;
            public GetItemByName($itemName: string):FairyGUI.PackageItem;
            public GetItemAsset($item: FairyGUI.PackageItem):any;
            public SetItemAsset($item: FairyGUI.PackageItem, $asset: any, $destroyMethod: FairyGUI.DestroyMethod):void;
            
        }
        enum PackageItemType { Image = 0, MovieClip = 1, Sound = 2, Component = 3, Atlas = 4, Font = 5, Swf = 6, Misc = 7, Unknown = 8, Spine = 9, DragoneBones = 10 }
        enum ObjectType { Image = 0, MovieClip = 1, Swf = 2, Graph = 3, Loader = 4, Group = 5, Text = 6, RichText = 7, InputText = 8, Component = 9, List = 10, Label = 11, Button = 12, ComboBox = 13, ProgressBar = 14, Slider = 15, ScrollBar = 16, Tree = 17, Loader3D = 18 }
        type PlayCompleteCallback = () => void;
        var PlayCompleteCallback: {new (func: () => void): PlayCompleteCallback;}
        type TransitionHook = () => void;
        var TransitionHook: {new (func: () => void): TransitionHook;}
        class TranslationHelper extends System.Object {
            public static strings: System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>;
            public constructor();
            public static LoadFromXML($source: FairyGUI.Utils.XML):void;
            public static TranslateComponent($item: FairyGUI.PackageItem):void;
            
        }
        class TreeNode extends System.Object {
            public data: any;
            public parent: FairyGUI.TreeNode;
            public tree: FairyGUI.TreeView;
            public cell: FairyGUI.GComponent;
            public level: number;
            public expanded: boolean;
            public isFolder: boolean;
            public text: string;
            public numChildren: number;
            public constructor($hasChild: boolean);
            public AddChild($child: FairyGUI.TreeNode):FairyGUI.TreeNode;
            public AddChildAt($child: FairyGUI.TreeNode, $index: number):FairyGUI.TreeNode;
            public RemoveChild($child: FairyGUI.TreeNode):FairyGUI.TreeNode;
            public RemoveChildAt($index: number):FairyGUI.TreeNode;
            public RemoveChildren($beginIndex?: number, $endIndex?: number):void;
            public GetChildAt($index: number):FairyGUI.TreeNode;
            public GetChildIndex($child: FairyGUI.TreeNode):number;
            public GetPrevSibling():FairyGUI.TreeNode;
            public GetNextSibling():FairyGUI.TreeNode;
            public SetChildIndex($child: FairyGUI.TreeNode, $index: number):void;
            public SwapChildren($child1: FairyGUI.TreeNode, $child2: FairyGUI.TreeNode):void;
            public SwapChildrenAt($index1: number, $index2: number):void;
            
        }
        class TreeView extends FairyGUI.EventDispatcher {
            public indent: number;
            public treeNodeCreateCell: FairyGUI.TreeView.TreeNodeCreateCellDelegate;
            public treeNodeRender: FairyGUI.TreeView.TreeNodeRenderDelegate;
            public treeNodeWillExpand: FairyGUI.TreeView.TreeNodeWillExpandDelegate;
            public list: FairyGUI.GList;
            public root: FairyGUI.TreeNode;
            public onClickNode: FairyGUI.EventListener;
            public onRightClickNode: FairyGUI.EventListener;
            public constructor($list: FairyGUI.GList);
            public GetSelectedNode():FairyGUI.TreeNode;
            public GetSelection():System.Collections.Generic.List$1<FairyGUI.TreeNode>;
            public AddSelection($node: FairyGUI.TreeNode, $scrollItToView?: boolean):void;
            public RemoveSelection($node: FairyGUI.TreeNode):void;
            public ClearSelection():void;
            public GetNodeIndex($node: FairyGUI.TreeNode):number;
            public UpdateNode($node: FairyGUI.TreeNode):void;
            public UpdateNodes($nodes: System.Collections.Generic.List$1<FairyGUI.TreeNode>):void;
            public ExpandAll($folderNode: FairyGUI.TreeNode):void;
            public CollapseAll($folderNode: FairyGUI.TreeNode):void;
            
        }
        class UIConfig extends UnityEngine.MonoBehaviour {
            public static defaultFont: string;
            public static windowModalWaiting: string;
            public static globalModalWaiting: string;
            public static modalLayerColor: UnityEngine.Color;
            public static buttonSound: FairyGUI.NAudioClip;
            public static buttonSoundVolumeScale: number;
            public static horizontalScrollBar: string;
            public static verticalScrollBar: string;
            public static defaultScrollStep: number;
            public static defaultScrollDecelerationRate: number;
            public static defaultScrollBarDisplay: FairyGUI.ScrollBarDisplayType;
            public static defaultScrollTouchEffect: boolean;
            public static defaultScrollBounceEffect: boolean;
            public static popupMenu: string;
            public static popupMenu_seperator: string;
            public static loaderErrorSign: string;
            public static tooltipsWin: string;
            public static defaultComboBoxVisibleItemCount: number;
            public static touchScrollSensitivity: number;
            public static touchDragSensitivity: number;
            public static clickDragSensitivity: number;
            public static allowSoftnessOnTopOrLeftSide: boolean;
            public static bringWindowToFrontOnClick: boolean;
            public static inputCaretSize: number;
            public static inputHighlightColor: UnityEngine.Color;
            public static frameTimeForAsyncUIConstruction: number;
            public static depthSupportForPaintingMode: boolean;
            public static enhancedTextOutlineEffect: boolean;
            public static makePixelPerfect: boolean;
            public Items: System.Collections.Generic.List$1<FairyGUI.UIConfig.ConfigValue>;
            public PreloadPackages: System.Collections.Generic.List$1<string>;
            public static soundLoader: FairyGUI.UIConfig.SoundLoader;
            public constructor();
            public Load():void;
            public static SetDefaultValue($key: FairyGUI.UIConfig.ConfigKey, $value: FairyGUI.UIConfig.ConfigValue):void;
            public static ClearResourceRefs():void;
            public ApplyModifiedProperties():void;
            
        }
        enum ScrollBarDisplayType { Default = 0, Visible = 1, Auto = 2, Hidden = 3 }
        class UIContentScaler extends UnityEngine.MonoBehaviour {
            public scaleMode: FairyGUI.UIContentScaler.ScaleMode;
            public screenMatchMode: FairyGUI.UIContentScaler.ScreenMatchMode;
            public designResolutionX: number;
            public designResolutionY: number;
            public fallbackScreenDPI: number;
            public defaultSpriteDPI: number;
            public constantScaleFactor: number;
            public ignoreOrientation: boolean;
            public static scaleFactor: number;
            public static scaleLevel: number;
            public constructor();
            public ApplyModifiedProperties():void;
            public ApplyChange():void;
            
        }
        class UIObjectFactory extends System.Object {
            public constructor();
            public static SetPackageItemExtension($url: string, $type: System.Type):void;
            public static SetPackageItemExtension($url: string, $creator: FairyGUI.UIObjectFactory.GComponentCreator):void;
            public static SetLoaderExtension($type: System.Type):void;
            public static SetLoaderExtension($creator: FairyGUI.UIObjectFactory.GLoaderCreator):void;
            public static Clear():void;
            public static NewObject($pi: FairyGUI.PackageItem, $userClass?: System.Type):FairyGUI.GObject;
            public static NewObject($type: FairyGUI.ObjectType):FairyGUI.GObject;
            
        }
        class UIPainter extends UnityEngine.MonoBehaviour {
            public packageName: string;
            public componentName: string;
            public sortingOrder: number;
            public container: FairyGUI.Container;
            public ui: FairyGUI.GComponent;
            public EM_sortingOrder: number;
            public constructor();
            public SetSortingOrder($value: number, $apply: boolean):void;
            public CreateUI():void;
            public ApplyModifiedProperties($sortingOrderChanged: boolean):void;
            public OnUpdateSource($data: System.Array$1<any>):void;
            public EM_BeforeUpdate():void;
            public EM_Update($context: FairyGUI.UpdateContext):void;
            public EM_Reload():void;
            
        }
        class UIPanel extends UnityEngine.MonoBehaviour {
            public packageName: string;
            public componentName: string;
            public fitScreen: FairyGUI.FitScreen;
            public sortingOrder: number;
            public container: FairyGUI.Container;
            public ui: FairyGUI.GComponent;
            public EM_sortingOrder: number;
            public constructor();
            public CreateUI():void;
            public SetSortingOrder($value: number, $apply: boolean):void;
            public SetHitTestMode($value: FairyGUI.HitTestMode):void;
            public CacheNativeChildrenRenderers():void;
            public ApplyModifiedProperties($sortingOrderChanged: boolean, $fitScreenChanged: boolean):void;
            public MoveUI($delta: UnityEngine.Vector3):void;
            public GetUIWorldPosition():UnityEngine.Vector3;
            public EM_BeforeUpdate():void;
            public EM_Update($context: FairyGUI.UpdateContext):void;
            public EM_Reload():void;
            
        }
        enum FitScreen { None = 0, FitSize = 1, FitWidthAndSetMiddle = 2, FitHeightAndSetCenter = 3 }
        enum HitTestMode { Default = 0, Raycast = 1 }
        class Timers extends System.Object {
            public static repeat: number;
            public static time: number;
            public static catchCallbackExceptions: boolean;
            public static inst: FairyGUI.Timers;
            public constructor();
            public Add($interval: number, $repeat: number, $callback: FairyGUI.TimerCallback):void;
            public Add($interval: number, $repeat: number, $callback: FairyGUI.TimerCallback, $callbackParam: any):void;
            public CallLater($callback: FairyGUI.TimerCallback):void;
            public CallLater($callback: FairyGUI.TimerCallback, $callbackParam: any):void;
            public AddUpdate($callback: FairyGUI.TimerCallback):void;
            public AddUpdate($callback: FairyGUI.TimerCallback, $callbackParam: any):void;
            public StartCoroutine($routine: System.Collections.IEnumerator):void;
            public Exists($callback: FairyGUI.TimerCallback):boolean;
            public Remove($callback: FairyGUI.TimerCallback):void;
            public Update():void;
            
        }
        type TimerCallback = (param: any) => void;
        var TimerCallback: {new (func: (param: any) => void): TimerCallback;}
        
    }
    namespace FairyEditor {
        class App extends System.Object {
            public static isMacOS: boolean;
            public static language: string;
            public static batchMode: boolean;
            public static preferences: FairyEditor.Preferences;
            public static localStore: FairyEditor.LocalStore;
            public static hotkeyManager: FairyEditor.HotkeyManager;
            public static externalImagePool: ExternalImagePool;
            public static groot: FairyGUI.GRoot;
            public static project: FairyEditor.FProject;
            public static workspaceSettings: FairyEditor.WorkspaceSettings;
            public static mainView: FairyEditor.View.MainView;
            public static docView: FairyEditor.View.DocumentView;
            public static libView: FairyEditor.View.LibraryView;
            public static inspectorView: FairyEditor.View.InspectorView;
            public static testView: FairyEditor.View.TestView;
            public static timelineView: FairyEditor.View.TimelineView;
            public static consoleView: FairyEditor.View.ConsoleView;
            public static menu: FairyEditor.Component.IMenu;
            public static viewManager: FairyEditor.ViewManager;
            public static dragManager: FairyEditor.DragDropManager;
            public static pluginManager: FairyEditor.PluginManager;
            public static docFactory: FairyEditor.View.DocumentFactory;
            public static activeDoc: FairyEditor.View.Document;
            public static preferenceFolder: string;
            public static isActive: boolean;
            public constructor();
            public static add_onProjectOpened($value: System.Action):void;
            public static remove_onProjectOpened($value: System.Action):void;
            public static add_onProjectClosed($value: System.Action):void;
            public static remove_onProjectClosed($value: System.Action):void;
            public static add_onUpdate($value: System.Action):void;
            public static remove_onUpdate($value: System.Action):void;
            public static add_onLateUpdate($value: System.Action):void;
            public static remove_onLateUpdate($value: System.Action):void;
            public static add_onValidate($value: System.Action):void;
            public static remove_onValidate($value: System.Action):void;
            public static GetString($index: number):string;
            public static GetString($index: string):string;
            public static GetIcon($key: string):string;
            public static GetIcon($key: string, $big: boolean):string;
            public static StartBackgroundJob():void;
            public static EndBackgroundJob():void;
            public static SetFrameRateFactor($factor: FairyEditor.App.FrameRateFactor, $enabled: boolean):void;
            public static OpenProject($path: string):void;
            public static CloseProject():void;
            public static RefreshProject():void;
            public static ShowPreview($pi: FairyEditor.FPackageItem):void;
            public static FindReference($source: string):void;
            public static GetActiveFolder():FairyEditor.FPackageItem;
            public static QueryToClose($restart: boolean):void;
            public static Close():void;
            public static Alert($msg: string):void;
            public static Alert($msg: string, $err: System.Exception):void;
            public static Alert($msg: string, $err: System.Exception, $callback: System.Action):void;
            public static Confirm($msg: string, $callback: System.Action$1<string>):void;
            public static Input($msg: string, $text: string, $callback: System.Action$1<string>):void;
            public static SetWaitCursor($value: boolean):void;
            public static ShowWaiting():void;
            public static ShowWaiting($msg: string):void;
            public static ShowWaiting($msg: string, $cancelCallback: System.Action):void;
            public static CloseWaiting():void;
            public static SetVar($key: string, $value: any):void;
            public static On($eventType: string, $callback: FairyGUI.EventCallback1):void;
            public static Off($eventType: string, $callback: FairyGUI.EventCallback1):void;
            public static Dispatch($eventType: string, $eventData?: any):void;
            public static ChangeColorSapce($colorSpace: UnityEngine.ColorSpace):void;
            
        }
        class Preferences extends System.Object {
            public language: string;
            public checkNewVersion: string;
            public genComPreview: boolean;
            public meaningfullChildName: boolean;
            public hideInvisibleChild: boolean;
            public publishAction: string;
            public saveBeforePublish: boolean;
            public PNGCompressionToolPath: string;
            public hotkeys: System.Collections.Generic.Dictionary$2<string, string>;
            public constructor();
            public Load():void;
            public Save():void;
            
        }
        class LocalStore extends System.Object {
            public constructor();
            public Set($key: string, $value: any):void;
            public Load():void;
            public Save():void;
            
        }
        class HotkeyManager extends System.Object {
            public functions: System.Collections.Generic.List$1<FairyEditor.HotkeyManager.FunctionDef>;
            public constructor();
            public Init():void;
            public SetHotkey($funcId: string, $hotkey: string):void;
            public ResetHotkey($funcId: string):void;
            public ResetAll():void;
            public CaptureHotkey($receiver: FairyGUI.GObject):void;
            public GetFunctionDef($funcId: string):FairyEditor.HotkeyManager.FunctionDef;
            public GetFunction($evt: FairyGUI.InputEvent, $code: $Ref<number>):string;
            public TranslateKey($hotkey: string):number;
            
        }
        class FProject extends System.Object {
            public isMain: boolean;
            public _globalFontVersion: number;
            public static FILE_EXT: string;
            public static ASSETS_PATH: string;
            public static SETTINGS_PATH: string;
            public static OBJS_PATH: string;
            public versionCode: number;
            public serialNumberSeed: string;
            public lastChanged: number;
            public opened: boolean;
            public id: string;
            public name: string;
            public type: string;
            public supportAtlas: boolean;
            public isH5: boolean;
            public supportExtractAlpha: boolean;
            public supportAlphaMask: boolean;
            public zipFormatOption: boolean;
            public binaryFormatOption: boolean;
            public supportCustomFileExtension: boolean;
            public basePath: string;
            public assetsPath: string;
            public objsPath: string;
            public settingsPath: string;
            public activeBranch: string;
            public allPackages: System.Collections.Generic.List$1<FairyEditor.FPackage>;
            public allBranches: System.Collections.Generic.List$1<string>;
            public constructor($main?: boolean);
            public SetChanged():void;
            public static CreateNew($projectPath: string, $name: string, $type: string, $pkgName?: string):void;
            public Open($projectDescFile: string):void;
            public ScanBranches():boolean;
            public Dispose():void;
            public GetSettings($name: string):FairyEditor.SettingsBase;
            public LoadAllSettings():void;
            public Rename($newName: string):void;
            public GetPackage($packageId: string):FairyEditor.FPackage;
            public GetPackageByName($packageName: string):FairyEditor.FPackage;
            public CreatePackage($newName: string):FairyEditor.FPackage;
            public AddPackage($folder: string):FairyEditor.FPackage;
            public DeletePackage($packageId: string):void;
            public Save():void;
            public GetItemByURL($url: string):FairyEditor.FPackageItem;
            public GetItem($pkgId: string, $itemId: string):FairyEditor.FPackageItem;
            public FindItemByFile($file: string):FairyEditor.FPackageItem;
            public GetItemNameByURL($url: string):string;
            public CreateBranch($branchName: string):void;
            public RenameBranch($oldName: string, $newName: string):void;
            public RemoveBranch($branchName: string):void;
            public RegisterComExtension($name: string, $className: string, $superClassName: string):void;
            public GetComExtension($className: string):FairyEditor.ComExtensionDef;
            public GetComExtensionNames():System.Collections.Generic.List$1<string>;
            public ClearComExtensions():void;
            public static ValidateName($newName: string):string;
            
        }
        class WorkspaceSettings extends System.Object {
            public constructor();
            public Set($key: string, $value: any):void;
            public Load():void;
            public Save():void;
            
        }
        class ViewManager extends FairyGUI.GComponent {
            public playMode: boolean;
            public viewIds: System.Collections.Generic.List$1<string>;
            public lastFocusedView: FairyGUI.GComponent;
            public constructor();
            public AddView($view: FairyGUI.GComponent, $viewId: string, $options: FairyEditor.ViewOptions):FairyGUI.GComponent;
            public RemoveView($viewId: string):void;
            public GetView($viewId: string):FairyGUI.GComponent;
            public IsViewShowing($viewId: string):boolean;
            public SetViewTitle($viewId: string, $title: string):void;
            public ShowView($viewId: string):void;
            public HideView($viewId: string):void;
            public LoadLayout():void;
            public ResetLayout():void;
            public SaveLayout():void;
            public ShowTabMenu($view: FairyGUI.GComponent):void;
            public OnDragGridStart($grid: FairyEditor.Component.ViewGrid, $tabButton: FairyGUI.GObject):void;
            
        }
        class DragDropManager extends System.Object {
            public agent: FairyGUI.GObject;
            public dragging: boolean;
            public constructor();
            public StartDrag($source?: FairyGUI.GObject, $sourceData?: any, $icon?: any, $cursor?: string, $onComplete?: System.Action$2<FairyGUI.GObject, any>, $onCancel?: System.Action$2<FairyGUI.GObject, any>, $onMove?: System.Action$3<FairyGUI.GObject, any, FairyGUI.EventContext>):void;
            public Cancel():void;
            
        }
        class PluginManager extends System.Object {
            public allPlugins: System.Collections.Generic.List$1<FairyEditor.PluginManager.PluginInfo>;
            public userPluginFolder: string;
            public projectPluginFolder: string;
            public basePath: string;
            public constructor();
            public Dispose():void;
            public Load():void;
            public LoadUIPackage($filePath: string):void;
            public SetHotkey($hotkey: string, $callback: System.Action):void;
            public HandleHotkey($keyCode: number):boolean;
            public CreateNewPlugin($name: string, $displayName: string, $icon: string, $desc: string, $template: string):void;
            
        }
        class FPackageItem extends System.Object {
            public exported: boolean;
            public favorite: boolean;
            public isError: boolean;
            public owner: FairyEditor.FPackage;
            public parent: FairyEditor.FPackageItem;
            public type: string;
            public id: string;
            public path: string;
            public branch: string;
            public isRoot: boolean;
            public isBranchRoot: boolean;
            public name: string;
            public file: string;
            public fileName: string;
            public modificationTime: Date;
            public sortKey: string;
            public version: number;
            public width: number;
            public height: number;
            public thumbnail: FairyGUI.NTexture;
            public children: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public folderAtlas: string;
            public supportAtlas: boolean;
            public supportResolution: boolean;
            public title: string;
            public contentHash: string;
            public isDisposed: boolean;
            public constructor($owner: FairyEditor.FPackage, $type: string, $id: string);
            public add_onChanged($value: System.Action$1<FairyEditor.FPackageItem>):void;
            public remove_onChanged($value: System.Action$1<FairyEditor.FPackageItem>):void;
            public add_onAlternativeAdded($value: System.Action$1<FairyEditor.FPackageItem>):void;
            public remove_onAlternativeAdded($value: System.Action$1<FairyEditor.FPackageItem>):void;
            public MatchName($key: string):boolean;
            public GetURL():string;
            public GetIcon($opened?: boolean, $big?: boolean, $thumbnail?: boolean):string;
            public CopySettings($source: FairyEditor.FPackageItem):void;
            public SetFile($path: string, $fileName: string, $checkStatus?: boolean):void;
            public SetChanged():void;
            public Touch():void;
            public SetUptoDate():void;
            public FileExists():boolean;
            public GetAsset():FairyEditor.AssetBase;
            public ReadAssetSettings($xml: FairyGUI.Utils.XML):void;
            public OpenWithDefaultApplication():void;
            public GetBranch($branchName: string):FairyEditor.FPackageItem;
            public GetTrunk():FairyEditor.FPackageItem;
            public GetHighResolution($scaleLevel: number):FairyEditor.FPackageItem;
            public GetStdResolution():FairyEditor.FPackageItem;
            public GetAtlasIndex():number;
            public SetVar($key: string, $value: any):void;
            public AddRef():void;
            public ReleaseRef():void;
            public UnloadAsset($timestamp?: number):void;
            public Dispose():void;
            public Serialize($forPublish?: boolean):FairyGUI.Utils.XML;
            
        }
        class Bootstrap extends UnityEngine.MonoBehaviour {
            public constructor();
            
        }
        class LoaderExtension extends FairyGUI.GLoader {
            public constructor();
            
        }
        class AniSprite extends FairyGUI.Image {
            public onPlayEnd: FairyGUI.EventListener;
            public animation: FairyEditor.AniData;
            public playing: boolean;
            public frame: number;
            public frameCount: number;
            public constructor();
            public Rewind():void;
            public Advance($time: number):void;
            public SetPlaySettings():void;
            public SetPlaySettings($start: number, $end: number, $times: number, $endAt: number):void;
            public StepNext():void;
            public StepPrev():void;
            
        }
        class AniData extends System.Object {
            public version: number;
            public boundsRect: UnityEngine.Rect;
            public fps: number;
            public speed: number;
            public repeatDelay: number;
            public swing: boolean;
            public frameList: System.Collections.Generic.List$1<FairyEditor.AniData.Frame>;
            public spriteList: System.Collections.Generic.List$1<FairyEditor.AniData.FrameSprite>;
            public static FILE_MARK: string;
            public frameCount: number;
            public constructor();
            public Load($file: string):void;
            public Load($ba: FairyGUI.Utils.ByteBuffer):void;
            public Save($file: string):void;
            public Save():System.Array$1<number>;
            public CalculateBoundsRect():void;
            public CopySettings($source: FairyEditor.AniData):void;
            public CopyFrom($source: FairyEditor.AniData):void;
            public Reset($ownsTexture?: boolean):void;
            public ImportImages($images: System.Collections.Generic.IList$1<string>, $CompressPng: boolean):void;
            
        }
        class AniAsset extends FairyEditor.AssetBase {
            public smoothing: boolean;
            public atlas: string;
            public animation: FairyEditor.AniData;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public Load():System.Threading.Tasks.Task;
            
        }
        class AssetBase extends System.Object {
            public isLoading: boolean;
            public isLoaded: boolean;
            public constructor($item: FairyEditor.FPackageItem);
            public ReadSettings($xml: FairyGUI.Utils.XML):void;
            public WriteSettings($xml: FairyGUI.Utils.XML, $forPublish: boolean):void;
            public LoadMeta():void;
            public Unload():void;
            public Dispose():void;
            public GetThumbnail():FairyGUI.NTexture;
            
        }
        class BmFontData extends System.Object {
            public face: string;
            public xadvance: number;
            public canTint: boolean;
            public resizable: boolean;
            public fontSize: number;
            public lineHeight: number;
            public atlasFile: string;
            public pages: number;
            public hasChannel: boolean;
            public baseline: number;
            public packed: number;
            public alphaChnl: number;
            public redChnl: number;
            public greenChnl: number;
            public blueChnl: number;
            public glyphs: System.Collections.Generic.List$1<FairyEditor.BmFontData.Glyph>;
            public constructor();
            public Load($content: string, $lazyLoadChars?: boolean):void;
            public LoadChars():void;
            public Build():string;
            
        }
        class ComponentAsset extends FairyEditor.AssetBase {
            public extension: string;
            public xml: FairyGUI.Utils.XML;
            public displayList: System.Collections.Generic.List$1<FairyEditor.ComponentAsset.DisplayListItem>;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public GetCustomProperties():System.Collections.Generic.IList$1<FairyEditor.ComProperty>;
            public GetControllerPages($name: string, $pageNames: System.Collections.Generic.List$1<string>, $pageIds: System.Collections.Generic.List$1<string>):void;
            public CreateObject($item: FairyEditor.FPackageItem, $flags?: number):System.Threading.Tasks.Task$1<FairyEditor.FComponent>;
            
        }
        class ComProperty extends System.Object {
            public target: string;
            public propertyId: number;
            public label: string;
            public value: any;
            public constructor();
            public CopyFrom($source: FairyEditor.ComProperty):void;
            
        }
        class FComponent extends FairyEditor.FObject {
            public customExtentionId: string;
            public initName: string;
            public designImage: string;
            public designImageOffsetX: number;
            public designImageOffsetY: number;
            public designImageAlpha: number;
            public designImageLayer: number;
            public designImageForTest: boolean;
            public bgColor: UnityEngine.Color;
            public bgColorEnabled: boolean;
            public hitTestSource: FairyEditor.FObject;
            public mask: FairyEditor.FObject;
            public reversedMask: boolean;
            public remark: string;
            public headerRes: string;
            public footerRes: string;
            public showSound: string;
            public hideSound: string;
            public numChildren: number;
            public children: System.Collections.Generic.List$1<FairyEditor.FObject>;
            public controllers: System.Collections.Generic.List$1<FairyEditor.FController>;
            public transitions: FairyEditor.FTransitions;
            public customProperties: System.Collections.Generic.List$1<FairyEditor.ComProperty>;
            public bounds: UnityEngine.Rect;
            public extention: FairyEditor.ComExtention;
            public extentionId: string;
            public scrollPane: FairyEditor.FScrollPane;
            public overflow: string;
            public overflow2: string;
            public scroll: string;
            public scrollBarFlags: number;
            public scrollBarDisplay: string;
            public margin: FairyEditor.FMargin;
            public marginStr: string;
            public scrollBarMargin: FairyEditor.FMargin;
            public scrollBarMarginStr: string;
            public hzScrollBarRes: string;
            public vtScrollBarRes: string;
            public clipSoftnessX: number;
            public clipSoftnessY: number;
            public viewWidth: number;
            public viewHeight: number;
            public opaque: boolean;
            public text: string;
            public icon: string;
            public childrenRenderOrder: string;
            public apexIndex: number;
            public pageController: string;
            public pageControllerObj: FairyEditor.FController;
            public scriptData: FairyGUI.Utils.XML;
            public constructor($flags: number);
            public AddChild($child: FairyEditor.FObject):FairyEditor.FObject;
            public AddChildAt($child: FairyEditor.FObject, $index: number):FairyEditor.FObject;
            public RemoveChild($child: FairyEditor.FObject, $dispose?: boolean):FairyEditor.FObject;
            public RemoveChildAt($index: number, $dispose?: boolean):FairyEditor.FObject;
            public RemoveChildren($beginIndex?: number, $endIndex?: number, $dispose?: boolean):void;
            public GetChildAt($index: number):FairyEditor.FObject;
            public GetChild($name: string):FairyEditor.FObject;
            public GetChildByPath($path: string):FairyEditor.FObject;
            public GetChildById($id: string):FairyEditor.FObject;
            public GetChildIndex($child: FairyEditor.FObject):number;
            public SetChildIndex($child: FairyEditor.FObject, $index: number):void;
            public SwapChildren($child1: FairyEditor.FObject, $child2: FairyEditor.FObject):void;
            public SwapChildrenAt($index1: number, $index2: number):void;
            public AddController($controller: FairyEditor.FController, $applyNow?: boolean):void;
            public GetController($name: string):FairyEditor.FController;
            public RemoveController($c: FairyEditor.FController):void;
            public UpdateChildrenVisible():void;
            public UpdateDisplayList($immediatelly?: boolean):void;
            public GetSnappingPosition($xValue: number, $yValue: number):UnityEngine.Vector2;
            public EnsureBoundsCorrect():void;
            public SetBoundsChangedFlag():void;
            public GetBounds():UnityEngine.Rect;
            public SetBounds($ax: number, $ay: number, $aw: number, $ah: number):void;
            public ApplyController($c: FairyEditor.FController):void;
            public ApplyAllControllers():void;
            public AdjustRadioGroupDepth($obj: FairyEditor.FObject, $c: FairyEditor.FController):void;
            public GetCustomProperty($target: string, $propertyId: number):FairyEditor.ComProperty;
            public ApplyCustomProperty($cp: FairyEditor.ComProperty):void;
            public UpdateOverflow():void;
            public Write_editMode():FairyGUI.Utils.XML;
            public ValidateChildren($checkOnly?: boolean):boolean;
            public CreateChild($xml: FairyGUI.Utils.XML):FairyEditor.FObject;
            public GetChildrenInfo():string;
            public GetNextId():string;
            public IsIdInUse($val: string):boolean;
            public ContainsComponent($pi: FairyEditor.FPackageItem):boolean;
            public NotifyChildReplaced($source: FairyEditor.FObject, $target: FairyEditor.FObject):void;
            
        }
        class FObject extends FairyGUI.EventDispatcher {
            public _parent: FairyEditor.FComponent;
            public _id: string;
            public _width: number;
            public _height: number;
            public _rawWidth: number;
            public _rawHeight: number;
            public _widthEnabled: boolean;
            public _heightEnabled: boolean;
            public _renderDepth: number;
            public _outlineVersion: number;
            public _opened: boolean;
            public _group: FairyEditor.FGroup;
            public _sizePercentInGroup: number;
            public _gearLocked: boolean;
            public _internalVisible: boolean;
            public _hasSnapshot: boolean;
            public _treeNode: FairyEditor.FTreeNode;
            public _pivotFromSource: boolean;
            public _pkg: FairyEditor.FPackage;
            public _res: FairyEditor.ResourceRef;
            public _objectType: string;
            public _docElement: FairyEditor.View.DocElement;
            public _flags: number;
            public _underConstruct: boolean;
            public sourceWidth: number;
            public sourceHeight: number;
            public initWidth: number;
            public initHeight: number;
            public customData: string;
            public static loadingSnapshot: boolean;
            public static MAX_GEAR_INDEX: number;
            public id: string;
            public name: string;
            public objectType: string;
            public pkg: FairyEditor.FPackage;
            public docElement: FairyEditor.View.DocElement;
            public touchable: boolean;
            public touchDisabled: boolean;
            public grayed: boolean;
            public enabled: boolean;
            public resourceURL: string;
            public x: number;
            public y: number;
            public xy: UnityEngine.Vector2;
            public xMin: number;
            public xMax: number;
            public yMin: number;
            public yMax: number;
            public height: number;
            public width: number;
            public size: UnityEngine.Vector2;
            public minWidth: number;
            public minHeight: number;
            public maxWidth: number;
            public maxHeight: number;
            public actualWidth: number;
            public actualHeight: number;
            public scaleX: number;
            public scaleY: number;
            public aspectLocked: boolean;
            public aspectRatio: number;
            public skewX: number;
            public skewY: number;
            public pivotX: number;
            public pivotY: number;
            public anchor: boolean;
            public locked: boolean;
            public hideByEditor: boolean;
            public useSourceSize: boolean;
            public rotation: number;
            public alpha: number;
            public visible: boolean;
            public internalVisible: boolean;
            public internalVisible2: boolean;
            public internalVisible3: boolean;
            public groupId: string;
            public tooltips: string;
            public filterData: FairyEditor.FilterData;
            public filter: string;
            public blendMode: string;
            public relations: FairyEditor.FRelations;
            public displayObject: FairyEditor.FDisplayObject;
            public parent: FairyEditor.FComponent;
            public text: string;
            public icon: string;
            public errorStatus: boolean;
            public topmost: FairyEditor.FComponent;
            public constructor($flags: number);
            public SetXY($xv: number, $yv: number):void;
            public SetTopLeft($xv: number, $yv: number):void;
            public SetSize($wv: number, $hv: number, $ignorePivot?: boolean, $dontCheckLock?: boolean):void;
            public SetScale($sx: number, $sy: number):void;
            public SetSkew($xv: number, $yv: number):void;
            public SetPivot($xv: number, $yv: number, $asAnchor: boolean):void;
            public InGroup($group: FairyEditor.FGroup):boolean;
            public GetGear($index: number, $createIfNull?: boolean):FairyEditor.Framework.Gears.IGear;
            public UpdateGear($index: number):void;
            public UpdateGearFromRelations($index: number, $dx: number, $dy: number):void;
            public SupportGear($index: number):boolean;
            public ValidateGears():void;
            public CheckGearController($index: number, $c: FairyEditor.FController):boolean;
            public CheckGearsController($c: FairyEditor.FController):boolean;
            public AddDisplayLock():number;
            public ReleaseDisplayLock($token: number):void;
            public CheckGearDisplay():void;
            public RemoveFromParent():void;
            public LocalToGlobal($pt: UnityEngine.Vector2):UnityEngine.Vector2;
            public GlobalToLocal($pt: UnityEngine.Vector2):UnityEngine.Vector2;
            public static cast($obj: FairyGUI.DisplayObject):FairyEditor.FObject;
            public HandleXYChanged():void;
            public HandleSizeChanged():void;
            public HandleGrayedChanged():void;
            public HandleAlphaChanged():void;
            public HandleVisibleChanged():void;
            public HandleControllerChanged($c: FairyEditor.FController):void;
            public GetProperty($propName: string):any;
            public SetProperty($propName: string, $value: any):void;
            public GetProp($index: FairyEditor.ObjectPropID):any;
            public SetProp($index: FairyEditor.ObjectPropID, $value: any):void;
            public IsObsolete():boolean;
            public Validate($checkOnly?: boolean):boolean;
            public GetDetailString():string;
            public Create():void;
            public Dispose():void;
            public Recreate():void;
            public Read_beforeAdd($xml: FairyGUI.Utils.XML, $strings: System.Collections.Generic.Dictionary$2<string, string>):void;
            public Read_afterAdd($xml: FairyGUI.Utils.XML, $strings: System.Collections.Generic.Dictionary$2<string, string>):void;
            public Write():FairyGUI.Utils.XML;
            public TakeSnapshot($ss: FairyEditor.ObjectSnapshot):void;
            public ReadSnapshot($ss: FairyEditor.ObjectSnapshot):void;
            
        }
        class DragonBonesAsset extends FairyEditor.SkeletonAsset {
            public data: DragonBones.DragonBonesData;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public static ParseBounds($sourceFile: string, $bounds: $Ref<UnityEngine.Rect>):void;
            public Load():System.Threading.Tasks.Task;
            
        }
        class SkeletonAsset extends FairyEditor.AssetBase {
            public files: System.Array$1<string>;
            public atlasNames: System.Array$1<string>;
            public anchorX: number;
            public anchorY: number;
            public shader: string;
            public pma: boolean;
            public constructor($packageItem: FairyEditor.FPackageItem);
            
        }
        class FBitmapFont extends FairyGUI.BitmapFont {
            public fontData: FairyEditor.BmFontData;
            public usingAtlas: boolean;
            public branch: string;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public constructor($parent: FairyEditor.FBitmapFont, $branch: string, $scaleLevel: number);
            public GetSubFont($branch: string, $scaleLevel: number):FairyEditor.FBitmapFont;
            
        }
        class FontAsset extends FairyEditor.AssetBase {
            public texture: string;
            public samplePointSize: number;
            public renderMode: string;
            public italicStyle: number;
            public boldWeight: number;
            public static DefaultItalicStyle: number;
            public static DefaultBoldWeight: number;
            public fontType: FairyEditor.FontAsset.FontType;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public static IsTTF($file: string):boolean;
            public GetFont($flags: number):FairyGUI.BaseFont;
            public GetFont($branch: string, $scaleLevel: number):FairyGUI.BaseFont;
            public static ParseRenderMode($str: string):UnityEngine.TextCore.LowLevel.GlyphRenderMode;
            
        }
        class ImageAsset extends FairyEditor.AssetBase {
            public scale9Grid: UnityEngine.Rect;
            public scaleOption: string;
            public qualityOption: string;
            public quality: number;
            public smoothing: boolean;
            public gridTile: number;
            public atlas: string;
            public duplicatePadding: boolean;
            public disableTrim: boolean;
            public svgWidth: number;
            public svgHeight: number;
            public static QUALITY_DEFAULT: string;
            public static QUALITY_SOURCE: string;
            public static QUALITY_CUSTOM: string;
            public static SCALE_9GRID: string;
            public static SCALE_TILE: string;
            public texture: FairyGUI.NTexture;
            public converting: boolean;
            public format: string;
            public targetQuality: number;
            public file: string;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public LoadTexture():System.Threading.Tasks.Task;
            public LoadForPublish($trim: boolean):System.Threading.Tasks.Task;
            
        }
        class SoundAsset extends FairyEditor.AssetBase {
            public audio: UnityEngine.AudioClip;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public Play($volumeScale?: number):void;
            
        }
        class SpineAsset extends FairyEditor.SkeletonAsset {
            public data: Spine.Unity.SkeletonDataAsset;
            public constructor($packageItem: FairyEditor.FPackageItem);
            public Load():System.Threading.Tasks.Task;
            public static ParseBounds($sourceFile: string, $bounds: $Ref<UnityEngine.Rect>):void;
            
        }
        class ComExtensionDef extends System.Object {
            public name: string;
            public className: string;
            public superClassName: string;
            public constructor();
            
        }
        class ComExtention extends FairyGUI.EventDispatcher {
            public _type: string;
            public owner: FairyEditor.FComponent;
            public text: string;
            public icon: string;
            public constructor();
            public Create():void;
            public Dispose():void;
            public Read_editMode($xml: FairyGUI.Utils.XML):void;
            public Write_editMode():FairyGUI.Utils.XML;
            public Read($xml: FairyGUI.Utils.XML, $strings: System.Collections.Generic.Dictionary$2<string, string>):void;
            public Write():FairyGUI.Utils.XML;
            public HandleControllerChanged($c: FairyEditor.FController):void;
            public GetProp($index: FairyEditor.ObjectPropID):any;
            public SetProp($index: FairyEditor.ObjectPropID, $value: any):void;
            public GetProperty($propName: string):any;
            public SetProperty($propName: string, $value: any):void;
            
        }
        class FController extends FairyGUI.EventDispatcher {
            public name: string;
            public autoRadioGroupDepth: boolean;
            public exported: boolean;
            public alias: string;
            public homePageType: string;
            public homePage: string;
            public parent: FairyEditor.FComponent;
            public changing: boolean;
            public selectedIndex: number;
            public previsousIndex: number;
            public selectedPage: string;
            public selectedPageId: string;
            public oppositePageId: string;
            public previousPage: string;
            public previousPageId: string;
            public pageCount: number;
            public constructor();
            public SetSelectedIndex($value: number):void;
            public GetPages():System.Collections.Generic.List$1<FairyEditor.FControllerPage>;
            public GetPageIds($ret?: System.Collections.Generic.List$1<string>):System.Collections.Generic.List$1<string>;
            public GetPageNames($ret?: System.Collections.Generic.List$1<string>):System.Collections.Generic.List$1<string>;
            public HasPageId($value: string):boolean;
            public HasPageName($value: string):boolean;
            public GetNameById($id: string, $emptyMsg: string):string;
            public GetNamesByIds($ids: System.Collections.IList, $emptyMsg: string):string;
            public AddPage($name: string):FairyEditor.FControllerPage;
            public AddPageAt($name: string, $index: number):FairyEditor.FControllerPage;
            public RemovePageAt($index: number):void;
            public SetPages($pages: System.Collections.Generic.IList$1<string>):void;
            public SwapPage($index1: number, $index2: number):void;
            public GetActions():System.Collections.Generic.List$1<FairyEditor.FControllerAction>;
            public AddAction($type: string):FairyEditor.FControllerAction;
            public RemoveAction($action: FairyEditor.FControllerAction):void;
            public SwapAction($index1: number, $index2: number):void;
            public RunActions():void;
            public Read($xml: FairyGUI.Utils.XML):void;
            public Write():FairyGUI.Utils.XML;
            public Reset():void;
            
        }
        enum ObjectPropID { Text = 0, Icon = 1, Color = 2, OutlineColor = 3, Playing = 4, Frame = 5, DeltaTime = 6, TimeScale = 7, FontSize = 8, Selected = 9 }
        class FEvents extends System.Object {
            public static POS_CHANGED: string;
            public static SIZE_CHANGED: string;
            public static CHANGED: string;
            public static PLAY_END: string;
            public static SUBMIT: string;
            public static ADDED: string;
            public static REMOVED: string;
            public static CLICK_ITEM: string;
            
        }
        class AlignConst extends System.Object {
            public static LEFT: string;
            public static CENTER: string;
            public static RIGHT: string;
            public static Parse($str: string):FairyGUI.AlignType;
            public static ToString($type: FairyGUI.AlignType):string;
            public ToString():string;
            
        }
        class VerticalAlignConst extends System.Object {
            public static TOP: string;
            public static MIDDLE: string;
            public static BOTTOM: string;
            public static Parse($str: string):FairyGUI.VertAlignType;
            public static ToString($type: FairyGUI.VertAlignType):string;
            public ToString():string;
            
        }
        class AutoSizeConst extends System.Object {
            public static NONE: string;
            public static HEIGHT: string;
            public static BOTH: string;
            public static SHRINK: string;
            public static Parse($str: string):FairyGUI.AutoSizeType;
            public static ToString($type: FairyGUI.AutoSizeType):string;
            public ToString():string;
            
        }
        class OverflowConst extends System.Object {
            public static VISIBLE: string;
            public static HIDDEN: string;
            public static SCROLL: string;
            
        }
        class ScrollBarDisplayConst extends System.Object {
            public static DEFAULT: string;
            public static VISIBLE: string;
            public static AUTO: string;
            public static HIDDEN: string;
            
        }
        class ScrollConst extends System.Object {
            public static HORIZONTAL: string;
            public static VERTICAL: string;
            public static BOTH: string;
            
        }
        class FlipConst extends System.Object {
            public static NONE: string;
            public static HZ: string;
            public static VT: string;
            public static BOTH: string;
            public static Parse($str: string):FairyGUI.FlipType;
            
        }
        class LoaderFillConst extends System.Object {
            public static NONE: string;
            public static SCALE_SHOW_ALL: string;
            public static SCALE_NO_BORDER: string;
            public static SCALE_MATCH_HEIGHT: string;
            public static SCALE_MATCH_WIDTH: string;
            public static SCALE_FREE: string;
            
        }
        class FillMethodConst extends System.Object {
            public static Parse($str: string):FairyGUI.FillMethod;
            
        }
        class EaseTypeConst extends System.Object {
            public static easeType: System.Array$1<string>;
            public static easeInOutType: System.Array$1<string>;
            public static Parse($value: string):FairyGUI.EaseType;
            
        }
        class FButton extends FairyEditor.ComExtention {
            public changeStageOnClick: boolean;
            public static COMMON: string;
            public static CHECK: string;
            public static RADIO: string;
            public static UP: string;
            public static DOWN: string;
            public static OVER: string;
            public static SELECTED_OVER: string;
            public static DISABLED: string;
            public static SELECTED_DISABLED: string;
            public icon: string;
            public selectedIcon: string;
            public title: string;
            public text: string;
            public selectedTitle: string;
            public titleColor: UnityEngine.Color;
            public titleColorSet: boolean;
            public titleFontSize: number;
            public titleFontSizeSet: boolean;
            public sound: string;
            public volume: number;
            public baseSound: string;
            public baseVolume: number;
            public soundSet: boolean;
            public downEffect: string;
            public downEffectValue: number;
            public selected: boolean;
            public mode: string;
            public controller: string;
            public controllerObj: FairyEditor.FController;
            public page: string;
            public constructor();
            public GetTextField():FairyEditor.FTextField;
            public HandleGrayChanged():boolean;
            
        }
        class FTextField extends FairyEditor.FObject {
            public clearOnPublish: boolean;
            public text: string;
            public textFormat: FairyGUI.TextFormat;
            public supportProEffect: boolean;
            public font: string;
            public fontSize: number;
            public color: UnityEngine.Color;
            public align: string;
            public verticalAlign: string;
            public leading: number;
            public letterSpacing: number;
            public underline: boolean;
            public bold: boolean;
            public italic: boolean;
            public strike: boolean;
            public stroke: boolean;
            public strokeColor: UnityEngine.Color;
            public strokeSize: number;
            public shadowY: number;
            public shadowX: number;
            public shadow: boolean;
            public shadowColor: UnityEngine.Color;
            public outlineSoftness: number;
            public underlaySoftness: number;
            public faceDilate: number;
            public ubbEnabled: boolean;
            public varsEnabled: boolean;
            public autoSize: string;
            public singleLine: boolean;
            public constructor($flags: number);
            public InitFrom($other: FairyEditor.FTextField):void;
            public CopyTextFormat($source: FairyEditor.FTextField):void;
            
        }
        class FComboBox extends FairyEditor.ComExtention {
            public clearOnPublish: boolean;
            public title: string;
            public text: string;
            public icon: string;
            public titleColor: UnityEngine.Color;
            public titleColorSet: boolean;
            public dropdown: string;
            public visibleItemCount: number;
            public direction: string;
            public items: System.Array$1<System.Array$1<string>>;
            public selectedIndex: number;
            public selectionController: string;
            public selectionControllerObj: FairyEditor.FController;
            public sound: string;
            public volume: number;
            public constructor();
            public GetTextField():FairyEditor.FTextField;
            
        }
        class FTransitions extends System.Object {
            public _loadingSnapshot: boolean;
            public items: System.Collections.Generic.List$1<FairyEditor.FTransition>;
            public isEmpty: boolean;
            public constructor($owner: FairyEditor.FComponent);
            public AddItem($name?: string):FairyEditor.FTransition;
            public RemoveItem($item: FairyEditor.FTransition):void;
            public GetItem($name: string):FairyEditor.FTransition;
            public Read($xml: FairyGUI.Utils.XML):void;
            public Write($xml?: FairyGUI.Utils.XML):FairyGUI.Utils.XML;
            public Dispose():void;
            public ClearSnapshot():void;
            public TakeSnapshot():void;
            public ReadSnapshot($readController?: boolean):void;
            public OnOwnerAddedToStage():void;
            public OnOwnerRemovedFromStage():void;
            
        }
        class FScrollPane extends System.Object {
            public static DISPLAY_ON_LEFT: number;
            public static SNAP_TO_ITEM: number;
            public static DISPLAY_IN_DEMAND: number;
            public static PAGE_MODE: number;
            public static TOUCH_EFFECT_ON: number;
            public static TOUCH_EFFECT_OFF: number;
            public static BOUNCE_BACK_EFFECT_ON: number;
            public static BOUNCE_BACK_EFFECT_OFF: number;
            public static INERTIA_DISABLED: number;
            public static MASK_DISABLED: number;
            public static FLOATING: number;
            public static DONT_CLIP_MARGIN: number;
            public vtScrollBar: FairyEditor.FScrollBar;
            public hzScrollBar: FairyEditor.FScrollBar;
            public owner: FairyEditor.FComponent;
            public percX: number;
            public percY: number;
            public posX: number;
            public posY: number;
            public contentWidth: number;
            public contentHeight: number;
            public viewWidth: number;
            public viewHeight: number;
            public pageX: number;
            public pageY: number;
            public constructor($owner: FairyEditor.FComponent);
            public Dispose():void;
            public Install():void;
            public Uninstall():void;
            public SetPercX($value: number, $ani?: boolean):void;
            public SetPercY($value: number, $ani?: boolean):void;
            public SetPosX($value: number, $ani?: boolean):void;
            public SetPosY($value: number, $ani?: boolean):void;
            public SetPageX($value: number, $ani?: boolean):void;
            public SetPageY($value: number, $ani?: boolean):void;
            public ScrollTop($ani?: boolean):void;
            public ScrollBottom($ani?: boolean):void;
            public ScrollUp($ratio?: number, $ani?: boolean):void;
            public ScrollDown($ratio?: number, $ani?: boolean):void;
            public ScrollLeft($ratio?: number, $ani?: boolean):void;
            public ScrollRight($ratio?: number, $ani?: boolean):void;
            public ScrollToView($obj: FairyEditor.FObject, $ani?: boolean, $setFirst?: boolean):void;
            public ScrollToView($rect: UnityEngine.Rect, $ani?: boolean, $setFirst?: boolean):void;
            public OnOwnerSizeChanged():void;
            public OnFlagsChanged($forceReceate?: boolean):void;
            public Validate($checkOnly?: boolean):boolean;
            public UpdateScrollRect():void;
            public SetContentSize($aWidth: number, $aHeight: number):void;
            public HandleControllerChanged($c: FairyEditor.FController):void;
            public UpdateScrollBarVisible():void;
            
        }
        class FMargin extends System.Object {
            public left: number;
            public right: number;
            public top: number;
            public bottom: number;
            public empty: boolean;
            public constructor();
            public Parse($str: string):void;
            public Reset():void;
            public Copy($source: FairyEditor.FMargin):void;
            
        }
        class FControllerPage extends System.Object {
            public id: string;
            public name: string;
            public remark: string;
            public constructor();
            
        }
        class FControllerAction extends System.Object {
            public type: string;
            public fromPage: System.Array$1<string>;
            public toPage: System.Array$1<string>;
            public transitionName: string;
            public repeat: number;
            public delay: number;
            public stopOnExit: boolean;
            public objectId: string;
            public controllerName: string;
            public targetPage: string;
            public constructor();
            public Run($controller: FairyEditor.FController, $prevPage: string, $curPage: string):void;
            public Reset():void;
            public GetFullControllerName($gcom: FairyEditor.FComponent):string;
            public GetControllerObj($gcom: FairyEditor.FComponent):FairyEditor.FController;
            public Read($xml: FairyGUI.Utils.XML):void;
            public Write():FairyGUI.Utils.XML;
            
        }
        class FCustomEase extends FairyGUI.CustomEase {
            public points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>;
            public constructor();
            public Update():void;
            
        }
        class FDisplayObject extends FairyGUI.Container {
            public owner: FairyEditor.FObject;
            public container: FairyGUI.Container;
            public content: FairyGUI.DisplayObject;
            public errorStatus: boolean;
            public constructor($owner: FairyEditor.FObject);
            public Reset():void;
            public HandleSizeChanged():void;
            public SetLoading($value: boolean):void;
            public ApplyBlendMode():void;
            public ApplyFilter():void;
            
        }
        class FGraph extends FairyEditor.FObject {
            public static EMPTY: string;
            public static RECT: string;
            public static ELLIPSE: string;
            public static POLYGON: string;
            public static REGULAR_POLYGON: string;
            public type: string;
            public isVerticesEditable: boolean;
            public shapeLocked: boolean;
            public cornerRadius: string;
            public lineColor: UnityEngine.Color;
            public lineSize: number;
            public fillColor: UnityEngine.Color;
            public polygonPoints: System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public verticesDistance: System.Collections.Generic.List$1<number>;
            public sides: number;
            public startAngle: number;
            public polygonData: any;
            public constructor($flags: number);
            public AddVertex($vx: number, $vy: number, $near: boolean):void;
            public RemoveVertex($index: number):void;
            public UpdateVertex($index: number, $xv: number, $yv: number):void;
            public UpdateVertexDistance($index: number, $value: number):void;
            public CalculatePolygonBounds():UnityEngine.Rect;
            public UpdateGraph():void;
            public HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class FGroup extends FairyEditor.FObject {
            public _updating: number;
            public _childrenDirty: boolean;
            public static HORIZONTAL: string;
            public static VERTICAL: string;
            public advanced: boolean;
            public excludeInvisibles: boolean;
            public autoSizeDisabled: boolean;
            public mainGridMinSize: number;
            public mainGridIndex: number;
            public hasMainGrid: boolean;
            public collapsed: boolean;
            public layout: string;
            public lineGap: number;
            public columnGap: number;
            public boundsChanged: boolean;
            public children: System.Collections.Generic.List$1<FairyEditor.FObject>;
            public empty: boolean;
            public constructor($flags: number);
            public Refresh($positionChangedOnly?: boolean):void;
            public FreeChildrenArray():void;
            public GetStartIndex():number;
            public UpdateImmdediately($param?: any):void;
            public MoveChildren($dx: number, $dy: number):void;
            public ResizeChildren($dw: number, $dh: number):void;
            
        }
        class FilterData extends System.Object {
            public type: string;
            public brightness: number;
            public contrast: number;
            public saturation: number;
            public hue: number;
            public constructor();
            public Read($xml: FairyGUI.Utils.XML):void;
            public Write($xml: FairyGUI.Utils.XML):void;
            public CopyFrom($source: FairyEditor.FilterData):void;
            public Clone():FairyEditor.FilterData;
            
        }
        class FImage extends FairyEditor.FObject {
            public color: UnityEngine.Color;
            public flip: string;
            public fillOrigin: number;
            public fillClockwise: boolean;
            public fillMethod: string;
            public fillAmount: number;
            public bitmap: FairyGUI.Image;
            public constructor($flags: number);
            public HitTest($contentRect: UnityEngine.Rect, $localPoint: UnityEngine.Vector2):boolean;
            
        }
        class FLabel extends FairyEditor.ComExtention {
            public restrict: string;
            public maxLength: number;
            public keyboardType: number;
            public icon: string;
            public title: string;
            public text: string;
            public titleColor: UnityEngine.Color;
            public titleColorSet: boolean;
            public titleFontSize: number;
            public titleFontSizeSet: boolean;
            public input: boolean;
            public password: boolean;
            public promptText: string;
            public sound: string;
            public volume: number;
            public constructor();
            public GetTextField():FairyEditor.FTextField;
            
        }
        class FList extends FairyEditor.FComponent {
            public clearOnPublish: boolean;
            public scrollItemToViewOnClick: boolean;
            public foldInvisibleItems: boolean;
            public clickToExpand: number;
            public static SINGLE_COLUMN: string;
            public static SINGLE_ROW: string;
            public static FLOW_HZ: string;
            public static FLOW_VT: string;
            public static PAGINATION: string;
            public layout: string;
            public selectionMode: string;
            public lineGap: number;
            public columnGap: number;
            public repeatX: number;
            public repeatY: number;
            public defaultItem: string;
            public autoResizeItem: boolean;
            public autoResizeItem1: boolean;
            public autoResizeItem2: boolean;
            public treeViewEnabled: boolean;
            public indent: number;
            public items: System.Collections.Generic.List$1<FairyEditor.ListItemData>;
            public align: string;
            public verticalAlign: string;
            public selectionController: string;
            public selectionControllerObj: FairyEditor.FController;
            public selectedIndex: number;
            public constructor($flags: number);
            public GetSelection($result?: System.Collections.Generic.List$1<number>):System.Collections.Generic.List$1<number>;
            public AddSelection($index: number, $scrollItToView?: boolean):void;
            public RemoveSelection($index: number):void;
            public ClearSelection():void;
            public AddItem($url: string):FairyEditor.FObject;
            public AddItemAt($url: string, $index: number):FairyEditor.FObject;
            public ResizeToFit($itemCount?: number, $minSize?: number):void;
            
        }
        class ListItemData extends System.Object {
            public url: string;
            public name: string;
            public title: string;
            public icon: string;
            public selectedTitle: string;
            public selectedIcon: string;
            public level: number;
            public properties: System.Collections.Generic.List$1<FairyEditor.ComProperty>;
            public constructor();
            public CopyFrom($source: FairyEditor.ListItemData):void;
            
        }
        class FLoader extends FairyEditor.FObject {
            public clearOnPublish: boolean;
            public url: string;
            public texture: FairyGUI.NTexture;
            public icon: string;
            public align: string;
            public verticalAlign: string;
            public fill: string;
            public shrinkOnly: boolean;
            public autoSize: boolean;
            public playing: boolean;
            public frame: number;
            public showErrorSign: boolean;
            public color: UnityEngine.Color;
            public fillOrigin: number;
            public fillClockwise: boolean;
            public fillMethod: string;
            public fillAmount: number;
            public contentRes: FairyEditor.ResourceRef;
            public constructor($flags: number);
            
        }
        class ResourceRef extends System.Object {
            public packageItem: FairyEditor.FPackageItem;
            public displayItem: FairyEditor.FPackageItem;
            public displayFont: FairyGUI.BaseFont;
            public name: string;
            public desc: string;
            public isMissing: boolean;
            public missingInfo: FairyEditor.MissingInfo;
            public sourceWidth: number;
            public sourceHeight: number;
            public constructor($pi?: FairyEditor.FPackageItem, $missingInfo?: FairyEditor.MissingInfo, $ownerFlags?: number);
            public SetPackageItem($res: FairyEditor.FPackageItem, $ownerFlags?: number):void;
            public IsObsolete():boolean;
            public GetURL():string;
            public Update():void;
            public Release():void;
            
        }
        class FLoader3D extends FairyEditor.FObject {
            public clearOnPublish: boolean;
            public url: string;
            public icon: string;
            public autoSize: boolean;
            public fill: string;
            public shrinkOnly: boolean;
            public align: string;
            public verticalAlign: string;
            public playing: boolean;
            public frame: number;
            public animationName: string;
            public skinName: string;
            public loop: boolean;
            public color: UnityEngine.Color;
            public spineObj: Spine.Unity.SkeletonAnimation;
            public dbObj: DragonBones.UnityArmatureComponent;
            public contentRes: FairyEditor.ResourceRef;
            public constructor($flags: number);
            
        }
        class FMovieClip extends FairyEditor.FObject {
            public playing: boolean;
            public frame: number;
            public color: UnityEngine.Color;
            public constructor($flags: number);
            public Advance($time: number):void;
            
        }
        class FTreeNode extends System.Object {
            public expanded: boolean;
            public isFolder: boolean;
            public parent: FairyEditor.FTreeNode;
            public data: any;
            public text: string;
            public cell: FairyEditor.FComponent;
            public level: number;
            public numChildren: number;
            public tree: FairyEditor.FTree;
            public constructor($hasChild: boolean, $resURL?: string);
            public AddChild($child: FairyEditor.FTreeNode):FairyEditor.FTreeNode;
            public AddChildAt($child: FairyEditor.FTreeNode, $index: number):FairyEditor.FTreeNode;
            public RemoveChild($child: FairyEditor.FTreeNode):FairyEditor.FTreeNode;
            public RemoveChildAt($index: number):FairyEditor.FTreeNode;
            public RemoveChildren($beginIndex?: number, $endIndex?: number):void;
            public GetChildAt($index: number):FairyEditor.FTreeNode;
            public GetChildIndex($child: FairyEditor.FTreeNode):number;
            public GetPrevSibling():FairyEditor.FTreeNode;
            public GetNextSibling():FairyEditor.FTreeNode;
            public SetChildIndex($child: FairyEditor.FTreeNode, $index: number):void;
            public SwapChildren($child1: FairyEditor.FTreeNode, $child2: FairyEditor.FTreeNode):void;
            public SwapChildrenAt($index1: number, $index2: number):void;
            public ExpandToRoot():void;
            
        }
        class FPackage extends System.Object {
            public opened: boolean;
            public project: FairyEditor.FProject;
            public id: string;
            public name: string;
            public basePath: string;
            public cacheFolder: string;
            public metaFolder: string;
            public items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public publishSettings: FairyEditor.PublishSettings;
            public rootItem: FairyEditor.FPackageItem;
            public strings: System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>;
            public constructor($project: FairyEditor.FProject, $folder: string);
            public GetBranchRootItem($branch: string):FairyEditor.FPackageItem;
            public BeginBatch():void;
            public EndBatch():void;
            public Open():void;
            public Save():void;
            public SetChanged():void;
            public Touch():void;
            public Dispose():void;
            public EnsureOpen():void;
            public FreeUnusedResources($ignoreTimeStamp: boolean):void;
            public GetNextId():string;
            public GetSequenceName($resName: string):string;
            public GetUniqueName($folder: FairyEditor.FPackageItem, $fileName: string):string;
            public GetItemListing($folder: FairyEditor.FPackageItem, $filters?: System.Array$1<string>, $sorted?: boolean, $recursive?: boolean, $result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public GetFavoriteItems($result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public GetItem($itemId: string):FairyEditor.FPackageItem;
            public FindItemByName($itemName: string):FairyEditor.FPackageItem;
            public GetItemByPath($path: string):FairyEditor.FPackageItem;
            public GetItemByName($folder: FairyEditor.FPackageItem, $itemName: string):FairyEditor.FPackageItem;
            public GetItemByFileName($folder: FairyEditor.FPackageItem, $fileName: string):FairyEditor.FPackageItem;
            public GetItemPath($pi: FairyEditor.FPackageItem, $result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public AddItem($pi: FairyEditor.FPackageItem):void;
            public RenameItem($pi: FairyEditor.FPackageItem, $newName: string):void;
            public MoveItem($pi: FairyEditor.FPackageItem, $path: string):void;
            public DeleteItem($pi: FairyEditor.FPackageItem):number;
            public DuplicateItem($pi: FairyEditor.FPackageItem, $newName: string):FairyEditor.FPackageItem;
            public EnsurePathExists($path: string, $allowCreateDirectory: boolean):FairyEditor.FPackageItem;
            public GetBranchPath($branch: string):string;
            public CreateBranch($branch: string):void;
            public CreateFolder($cname: string, $path?: string, $autoRename?: boolean):FairyEditor.FPackageItem;
            public CreatePath($path: string):FairyEditor.FPackageItem;
            public CreateComponentItem($cname: string, $width: number, $height: number, $path?: string, $extentionId?: string, $exported?: boolean, $autoRename?: boolean):FairyEditor.FPackageItem;
            public CreateFontItem($cname: string, $path?: string, $autoRename?: boolean):FairyEditor.FPackageItem;
            public CreateMovieClipItem($cname: string, $path?: string, $autoRename?: boolean):FairyEditor.FPackageItem;
            public ImportResource($sourceFile: string, $toPath: string, $resName: string):System.Threading.Tasks.Task$1<FairyEditor.FPackageItem>;
            public UpdateResource($pi: FairyEditor.FPackageItem, $sourceFile: string):System.Threading.Tasks.Task;
            
        }
        class FRelations extends System.Object {
            public handling: FairyEditor.FObject;
            public widthLocked: boolean;
            public heightLocked: boolean;
            public items: System.Collections.Generic.List$1<FairyEditor.FRelationItem>;
            public isEmpty: boolean;
            public constructor($owner: FairyEditor.FObject);
            public AddItem($target: FairyEditor.FObject, $type: number, $usePercent?: boolean):FairyEditor.FRelationItem;
            public AddItem($target: FairyEditor.FObject, $sidePair: string):FairyEditor.FRelationItem;
            public RemoveItem($item: FairyEditor.FRelationItem):void;
            public ReplaceItem($index: number, $target: FairyEditor.FObject, $sidePair: string):void;
            public GetItem($target: FairyEditor.FObject):FairyEditor.FRelationItem;
            public HasTarget($target: FairyEditor.FObject):boolean;
            public RemoveTarget($target: FairyEditor.FObject):void;
            public ReplaceTarget($originTarget: FairyEditor.FObject, $newTarget: FairyEditor.FObject):void;
            public OnOwnerSizeChanged($dWidth: number, $dHeight: number, $applyPivot: boolean):void;
            public Reset():void;
            public Read($xml: FairyGUI.Utils.XML, $inSource?: boolean):void;
            public Write($xml?: FairyGUI.Utils.XML):FairyGUI.Utils.XML;
            
        }
        class ObjectSnapshot extends System.Object {
            public x: number;
            public y: number;
            public width: number;
            public height: number;
            public scaleX: number;
            public scaleY: number;
            public skewX: number;
            public skewY: number;
            public pivotX: number;
            public pivotY: number;
            public anchor: boolean;
            public alpha: number;
            public rotation: number;
            public color: UnityEngine.Color;
            public playing: boolean;
            public frame: number;
            public visible: boolean;
            public filterData: FairyEditor.FilterData;
            public text: string;
            public icon: string;
            public constructor();
            public static GetFromPool($obj: FairyEditor.FObject):FairyEditor.ObjectSnapshot;
            public static ReturnToPool($col: System.Collections.Generic.List$1<FairyEditor.ObjectSnapshot>):void;
            public Take():void;
            public Load():void;
            
        }
        class FObjectFactory extends System.Object {
            public static constructingDepth: number;
            public constructor();
            public static CreateObject($pi: FairyEditor.FPackageItem, $flags?: number):FairyEditor.FObject;
            public static CreateObject($pkg: FairyEditor.FPackage, $type: string, $missingInfo?: FairyEditor.MissingInfo, $flags?: number):FairyEditor.FObject;
            public static CreateObject($di: FairyEditor.ComponentAsset.DisplayListItem, $flags?: number):FairyEditor.FObject;
            public static NewObject($pi: FairyEditor.FPackageItem, $flags?: number):FairyEditor.FObject;
            public static NewObject($pkg: FairyEditor.FPackage, $type: string, $missingInfo?: FairyEditor.MissingInfo, $flags?: number):FairyEditor.FObject;
            public static NewObject($di: FairyEditor.ComponentAsset.DisplayListItem, $flags?: number):FairyEditor.FObject;
            public static NewExtention($pkg: FairyEditor.FPackage, $type: string):FairyEditor.ComExtention;
            public static GetClassByType($type: string):System.Type;
            
        }
        class MissingInfo extends System.Object {
            public pkgName: string;
            public pkgId: string;
            public itemId: string;
            public fileName: string;
            public constructor($pkgId: string, $itemId: string, $fileName: string);
            public constructor($url: string);
            
        }
        class FObjectFlags extends System.Object {
            public static IN_DOC: number;
            public static IN_TEST: number;
            public static IN_PREVIEW: number;
            public static INSPECTING: number;
            public static ROOT: number;
            public constructor();
            public static IsDocRoot($flags: number):boolean;
            public static GetScaleLevel($flags: number):number;
            
        }
        class FObjectType extends System.Object {
            public static PACKAGE: string;
            public static FOLDER: string;
            public static IMAGE: string;
            public static GRAPH: string;
            public static LIST: string;
            public static LOADER: string;
            public static TEXT: string;
            public static RICHTEXT: string;
            public static INPUTTEXT: string;
            public static GROUP: string;
            public static SWF: string;
            public static MOVIECLIP: string;
            public static COMPONENT: string;
            public static Loader3D: string;
            public static EXT_BUTTON: string;
            public static EXT_LABEL: string;
            public static EXT_COMBOBOX: string;
            public static EXT_PROGRESS_BAR: string;
            public static EXT_SLIDER: string;
            public static EXT_SCROLLBAR: string;
            public static NAME_PREFIX: System.Collections.Generic.Dictionary$2<string, string>;
            public constructor();
            
        }
        class PublishSettings extends System.Object {
            public path: string;
            public fileName: string;
            public packageCount: number;
            public genCode: boolean;
            public codePath: string;
            public branchPath: string;
            public useGlobalAtlasSettings: boolean;
            public atlasList: System.Collections.Generic.List$1<FairyEditor.AtlasSettings>;
            public excludedList: System.Collections.Generic.List$1<string>;
            public constructor();
            public FillCombo($cb: FairyGUI.GComboBox):void;
            
        }
        class FPackageItemType extends System.Object {
            public static FOLDER: string;
            public static IMAGE: string;
            public static SWF: string;
            public static MOVIECLIP: string;
            public static SOUND: string;
            public static COMPONENT: string;
            public static FONT: string;
            public static MISC: string;
            public static ATLAS: string;
            public static SPINE: string;
            public static DRAGONBONES: string;
            public static fileExtensionMap: System.Collections.Generic.Dictionary$2<string, string>;
            public constructor();
            public static GetFileType($file: string):string;
            
        }
        class FProgressBar extends FairyEditor.ComExtention {
            public static TITLE_PERCENT: string;
            public static TITLE_VALUE_AND_MAX: string;
            public static TITLE_VALUE_ONLY: string;
            public static TITLE_MAX_ONLY: string;
            public titleType: string;
            public min: number;
            public max: number;
            public value: number;
            public reverse: boolean;
            public sound: string;
            public volume: number;
            public constructor();
            public Update():void;
            
        }
        class SettingsBase extends System.Object {
            public fileName: string;
            public Touch($forced?: boolean):void;
            public Save():void;
            
        }
        class FRelationDef extends System.Object {
            public affectBySelfSizeChanged: boolean;
            public percent: boolean;
            public type: number;
            public constructor();
            
        }
        class FRelationItem extends System.Object {
            public owner: FairyEditor.FObject;
            public readOnly: boolean;
            public target: FairyEditor.FObject;
            public containsWidthRelated: boolean;
            public containsHeightRelated: boolean;
            public defs: System.Collections.Generic.List$1<FairyEditor.FRelationDef>;
            public desc: string;
            public constructor($owner: FairyEditor.FObject);
            public Set($target: FairyEditor.FObject, $sidePairs: string, $readOnly?: boolean):void;
            public Dispose():void;
            public AddDef($type: number, $usePercent?: boolean, $checkDuplicated?: boolean):void;
            public AddDefs($sidePairs: string, $checkDuplicated?: boolean):void;
            public HasDef($type: number):boolean;
            public ApplySelfSizeChanged($dWidth: number, $dHeight: number, $applyPivot: boolean):void;
            
        }
        class FRelationType extends System.Object {
            public static Left_Left: number;
            public static Left_Center: number;
            public static Left_Right: number;
            public static Center_Center: number;
            public static Right_Left: number;
            public static Right_Center: number;
            public static Right_Right: number;
            public static LeftExt_Left: number;
            public static LeftExt_Right: number;
            public static RightExt_Left: number;
            public static RightExt_Right: number;
            public static Width: number;
            public static Top_Top: number;
            public static Top_Middle: number;
            public static Top_Bottom: number;
            public static Middle_Middle: number;
            public static Bottom_Top: number;
            public static Bottom_Middle: number;
            public static Bottom_Bottom: number;
            public static TopExt_Top: number;
            public static TopExt_Bottom: number;
            public static BottomExt_Top: number;
            public static BottomExt_Bottom: number;
            public static Height: number;
            public static Size: number;
            public static Names: System.Array$1<string>;
            public constructor();
            
        }
        class FRichTextField extends FairyEditor.FTextField {
            public constructor($flags: number);
            
        }
        class FScrollBar extends FairyEditor.ComExtention {
            public minSize: number;
            public fixedGripSize: boolean;
            public gripDragging: boolean;
            public constructor();
            public SetScrollPane($scrollPane: FairyEditor.FScrollPane, $vertical: boolean):void;
            public SetDisplayPerc($value: number):void;
            public SetScrollPerc($val: number):void;
            
        }
        class FSlider extends FairyEditor.ComExtention {
            public changeOnClick: boolean;
            public static TITLE_PERCENT: string;
            public static TITLE_VALUE_AND_MAX: string;
            public static TITLE_VALUE_ONLY: string;
            public static TITLE_MAX_ONLY: string;
            public titleType: string;
            public min: number;
            public max: number;
            public value: number;
            public reverse: boolean;
            public wholeNumbers: boolean;
            public constructor();
            public Update():void;
            
        }
        class FSwfObject extends FairyEditor.FObject {
            public playing: boolean;
            public frame: number;
            public constructor($flags: number);
            public Advance($timeInMiniseconds: number):void;
            
        }
        class FTextInput extends FairyEditor.FTextField {
            public password: boolean;
            public keyboardType: number;
            public maxLength: number;
            public restrict: string;
            public promptText: string;
            public constructor($flags: number);
            
        }
        class FTransition extends System.Object {
            public static OPTION_IGNORE_DISPLAY_CONTROLLER: number;
            public static OPTION_AUTO_STOP_DISABLED: number;
            public static OPTION_AUTO_STOP_AT_END: number;
            public owner: FairyEditor.FComponent;
            public name: string;
            public options: number;
            public autoPlay: boolean;
            public autoPlayDelay: number;
            public autoPlayRepeat: number;
            public frameRate: number;
            public items: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>;
            public maxFrame: number;
            public playing: boolean;
            public playTimes: number;
            public constructor($owner: FairyEditor.FComponent);
            public Dispose():void;
            public CreateItem($targetId: string, $type: string, $frame: number):FairyEditor.FTransitionItem;
            public FindItem($frame: number, $targetId: string, $type: string):FairyEditor.FTransitionItem;
            public FindItems($frameStart: number, $frameEnd: number, $targetId: string, $type: string, $result: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>):void;
            public GetItemWithPath($frame: number, $targetId: string):FairyEditor.FTransitionItem;
            public AddItem($transItem: FairyEditor.FTransitionItem):void;
            public AddItems($items: System.Collections.Generic.IEnumerable$1<FairyEditor.FTransitionItem>):void;
            public DeleteItem($item: FairyEditor.FTransitionItem):void;
            public DeleteItems($targetId: string, $type: string):System.Array$1<FairyEditor.FTransitionItem>;
            public CopyItems($targetId: string, $type: string):FairyGUI.Utils.XML;
            public CopyItems($items: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>):FairyGUI.Utils.XML;
            public static GetAllowType($obj: FairyEditor.FObject, $type: string):boolean;
            public static SupportTween($type: string):boolean;
            public UpdateFromRelations($targetId: string, $dx: number, $dy: number):void;
            public Validate():void;
            public Read($xml: FairyGUI.Utils.XML):void;
            public Write($forSaving: boolean):FairyGUI.Utils.XML;
            public OnExit():void;
            public OnOwnerAddedToStage():void;
            public OnOwnerRemovedFromStage():void;
            public Play($onComplete?: System.Action, $times?: number, $delay?: number, $startFrame?: number, $endFrame?: number, $editMode?: boolean):void;
            public Stop($setToComplete?: boolean, $processCallback?: boolean):void;
            public GetProperty($propName: string):any;
            public SetProperty($propName: string, $value: any):void;
            public static ReadItems($owner: FairyEditor.FTransition, $col: System.Collections.Generic.List$1<FairyGUI.Utils.XML>, $result: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>):void;
            public static WriteItems($items: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>, $xml: FairyGUI.Utils.XML, $forSaving: boolean):void;
            
        }
        class FTransitionItem extends System.Object {
            public easeType: string;
            public easeInOutType: string;
            public repeat: number;
            public yoyo: boolean;
            public label: string;
            public value: FairyEditor.FTransitionValue;
            public tweenValue: FairyEditor.FTransitionValue;
            public pathOffsetX: number;
            public pathOffsetY: number;
            public target: FairyEditor.FObject;
            public owner: FairyEditor.FTransition;
            public tweener: FairyGUI.GTweener;
            public innerTrans: FairyEditor.FTransition;
            public nextItem: FairyEditor.FTransitionItem;
            public prevItem: FairyEditor.FTransitionItem;
            public displayLockToken: number;
            public type: string;
            public targetId: string;
            public frame: number;
            public tween: boolean;
            public easeName: string;
            public usePath: boolean;
            public path: FairyEditor.GPathExt;
            public pathPoints: System.Collections.Generic.List$1<FairyGUI.GPathPoint>;
            public customEase: FairyEditor.FCustomEase;
            public pathData: string;
            public customEaseData: string;
            public encodedValue: string;
            public constructor($owner: FairyEditor.FTransition);
            public SetPathToTweener():void;
            public AddPathPoint($px: number, $py: number, $near: boolean):void;
            public RemovePathPoint($pointIndex: number):void;
            public UpdatePathPoint($pointIndex: number, $x: number, $y: number):void;
            public UpdateControlPoint($pointIndex: number, $controlIndex: number, $x: number, $y: number):void;
            public GetProperty($propName: string):any;
            public SetProperty($propName: string, $value: any):void;
            
        }
        class FTransitionValue extends System.Object {
            public f1: number;
            public f2: number;
            public f3: number;
            public f4: number;
            public iu: UnityEngine.Color;
            public i: number;
            public s: string;
            public b1: boolean;
            public b2: boolean;
            public b3: boolean;
            public constructor();
            public CopyFrom($source: FairyEditor.FTransitionValue):void;
            public Reset():void;
            public Equals($other: FairyEditor.FTransitionValue):boolean;
            public Decode($type: string, $str: string):void;
            public Encode($type: string):string;
            public Equals($obj: any):boolean;
            
        }
        class GPathExt extends FairyGUI.GPath {
            public points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>;
            public constructor();
            public Update():void;
            public GetSegmentType($segmentIndex: number):FairyGUI.GPathPoint.CurveType;
            public GetAnchorsInSegment($segmentIndex: number, $result?: System.Collections.Generic.List$1<UnityEngine.Vector2>):System.Collections.Generic.List$1<UnityEngine.Vector2>;
            public FindSegmentNear($pt: UnityEngine.Vector3):number;
            public static PointLineDistance($pointX: number, $pointY: number, $startX: number, $startY: number, $endX: number, $endY: number, $isSegment: boolean):number;
            
        }
        class FTree extends FairyEditor.FTreeNode {
            public treeNodeRender: FairyEditor.FTree.TreeNodeRenderDelegate;
            public treeNodeWillExpand: FairyEditor.FTree.TreeNodeWillExpandDelegate;
            public indent: number;
            public constructor($list: FairyEditor.FList);
            public GetSelectedNode():FairyEditor.FTreeNode;
            public GetSelectedNodes($result?: System.Collections.Generic.List$1<FairyEditor.FTreeNode>):System.Collections.Generic.List$1<FairyEditor.FTreeNode>;
            public SelectNode($node: FairyEditor.FTreeNode, $scrollItToView?: boolean):void;
            public UnselectNode($node: FairyEditor.FTreeNode):void;
            public GetNodeIndex($node: FairyEditor.FTreeNode):number;
            public UpdateNode($node: FairyEditor.FTreeNode):void;
            public UpdateNodes($nodes: System.Collections.Generic.List$1<FairyEditor.FTreeNode>):void;
            public ExpandAll($folderNode?: FairyEditor.FTreeNode):void;
            public CollapseAll($folderNode?: FairyEditor.FTreeNode):void;
            public CreateCell($node: FairyEditor.FTreeNode):void;
            
        }
        class FHtmlImage extends System.Object {
            public loader: FairyEditor.FLoader;
            public displayObject: FairyGUI.DisplayObject;
            public element: FairyGUI.Utils.HtmlElement;
            public width: number;
            public height: number;
            public constructor();
            public Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            public SetPosition($x: number, $y: number):void;
            public Add():void;
            public Remove():void;
            public Release():void;
            public Dispose():void;
            
        }
        class FHtmlPageContext extends System.Object {
            public static inst: FairyGUI.Utils.HtmlPageContext;
            public constructor();
            public CreateObject($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):FairyGUI.Utils.IHtmlObject;
            public FreeObject($obj: FairyGUI.Utils.IHtmlObject):void;
            public GetImageTexture($image: FairyGUI.Utils.HtmlImage):FairyGUI.NTexture;
            public FreeImageTexture($image: FairyGUI.Utils.HtmlImage, $texture: FairyGUI.NTexture):void;
            
        }
        class ProjectType extends System.Object {
            public static Flash: string;
            public static Starling: string;
            public static Unity: string;
            public static Egret: string;
            public static Layabox: string;
            public static Haxe: string;
            public static PIXI: string;
            public static Cocos2dx: string;
            public static CryEngine: string;
            public static Vision: string;
            public static MonoGame: string;
            public static CocosCreator: string;
            public static LibGDX: string;
            public static Unreal: string;
            public static Corona: string;
            public static ThreeJS: string;
            public static IDs: System.Array$1<string>;
            public static Names: System.Array$1<string>;
            public constructor();
            
        }
        class PublishHandler extends System.Object {
            public static CODE_FILE_MARK: string;
            public genCodeHandler: System.Action$1<FairyEditor.PublishHandler>;
            public pkg: FairyEditor.FPackage;
            public project: FairyEditor.FProject;
            public isSuccess: boolean;
            public publishDescOnly: boolean;
            public exportPath: string;
            public exportCodePath: string;
            public useAtlas: boolean;
            public fileName: string;
            public fileExtension: string;
            public genCode: boolean;
            public items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public paused: boolean;
            public constructor($pkg: FairyEditor.FPackage, $branch: string);
            public ExportBinaryDesc($descFile: string):void;
            public ExportDescZip($zipArchive: System.IO.Compression.ZipStorer):void;
            public ExportResZip($zipArchive: System.IO.Compression.ZipStorer, $compress: boolean):void;
            public ExportResFiles($resPrefix: string):System.Threading.Tasks.Task;
            public ClearOldResFiles($folder: string):void;
            public CollectClasses($stripMember: boolean, $stripClass: boolean, $fguiNamespace: string):System.Collections.Generic.List$1<FairyEditor.PublishHandler.ClassInfo>;
            public SetupCodeFolder($path: string, $codeFileExtensions: string):void;
            public SetupCodeFolder($path: string, $codeFileExtensions: string, $fileMark: string):void;
            public ToFilename($source: string):string;
            public add_onComplete($value: System.Action):void;
            public remove_onComplete($value: System.Action):void;
            public IsInList($item: FairyEditor.FPackageItem):boolean;
            public GetItemDesc($item: FairyEditor.FPackageItem):any;
            public GetScriptData($item: FairyEditor.FPackageItem):FairyGUI.Utils.XML;
            public Run():System.Threading.Tasks.Task;
            
        }
        class Clipboard extends System.Object {
            public static TEXT: string;
            public static OBJECT_KEY: string;
            public static ITEM_KEY: string;
            public static TIMELINE_KEY: string;
            public static SetText($value: string):void;
            public static GetText():string;
            public static GetValue($key: string):any;
            public static SetValue($key: string, $value: any):void;
            public static HasFormat($key: string):boolean;
            
        }
        class ComponentTemplates extends System.Object {
            public constructor($pkg: FairyEditor.FPackage);
            public CreateLabelItem($cname: string, $width: number, $height: number, $path: string):FairyEditor.FPackageItem;
            public CreateButtonItem($cname: string, $extentionId: string, $mode: string, $images: System.Array$1<string>, $width: number, $height: number, $asListItem: boolean, $createRelations: boolean, $createText: boolean, $createIcon: boolean, $exported: boolean, $path: string):FairyEditor.FPackageItem;
            public CreateComboBoxItem($cname: string, $buttonImages: System.Array$1<string>, $bgImage: string, $itemImages: System.Array$1<string>, $path: string):FairyEditor.FPackageItem;
            public CreateScrollBarItem($cname: string, $type: number, $createArrows: boolean, $arrow1Images: System.Array$1<string>, $arrow2Images: System.Array$1<string>, $bgImage: string, $gripImages: System.Array$1<string>, $path: string):FairyEditor.FPackageItem;
            public CreateProgressBarItem($cname: string, $bgImage: string, $barImage: string, $titleType: string, $reverse: boolean, $path: string):FairyEditor.FPackageItem;
            public CreateSliderItem($cname: string, $type: number, $bgImage: string, $barImage: string, $gripImages: System.Array$1<string>, $titleType: string, $path: string):FairyEditor.FPackageItem;
            public CreatePopupMenuItem($cname: string, $bgImage: string, $itemImages: System.Array$1<string>, $path: string):FairyEditor.FPackageItem;
            public CreateWindowFrameItem($cname: string, $bgImage: string, $closeButton: string, $createTitle: boolean, $createIcon: boolean, $path: string):FairyEditor.FPackageItem;
            
        }
        class CopyHandler extends System.Object {
            public resultList: System.Collections.Generic.List$1<FairyEditor.DepItem>;
            public existsItemCount: number;
            public constructor();
            public InitWithItems($items: System.Collections.Generic.IList$1<FairyEditor.FPackageItem>, $targetPkg: FairyEditor.FPackage, $targetPath: string, $seekLevel: FairyEditor.DependencyQuery.SeekLevel):void;
            public InitWithObject($sourcePkg: FairyEditor.FPackage, $xml: FairyGUI.Utils.XML, $targetPkg: FairyEditor.FPackage, $targetPath: string, $ignoreExported?: boolean):void;
            public Copy($targetPkg: FairyEditor.FPackage, $overrideOption: FairyEditor.CopyHandler.OverrideOption, $isMove?: boolean):void;
            
        }
        class DepItem extends System.Object {
            public item: FairyEditor.FPackageItem;
            public content: any;
            public isSource: boolean;
            public analysed: boolean;
            public targetPath: string;
            public refCount: number;
            public constructor();
            
        }
        class CursorType extends System.Object {
            public static H_RESIZE: string;
            public static V_RESIZE: string;
            public static TL_RESIZE: string;
            public static TR_RESIZE: string;
            public static BL_RESIZE: string;
            public static BR_RESIZE: string;
            public static SELECT: string;
            public static HAND: string;
            public static DRAG: string;
            public static ADJUST: string;
            public static FINGER: string;
            public static COLOR_PICKER: string;
            public static WAIT: string;
            public constructor();
            
        }
        class DependencyQuery extends System.Object {
            public resultList: System.Collections.Generic.List$1<FairyEditor.DepItem>;
            public references: System.Collections.Generic.List$1<FairyEditor.ReferenceInfo>;
            public constructor();
            public QueryDependencies($items: System.Collections.Generic.IList$1<FairyEditor.FPackageItem>, $seekLevel: FairyEditor.DependencyQuery.SeekLevel):void;
            public QueryDependencies($project: FairyEditor.FProject, $url: string, $seekLevel: FairyEditor.DependencyQuery.SeekLevel):void;
            public QueryDependencies($pkg: FairyEditor.FPackage, $xml: FairyGUI.Utils.XML, $seekLevel: FairyEditor.DependencyQuery.SeekLevel):void;
            public QueryReferences($project: FairyEditor.FProject, $url: string):void;
            public ReplaceReferences($newItem: FairyEditor.FPackageItem):void;
            
        }
        class ReferenceInfo extends System.Object {
            public ownerPkg: FairyEditor.FPackage;
            public pkgId: string;
            public itemId: string;
            public content: any;
            public propKey: string;
            public arrayIndex: number;
            public valueType: FairyEditor.ReferenceInfo.ValueType;
            public constructor();
            public Update($newItem: FairyEditor.FPackageItem):boolean;
            
        }
        class EditorEvents extends System.Object {
            public static SelectionChanged: string;
            public static DocumentActivated: string;
            public static DocumentDeactivated: string;
            public static TestStart: string;
            public static TestStop: string;
            public static PackageListChanged: string;
            public static PackageReloaded: string;
            public static PackageTreeChanged: string;
            public static PackageItemChanged: string;
            public static HierarchyChanged: string;
            public static ProjectRefreshStart: string;
            public static ProjectRefreshEnd: string;
            public static BackgroundChanged: string;
            public static PluginListChanged: string;
            public constructor();
            
        }
        class ExportStringsHandler extends System.Object {
            public constructor();
            public Parse($pkgs: System.Collections.Generic.IList$1<FairyEditor.FPackage>, $ignoreDiscarded?: boolean):void;
            public Export($file: string, $merge: boolean):void;
            
        }
        class FindDuplicateResource extends System.Object {
            public result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public constructor();
            public GetGroup($index: number, $result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):void;
            public Start($pkgs: System.Collections.Generic.List$1<FairyEditor.FPackage>, $onProgress: System.Action$1<number>, $onComplete: System.Action):void;
            public Cancel():void;
            
        }
        class FindUnusedResource extends System.Object {
            public result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public constructor();
            public Start($pkgs: System.Collections.Generic.List$1<FairyEditor.FPackage>, $onProgress: System.Action$1<number>, $onComplete: System.Action):void;
            public Cancel():void;
            
        }
        class FullSearch extends System.Object {
            public result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public constructor();
            public Start($strName: string, $strTypes: string, $includeBrances: boolean):void;
            
        }
        class ImportStringsHandler extends System.Object {
            public strings: System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, System.Collections.Generic.Dictionary$2<string, string>>>;
            public constructor();
            public Parse($file: string):void;
            public Import():void;
            
        }
        class ProjectRefreshHandler extends System.Object {
            public constructor();
            public Dispose():void;
            public Run():void;
            
        }
        class ResourceImportQueue extends System.Object {
            public static Create($toPkg: FairyEditor.FPackage):FairyEditor.ResourceImportQueue;
            public Add($file: string, $targetPath?: string, $resName?: string):FairyEditor.ResourceImportQueue;
            public AddRelative($file: string, $targetPath?: string, $basePath?: string, $resName?: string):FairyEditor.ResourceImportQueue;
            public Process($callback?: System.Action$1<System.Collections.Generic.IList$1<FairyEditor.FPackageItem>>, $dropToDocument?: boolean, $dropPos?: System.Nullable$1<UnityEngine.Vector2>):void;
            
        }
        class ViewOptions extends System.Object {
            public title: string;
            public icon: string;
            public hResizePriority: number;
            public vResizePriority: number;
            public location: string;
            public constructor();
            
        }
        class AdaptationSettings extends FairyEditor.SettingsBase {
            public scaleMode: string;
            public screenMathMode: string;
            public designResolutionX: number;
            public designResolutionY: number;
            public devices: System.Collections.Generic.List$1<FairyEditor.AdaptationSettings.DeviceInfo>;
            public defaultDevices: System.Collections.Generic.List$1<FairyEditor.AdaptationSettings.DeviceInfo>;
            public constructor($project: FairyEditor.FProject);
            public GetDeviceResolution($device: string):FairyEditor.AdaptationSettings.DeviceInfo;
            public FillCombo($cb: FairyGUI.GComboBox):void;
            
        }
        class AtlasSettings extends System.Object {
            public name: string;
            public compression: boolean;
            public extractAlpha: boolean;
            public packSettings: FairyEditor.PackSettings;
            public constructor();
            public CopyFrom($source: FairyEditor.AtlasSettings):void;
            
        }
        class PackSettings extends System.Object {
            public pot: boolean;
            public mof: boolean;
            public padding: number;
            public rotation: boolean;
            public minWidth: number;
            public minHeight: number;
            public maxWidth: number;
            public maxHeight: number;
            public square: boolean;
            public fast: boolean;
            public edgePadding: boolean;
            public duplicatePadding: boolean;
            public multiPage: boolean;
            public constructor();
            public CopyFrom($source: FairyEditor.PackSettings):void;
            
        }
        class CommonSettings extends FairyEditor.SettingsBase {
            public font: string;
            public fontSize: number;
            public textColor: UnityEngine.Color;
            public fontAdjustment: boolean;
            public colorScheme: System.Collections.Generic.List$1<string>;
            public fontSizeScheme: System.Collections.Generic.List$1<string>;
            public fontScheme: System.Collections.Generic.List$1<string>;
            public scrollBars: FairyEditor.CommonSettings.ScrollBarConfig;
            public tipsRes: string;
            public buttonClickSound: string;
            public pivot: string;
            public constructor($project: FairyEditor.FProject);
            
        }
        class CustomProps extends FairyEditor.SettingsBase {
            public elements: System.Collections.Generic.Dictionary$2<string, string>;
            public constructor($project: FairyEditor.FProject);
            public FillCombo($cb: FairyGUI.GComboBox):void;
            
        }
        class GlobalPublishSettings extends FairyEditor.SettingsBase {
            public path: string;
            public branchPath: string;
            public fileExtension: string;
            public packageCount: number;
            public compressDesc: boolean;
            public binaryFormat: boolean;
            public jpegQuality: number;
            public compressPNG: boolean;
            public codeGeneration: FairyEditor.GlobalPublishSettings.CodeGenerationConfig;
            public includeHighResolution: number;
            public branchProcessing: number;
            public seperatedAtlasForBranch: boolean;
            public atlasSetting: FairyEditor.GlobalPublishSettings.AtlasSetting;
            public include2x: boolean;
            public include3x: boolean;
            public include4x: boolean;
            public constructor($project: FairyEditor.FProject);
            
        }
        class I18nSettings extends FairyEditor.SettingsBase {
            public langFiles: System.Collections.Generic.List$1<FairyEditor.I18nSettings.LanguageFile>;
            public langFileName: string;
            public langFile: string;
            public constructor($project: FairyEditor.FProject);
            public LoadStrings():void;
            public FillCombo($cb: FairyGUI.GComboBox):void;
            
        }
        class PackageGroupSettings extends FairyEditor.SettingsBase {
            public groups: System.Collections.Generic.List$1<FairyEditor.PackageGroupSettings.PackageGroup>;
            public constructor($project: FairyEditor.FProject);
            public GetGroup($name: string):FairyEditor.PackageGroupSettings.PackageGroup;
            
        }
        class ArrowKeyHelper extends System.Object {
            public static direction: number;
            public static shift: boolean;
            public static ctrlOrCmd: boolean;
            public static OnKeyDown($evt: FairyGUI.InputEvent):void;
            public static OnKeyUp($evt: FairyGUI.InputEvent):void;
            public static Reset():void;
            
        }
        class AssetSizeUtil extends System.Object {
            public static GetSize($file: string):FairyEditor.AssetSizeUtil.Result;
            
        }
        class BuilderUtil extends System.Object {
            public static TimeBase: Date;
            public static GenerateUID():string;
            public static GenDevCode():string;
            public static ToStringBase36($num: bigint):string;
            public static ToNumberBase36($str: string):number;
            public static Encrypt_MD5($input: string, $encode?: System.Text.Encoding):string;
            public static GetMD5HashFromFile($filePath: string):string;
            public static Decrypt_DES16($base64String: string, $key: string):string;
            public static Encrypt_DES16($source: string, $key: string):string;
            public static Union($rect1: UnityEngine.Rect, $rect2: UnityEngine.Rect):UnityEngine.Rect;
            public static GetNameFromId($aId: string):string;
            public static GetFileExtension($fileName: string, $keepCase?: boolean):string;
            public static PointLineDistance($pointX: number, $pointY: number, $startX: number, $startY: number, $endX: number, $endY: number, $isSegment: boolean):number;
            public static GetSizeName($val: number, $digits?: number):string;
            public static OpenURL($url: string):void;
            public static OpenWithDefaultApp($file: string):void;
            public static RevealInExplorer($file: string):void;
            public static ToUnixTimestamp($dateTime: Date):bigint;
            public static WaitForNextFrame():System.Threading.Tasks.Task;
            public static CreateZip($zipFile: string, $dir: string):void;
            public static Unzip($zipFile: string, $dir: string):void;
            
        }
        class BytesWriter extends System.Object {
            public littleEndian: boolean;
            public length: number;
            public position: number;
            public constructor();
            public ReadByte($pos: number):number;
            public WriteByte($value: number):void;
            public WriteBoolean($value: boolean):void;
            public WriteShort($value: number):void;
            public WriteInt($value: number):void;
            public WriteFloat($value: number):void;
            public WriteUTF($str: string):void;
            public WriteUTFBytes($str: string):void;
            public WriteBytes($bytes: System.Array$1<number>):void;
            public WriteBytes($ba: FairyEditor.BytesWriter):void;
            public WriteColor($c: UnityEngine.Color32):void;
            public ToBytes():System.Array$1<number>;
            
        }
        class ColorUtil extends System.Object {
            public static ToHexString($color: UnityEngine.Color, $includeAlpha?: boolean):string;
            public static FromHexString($str: string, $hasAlpha?: boolean):UnityEngine.Color;
            public static FromARGB($argb: number):UnityEngine.Color;
            public static FromRGB($rgb: number):UnityEngine.Color;
            public static ToRGB($color: UnityEngine.Color):number;
            public static ToARGB($color: UnityEngine.Color):number;
            
        }
        class FontUtil extends System.Object {
            public static GetOSInstalledFontNames($forceRefresh: boolean):System.Collections.Generic.List$1<FairyEditor.FontUtil.FontInfo>;
            public static RequestFont($family: string):void;
            public static GetFontName($fontFile: string):FairyEditor.FontUtil.FontName;
            public static GetPreviewTexture($fontInfo: FairyEditor.FontUtil.FontInfo):FairyGUI.NTexture;
            
        }
        class ImageUtil extends System.Object {
            public static ToolAvailable: boolean;
            public static Init():void;
            public static Quantize($image: FairyEditor.VImage):System.Array$1<number>;
            public static Quantize($pngFile: string, $targetFile: string):boolean;
            public static Quantize($pngFile: string):string;
            
        }
        class VImage extends System.Object {
            public width: number;
            public height: number;
            public transparent: boolean;
            public bandCount: number;
            public static New($width: number, $height: number, $transparent: boolean):FairyEditor.VImage;
            public static New($width: number, $height: number, $transparent: boolean, $fillColor: UnityEngine.Color):FairyEditor.VImage;
            public static New($file: string):FairyEditor.VImage;
            public static New($data: System.Array$1<number>):FairyEditor.VImage;
            public static New($file: string, $width: number, $height: number):FairyEditor.VImage;
            public static Thumbnail($file: string, $width: number):FairyEditor.VImage;
            public static GetImageSize($file: string, $width: $Ref<number>, $height: $Ref<number>):boolean;
            public Dispose():void;
            public Resize($width: number, $height: number, $kernel?: FairyEditor.VImage.Kernel):void;
            public ResizeBy($hscale: number, $vscale: number, $kernel?: FairyEditor.VImage.Kernel):void;
            public Rotate($angle: number):void;
            public FindTrim():UnityEngine.Rect;
            public Crop($rect: UnityEngine.Rect):void;
            public Embed($x: number, $y: number, $width: number, $height: number, $extend: FairyEditor.VImage.Extend, $background: UnityEngine.Color):void;
            public AlphaBlend($another: FairyEditor.VImage, $x: number, $y: number):void;
            public CopyPixels($another: FairyEditor.VImage, $x: number, $y: number):void;
            public CopyPixels($another: FairyEditor.VImage, $sourceRect: UnityEngine.Rect, $destPoint: UnityEngine.Vector2):void;
            public Composite($another: FairyEditor.VImage, $x: number, $y: number, $blendMode: FairyEditor.VImage.BlendMode, $premultiplied: boolean):void;
            public Composite($images: System.Collections.Generic.IList$1<FairyEditor.VImage>, $pos: System.Collections.Generic.IList$1<UnityEngine.Vector2>, $blendMode: FairyEditor.VImage.BlendMode, $premultiplied: boolean):void;
            public ExtractAlpha($returnAlpha: boolean):FairyEditor.VImage;
            public Clear($color: UnityEngine.Color):void;
            public DrawRect($x: number, $y: number, $width: number, $height: number, $color: UnityEngine.Color, $fill: boolean):void;
            public GetRawData():System.IntPtr;
            public GetRawDataSize():number;
            public GetPixels():System.Array$1<number>;
            public ToTexture($smoothing: boolean, $makeNoLongerReadable: boolean):UnityEngine.Texture2D;
            public GetAnimation():FairyEditor.VImage.Animation;
            public Save($file: string):void;
            public Save($file: string, $format: string):void;
            public Save($file: string, $format: string, $quality: number):void;
            public Clone():FairyEditor.VImage;
            public static InitLibrary():void;
            
        }
        class IOUtil extends System.Object {
            public static DeleteFile($file: string, $toTrash?: boolean):void;
            public static CopyFile($sourceFile: string, $destFile: string):void;
            public static BrowseForDirectory($title: string, $callback: System.Action$1<string>):void;
            public static BrowseForOpen($title: string, $directory: string, $extensions: System.Array$1<SFB.ExtensionFilter>, $callback: System.Action$1<string>):void;
            public static BrowseForOpenMultiple($title: string, $directory: string, $extensions: System.Array$1<SFB.ExtensionFilter>, $callback: System.Action$1<System.Array$1<string>>):void;
            public static BrowseForSave($title: string, $directory: string, $extension: SFB.ExtensionFilter, $callback: System.Action$1<string>):void;
            public static BrowseForSave($title: string, $directory: string, $defaultName: string, $defaultExt: string, $callback: System.Action$1<string>):void;
            
        }
        class JsonUtil extends System.Object {
            public static ColorHexFormat: boolean;
            public static DecodeJson($content: string):any;
            public static EncodeJson($obj: any):string;
            public static EncodeJson($obj: any, $indent: boolean):string;
            
        }
        class NativeDragDrop extends System.Object {
            public static Init():void;
            public static Dispose():void;
            
        }
        class UserActionException extends System.Exception {
            public constructor($message: string);
            
        }
        class PathPointsUtil extends System.Object {
            public static InsertPoint($pos: UnityEngine.Vector3, $index: number, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>):void;
            public static RemovePoint($index: number, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>):void;
            public static UpdatePoint($index: number, $pos: UnityEngine.Vector3, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>):void;
            public static UpdateControlPoint($pointIndex: number, $controlIndex: number, $pos: UnityEngine.Vector3, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>):void;
            public static SerializeFrom($source: string, $points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>):void;
            public static SerializeTo($points: System.Collections.Generic.List$1<FairyGUI.GPathPoint>):string;
            
        }
        class PlistElement extends System.Object {
            public AsString():string;
            public AsInteger():number;
            public AsBoolean():boolean;
            public AsArray():FairyEditor.PlistElementArray;
            public AsDict():FairyEditor.PlistElementDict;
            public AsReal():number;
            public AsDate():Date;
            public get_Item($key: string):FairyEditor.PlistElement;
            public set_Item($key: string, $value: FairyEditor.PlistElement):void;
            
        }
        class PlistElementArray extends FairyEditor.PlistElement {
            public values: System.Collections.Generic.List$1<FairyEditor.PlistElement>;
            public constructor();
            public AddString($val: string):void;
            public AddInteger($val: number):void;
            public AddBoolean($val: boolean):void;
            public AddDate($val: Date):void;
            public AddReal($val: number):void;
            public AddArray():FairyEditor.PlistElementArray;
            public AddDict():FairyEditor.PlistElementDict;
            
        }
        class PlistElementDict extends FairyEditor.PlistElement {
            public values: System.Collections.Generic.IDictionary$2<string, FairyEditor.PlistElement>;
            public constructor();
            public get_Item($key: string):FairyEditor.PlistElement;
            public set_Item($key: string, $value: FairyEditor.PlistElement):void;
            public SetInteger($key: string, $val: number):void;
            public SetString($key: string, $val: string):void;
            public SetBoolean($key: string, $val: boolean):void;
            public SetDate($key: string, $val: Date):void;
            public SetReal($key: string, $val: number):void;
            public CreateArray($key: string):FairyEditor.PlistElementArray;
            public CreateDict($key: string):FairyEditor.PlistElementDict;
            public get_Item($key: string):FairyEditor.PlistElement;
            public set_Item($key: string, $value: FairyEditor.PlistElement):void;
            
        }
        class PlistElementString extends FairyEditor.PlistElement {
            public value: string;
            public constructor($v: string);
            
        }
        class PlistElementInteger extends FairyEditor.PlistElement {
            public value: number;
            public constructor($v: number);
            
        }
        class PlistElementReal extends FairyEditor.PlistElement {
            public value: number;
            public constructor($v: number);
            
        }
        class PlistElementBoolean extends FairyEditor.PlistElement {
            public value: boolean;
            public constructor($v: boolean);
            
        }
        class PlistElementDate extends FairyEditor.PlistElement {
            public value: Date;
            public constructor($date: Date);
            
        }
        class PlistDocument extends System.Object {
            public root: FairyEditor.PlistElementDict;
            public version: string;
            public constructor();
            public Create():void;
            public ReadFromFile($path: string):void;
            public ReadFromStream($tr: System.IO.TextReader):void;
            public ReadFromString($text: string):void;
            public WriteToFile($path: string):void;
            public WriteToStream($tw: System.IO.TextWriter):void;
            public WriteToString():string;
            
        }
        class PrimitiveExtension extends System.Object {
            public static FormattedString($value: number, $fractionDigits?: number):string;
            
        }
        class ProcessUtil extends System.Object {
            public static LaunchApp():void;
            public static Start($path: string, $args: System.Array$1<string>, $dir: string, $waitUntilExit: boolean):string;
            public static GetOpenFilename():string;
            public static GetUUID():string;
            public static GetAppVersion():string;
            
        }
        class ReflectionUtil extends System.Object {
            public static GetInfo($type: System.Type, $propName: string):any;
            public static GetProperty($obj: any, $propName: string):any;
            public static SetProperty($obj: any, $propName: string, $value: any):void;
            
        }
        class WindowUtil extends System.Object {
            public static ChangeTitle($title: string):void;
            public static ChangeIcon($icon: string):void;
            public static GetScaleFactor():number;
            public static BringToFront():void;
            
        }
        class XMLExtension extends System.Object {
            public static Load($file: string):FairyGUI.Utils.XML;
            public static LoadXMLBrief($file: string):FairyGUI.Utils.XML;
            public static GetAttributeArray($xml: FairyGUI.Utils.XML, $attrName: string, $i1: $Ref<number>, $i2: $Ref<number>):boolean;
            public static GetAttributeArray($xml: FairyGUI.Utils.XML, $attrName: string, $i1: $Ref<number>, $i2: $Ref<number>, $i3: $Ref<number>, $i4: $Ref<number>):boolean;
            public static GetAttributeArray($xml: FairyGUI.Utils.XML, $attrName: string, $f1: $Ref<number>, $f2: $Ref<number>, $f3: $Ref<number>, $f4: $Ref<number>):boolean;
            public static GetAttributeArray($xml: FairyGUI.Utils.XML, $attrName: string, $f1: $Ref<number>, $f2: $Ref<number>):boolean;
            public static GetAttributeArray($xml: FairyGUI.Utils.XML, $attrName: string, $s1: $Ref<string>, $s2: $Ref<string>):boolean;
            
        }
        
    }
    
        class ExternalImagePool extends System.Object {
            
        }
        
    
    namespace FairyEditor.View {
        class MainView extends System.Object {
            public panel: FairyGUI.GComponent;
            public toolbar: FairyGUI.GComponent;
            public constructor();
            public UpdateUserInfo():void;
            public ShowNewVersionPrompt($versionName: string):void;
            public ShowRestartPrompt():void;
            public ShowAlreadyUpdatedPrompt():void;
            public ShowStartScene():void;
            public HandleGlobalHotkey($funcId: string):boolean;
            public FillLanguages():void;
            public DropFiles($mousePos: UnityEngine.Vector2, $arrFiles: System.Array$1<string>):void;
            
        }
        class DocumentView extends FairyGUI.GComponent {
            public docContainer: FairyGUI.GComponent;
            public activeDoc: FairyEditor.View.IDocument;
            public viewScale: number;
            public constructor();
            public AddFactory($factory: FairyEditor.View.IDocumentFactory):void;
            public RemoveFactory($factory: FairyEditor.View.IDocumentFactory):void;
            public FindDocument($docURL: string):FairyEditor.View.IDocument;
            public CloseDocuments($pkg: FairyEditor.FPackage):void;
            public OpenDocument($url: string, $activateIt?: boolean):FairyEditor.View.IDocument;
            public SaveDocument($doc?: FairyEditor.View.IDocument):void;
            public SaveAllDocuments():void;
            public CloseDocument($doc?: FairyEditor.View.IDocument):void;
            public CloseAllDocuments():void;
            public QueryToCloseDocument($doc?: FairyEditor.View.IDocument):void;
            public QueryToCloseOtherDocuments():void;
            public QueryToCloseAllDocuments():void;
            public QueryToSaveAllDocuments($callback: System.Action$1<string>):void;
            public HasUnsavedDocuments():boolean;
            public UpdateTab($doc: FairyEditor.View.IDocument):void;
            public HandleHotkey($context: FairyGUI.EventContext):void;
            
        }
        class LibraryView extends FairyGUI.GComponent {
            public contextMenu: FairyEditor.Component.IMenu;
            public currentGroup: string;
            public constructor();
            public GetFolderUnderPoint($globalPos: UnityEngine.Vector2, $touchTarget: FairyGUI.GObject):FairyEditor.FPackageItem;
            public GetSelectedResource():FairyEditor.FPackageItem;
            public GetSelectedResources($includeChildren: boolean):System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public GetSelectedFolder():FairyEditor.FPackageItem;
            public Highlight($pi: FairyEditor.FPackageItem, $setFocus?: boolean):void;
            public MoveResources($dropTarget: FairyEditor.FPackageItem, $items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):void;
            public DeleteResources($items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):void;
            public SetResourcesExported($items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>, $value: boolean):void;
            public SetResourcesFavorite($items: System.Collections.Generic.List$1<FairyEditor.FPackageItem>, $value: boolean):void;
            public OpenResource($pi: FairyEditor.FPackageItem):void;
            public OpenResources($pis: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):void;
            public RevealInExplorer($pi: FairyEditor.FPackageItem):void;
            public ShowUpdateResourceDialog($pi: FairyEditor.FPackageItem):void;
            public ShowImportResourcesDialog($pkg?: FairyEditor.FPackage, $toPath?: string):void;
            public AddPackageToGroup($pkg: FairyEditor.FPackage):void;
            public GetPackagesInGroup($group: string, $result: System.Collections.Generic.List$1<FairyEditor.FPackage>):System.Collections.Generic.List$1<FairyEditor.FPackage>;
            
        }
        class InspectorView extends FairyGUI.GComponent {
            public visibleInspectors: System.Collections.Generic.List$1<FairyEditor.View.IInspector>;
            public scrollPos: number;
            public constructor();
            public GetInspector($name: string):FairyEditor.View.IInspector;
            public AddInspector($type: System.Type, $name: string, $title: string):void;
            public AddInspector($luaTable: XLua.LuaTable, $name: string, $title: string):void;
            public AddInspector($factoryMethod: System.Func$1<FairyEditor.View.PluginInspector>, $name: string, $title: string):void;
            public RemoveInspector($name: string):void;
            public RemoveAllPluginInspectors():void;
            public SetTitle($name: string, $title: string):void;
            public Show($name: string):void;
            public Show($names: System.Array$1<string>):void;
            public Show($names: System.Collections.Generic.List$1<string>):void;
            public UpdateInspector($inst: FairyEditor.View.IInspector):void;
            public ShowPopup($name: string, $target: FairyGUI.GObject, $dir?: FairyGUI.PopupDirection, $closeUntilMouseUp?: boolean):void;
            public Refresh($name: string):void;
            public Clear():void;
            
        }
        class TestView extends FairyGUI.GComponent {
            public running: boolean;
            public constructor();
            public Start():void;
            public Reload():void;
            public Stop():void;
            public PlayTransition($name: string):void;
            public TogglePopup($popup: FairyEditor.FObject, $target?: FairyEditor.FObject, $direction?: string):void;
            public ShowPopup($popup: FairyEditor.FObject, $target?: FairyEditor.FObject, $direction?: string):void;
            public HidePopup():void;
            public ShowTooltips($msg: string):void;
            public HideTooltips():void;
            
        }
        class TimelineView extends FairyGUI.GComponent {
            public constructor();
            public Refresh($transItem?: FairyEditor.FTransitionItem):void;
            public SelectKeyFrame($transItem: FairyEditor.FTransitionItem):void;
            public GetSelection():FairyEditor.FTransitionItem;
            public GetSelections($result: System.Collections.Generic.List$1<FairyEditor.FTransitionItem>):void;
            
        }
        class ConsoleView extends FairyGUI.GComponent {
            public constructor();
            public Log($msg: string):void;
            public Log($logType: UnityEngine.LogType, $msg: string):void;
            public LogError($msg: string):void;
            public LogError($msg: string, $err?: System.Exception):void;
            public LogWarning($msg: string):void;
            public Clear():void;
            
        }
        class DocumentFactory extends System.Object {
            public contextMenu: FairyEditor.Component.IMenu;
            public constructor();
            public CreateDocument($docURL: string):FairyEditor.View.IDocument;
            public InvokeDocumentMethod($methodName: string, $args?: System.Array$1<any>):void;
            public ConnectInspector($inspectorName: string):void;
            public ConnectInspector($inspectorName: string, $forObjectType: string, $forEmptySelection: boolean, $forTimelineMode: boolean):void;
            public disconnectInspector($inspectorName: string):void;
            
        }
        class Document extends System.Object {
            public panel: FairyGUI.GComponent;
            public selectionLayer: FairyGUI.Container;
            public inspectingTarget: FairyEditor.FObject;
            public inspectingTargets: System.Collections.Generic.IList$1<FairyEditor.FObject>;
            public inspectingObjectType: string;
            public packageItem: FairyEditor.FPackageItem;
            public content: FairyEditor.FComponent;
            public displayTitle: string;
            public displayIcon: string;
            public history: FairyEditor.View.ActionHistory;
            public docURL: string;
            public isModified: boolean;
            public savedVersion: number;
            public openedGroup: FairyEditor.FObject;
            public isPickingObject: boolean;
            public timelineMode: boolean;
            public editingTransition: FairyEditor.FTransition;
            public head: number;
            public constructor();
            public Open($pi: FairyEditor.FPackageItem):void;
            public OnEnable():void;
            public OnDisable():void;
            public OnDestroy():void;
            public OnValidate():void;
            public SetMeta($key: string, $value: any):void;
            public OnUpdate():void;
            public GetInspectingTargetCount($objectType: string):number;
            public SetModified($value?: boolean):void;
            public Serialize():FairyGUI.Utils.XML;
            public Save():void;
            public OnViewSizeChanged():void;
            public OnViewScaleChanged():void;
            public OnViewBackgroundChanged():void;
            public SelectObject($obj: FairyEditor.FObject, $scrollToView?: boolean, $allowOpenGroups?: boolean):void;
            public SelectAll():void;
            public GetSelection():System.Collections.Generic.IList$1<FairyEditor.FObject>;
            public UnselectObject($obj: FairyEditor.FObject):void;
            public UnselectAll():void;
            public SetSelection($obj: FairyEditor.FObject):void;
            public SetSelection($objs: System.Collections.Generic.IList$1<FairyEditor.FObject>):void;
            public CopySelection():void;
            public DeleteSelection():void;
            public DeleteGroupContent($group: FairyEditor.FGroup):void;
            public MoveSelection($dx: number, $dy: number):void;
            public GlobalToCanvas($stagePos: UnityEngine.Vector2):UnityEngine.Vector2;
            public GetCenterPos():UnityEngine.Vector2;
            public Paste($pos?: System.Nullable$1<UnityEngine.Vector2>, $pasteToCenter?: boolean):void;
            public ReplaceSelection($url: string):void;
            public OpenChild($target: FairyEditor.FObject):void;
            public StartInlineEdit($target: FairyEditor.FTextField):void;
            public ShowContextMenu():void;
            public UpdateEditMenu($editMeu: FairyEditor.Component.IMenu):void;
            public InsertObject($url: string, $pos?: System.Nullable$1<UnityEngine.Vector2>, $insertIndex?: number):FairyEditor.FObject;
            public RemoveObject($obj: FairyEditor.FObject):void;
            public AdjustDepth($index: number):void;
            public CreateGroup():void;
            public DestroyGroup():void;
            public OpenGroup($group: FairyEditor.FObject):void;
            public CloseGroup($depth?: number):void;
            public NotifyGroupRemoved($group: FairyEditor.FGroup):void;
            public HandleHotkey($hotkeyId: string):void;
            public PickObject($initValue: FairyEditor.FObject, $callback: System.Action$1<FairyEditor.FObject>, $validator?: System.Func$2<FairyEditor.FObject, boolean>, $cancelCallback?: System.Action):void;
            public CancelPickObject():void;
            public EnterTimelineMode($name: string):void;
            public ExitTimelineMode():void;
            public RefreshTransition():void;
            public RefreshInspectors($flag?: number):void;
            public GetOutlineLocks($obj: FairyEditor.FObject):number;
            public SetTransitionProperty($trans: FairyEditor.FTransition, $propName: string, $propValue: any):void;
            public SetKeyFrameProperty($item: FairyEditor.FTransitionItem, $propName: string, $propValue: any):void;
            public SetKeyFrameValue($item: FairyEditor.FTransitionItem, ...values: any[]):void;
            public SetKeyFramePathPos($item: FairyEditor.FTransitionItem, $pointIndex: number, $x: number, $y: number):void;
            public SetKeyFrameControlPointPos($item: FairyEditor.FTransitionItem, $pointIndex: number, $controlIndex: number, $x: number, $y: number):void;
            public SetKeyFrameControlPointSmooth($item: FairyEditor.FTransitionItem, $pointIndex: number, $smooth: boolean):void;
            public SetKeyFrame($targetId: string, $type: string, $frame: number):void;
            public AddKeyFrames($keyFrames: System.Collections.Generic.IEnumerable$1<FairyEditor.FTransitionItem>):void;
            public CreateKeyFrame($transType: string):void;
            public CreateKeyFrame($child: FairyEditor.FObject, $type: string):FairyEditor.FTransitionItem;
            public AddKeyFrame($item: FairyEditor.FTransitionItem):void;
            public AddKeyFrames($items: System.Array$1<FairyEditor.FTransitionItem>):void;
            public RemoveKeyFrame($item: FairyEditor.FTransitionItem):void;
            public RemoveKeyFrames($targetId: string, $type: string):void;
            public UpdateTransition($xml: FairyGUI.Utils.XML):void;
            public AddTransition($name?: string):FairyEditor.FTransition;
            public RemoveTransition($name: string):void;
            public DuplicateTransition($name: string, $newInstanceName?: string):FairyEditor.FTransition;
            public UpdateTransitions($data: FairyGUI.Utils.XML):void;
            public AddController($data: FairyGUI.Utils.XML):void;
            public UpdateController($controllerName: string, $data: FairyGUI.Utils.XML):void;
            public RemoveController($controllerName: string):void;
            public SwitchPage($controllerName: string, $index: number):number;
            
        }
        class DocElement extends System.Object {
            public owner: FairyEditor.View.Document;
            public isRoot: boolean;
            public isValid: boolean;
            public relationsDisabled: boolean;
            public displayIcon: string;
            public selected: boolean;
            public gizmo: FairyEditor.View.Gizmo;
            public constructor($doc: FairyEditor.View.Document, $obj: FairyEditor.FObject, $isRoot?: boolean);
            public SetProperty($propName: string, $propValue: any):void;
            public SetGearProperty($gearIndex: number, $propName: string, $propValue: any):void;
            public SetRelation($target: FairyEditor.FObject, $desc: string):void;
            public RemoveRelation($target: FairyEditor.FObject):void;
            public UpdateRelations($data: FairyGUI.Utils.XML):void;
            public SetExtensionProperty($propName: string, $propValue: any):void;
            public SetChildProperty($target: string, $propertyId: number, $propValue: any):void;
            public SetVertexPosition($pointIndex: number, $x: number, $y: number):void;
            public SetVertexDistance($pointIndex: number, $distance: number):void;
            public SetScriptData($name: string, $value: string):void;
            
        }
        interface IActionHistoryItem {
            isPersists: boolean;
            Process($owner: FairyEditor.View.Document):boolean;
            
        }
        class ActionHistory extends System.Object {
            public processing: boolean;
            public constructor($doc: FairyEditor.View.Document);
            public CanUndo():boolean;
            public CanRedo():boolean;
            public Add($item: FairyEditor.View.IActionHistoryItem):void;
            public GetPendingList():System.Collections.Generic.List$1<FairyEditor.View.IActionHistoryItem>;
            public Reset():void;
            public PushHistory():void;
            public PopHistory():void;
            public Undo():boolean;
            public Redo():boolean;
            
        }
        class Gizmo extends FairyGUI.Container {
            public static RESIZE_HANDLE: number;
            public static VERTEX_HANDLE: number;
            public static PATH_HANDLE: number;
            public static CONTROL_HANDLE: number;
            public static HANDLE_SIZE: number;
            public static OUTLINE_COLOR: UnityEngine.Color;
            public static OUTLINE_COLOR_COM: UnityEngine.Color;
            public static OUTLINE_COLOR_GROUP: UnityEngine.Color;
            public static PATH_COLOR: UnityEngine.Color;
            public static TANGENT_COLOR: UnityEngine.Color;
            public static VERTEX_HANDLE_COLOR: UnityEngine.Color;
            public static PATH_HANDLE_COLOR: UnityEngine.Color;
            public static CONTROLL_HANDLE_COLOR: UnityEngine.Color;
            public owner: FairyEditor.FObject;
            public activeHandleIndex: number;
            public activeHandleType: number;
            public verticesEditing: boolean;
            public keyFrame: FairyEditor.FTransitionItem;
            public activeHandle: FairyEditor.View.GizmoHandle;
            public constructor($doc: FairyEditor.View.Document, $owner: FairyEditor.FObject);
            public EditVertices():void;
            public EditPath($keyFrame: FairyEditor.FTransitionItem):void;
            public EditComplete():void;
            public Refresh($immediately?: boolean):void;
            public ShowDecorations($visible: boolean):void;
            public SetSelected($value: boolean):void;
            public OnUpdate():void;
            public OnDragStart($context: FairyGUI.EventContext):void;
            public OnDragMove($context: FairyGUI.EventContext):void;
            public OnDragEnd($context: FairyGUI.EventContext):void;
            
        }
        interface IDocument {
            panel: FairyGUI.GComponent;
            packageItem: FairyEditor.FPackageItem;
            docURL: string;
            displayTitle: string;
            displayIcon: string;
            isModified: boolean;
            Save():void;
            UpdateEditMenu($editMenu: FairyEditor.Component.IMenu):void;
            HandleHotkey($hotkeyId: string):void;
            OnEnable():void;
            OnDisable():void;
            OnValidate():void;
            OnUpdate():void;
            OnDestroy():void;
            OnViewSizeChanged():void;
            OnViewScaleChanged():void;
            OnViewBackgroundChanged():void;
            
        }
        class GizmoHandle extends FairyGUI.Shape {
            public index: number;
            public type: number;
            public selected: boolean;
            public constructor($type: number, $color: UnityEngine.Color, $shape?: number);
            
        }
        class GizmoHandleSet extends System.Object {
            public constructor($manager: FairyGUI.DisplayObject, $type: number, $color: UnityEngine.Color, $shape?: number);
            public ResetIndex():void;
            public GetHandle():FairyEditor.View.GizmoHandle;
            public RemoveSpares():void;
            
        }
        class GridMesh extends System.Object {
            public gridSize: number;
            public offset: UnityEngine.Vector2;
            public constructor();
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class InspectorUpdateFlags extends System.Object {
            public static COMMON: number;
            public static TRANSFORM: number;
            public static GEAR: number;
            public static RELATION: number;
            public static GIZMO: number;
            public static FlagsByName: System.Collections.Generic.Dictionary$2<string, number>;
            
        }
        class PathLineMesh extends System.Object {
            public pathLine: FairyGUI.LineMesh;
            public controlLines: System.Collections.Generic.List$1<FairyGUI.StraightLineMesh>;
            public controlLineCount: number;
            public constructor();
            public GetControlLine():FairyGUI.StraightLineMesh;
            public OnPopulateMesh($vb: FairyGUI.VertexBuffer):void;
            
        }
        class DocCamera extends UnityEngine.MonoBehaviour {
            public cachedTransform: UnityEngine.Transform;
            public cachedCamera: UnityEngine.Camera;
            public owner: FairyGUI.GComponent;
            public constructor();
            
        }
        interface IDocumentFactory {
            CreateDocument($url: string):FairyEditor.View.IDocument;
            
        }
        class FavoritesView extends FairyGUI.GComponent {
            public constructor();
            
        }
        class HierarchyView extends FairyGUI.GComponent {
            public constructor();
            
        }
        interface IInspector {
            panel: FairyGUI.GComponent;
            UpdateUI():boolean;
            Dispose():void;
            
        }
        class PluginInspector extends System.Object {
            public updateAction: System.Func$1<boolean>;
            public disposeAction: System.Action;
            public panel: FairyGUI.GComponent;
            public constructor();
            public UpdateUI():boolean;
            public Dispose():void;
            
        }
        class MainMenu extends System.Object {
            public root: FairyEditor.Component.IMenu;
            public constructor($root: FairyEditor.Component.IMenu);
            public AddStartSceneMenu():void;
            public AddProjectMenu():void;
            
        }
        class PlugInView extends FairyGUI.GComponent {
            public constructor();
            
        }
        class PreviewView extends FairyGUI.GComponent {
            public constructor();
            public Show($pi?: FairyEditor.FPackageItem):void;
            
        }
        class ProjectView extends System.Object {
            public onContextMenu: FairyEditor.View.ProjectView.OnContextMenuDelegate;
            public onGetItemListing: FairyEditor.View.ProjectView.OnGetItemListingDelegate;
            public allowDrag: boolean;
            public project: FairyEditor.FProject;
            public treeView: FairyGUI.GTree;
            public listView: FairyGUI.GList;
            public showListView: boolean;
            public constructor($proj: FairyEditor.FProject, $tree: FairyGUI.GTree, $sep?: FairyGUI.GObject, $list?: FairyGUI.GList);
            public SetChanged($pi: FairyEditor.FPackageItem):boolean;
            public SetTreeChanged($pi: FairyEditor.FPackageItem, $recursive?: boolean, $applyImmediately?: boolean):void;
            public GetSelectedPackage():FairyEditor.FPackage;
            public GetSelectedFolder():FairyEditor.FPackageItem;
            public GetSelectedResource():FairyEditor.FPackageItem;
            public GetSelectedResources($result?: System.Collections.Generic.List$1<FairyEditor.FPackageItem>):System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public GetFolderUnderPoint($globalPos: UnityEngine.Vector2, $touchTarget: FairyGUI.GObject):FairyEditor.FPackageItem;
            public GetExpandedFolders($parentNode?: FairyGUI.GTreeNode, $result?: System.Collections.Generic.List$1<string>):System.Collections.Generic.List$1<string>;
            public SetExpanedFolders($arr: System.Collections.IList):void;
            public IsInView($pi: FairyEditor.FPackageItem):boolean;
            public Select($pi: FairyEditor.FPackageItem):boolean;
            public SelectNextTo($pi: FairyEditor.FPackageItem):void;
            public Expand($pi: FairyEditor.FPackageItem):void;
            public Rename($pi?: FairyEditor.FPackageItem):void;
            public Open():void;
            public ChangeIconSize($scale: number):void;
            
        }
        class ReferenceView extends FairyGUI.GComponent {
            public constructor();
            public Open($source: string):void;
            public FillMenuTargets():void;
            
        }
        class ResourceMenu extends System.Object {
            public realMenu: FairyEditor.Component.IMenu;
            public targetItems: System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
            public constructor();
            public Show():void;
            
        }
        class SearchView extends FairyGUI.GComponent {
            public constructor();
            public FillMenuTargets():void;
            
        }
        class TransitionListView extends FairyGUI.GComponent {
            public constructor();
            public Refresh():void;
            
        }
        
    }
    namespace FairyEditor.Component {
        interface IMenu {
            AddItem($caption: string, $name: string, $selectCallback: System.Action$1<string>):void;
            AddItem($caption: string, $name: string, $atIndex: number, $isSubMenu: boolean, $selectCallback: System.Action$1<string>):void;
            AddSeperator():void;
            AddSeperator($atIndex: number):void;
            RemoveItem($name: string):void;
            SetItemEnabled($name: string, $enabled: boolean):void;
            SetItemChecked($name: string, $checked: boolean):void;
            IsItemChecked($name: string):boolean;
            SetItemText($name: string, $text: string):void;
            ClearItems():void;
            GetSubMenu($name: string):FairyEditor.Component.IMenu;
            Invoke($name: string):void;
            Dispose():void;
            
        }
        class ViewGrid extends FairyGUI.GComponent {
            public uid: string;
            public showTabs: boolean;
            public numViews: number;
            public selectedIndex: number;
            public selectedView: FairyGUI.GComponent;
            public constructor();
            public GetViewAt($index: number):FairyGUI.GComponent;
            public AddView($view: FairyGUI.GComponent):void;
            public AddViewAt($view: FairyGUI.GComponent, $index: number):void;
            public RemoveView($view: FairyGUI.GComponent):void;
            public RemoveViewAt($index: number):void;
            public SetViewIndex($view: FairyGUI.GComponent, $index: number):void;
            public GetViewIndex($view: FairyGUI.GComponent):number;
            public GetViewIndexById($viewId: string):number;
            public ContainsView($ids: System.Array$1<string>):boolean;
            public MoveViews($anotherGrid: FairyEditor.Component.ViewGrid):void;
            public Clear():void;
            public Refresh():void;
            public SetViewTitle($index: number, $title: string):void;
            
        }
        class ChildObjectInput extends FairyGUI.GLabel {
            public typeFilter: System.Array$1<string>;
            public value: FairyEditor.FObject;
            public constructor();
            public Start():void;
            
        }
        class ColorInput extends FairyGUI.GButton {
            public showAlpha: boolean;
            public colorValue: UnityEngine.Color;
            public constructor();
            
        }
        class ColorPicker extends System.Object {
            public isShowing: boolean;
            public constructor();
            public Show($input: FairyEditor.Component.ColorInput, $popupTarget: FairyGUI.GObject, $color: UnityEngine.Color, $showAlpha: boolean):void;
            public Hide():void;
            
        }
        class ComPropertyInput extends FairyGUI.GLabel {
            public value: any;
            public constructor();
            public Update($cp: FairyEditor.ComProperty, $pagesSupplier: any):void;
            
        }
        class ControllerInput extends FairyGUI.GLabel {
            public prompt: string;
            public includeChildren: boolean;
            public owner: FairyEditor.FComponent;
            public value: string;
            public constructor();
            
        }
        class ControllerMultiPageInput extends FairyGUI.GLabel {
            public prompt: string;
            public controller: FairyEditor.FController;
            public value: System.Array$1<string>;
            public constructor();
            
        }
        class ControllerPageInput extends FairyGUI.GLabel {
            public prompt: string;
            public nullPageOption: boolean;
            public additionalOptions: boolean;
            public controller: FairyEditor.FController;
            public value: string;
            public constructor();
            
        }
        class EditableListItem extends FairyEditor.Component.ListItem {
            public sign: FairyGUI.GLoader;
            public editable: boolean;
            public toggleClickCount: number;
            public constructor();
            public StartEditing($text?: string):void;
            
        }
        class ListItem extends FairyGUI.GButton {
            public titleObj: FairyGUI.GTextField;
            public iconObj: FairyGUI.GLoader;
            public constructor();
            
        }
        class EditableTreeItem extends FairyGUI.GButton {
            public toggleClickCount: number;
            public editable: boolean;
            public constructor();
            public StartEditing($text?: string):void;
            
        }
        class FontInput extends FairyGUI.GLabel {
            public constructor();
            
        }
        class FontSizeInput extends FairyGUI.GLabel {
            public value: number;
            public max: number;
            public constructor();
            
        }
        class InputElement extends System.ValueType {
            public name: string;
            public type: string;
            public prop: string;
            public dummy: boolean;
            public extData: any;
            public min: FairyEditor.Component.InputElement.OptionalValue$1<number>;
            public max: FairyEditor.Component.InputElement.OptionalValue$1<number>;
            public step: FairyEditor.Component.InputElement.OptionalValue$1<number>;
            public precision: FairyEditor.Component.InputElement.OptionalValue$1<number>;
            public items: System.Array$1<string>;
            public values: System.Array$1<string>;
            public visibleItemCount: FairyEditor.Component.InputElement.OptionalValue$1<number>;
            public valueName: string;
            public inverted: boolean;
            public showAlpha: boolean;
            public filter: System.Array$1<string>;
            public pages: string;
            public includeChildren: boolean;
            public prompt: string;
            public readonly: boolean;
            public disableIME: boolean;
            public trim: boolean;
            
        }
        class FormHelper extends System.Object {
            public onPropChanged: FairyEditor.Component.FormHelper.OnPropChangedDelegate;
            public owner: FairyGUI.GComponent;
            public constructor($owner: FairyGUI.GComponent);
            public BindControls($data: System.Collections.Generic.IList$1<FairyEditor.Component.InputElement>):void;
            public GetControl($controlName: string):FairyGUI.GObject;
            public UpdateValuesFrom($obj: any, $controlNames?: System.Collections.IList):void;
            public SetValue($controlName: string, $value: any):void;
            public GetValue($controlName: string):any;
            public UpdateUI():void;
            
        }
        class InlineSearchBar extends FairyGUI.GButton {
            public pattern: System.Text.RegularExpressions.Regex;
            public constructor();
            public Reset():void;
            public HandleKeyEvent($evt: FairyGUI.InputEvent):boolean;
            
        }
        class LinkButton extends FairyGUI.GButton {
            public constructor();
            
        }
        class ListHelper extends System.Object {
            public onInsert: System.Action$2<number, FairyGUI.GComponent>;
            public onRemove: System.Action$1<number>;
            public onSwap: System.Action$2<number, number>;
            public constructor($list: FairyGUI.GList, $indexColumn?: string);
            public Add($context?: FairyGUI.EventContext):void;
            public Insert($context?: FairyGUI.EventContext):void;
            public Remove($context?: FairyGUI.EventContext):void;
            public MoveUp($context?: FairyGUI.EventContext):void;
            public MoveDown($context?: FairyGUI.EventContext):void;
            
        }
        class ListItemInput extends FairyGUI.GLabel {
            public toggleClickCount: number;
            public editable: boolean;
            public constructor();
            public StartEditing($text?: string):void;
            
        }
        class ListItemResourceInput extends FairyEditor.Component.ResourceInput {
            public toggleClickCount: number;
            public constructor();
            public StartEditing():void;
            
        }
        class ResourceInput extends FairyGUI.GLabel {
            public promptText: string;
            public isFontInput: boolean;
            public text: string;
            public constructor();
            
        }
        class MenuBar extends System.Object {
            public constructor($panel: FairyGUI.GComponent);
            public Dispose():void;
            public AddItem($caption: string, $name: string, $selectCallback: System.Action$1<string>):void;
            public AddItem($caption: string, $name: string, $atIndex: number, $isSubMenu: boolean, $selectCallback: System.Action$1<string>):void;
            public GetSubMenu($name: string):FairyEditor.Component.IMenu;
            public RemoveItem($name: string):void;
            public AddSeperator():void;
            public AddSeperator($atIndex: number):void;
            public SetItemEnabled($name: string, $enabled: boolean):void;
            public SetItemChecked($name: string, $checked: boolean):void;
            public IsItemChecked($name: string):boolean;
            public SetItemText($name: string, $text: string):void;
            public ClearItems():void;
            public Invoke($name: string):void;
            
        }
        class NativeMenu extends System.Object {
            public static applicationMenu: FairyEditor.Component.NativeMenu;
            public static dockIconMenu: FairyEditor.Component.NativeMenu;
            public Dispose():void;
            public AddItem($caption: string, $name: string, $selectCallback: System.Action$1<string>):void;
            public AddItem($caption: string, $name: string, $atIndex: number, $isSubMenu: boolean, $selectCallback: System.Action$1<string>):void;
            public AddSeperator():void;
            public AddSeperator($atIndex: number):void;
            public SetItemEnabled($name: string, $enabled: boolean):void;
            public SetItemChecked($name: string, $checked: boolean):void;
            public IsItemChecked($name: string):boolean;
            public SetItemText($name: string, $text: string):void;
            public GetSubMenu($name: string):FairyEditor.Component.IMenu;
            public RemoveItem($name: string):void;
            public ClearItems():void;
            public Invoke($name: string):void;
            
        }
        class NumericInput extends FairyGUI.GLabel {
            public max: number;
            public min: number;
            public value: number;
            public step: number;
            public fractionDigits: number;
            public text: string;
            public constructor();
            
        }
        class SelectPivotMenu extends System.Object {
            public constructor();
            public static GetInstance():FairyEditor.Component.SelectPivotMenu;
            public Show($input1: FairyGUI.GObject, $input2: FairyGUI.GObject, $popupTarget?: FairyGUI.GObject):void;
            
        }
        class TextArea extends FairyGUI.GLabel {
            public constructor();
            
        }
        class TextInput extends FairyGUI.GLabel {
            public text: string;
            public constructor();
            
        }
        class TransitionInput extends FairyGUI.GLabel {
            public prompt: string;
            public includeChildren: boolean;
            public owner: FairyEditor.FComponent;
            public value: string;
            public constructor();
            
        }
        class ViewGridGroup extends FairyGUI.GComponent {
            public uid: string;
            public layout: FairyGUI.GroupLayoutType;
            public numGrids: number;
            public constructor($layout: FairyGUI.GroupLayoutType);
            public AddGrid($child: FairyGUI.GObject):void;
            public AddGridAt($child: FairyGUI.GObject, $index: number):void;
            public ResetChildrenSize():void;
            public RemoveGrid($child: FairyGUI.GObject, $dispose?: boolean):void;
            public ReplaceGrid($oldChild: FairyGUI.GObject, $newChild: FairyGUI.GObject):void;
            public MoveGrids($anotherGroup: FairyEditor.Component.ViewGridGroup, $index: number):void;
            public GetGridAt($index: number):FairyGUI.GObject;
            public GetGridIndex($grid: FairyGUI.GObject):number;
            public FindGrid($view: FairyGUI.GComponent, $recursive?: boolean):FairyEditor.Component.ViewGrid;
            public FindGridById($id: string, $recursive?: boolean):FairyEditor.Component.ViewGrid;
            public FindGridByIds($ids: System.Array$1<string>, $recursive?: boolean):FairyEditor.Component.ViewGrid;
            public FindGroup($id: string):FairyEditor.Component.ViewGridGroup;
            public static EachGrid($grp: FairyEditor.Component.ViewGridGroup, $recursive: boolean, $callback: FairyEditor.Component.ViewGridGroup.EachGridCallback):FairyEditor.Component.ViewGrid;
            
        }
        
    }
    namespace FairyEditor.App {
        enum FrameRateFactor { BackgroundJob = 1, NativeDragDrop = 2, DraggingObject = 256, Testing = 512 }
        
    }
    namespace FairyGUI.Utils {
        class XML extends System.Object {
            public name: string;
            public text: string;
            public attributes: System.Collections.Generic.Dictionary$2<string, string>;
            public elements: FairyGUI.Utils.XMLList;
            public constructor($XmlString: string);
            public static Create($tag: string):FairyGUI.Utils.XML;
            public HasAttribute($attrName: string):boolean;
            public GetAttribute($attrName: string):string;
            public GetAttribute($attrName: string, $defValue: string):string;
            public GetAttributeInt($attrName: string):number;
            public GetAttributeInt($attrName: string, $defValue: number):number;
            public GetAttributeFloat($attrName: string):number;
            public GetAttributeFloat($attrName: string, $defValue: number):number;
            public GetAttributeBool($attrName: string):boolean;
            public GetAttributeBool($attrName: string, $defValue: boolean):boolean;
            public GetAttributeArray($attrName: string):System.Array$1<string>;
            public GetAttributeArray($attrName: string, $seperator: number):System.Array$1<string>;
            public GetAttributeColor($attrName: string, $defValue: UnityEngine.Color):UnityEngine.Color;
            public GetAttributeVector($attrName: string):UnityEngine.Vector2;
            public SetAttribute($attrName: string, $attrValue: string):void;
            public SetAttribute($attrName: string, $attrValue: boolean):void;
            public SetAttribute($attrName: string, $attrValue: number):void;
            public SetAttribute($attrName: string, $attrValue: number):void;
            public RemoveAttribute($attrName: string):void;
            public GetNode($selector: string):FairyGUI.Utils.XML;
            public Elements():FairyGUI.Utils.XMLList;
            public Elements($selector: string):FairyGUI.Utils.XMLList;
            public GetEnumerator():FairyGUI.Utils.XMLList.Enumerator;
            public GetEnumerator($selector: string):FairyGUI.Utils.XMLList.Enumerator;
            public AppendChild($child: FairyGUI.Utils.XML):void;
            public RemoveChild($child: FairyGUI.Utils.XML):void;
            public RemoveChildren($selector: string):void;
            public Parse($aSource: string):void;
            public Reset():void;
            public ToXMLString($includeHeader: boolean):string;
            
        }
        interface XML {
            GetAttributeArray($attrName: string, $i1: $Ref<number>, $i2: $Ref<number>):boolean;
            GetAttributeArray($attrName: string, $i1: $Ref<number>, $i2: $Ref<number>, $i3: $Ref<number>, $i4: $Ref<number>):boolean;
            GetAttributeArray($attrName: string, $f1: $Ref<number>, $f2: $Ref<number>, $f3: $Ref<number>, $f4: $Ref<number>):boolean;
            GetAttributeArray($attrName: string, $f1: $Ref<number>, $f2: $Ref<number>):boolean;
            GetAttributeArray($attrName: string, $s1: $Ref<string>, $s2: $Ref<string>):boolean;
            
        }
        
        class ByteBuffer extends System.Object {
            public littleEndian: boolean;
            public stringTable: System.Array$1<string>;
            public version: number;
            public position: number;
            public length: number;
            public bytesAvailable: boolean;
            public buffer: System.Array$1<number>;
            public constructor($data: System.Array$1<number>, $offset?: number, $length?: number);
            public Skip($count: number):number;
            public ReadByte():number;
            public ReadBytes($output: System.Array$1<number>, $destIndex: number, $count: number):System.Array$1<number>;
            public ReadBytes($count: number):System.Array$1<number>;
            public ReadBuffer():FairyGUI.Utils.ByteBuffer;
            public ReadChar():number;
            public ReadBool():boolean;
            public ReadShort():number;
            public ReadUshort():number;
            public ReadInt():number;
            public ReadUint():number;
            public ReadFloat():number;
            public ReadLong():bigint;
            public ReadDouble():number;
            public ReadString():string;
            public ReadString($len: number):string;
            public ReadS():string;
            public ReadSArray($cnt: number):System.Array$1<string>;
            public ReadPath():System.Collections.Generic.List$1<FairyGUI.GPathPoint>;
            public WriteS($value: string):void;
            public ReadColor():UnityEngine.Color;
            public Seek($indexTablePos: number, $blockIndex: number):boolean;
            
        }
        class HtmlElement extends System.Object {
            public type: FairyGUI.Utils.HtmlElementType;
            public name: string;
            public text: string;
            public format: FairyGUI.TextFormat;
            public charIndex: number;
            public htmlObject: FairyGUI.Utils.IHtmlObject;
            public status: number;
            public space: number;
            public position: UnityEngine.Vector2;
            public isEntity: boolean;
            public constructor();
            public Get($attrName: string):any;
            public Set($attrName: string, $attrValue: any):void;
            public GetString($attrName: string):string;
            public GetString($attrName: string, $defValue: string):string;
            public GetInt($attrName: string):number;
            public GetInt($attrName: string, $defValue: number):number;
            public GetFloat($attrName: string):number;
            public GetFloat($attrName: string, $defValue: number):number;
            public GetBool($attrName: string):boolean;
            public GetBool($attrName: string, $defValue: boolean):boolean;
            public GetColor($attrName: string, $defValue: UnityEngine.Color):UnityEngine.Color;
            public FetchAttributes():void;
            public static GetElement($type: FairyGUI.Utils.HtmlElementType):FairyGUI.Utils.HtmlElement;
            public static ReturnElement($element: FairyGUI.Utils.HtmlElement):void;
            public static ReturnElements($elements: System.Collections.Generic.List$1<FairyGUI.Utils.HtmlElement>):void;
            
        }
        class HtmlPageContext extends System.Object {
            public static inst: FairyGUI.Utils.HtmlPageContext;
            public constructor();
            public CreateObject($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):FairyGUI.Utils.IHtmlObject;
            public FreeObject($obj: FairyGUI.Utils.IHtmlObject):void;
            public GetImageTexture($image: FairyGUI.Utils.HtmlImage):FairyGUI.NTexture;
            public FreeImageTexture($image: FairyGUI.Utils.HtmlImage, $texture: FairyGUI.NTexture):void;
            
        }
        interface IHtmlObject {
            width: number;
            height: number;
            displayObject: FairyGUI.DisplayObject;
            element: FairyGUI.Utils.HtmlElement;
            Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            SetPosition($x: number, $y: number):void;
            Add():void;
            Remove():void;
            Release():void;
            Dispose():void;
            
        }
        class HtmlImage extends System.Object {
            public loader: FairyGUI.GLoader;
            public displayObject: FairyGUI.DisplayObject;
            public element: FairyGUI.Utils.HtmlElement;
            public width: number;
            public height: number;
            public constructor();
            public Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            public SetPosition($x: number, $y: number):void;
            public Add():void;
            public Remove():void;
            public Release():void;
            public Dispose():void;
            
        }
        interface IHtmlPageContext {
            CreateObject($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):FairyGUI.Utils.IHtmlObject;
            FreeObject($obj: FairyGUI.Utils.IHtmlObject):void;
            GetImageTexture($image: FairyGUI.Utils.HtmlImage):FairyGUI.NTexture;
            FreeImageTexture($image: FairyGUI.Utils.HtmlImage, $texture: FairyGUI.NTexture):void;
            
        }
        class HtmlParseOptions extends System.Object {
            public linkUnderline: boolean;
            public linkColor: UnityEngine.Color;
            public linkBgColor: UnityEngine.Color;
            public linkHoverBgColor: UnityEngine.Color;
            public ignoreWhiteSpace: boolean;
            public static DefaultLinkUnderline: boolean;
            public static DefaultLinkColor: UnityEngine.Color;
            public static DefaultLinkBgColor: UnityEngine.Color;
            public static DefaultLinkHoverBgColor: UnityEngine.Color;
            public constructor();
            
        }
        class HtmlButton extends System.Object {
            public static CLICK_EVENT: string;
            public static resource: string;
            public button: FairyGUI.GComponent;
            public displayObject: FairyGUI.DisplayObject;
            public element: FairyGUI.Utils.HtmlElement;
            public width: number;
            public height: number;
            public constructor();
            public Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            public SetPosition($x: number, $y: number):void;
            public Add():void;
            public Remove():void;
            public Release():void;
            public Dispose():void;
            
        }
        enum HtmlElementType { Text = 0, Link = 1, Image = 2, Input = 3, Select = 4, Object = 5, LinkEnd = 6 }
        class HtmlInput extends System.Object {
            public static defaultBorderSize: number;
            public static defaultBorderColor: UnityEngine.Color;
            public static defaultBackgroundColor: UnityEngine.Color;
            public textInput: FairyGUI.GTextInput;
            public displayObject: FairyGUI.DisplayObject;
            public element: FairyGUI.Utils.HtmlElement;
            public width: number;
            public height: number;
            public constructor();
            public Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            public SetPosition($x: number, $y: number):void;
            public Add():void;
            public Remove():void;
            public Release():void;
            public Dispose():void;
            
        }
        class HtmlLink extends System.Object {
            public displayObject: FairyGUI.DisplayObject;
            public element: FairyGUI.Utils.HtmlElement;
            public width: number;
            public height: number;
            public constructor();
            public Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            public SetArea($startLine: number, $startCharX: number, $endLine: number, $endCharX: number):void;
            public SetPosition($x: number, $y: number):void;
            public Add():void;
            public Remove():void;
            public Release():void;
            public Dispose():void;
            
        }
        class HtmlParser extends System.Object {
            public static inst: FairyGUI.Utils.HtmlParser;
            public constructor();
            public Parse($aSource: string, $defaultFormat: FairyGUI.TextFormat, $elements: System.Collections.Generic.List$1<FairyGUI.Utils.HtmlElement>, $parseOptions: FairyGUI.Utils.HtmlParseOptions):void;
            
        }
        class HtmlSelect extends System.Object {
            public static CHANGED_EVENT: string;
            public static resource: string;
            public comboBox: FairyGUI.GComboBox;
            public displayObject: FairyGUI.DisplayObject;
            public element: FairyGUI.Utils.HtmlElement;
            public width: number;
            public height: number;
            public constructor();
            public Create($owner: FairyGUI.RichTextField, $element: FairyGUI.Utils.HtmlElement):void;
            public SetPosition($x: number, $y: number):void;
            public Add():void;
            public Remove():void;
            public Release():void;
            public Dispose():void;
            
        }
        class ToolSet extends System.Object {
            public static ConvertFromHtmlColor($str: string):UnityEngine.Color;
            public static ColorFromRGB($value: number):UnityEngine.Color;
            public static ColorFromRGBA($value: number):UnityEngine.Color;
            public static CharToHex($c: number):number;
            public static Intersection($rect1: $Ref<UnityEngine.Rect>, $rect2: $Ref<UnityEngine.Rect>):UnityEngine.Rect;
            public static Union($rect1: $Ref<UnityEngine.Rect>, $rect2: $Ref<UnityEngine.Rect>):UnityEngine.Rect;
            public static SkewMatrix($matrix: $Ref<UnityEngine.Matrix4x4>, $skewX: number, $skewY: number):void;
            public static RotateUV($uv: System.Array$1<UnityEngine.Vector2>, $baseUVRect: $Ref<UnityEngine.Rect>):void;
            
        }
        class UBBParser extends System.Object {
            public static inst: FairyGUI.Utils.UBBParser;
            public defaultTagHandler: FairyGUI.Utils.UBBParser.TagHandler;
            public handlers: System.Collections.Generic.Dictionary$2<string, FairyGUI.Utils.UBBParser.TagHandler>;
            public defaultImgWidth: number;
            public defaultImgHeight: number;
            public constructor();
            public GetTagText($remove: boolean):string;
            public Parse($text: string):string;
            
        }
        class XMLList extends System.Object {
            public rawList: System.Collections.Generic.List$1<FairyGUI.Utils.XML>;
            public Count: number;
            public constructor();
            public constructor($list: System.Collections.Generic.List$1<FairyGUI.Utils.XML>);
            public Add($xml: FairyGUI.Utils.XML):void;
            public Clear():void;
            public get_Item($index: number):FairyGUI.Utils.XML;
            public GetEnumerator():FairyGUI.Utils.XMLList.Enumerator;
            public GetEnumerator($selector: string):FairyGUI.Utils.XMLList.Enumerator;
            public Filter($selector: string):FairyGUI.Utils.XMLList;
            public Find($selector: string):FairyGUI.Utils.XML;
            public RemoveAll($selector: string):void;
            
        }
        class XMLIterator extends System.Object {
            public static tagName: string;
            public static tagType: FairyGUI.Utils.XMLTagType;
            public static lastTagName: string;
            public constructor();
            public static Begin($source: string, $lowerCaseName?: boolean):void;
            public static NextTag():boolean;
            public static GetTagSource():string;
            public static GetRawText($trim?: boolean):string;
            public static GetText($trim?: boolean):string;
            public static HasAttribute($attrName: string):boolean;
            public static GetAttribute($attrName: string):string;
            public static GetAttribute($attrName: string, $defValue: string):string;
            public static GetAttributeInt($attrName: string):number;
            public static GetAttributeInt($attrName: string, $defValue: number):number;
            public static GetAttributeFloat($attrName: string):number;
            public static GetAttributeFloat($attrName: string, $defValue: number):number;
            public static GetAttributeBool($attrName: string):boolean;
            public static GetAttributeBool($attrName: string, $defValue: boolean):boolean;
            public static GetAttributes($result: System.Collections.Generic.Dictionary$2<string, string>):System.Collections.Generic.Dictionary$2<string, string>;
            public static GetAttributes($result: System.Collections.Hashtable):System.Collections.Hashtable;
            
        }
        enum XMLTagType { Start = 0, End = 1, Void = 2, CDATA = 3, Comment = 4, Instruction = 5 }
        class XMLUtils extends System.Object {
            public constructor();
            public static DecodeString($aSource: string):string;
            public static EncodeString($sb: System.Text.StringBuilder, $start: number, $isAttribute?: boolean):void;
            public static EncodeString($str: string, $isAttribute?: boolean):string;
            
        }
        class ZipReader extends System.Object {
            public entryCount: number;
            public constructor($data: System.Array$1<number>);
            public GetNextEntry($entry: FairyGUI.Utils.ZipReader.ZipEntry):boolean;
            public GetEntryData($entry: FairyGUI.Utils.ZipReader.ZipEntry):System.Array$1<number>;
            
        }
        
    }
    namespace System.Threading.Tasks {
        class Task extends System.Object {
            
        }
        class Task$1<TResult> extends System.Threading.Tasks.Task {
            
        }
        
    }
    namespace FairyEditor.AniData {
        class Frame extends System.Object {
            public rect: UnityEngine.Rect;
            public spriteIndex: number;
            public delay: number;
            public constructor();
            
        }
        class FrameSprite extends System.Object {
            public texture: FairyGUI.NTexture;
            public frameIndex: number;
            public raw: System.Array$1<number>;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.BmFontData {
        class Glyph extends System.ValueType {
            public id: number;
            public x: number;
            public y: number;
            public xoffset: number;
            public yoffset: number;
            public width: number;
            public height: number;
            public xadvance: number;
            public img: string;
            public channel: number;
            
        }
        
    }
    namespace FairyEditor.ComponentAsset {
        class DisplayListItem extends System.Object {
            public packageItem: FairyEditor.FPackageItem;
            public pkg: FairyEditor.FPackage;
            public type: string;
            public desc: FairyGUI.Utils.XML;
            public missingInfo: FairyEditor.MissingInfo;
            public existingInstance: FairyEditor.FObject;
            public constructor();
            
        }
        
    }
    namespace DragonBones {
        class DragonBonesData extends DragonBones.BaseObject {
            
        }
        class BaseObject extends System.Object {
            
        }
        class UnityArmatureComponent extends DragonBones.DragonBoneEventDispatcher {
            
        }
        class DragonBoneEventDispatcher extends DragonBones.UnityEventDispatcher$1<DragonBones.EventObject> {
            
        }
        class EventObject extends DragonBones.BaseObject {
            
        }
        class UnityEventDispatcher$1<T> extends UnityEngine.MonoBehaviour {
            
        }
        
    }
    namespace FairyEditor.FontAsset {
        enum FontType { Sprites = 0, Fnt = 1, TTF = 2 }
        
    }
    namespace UnityEngine.TextCore.LowLevel {
        enum GlyphRenderMode { SMOOTH_HINTED = 4121, SMOOTH = 4117, RASTER_HINTED = 4122, RASTER = 4118, SDF = 4138, SDF8 = 8234, SDF16 = 16426, SDF32 = 32810, SDFAA_HINTED = 4169, SDFAA = 4165 }
        
    }
    namespace Spine.Unity {
        class SkeletonDataAsset extends UnityEngine.ScriptableObject {
            
        }
        class SkeletonAnimation extends Spine.Unity.SkeletonRenderer {
            
        }
        class SkeletonRenderer extends UnityEngine.MonoBehaviour {
            
        }
        
    }
    namespace FairyEditor.Framework.Gears {
        interface IGear {
            
        }
        
    }
    namespace FairyEditor.FTree {
        type TreeNodeRenderDelegate = (node: FairyEditor.FTreeNode, obj: FairyEditor.FComponent) => void;
        var TreeNodeRenderDelegate: {new (func: (node: FairyEditor.FTreeNode, obj: FairyEditor.FComponent) => void): TreeNodeRenderDelegate;}
        type TreeNodeWillExpandDelegate = (node: FairyEditor.FTreeNode, expand: boolean) => void;
        var TreeNodeWillExpandDelegate: {new (func: (node: FairyEditor.FTreeNode, expand: boolean) => void): TreeNodeWillExpandDelegate;}
        
    }
    namespace FairyGUI.GPathPoint {
        enum CurveType { CRSpline = 0, Bezier = 1, CubicBezier = 2, Straight = 3 }
        
    }
    namespace System.IO.Compression {
        class ZipStorer extends System.Object {
            
        }
        
    }
    namespace FairyEditor.PublishHandler {
        class ClassInfo extends System.Object {
            public className: string;
            public superClassName: string;
            public resId: string;
            public resName: string;
            public res: FairyEditor.FPackageItem;
            public members: System.Collections.Generic.List$1<FairyEditor.PublishHandler.MemberInfo>;
            public references: System.Collections.Generic.List$1<string>;
            public constructor();
            
        }
        class MemberInfo extends System.Object {
            public name: string;
            public varName: string;
            public type: string;
            public index: number;
            public group: number;
            public res: FairyEditor.FPackageItem;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.DependencyQuery {
        enum SeekLevel { SELECTION = 0, SAME_PACKAGE_BUT_NOT_EXPORTED = 1, SAME_PACKAGE = 2, ALL = 3 }
        
    }
    namespace FairyEditor.CopyHandler {
        enum OverrideOption { RENAME = 0, REPLACE = 1, SKIP = 2 }
        
    }
    namespace FairyEditor.HotkeyManager {
        class FunctionDef extends System.Object {
            public id: string;
            public hotkey: string;
            public desc: string;
            public isCustomized: boolean;
            public constructor($id: string, $hotkey: string, $desc: string);
            
        }
        
    }
    namespace FairyEditor.PluginManager {
        class PluginInfo extends System.Object {
            public name: string;
            public displayName: string;
            public description: string;
            public version: string;
            public author: FairyEditor.PluginManager.PluginInfo.Author;
            public icon: string;
            public main: string;
            public onPublish: System.Action$1<FairyEditor.PublishHandler>;
            public onDestroy: System.Action;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.ReferenceInfo {
        enum ValueType { ID = 0, URL = 1, URL_COMPLEX = 2, CHAR_IMG = 3, ASSET_PROP = 4 }
        
    }
    namespace FairyEditor.AdaptationSettings {
        class DeviceInfo extends System.ValueType {
            public name: string;
            public resolutionX: number;
            public resolutionY: number;
            
        }
        
    }
    namespace FairyEditor.CommonSettings {
        class ScrollBarConfig extends System.Object {
            public horizontal: string;
            public vertical: string;
            public defaultDisplay: string;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.GlobalPublishSettings {
        class CodeGenerationConfig extends System.Object {
            public allowGenCode: boolean;
            public codePath: string;
            public classNamePrefix: string;
            public memberNamePrefix: string;
            public packageName: string;
            public ignoreNoname: boolean;
            public getMemberByName: boolean;
            public codeType: string;
            public constructor();
            
        }
        class AtlasSetting extends System.Object {
            public maxSize: number;
            public paging: boolean;
            public sizeOption: string;
            public forceSquare: boolean;
            public allowRotation: boolean;
            public trimImage: boolean;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.I18nSettings {
        class LanguageFile extends System.Object {
            public name: string;
            public path: string;
            public modificationDate: Date;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.PackageGroupSettings {
        class PackageGroup extends System.Object {
            public name: string;
            public pkgs: System.Collections.Generic.List$1<string>;
            public constructor();
            
        }
        
    }
    namespace FairyEditor.AssetSizeUtil {
        class Result extends System.ValueType {
            public width: number;
            public height: number;
            public type: string;
            public bitDepth: number;
            public colorType: number;
            
        }
        
    }
    namespace FairyEditor.FontUtil {
        class FontInfo extends System.Object {
            public family: string;
            public localeFamily: string;
            public file: string;
            public externalLoad: boolean;
            public constructor();
            
        }
        class FontName extends System.Object {
            public family: string;
            public localeFamily: string;
            public constructor();
            
        }
        
    }
    namespace SFB {
        class ExtensionFilter extends System.ValueType {
            
        }
        
    }
    namespace FairyEditor.VImage {
        enum Kernel { NEAREST = 0, LINEAR = 1, CUBIC = 2, MITCHELL = 3, LANCZOS2 = 4, LANCZOS3 = 5, LAST = 6 }
        enum Extend { BLACK = 0, COPY = 1, REPEAT = 2, MIRROR = 3, WHITE = 4, BACKGROUND = 5, LAST = 6 }
        enum BlendMode { CLEAR = 0, SOURCE = 1, OVER = 2, IN = 3, OUT = 4, ATOP = 5, DEST = 6, DEST_OVER = 7, DEST_IN = 8, DEST_OUT = 9, DEST_ATOP = 10, XOR = 11, ADD = 12, SATURATE = 13, MULTIPLY = 14, SCREEN = 15, OVERLAY = 16, DARKEN = 17, LIGHTEN = 18, COLOUR_DODGE = 19, COLOUR_BURN = 20, HARD_LIGHT = 21, SOFT_LIGHT = 22, DIFFERENCE = 23, EXCLUSION = 24, LAST = 25 }
        class Animation extends System.ValueType {
            public frames: System.Array$1<FairyEditor.VImage>;
            public frameDelays: System.Array$1<number>;
            public loopDelay: number;
            
        }
        
    }
    namespace XLua {
        class LuaTable extends XLua.LuaBase {
            
        }
        class LuaBase extends System.Object {
            
        }
        
    }
    namespace FairyEditor.View.ProjectView {
        type OnContextMenuDelegate = (pi: FairyEditor.FPackageItem, context: FairyGUI.EventContext) => void;
        var OnContextMenuDelegate: {new (func: (pi: FairyEditor.FPackageItem, context: FairyGUI.EventContext) => void): OnContextMenuDelegate;}
        type OnGetItemListingDelegate = (folder: FairyEditor.FPackageItem, filters: System.Array$1<string>, result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) => System.Collections.Generic.List$1<FairyEditor.FPackageItem>;
        var OnGetItemListingDelegate: {new (func: (folder: FairyEditor.FPackageItem, filters: System.Array$1<string>, result: System.Collections.Generic.List$1<FairyEditor.FPackageItem>) => System.Collections.Generic.List$1<FairyEditor.FPackageItem>): OnGetItemListingDelegate;}
        
    }
    namespace FairyEditor.Component.InputElement {
        class OptionalValue$1<T> extends System.ValueType {
            
        }
        
    }
    namespace FairyEditor.Component.FormHelper {
        type OnPropChangedDelegate = (propName: string, propValue: any, extData: any) => boolean;
        var OnPropChangedDelegate: {new (func: (propName: string, propValue: any, extData: any) => boolean): OnPropChangedDelegate;}
        
    }
    namespace System.Text.RegularExpressions {
        class Regex extends System.Object {
            
        }
        
    }
    namespace FairyEditor.Component.ViewGridGroup {
        type EachGridCallback = (grid: FairyEditor.Component.ViewGrid) => boolean;
        var EachGridCallback: {new (func: (grid: FairyEditor.Component.ViewGrid) => boolean): EachGridCallback;}
        
    }
    namespace FairyEditor.PluginManager.PluginInfo {
        class Author extends System.Object {
            public name: string;
            public constructor();
            
        }
        
    }
    namespace FairyGUI.BlendModeUtils {
        class BlendFactor extends System.Object {
            public srcFactor: UnityEngine.Rendering.BlendMode;
            public dstFactor: UnityEngine.Rendering.BlendMode;
            public pma: boolean;
            public constructor($srcFactor: UnityEngine.Rendering.BlendMode, $dstFactor: UnityEngine.Rendering.BlendMode, $pma?: boolean);
            
        }
        
    }
    namespace FairyGUI.MovieClip {
        class Frame extends System.Object {
            public texture: FairyGUI.NTexture;
            public addDelay: number;
            public constructor();
            
        }
        
    }
    namespace FairyGUI.NGraphics {
        class VertexMatrix extends System.Object {
            public cameraPos: UnityEngine.Vector3;
            public matrix: UnityEngine.Matrix4x4;
            public constructor();
            
        }
        
    }
    namespace FairyGUI.ShaderConfig {
        type GetFunction = (name: string) => UnityEngine.Shader;
        var GetFunction: {new (func: (name: string) => UnityEngine.Shader): GetFunction;}
        
    }
    namespace FairyGUI.BitmapFont {
        class BMGlyph extends System.Object {
            public x: number;
            public y: number;
            public width: number;
            public height: number;
            public advance: number;
            public lineHeight: number;
            public uv: System.Array$1<UnityEngine.Vector2>;
            public channel: number;
            public constructor();
            
        }
        
    }
    namespace FairyGUI.RTLSupport {
        enum DirectionType { UNKNOW = 0, LTR = 1, RTL = 2, NEUTRAL = 3 }
        
    }
    namespace FairyGUI.TextField {
        class LineInfo extends System.Object {
            public width: number;
            public height: number;
            public baseline: number;
            public charIndex: number;
            public charCount: number;
            public y: number;
            public constructor();
            public static Borrow():FairyGUI.TextField.LineInfo;
            public static Return($value: FairyGUI.TextField.LineInfo):void;
            public static Return($values: System.Collections.Generic.List$1<FairyGUI.TextField.LineInfo>):void;
            
        }
        class CharPosition extends System.ValueType {
            public charIndex: number;
            public lineIndex: number;
            public offsetX: number;
            public vertCount: number;
            public width: number;
            public imgIndex: number;
            
        }
        class LineCharInfo extends System.ValueType {
            public width: number;
            public height: number;
            public baseline: number;
            
        }
        
    }
    namespace FairyGUI.TextFormat {
        enum SpecialStyle { None = 0, Superscript = 1, Subscript = 2 }
        
    }
    namespace FairyGUI.UpdateContext {
        class ClipInfo extends System.ValueType {
            public rect: UnityEngine.Rect;
            public clipBox: UnityEngine.Vector4;
            public soft: boolean;
            public softness: UnityEngine.Vector4;
            public clipId: number;
            public rectMaskDepth: number;
            public referenceValue: number;
            public reversed: boolean;
            
        }
        
    }
    namespace TMPro {
        class TMP_FontAsset extends TMPro.TMP_Asset {
            
        }
        class TMP_Asset extends UnityEngine.ScriptableObject {
            
        }
        enum FontWeight { Thin = 100, ExtraLight = 200, Light = 300, Regular = 400, Medium = 500, SemiBold = 600, Bold = 700, Heavy = 800, Black = 900 }
        
    }
    namespace FairyGUI.ControllerAction {
        enum ActionType { PlayTransition = 0, ChangePage = 1 }
        
    }
    namespace FairyGUI.UIPackage {
        type CreateObjectCallback = (result: FairyGUI.GObject) => void;
        var CreateObjectCallback: {new (func: (result: FairyGUI.GObject) => void): CreateObjectCallback;}
        type LoadResource = (name: string, extension: string, type: System.Type, destroyMethod: $Ref<FairyGUI.DestroyMethod>) => any;
        var LoadResource: {new (func: (name: string, extension: string, type: System.Type, destroyMethod: $Ref<FairyGUI.DestroyMethod>) => any): LoadResource;}
        type LoadResourceAsync = (name: string, extension: string, type: System.Type, item: FairyGUI.PackageItem) => void;
        var LoadResourceAsync: {new (func: (name: string, extension: string, type: System.Type, item: FairyGUI.PackageItem) => void): LoadResourceAsync;}
        
    }
    namespace FairyGUI.GObjectPool {
        type InitCallbackDelegate = (obj: FairyGUI.GObject) => void;
        var InitCallbackDelegate: {new (func: (obj: FairyGUI.GObject) => void): InitCallbackDelegate;}
        
    }
    namespace FairyGUI.UIContentScaler {
        enum ScreenMatchMode { MatchWidthOrHeight = 0, MatchWidth = 1, MatchHeight = 2 }
        enum ScaleMode { ConstantPixelSize = 0, ScaleWithScreenSize = 1, ConstantPhysicalSize = 2 }
        
    }
    namespace FairyGUI.GTree {
        type TreeNodeRenderDelegate = (node: FairyGUI.GTreeNode, obj: FairyGUI.GComponent) => void;
        var TreeNodeRenderDelegate: {new (func: (node: FairyGUI.GTreeNode, obj: FairyGUI.GComponent) => void): TreeNodeRenderDelegate;}
        type TreeNodeWillExpandDelegate = (node: FairyGUI.GTreeNode, expand: boolean) => void;
        var TreeNodeWillExpandDelegate: {new (func: (node: FairyGUI.GTreeNode, expand: boolean) => void): TreeNodeWillExpandDelegate;}
        
    }
    namespace FairyGUI.UIObjectFactory {
        type GComponentCreator = () => FairyGUI.GComponent;
        var GComponentCreator: {new (func: () => FairyGUI.GComponent): GComponentCreator;}
        type GLoaderCreator = () => FairyGUI.GLoader;
        var GLoaderCreator: {new (func: () => FairyGUI.GLoader): GLoaderCreator;}
        
    }
    namespace FairyGUI.TreeView {
        type TreeNodeCreateCellDelegate = (node: FairyGUI.TreeNode) => FairyGUI.GComponent;
        var TreeNodeCreateCellDelegate: {new (func: (node: FairyGUI.TreeNode) => FairyGUI.GComponent): TreeNodeCreateCellDelegate;}
        type TreeNodeRenderDelegate = (node: FairyGUI.TreeNode) => void;
        var TreeNodeRenderDelegate: {new (func: (node: FairyGUI.TreeNode) => void): TreeNodeRenderDelegate;}
        type TreeNodeWillExpandDelegate = (node: FairyGUI.TreeNode, expand: boolean) => void;
        var TreeNodeWillExpandDelegate: {new (func: (node: FairyGUI.TreeNode, expand: boolean) => void): TreeNodeWillExpandDelegate;}
        
    }
    namespace FairyGUI.UIConfig {
        class ConfigValue extends System.Object {
            public valid: boolean;
            public s: string;
            public i: number;
            public f: number;
            public b: boolean;
            public c: UnityEngine.Color;
            public constructor();
            public Reset():void;
            
        }
        type SoundLoader = (url: string) => FairyGUI.NAudioClip;
        var SoundLoader: {new (func: (url: string) => FairyGUI.NAudioClip): SoundLoader;}
        enum ConfigKey { DefaultFont = 0, ButtonSound = 1, ButtonSoundVolumeScale = 2, HorizontalScrollBar = 3, VerticalScrollBar = 4, DefaultScrollStep = 5, DefaultScrollBarDisplay = 6, DefaultScrollTouchEffect = 7, DefaultScrollBounceEffect = 8, TouchScrollSensitivity = 9, WindowModalWaiting = 10, GlobalModalWaiting = 11, PopupMenu = 12, PopupMenu_seperator = 13, LoaderErrorSign = 14, TooltipsWin = 15, DefaultComboBoxVisibleItemCount = 16, TouchDragSensitivity = 17, ClickDragSensitivity = 18, ModalLayerColor = 19, RenderingTextBrighterOnDesktop = 20, AllowSoftnessOnTopOrLeftSide = 21, InputCaretSize = 22, InputHighlightColor = 23, EnhancedTextOutlineEffect = 24, DepthSupportForPaintingMode = 25, RichTextRowVerticalAlign = 26, Branch = 27, PleaseSelect = 100 }
        
    }
    namespace FairyGUI.Utils.UBBParser {
        type TagHandler = (tagName: string, end: boolean, attr: string) => string;
        var TagHandler: {new (func: (tagName: string, end: boolean, attr: string) => string): TagHandler;}
        
    }
    namespace FairyGUI.Utils.XMLList {
        class Enumerator extends System.ValueType {
            public Current: FairyGUI.Utils.XML;
            public constructor($source: System.Collections.Generic.List$1<FairyGUI.Utils.XML>, $selector: string);
            public MoveNext():boolean;
            public Erase():void;
            public Reset():void;
            
        }
        
    }
    namespace FairyGUI.Utils.ZipReader {
        class ZipEntry extends System.Object {
            public name: string;
            public compress: number;
            public crc: number;
            public size: number;
            public sourceSize: number;
            public offset: number;
            public isDirectory: boolean;
            public constructor();
            
        }
        
    }
    
}