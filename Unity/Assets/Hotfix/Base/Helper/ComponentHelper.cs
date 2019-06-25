namespace ETHotfix
{
    public static class ComponentHelper
    {
        public static bool IsEmptyOrDisposed(this Component component)
        {
            return component == null || component.IsDisposed;
        }

        public static bool ExistAndUndisposed(this Component component)
        {
            return component != null && !component.IsDisposed;
        }
    }
}
