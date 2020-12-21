namespace LabyrinthGame.Tech.PlayerLoop
{
    public interface IPlayerLoopProcessor
    {
        void SubscribeToLoop (IPlayerLoop subProcessor);
        void UnsubscribeFromLoop(IPlayerLoop subProcessor);
        
        
        void SubscribeUpdate(PlayerLoopProcess process);
        void UnsubscribeUpdate (PlayerLoopProcess process);
        
        
        void SubscribeFixedUpdate (PlayerLoopProcess process);
        void UnsubscribeFixedUpdate (PlayerLoopProcess process);

        void SubscribeLateUpdate(PlayerLoopProcess process);
        void UnsubscribeLateUpdate (PlayerLoopProcess process);

        void Shutdown();

    }
}