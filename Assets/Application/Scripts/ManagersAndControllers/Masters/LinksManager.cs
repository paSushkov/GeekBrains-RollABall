using LabirinthGame.Camera;
using LabirinthGame.Common.Interfaces;
using LabirinthGame.Player;
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
        [SerializeField] private UserInputManager userInputManager = null;
        
        #endregion
        
        
        #region Tech links

        public IPlayerLoopProcessor PlayerLoopProcessor { get; private set; }
        public IInputTranslator InputTranslator => inputManagerTranslator;
        public IInputListener InputListener => inputManagerListener;
        public IUserInputManager UserInputManager => userInputManager; 

        #endregion


        #region Game-logic essencial links

        public IRotator RotationManager => rotationManager;
        public IPlayer ActivePlayer { get; set; }
        public ICameraController CameraController { get; set; }

        #endregion


        #region Public methods

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor)
        {
            PlayerLoopProcessor = playerLoopProcessor;
            
            RotationManager?.Initialize(playerLoopProcessor);
            InputTranslator?.Initialize(playerLoopProcessor);
            InputListener?.Initialize(InputTranslator);
        }

        public void Shutdown()
        {
            ActivePlayer?.Shutdown();
            ActivePlayer = null;
            CameraController?.Shutdown();
            CameraController = null;

            RotationManager?.Shutdown();
            InputTranslator?.Shutdown();
            InputListener?.Shutdown(inputManagerTranslator);
            UserInputManager?.Shutdown();
        }

        #endregion
    }
}