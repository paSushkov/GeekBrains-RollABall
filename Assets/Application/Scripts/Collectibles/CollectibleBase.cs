using LabyrinthGame.Common;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Managers;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    public class CollectibleBase : ICollectible, IRotatable
    {
        #region Private data

        private LayerMask reactLayers;
        private bool isSubscribedToTrigger;

        #endregion



        #region ICollectible implementation

        public bool IsMandatory { get; private set; }

        public void Initialize(bool isMandatory, Transform gameTransform, TriggerListener listener, LayerMask reactLayers)
        {
            if (isMandatory)
            {
                IsMandatory = isMandatory;
                MasterManager.Instance.mandatoryScore++;
            }

            RotationalTransform = GameTransform = gameTransform;
            RegisterAsTransformOwner();
            MyTriggerListener = listener;
            this.reactLayers = reactLayers;
            SubscribeToTriggerListener();
            
            RotationSpeed = new Vector3(Random.Range(-1f, 1f),Random.Range(-1f, 1f), Random.Range(-1f, 1f) ) * 360f;
            StartRotating(MasterManager.Instance.LinksHolder.RotationManager);            
        }

        public void Shutdown()
        {
            UnsubscribeFromTriggerListener();
            StopRotating();
            RotationalTransform = GameTransform = null;
            MyTriggerListener = null;
        }

        public virtual void Collect(Collider collider)
        {
            if (IsMandatory)
            {
                MasterManager.Instance.mandatoryScore--;
            }
            DisposeTransform();
            Shutdown();
        }


        #region IHaveTransform implementation

        public Transform GameTransform { get; private set; }
        public void RegisterAsTransformOwner()
        {
            MasterManager.Instance.LinksHolder.RegisterTransform(this, GameTransform);
        }

        public void DisposeTransform()
        {
            MasterManager.Instance.LinksHolder.DismissTransform(this);
        }

        #endregion


        #region IListenTrigger imlementation
        
        public TriggerListener MyTriggerListener { get; private set; }

        public void SubscribeToTriggerListener()
        {
            if (!isSubscribedToTrigger && MyTriggerListener != null)
            {
                MyTriggerListener.EnterTrigger += CheckLayerAndDoSomething;
                isSubscribedToTrigger = true;
            }
        }

        public void UnsubscribeFromTriggerListener()
        {
            if (isSubscribedToTrigger && MyTriggerListener != null)
            {
                MyTriggerListener.EnterTrigger -= CheckLayerAndDoSomething;
            }

            isSubscribedToTrigger = false;
        }


        #endregion
        
        
        #region IRotatable implementation

        public IRotator RotationOperator { get; private set; }
        public Transform RotationalTransform { get; private set; }

        public Vector3 RotationSpeed { get; private set; }

        public void StartRotating(IRotator rotator)
        {
            if (rotator != null)
            {
                RotationOperator = rotator;
                rotator.RotateInFixedUpdate(this);
            }
        }

        public void StopRotating()
        {
            if (RotationOperator != null)
            {
                RotationOperator.StopRotateInFixedUpdate(this);
                RotationOperator = null;
            }
        }

        #endregion
        

        #endregion

        
        #region Private methods

        private void CheckLayerAndDoSomething(Collider other)
        {
            if (TriggerListener.IsInLayerMask(other.gameObject.layer, reactLayers))
            {
                Collect(other);
            }
        }


        #endregion

    }
}