namespace LabirinthGame.Tech.PlayerLoop
{
    public class PlayerLoopSubscriptionController : IPlayerLoopSubscriptionController
    {
        #region Properties
        public bool IsSubscribedForPlayerLoop { get; private set; }
        
        #endregion
        

        #region IPlayerLoopSubscriptionController implementation

        public IPlayerLoop PlayerLoopUser { get; private set; }
        public IPlayerLoopProcessor PlayerLoopProcessor { get; private set; }
        
        
        public void Initialize(IPlayerLoop playerLoopUser, IPlayerLoopProcessor loopProcessor)
        {
            PlayerLoopUser = playerLoopUser;
            PlayerLoopProcessor = loopProcessor;
        }

        public void SubscribeToLoop()
        {
            if (!IsSubscribedForPlayerLoop && PlayerLoopProcessor != null && PlayerLoopUser!=null)
            {
                PlayerLoopProcessor.SubscribeToLoop(PlayerLoopUser);
                IsSubscribedForPlayerLoop = true;
            }
        }

        public void UnsubscribeFromLoop()
        {
            if (IsSubscribedForPlayerLoop && PlayerLoopProcessor != null && PlayerLoopUser!=null)
            {
                PlayerLoopProcessor.UnsubscribeFromLoop(PlayerLoopUser);
            }

            IsSubscribedForPlayerLoop = false;
        }

        public void Shutdown()
        {
            UnsubscribeFromLoop();
            PlayerLoopUser = null;
            PlayerLoopProcessor = null;
        }
        
        #endregion

    }
}