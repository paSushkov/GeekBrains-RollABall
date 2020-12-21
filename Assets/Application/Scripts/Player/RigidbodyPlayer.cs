using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Stats;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Player
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

        public override void Initialize(Transform modelTransform, IPlayerLoopProcessor playerLoopProcessor, StatsDictionary stats)
        {
            base.Initialize(modelTransform, playerLoopProcessor, stats);
            if (modelTransform.TryGetComponent<Rigidbody>(out var cachedRigidBody))
                ModelRigidbody = cachedRigidBody;
            else
            {
                Debug.LogWarning($"{modelTransform.name} does not contain Rigidbody, which is required by {GetType().Name}." +
                                 " Default Rigidbody component will be added now and cleaned up during Shutdown process.");
                ModelRigidbody = modelTransform.gameObject.AddComponent<Rigidbody>();
                needToCleanUpRigidbody = true;
            }
        }

        public override void Shutdown()
        {
            if (ModelRigidbody && needToCleanUpRigidbody &&
                GameTransform.TryGetComponent<Rigidbody>(out var currentBody) &&
                currentBody == ModelRigidbody)
            {
                Object.Destroy(currentBody);
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
            ModelRigidbody.AddForce(direction * speed, ForceMode.Force);
            var currentVelocity = ModelRigidbody.velocity;
            var sqrMagnitude = currentVelocity.sqrMagnitude;
            if (sqrMagnitude > MaxMoveSpeedSqr)
            {
                ModelRigidbody.velocity = Vector3.ClampMagnitude(currentVelocity, MaxMoveSpeed);
            }
        }

        #endregion


    }
}