namespace LabirinthGame.Tech.PlayerLoop
{
    public interface IPlayerLoopSubscriptionController
    {
        bool IsSubscribedForPlayerLoop { get; }
        IPlayerLoop PlayerLoopUser { get; }
        IPlayerLoopProcessor PlayerLoopProcessor { get; }
        void Initialize(IPlayerLoop playerLoopUser, IPlayerLoopProcessor loopProcessor);
        void SubscribeToLoop();
        void UnsubscribeFromLoop();
        void Shutdown();
    }
}