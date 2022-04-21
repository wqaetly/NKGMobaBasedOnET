namespace UnityEngine
{
    public interface ISerializationCallbackReceiver
    {
        void OnBeforeSerialize();
        
        void OnAfterDeserialize();
    }
}