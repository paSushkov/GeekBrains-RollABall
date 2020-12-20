using Application.Scripts.Common.Interfaces;
using LabirinthGame.Common;
using LabirinthGame.Common.Interfaces;
using UnityEngine;

namespace LabirinthGame.Collectibles
{
    // TODO: Re-implement with interface
    public class CollectibleBase : IRotatable
    {
        #region Private data

        private bool isMandatory;
        private LayerMask reactLayers = new LayerMask();
        private TriggerListener triggerListener = null;
        private Transform modelTransform = null;
        private TriggerListener listener = null;
        private bool isSubscribedToTrigger;
        private IObjectPool<CollectibleBase> selfPool = null;
        private IObjectPool<Transform> modelPool = null;
        private IRotator masterRotator;

        #endregion

        #region Properties

        public bool Initialized { get; protected set; }

        #endregion
        
        

        #region Private methods

        private void CheckLayerAndDoSomething(Collider other)
        {
            if (TriggerListener.IsInLayerMask(other.gameObject.layer, reactLayers))
            {
                OnCollectEffect(other);
            }
        }

        public virtual void Initialize(
            IObjectPool<CollectibleBase> selfPool,
            IObjectPool<Transform> modelPool,
            IRotator masterRotator,
            bool isMandatory,
            Transform modelTransform,
            TriggerListener listener,
            LayerMask reactLayers)
        {
            this.isMandatory = isMandatory;
            this.modelTransform = modelTransform;
            this.listener = listener;
            this.reactLayers = reactLayers;
            this.modelPool = modelPool;
            this.selfPool = selfPool;
            this.masterRotator = masterRotator;

            var range = Random.Range(0f, 1f);
            RotationSpeed = new Vector3(Random.Range(-range, range),Random.Range(-range, range), Random.Range(-range, range) ) * 360f;
            
            
            // if (isMandatory)
            //     GameManager.Instance.RegisterMandatoryCollectible(this);

            
            RotationalTransform = modelTransform;
            SubscribeToTriggerListener();
            Register(masterRotator);
            Initialized = true;
        }

        protected virtual void Shutdown()
        {
            //(this as IRotatable).UnRegister(MasterRotator.Instance);
            UnsubscribeFromTriggerListener();
            // if (isMandatory)
            //     GameManager.Instance.UnregisterMandatoryCollectible(this);
            
            modelTransform = null;
            listener = null;
            selfPool.ReturnObject(this);
            modelPool.ReturnObject(modelTransform);
            Initialized = false;
        }

        protected virtual void OnCollectEffect(Collider other)
        {
            Shutdown();
        }

        #endregion


        #region IRotatable implementation

        public Transform RotationalTransform { get; private set; }

        public Vector3 RotationSpeed { get; private set; }

        public void Register(IRotator rotator)
        {
            if (rotator!=null)
                rotator.RotateInFixedUpdate(this);
        }

        public void Unregister(IRotator rotator)
        {
            if (rotator!=null)
                rotator.StopRotateInFixedUpdate(this);
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