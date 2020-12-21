namespace LabyrinthGame.Common.Interfaces
{
    public interface IListenTrigger
    {
        TriggerListener MyTriggerListener { get; }
        void SubscribeToTriggerListener();
        void UnsubscribeFromTriggerListener();
    }
}