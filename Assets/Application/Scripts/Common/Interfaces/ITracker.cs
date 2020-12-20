namespace LabirinthGame.Common.Interfaces
{
    public interface ITracker
    {
        ITrackable Target { get; set; }
        void StartTracking();
        void StopTracking();

    }
}