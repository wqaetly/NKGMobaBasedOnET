namespace Slate
{

    public interface IEvent
    {
        string name { get; }
        void Invoke();
    }
}