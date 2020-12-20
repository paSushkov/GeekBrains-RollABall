using LabirinthGame.Camera;
using LabirinthGame.Player;
using LabirinthGame.Stats;
using LabirinthGame.Tech.Input;
using LabirinthGame.Tech.PlayerLoop;
using Sushkov.SingletonScriptableObject;
using UnityEngine;

namespace LabirinthGame.Managers
{
    
    [CreateAssetMenu(menuName = "Sushkov/SingletonScriptableObject/MasterManager")]
    public class MasterManager : SingletonScriptableObject<MasterManager>
    {
        [SerializeField] private LinksManager linksManager = null;

        #region Properties

        public LinksManager LinksManager => linksManager;
        

        #endregion


        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FirstInitialize()
        {
            Debug.Log("Master manger initialized");
        }
        
        #endregion

        
        #region Public methods

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor, Transform camera, Transform player)
        {
            if (LinksManager)
                LinksManager.Initialize(playerLoopProcessor);

            
            
            LinksManager.CameraController = new CameraController();
            LinksManager.UserInputManager.Initialize(new GameProcessInputController(), null);

            // Temp
            LinksManager.ActivePlayer = new RigidbodyPlayer();
            LinksManager.ActivePlayer.Initialize(player, playerLoopProcessor, new Stat(0, 800, 400));
            LinksManager.CameraController.Initialize(playerLoopProcessor, camera);
            LinksManager.CameraController.StartTracking(LinksManager.ActivePlayer);
            
            LinksManager.UserInputManager.GetGameActiveController().Start();
            

        }

        public void Shutdown()
        {
            if (linksManager)
                linksManager.Shutdown();

        }

        #endregion
        
    }
}