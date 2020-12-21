namespace LabyrinthGame.Common.Interfaces
{
    public interface ITracker
    {
        ITrackable Target { get; }
        void StartTracking(ITrackable target);
        void StopTracking();

    }
}