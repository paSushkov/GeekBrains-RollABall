using LabirinthGame.Common;
using LabirinthGame.Common.Interfaces;
using LabirinthGame.Managers;
using UnityEngine;

namespace LabirinthGame.Collectibles
{
    [RequireComponent(typeof(TriggerListener))]
    public class CollectibleBase : MonoBehaviour, IRotatable, IFlickerableMaterialSender
    {
        #region Private data

        [SerializeField] private bool isMandatory;
        [SerializeField] private Vector3 rotationSpeed = Vector3.zero;
        [SerializeField] private LayerMask reactLayers = new LayerMask();
        private TriggerListener triggerListener = null;
        private Transform cachedTransform = null;
        private TriggerListener listener = null;
        private bool isSubscribedToTrigger;

        #endregion


        #region Unity events

        private void Awake()
        {
            AwakeInit();
        }

        private void OnDestroy()
        {
            DestroyShutdown();
        }

        #endregion


        #region Private methods

        private void CheckLayerAndDoSomething(Collider other)
        {
            if (TriggerListener.IsInLayerMask(other.gameObject.layer, reactLayers))
            {
                OnCollectEffect(other);
            }
        }

        protected virtual void AwakeInit()
        {
            if (isMandatory)
                GameManager.Instance.RegisterMandatoryCollectible(this);

            RotationSpeed = rotationSpeed;
            RotationalTransform = cachedTransform = transform;
            TryGetComponent(out triggerListener);
            SubscribeToTriggerListener();

            (this as IRotatable).Register(MasterRotator.Instance);

            if (TryGetComponent(out MeshRenderer renderer))
            {
                FlickeringMaterial = renderer.sharedMaterial;
                (this as IFlickerableMaterialSender).Register(MasterFlicker.Instance);
            }
        }

        protected virtual void DestroyShutdown()
        {
            (this as IRotatable).UnRegister(MasterRotator.Instance);
            UnsubscribeFromTriggerListener();
            if (isMandatory)
                GameManager.Instance.UnregisterMandatoryCollectible(this);
        }

        protected virtual void OnCollectEffect(Collider other)
        {
            Destroy(gameObject);
        }

        #endregion


        #region IRotatable implementation

        public Transform RotationalTransform { get; private set; }

        public Vector3 RotationSpeed { get; private set; }

        public void Register(MasterRotator master)
        {
            if (master)
                master.Register(this);
        }

        public void UnRegister(MasterRotator master)
        {
            if (master != null)
                master.Unregister(this);
        }

        #endregion


        #region IFlickerableMaterialSender implementation

        public Material FlickeringMaterial { get; private set; }

        public void Register(MasterFlicker master)
        {
            master.Register(FlickeringMaterial);
        }

        #endregion


        #region Trigger listener subscribtion

        private void SubscribeToTriggerListener()
        {
            if (!isSubscribedToTrigger && triggerListener != null)
            {
                triggerListener.EnterTrigger += CheckLayerAndDoSomething;
                isSubscribedToTrigger = true;
            }
        }

        private void UnsubscribeFromTriggerListener()
        {
            if (isSubscribedToTrigger && triggerListener != null)
            {
                triggerListener.EnterTrigger -= CheckLayerAndDoSomething;
            }

            isSubscribedToTrigger = false;
        }

        #endregion
    }
}