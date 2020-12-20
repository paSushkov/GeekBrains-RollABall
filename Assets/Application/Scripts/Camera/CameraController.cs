using System.Collections.Generic;
using LabirinthGame.Common.Handlers;
using LabirinthGame.Common.Interfaces;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Camera
{
    public class CameraController : ICameraController, IPlayerLoop
    {
        public event VoidHandler TrackingProcess;
        
        #region Public data

        public float moveSpeed = 0.5f;
        public float maxDistance = 2f;
        public Vector3 _offset = new Vector3(0, 20f, -5f);

        #endregion

        
        #region Private data

        private Vector3 _desiredPosition;
        private List<PositionChangeProcessor> subscribedProcessors = new List<PositionChangeProcessor>();
        private ITrackable _target;


        #endregion

        #region Properties

        public bool IsTracking { get; private set; }

        #endregion


        #region Public methods

        public void Initilize(IPlayerLoopProcessor playerLoopProcessor, ITrackable target, Transform cameraTransform)
        {
            PlayerLoopSubscriptionController.Initialize(this, playerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
            Target = target;
            CameraTransform = cameraTransform;
        }
        
        #endregion


        public void Shutdown()
        {
            PlayerLoopSubscriptionController.Shutdown();
            StopTracking();
            CameraTransform = null;
            Target = null;

        }

        
        #region Private methods

        private void HandleMovement()
        {
            var direction = _desiredPosition - CameraTransform.position;
            var sqrDistance = Vector3.SqrMagnitude(direction);

            if (sqrDistance > float.Epsilon)
            {
                if (sqrDistance > maxDistance * maxDistance)
                {
                    CameraTransform.position = _desiredPosition - direction.normalized * maxDistance;
                }

                MoveTowards(_desiredPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                CameraTransform.position = _desiredPosition;
            }
        }

        private void GetDesiredPosition(Vector3 position)
        {
            _desiredPosition = position + _offset;
        }

        #endregion


        #region IMovableImplementation

        public float SelfMoveSpeed { get; set; }

        public void MoveTo(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        public void MoveTowards(Vector3 position, float speed)
        {
            CameraTransform.position = Vector3.Lerp(CameraTransform.position, position, speed);
        }

        public void Move(Vector3 direction, float speed)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        
        #region ICameraController implementation

        public Transform CameraTransform { get; private set; }
        public void LookAt(Vector3 point)
        {
            CameraTransform.LookAt(point, Vector3.up);
        }

        #endregion


        
        #region ITracker implementation

        public ITrackable Target 
        { get => _target;
            set
            {
             if (_target!=null)
                 StopTracking();
             _target = value;
            }
        }
        
        public void StartTracking()
        {
            if (Target != null && !IsTracking)
            {
                GetDesiredPosition(Target.Position);
                subscribedProcessors.Add(GetDesiredPosition);
                Target.OnPositionChange += GetDesiredPosition;
                TrackingProcess += HandleMovement;
                IsTracking = true;
            }
        }

        public void StopTracking()
        {
            if (Target != null && IsTracking)
            {
                foreach (var processor in subscribedProcessors)
                {
                    Target.OnPositionChange -= processor;
                }
            }

            subscribedProcessors.Clear();
            IsTracking = false;
        }
        
        
        #endregion
        

        #region IPlayerLoop implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; } = new PlayerLoopSubscriptionController();

        public void ProcessUpdate(float deltaTime)
        {
        }
        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
            TrackingProcess?.Invoke();
        }
        
        #endregion

    }
}