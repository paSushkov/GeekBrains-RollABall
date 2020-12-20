using LabirinthGame.Camera;
using LabirinthGame.Common.Interfaces;
using LabirinthGame.Player;
using LabirinthGame.Stats;
using LabirinthGame.Tech.Input;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/Managers/LinksManager")]
    public class LinksManager : ScriptableObject
    {
        #region Private data

        [SerializeField] private RotationManager rotationManager = null;
        [SerializeField] private InputManagerTranslator inputManagerTranslator = null;
        [SerializeField] private InputManagerListener inputManagerListener = null;


        private CameraController camCon = new CameraController();
        private RigidbodyPlayer player = new RigidbodyPlayer();
            
        #endregion
        
        
        #region Tech links

        public IPlayerLoopProcessor PlayerLoopProcessor { get; private set; }
        public IInputTranslator InputTranslator => inputManagerTranslator;
        public IInputListener InputListener => inputManagerListener;

        #endregion


        #region Game-logic essencial links

        public IRotator RotationManager => rotationManager;
        public IPlayer ActivePlayer { get; private set; }

        #endregion


        #region Public methods

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor, Transform playerModel, Transform cameraModel)
        {
            PlayerLoopProcessor = playerLoopProcessor;
            
            if (RotationManager != null)
                RotationManager.Initialize(PlayerLoopProcessor); 
            
            if (inputManagerTranslator != null)
                inputManagerTranslator.Initialize(PlayerLoopProcessor);    

            if (inputManagerListener != null)
                inputManagerListener.Initialize(inputManagerTranslator);
            
            player.Initialize(playerModel, playerLoopProcessor, new Stat(0,400,200), inputManagerListener);
            camCon.Initilize(PlayerLoopProcessor, player,cameraModel);
            camCon.StartTracking();
        }

        public void Shutdown()
        {
            if (rotationManager != null)
                rotationManager.Shutdown();
            
            if (inputManagerTranslator != null)
                inputManagerTranslator.Shutdown();
            
            if (inputManagerListener != null)
                inputManagerListener.Shutdown(inputManagerTranslator);    
            
            camCon.StopTracking();

        }

        #endregion
    }
}