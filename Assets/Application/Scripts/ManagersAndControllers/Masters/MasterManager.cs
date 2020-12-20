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
            if (linksManager)
                linksManager.Initialize(playerLoopProcessor, player, camera );
            
        }

        public void Shutdown()
        {
            if (linksManager)
                linksManager.Shutdown();

        }

        #endregion
        
    }
}