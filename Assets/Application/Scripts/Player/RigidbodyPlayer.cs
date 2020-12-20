using LabirinthGame.Common.Interfaces;
using LabirinthGame.Stats;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Player
{
    public class RigidbodyPlayer : PlayerBase, IRigidbody
    {
        #region Private data
        
        private bool needToCleanUpRigidbody;

        #endregion


        #region IRigidbody implementation

        public Rigidbody ModelRigidbody { get; private set; }

        #endregion


        #region Private methods

        public override void Initialize(Transform modelTransform, IPlayerLoopProcessor playerLoopProcessor, Stat speedStat)
        {
            base.Initialize(modelTransform, playerLoopProcessor, speedStat);
            if (modelTransform.TryGetComponent<Rigidbody>(out var cachedRigidBody))
                ModelRigidbody = cachedRigidBody;
            else
            {
                Debug.LogWarning($"{modelTransform.name} does not contain Rigidbody, which is required by {GetType().Name}." +
                                 " Default Rigidbody component will be added now and cleaned up during Shutdown process.");
                ModelRigidbody = modelTransform.gameObject.AddComponent<Rigidbody>();
                needToCleanUpRigidbody = true;
            }
            //CachedRigidBody.maxAngularVelocity = 21f; // magic number. feels good
        }

        public override void Shutdown()
        {
            if (needToCleanUpRigidbody &&
                GameTransform.TryGetComponent<Rigidbody>(out var currentBody) &&
                currentBody == ModelRigidbody)
            {
                Object.Destroy(ModelRigidbody);
            }

            base.Shutdown();
        }

        #endregion

        
        #region IFixedUpdateProcessor implementation

        public override void ProcessFixedUpdate(float fixedDeltaTime)
        {
            if (MoveDirection.sqrMagnitude>0f)
                Move(MoveDirection, MoveSpeed);
            base.ProcessFixedUpdate(fixedDeltaTime);
        }

        #endregion
        
        
        #region IMovable implementation

        public override void MoveTowards(Vector3 position, float speed)
        {
            var direction = position - GameTransform.position;
            Move(direction, speed);
        }

        public override void Move(Vector3 direction, float speed)
        {
            ModelRigidbody.AddTorque(direction * speed, ForceMode.Acceleration);
        }

        #endregion


    }
}