namespace LabirinthGame.Common.Interfaces
{
    public interface ITracker
    {
        void SetTarget(ITrackable target);

        ITrackable GetTarget();

    }
}