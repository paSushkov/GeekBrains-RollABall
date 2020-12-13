using LabirinthGame.Managers;
using UnityEngine;

namespace LabirinthGame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyPlayer : PlayerBase
    {
        #region Private data

        private bool _isSubscribedForInput;
        private Vector3 moveInput;

        #endregion


        #region Properties

        public Rigidbody CachedRigidBody { get; private set; }

        #endregion


        #region Unity events

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            
            if (moveInput.sqrMagnitude>0f)
                Move(moveInput, moveSpeed);
        }

        private void Update()
        {
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);
        }

        #endregion


        #region Private methods

        protected override void AwakeInitialize()
        {
            base.AwakeInitialize();
            CachedRigidBody = GetComponent<Rigidbody>();
            CachedRigidBody.maxAngularVelocity = 21f; // magic number. feels good
            SubscribeForInput();
        }

        protected override void DestroyShutdown()
        {
            base.DestroyShutdown();
            UnsubscribeFromInput();
        }

        private void GetInput(float horizontal, float vertical)
        {
            moveInput = Vector3.ClampMagnitude(new Vector3(vertical, 0f, -horizontal), 1f);
        }
        
        #endregion

        
        #region Input subscribtion management

        private void SubscribeForInput()
        {
            if (_isSubscribedForInput || InputManager.Instance == null)
                return;
            InputManager.Instance.OnAxisInputDone += GetInput;
            _isSubscribedForInput = true;
        }

        private void UnsubscribeFromInput()
        {
            if (!_isSubscribedForInput)
                return;
            if (InputManager.Instance == null)
            {
                _isSubscribedForInput = false;
                return;
            }

            InputManager.Instance.OnAxisInputDone -= GetInput;
            _isSubscribedForInput = false;
        }

        #endregion


        #region IMovable implementation

        public override void MoveTowards(Vector3 position, float speed)
        {
            var direction = position - CachedTransform.position;
            Move(direction, speed);
        }

        public override void Move(Vector3 direction, float speed)
        {
            CachedRigidBody.AddTorque(direction * speed, ForceMode.Acceleration);
        }

        #endregion
    }
}