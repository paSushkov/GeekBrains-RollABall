using LabyrinthGame.Common.Handlers;
using UnityEngine;


namespace LabyrinthGame.Common
{
    public sealed class TriggerListener : MonoBehaviour
    {
        #region Public data

        public Collider listeningCollider;

        #endregion

        
        #region Static metods

        public static bool IsInLayerMask(int layer, LayerMask layerMask)
        {
            return layerMask == (layerMask | (1 << layer));
        }

        #endregion
        
        
        #region Unity events

        private void Awake()
        {
            TryGetComponent(out listeningCollider);
        }

        private void OnTriggerStay(Collider other)
        {
            StayingInTrigger?.Invoke(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            EnterTrigger?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            ExitTrigger?.Invoke(other);
        }

        #endregion

        
        #region Events
        
        public event TriggerEventHandler StayingInTrigger;
        public event TriggerEventHandler EnterTrigger;
        public event TriggerEventHandler ExitTrigger;

        #endregion
    }
}