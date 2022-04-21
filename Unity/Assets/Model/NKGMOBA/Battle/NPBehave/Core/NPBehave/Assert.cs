using ET;

namespace NPBehave
{
    public class Debug
    {
        public static void Assert(bool result, string errorMessage = "")
        {
            if (!result)
            {
                Log.Error($"NPBehave Assert Fail!!!: {errorMessage}");
            }
        }
    }
}