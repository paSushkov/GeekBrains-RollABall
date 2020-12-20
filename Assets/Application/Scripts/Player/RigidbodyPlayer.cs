using LabirinthGame.Common.Interfaces;
using LabirinthGame.Stats;
using LabirinthGame.Tech.Input;
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

        public override void Initialize(Transform modelTransform, IPlayerLoopProcessor playerLoopProcessor, Stat speedStat, IInputListener inputListener)
        {
            base.Initialize(modelTransform, playerLoopProcessor, speedStat, inputListener);
            Debug.Log(speedStat.CurrentValue);
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
                ModelTransform.TryGetComponent<Rigidbody>(out var currentBody) &&
                currentBody == ModelRigidbody)
            {
                Object.Destroy(ModelRigidbody);
            }

            base.Shutdown();
        }

        private void GetInput()
        {
            moveInput = new Vector3(_inputListener.Vertical, 0, -_inputListener.Horizontal);
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);
        }
        
        #endregion

        
        #region IFixedUpdateProcessor implementation

        public override void ProcessFixedUpdate(float fixedDeltaTime)
        {
            Debug.Log("ProcessFixedUpdate");
            Debug.Log($"{SelfMoveSpeed}");
            GetInput();
            if (moveInput.sqrMagnitude>0f)
                Move(moveInput, SelfMoveSpeed);
            base.ProcessFixedUpdate(fixedDeltaTime);
        }

        #endregion
        
        
        #region IMovable implementation

        public override void MoveTowards(Vector3 position, float speed)
        {
            var direction = position - ModelTransform.position;
            Move(direction, speed);
        }

        public override void Move(Vector3 direction, float speed)
        {
            Debug.Log("MOVING");
            ModelRigidbody.AddTorque(direction * speed, ForceMode.Acceleration);
        }

        #endregion


    }
}