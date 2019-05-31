namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// 异步方法特性
    /// </summary>
    public sealed class AsyncMethodBuilderAttribute: Attribute
    {
        /// <summary>
        /// 异步方法类型
        /// </summary>
        public Type BuilderType { get; }

        public AsyncMethodBuilderAttribute(Type builderType)
        {
            BuilderType = builderType;
        }
    }
}