using System.Collections.Generic;
using LabyrinthGame.Common.Handlers;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Managers;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Camera
{
    public class CameraController : ICameraController
    {
        public event VoidHandler TrackingProcess;
        
        #region Public data

        public float moveSpeed = 0.5f;
        public float maxDistance = 0.5f;
        public Vector3 _offset = new Vector3(0, 20f, -5f);

        #endregion

        
        #region Private data

        private Vector3 _desiredPosition;
        private List<PositionChangeProcessor> subscribedProcessors = new List<PositionChangeProcessor>();

        #endregion

        #region Properties
        public bool IsTracking { get; private set; }

        #endregion
        
        #region Private methods

        private void HandleMovement()
        {
            var direction = _desiredPosition - GameTransform.position;
            var sqrDistance = Vector3.SqrMagnitude(direction);

            if (sqrDistance > float.Epsilon)
            {
                if (sqrDistance > maxDistance * maxDistance)
                {
                    GameTransform.position = _desiredPosition - direction.normalized * maxDistance;
                }

                MoveTowards(_desiredPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                GameTransform.position = _desiredPosition;
            }
            LookAt(Target.Position);
        }

        private void GetDesiredPosition(Vector3 position)
        {
            _desiredPosition = position + _offset;
        }

        #endregion


        #region IMovableImplementation

        public float MoveSpeed { get; set; }

        public void MoveTo(Vector3 position)
        {
            GameTransform.position = position;
        }

        public void MoveTowards(Vector3 position, float speed)
        {
            GameTransform.position = Vector3.Lerp(GameTransform.position, position, speed);
        }

        public void Move(Vector3 direction, float speed)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        
        #region ICameraController implementation

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor, Transform cameraTransform)
        {
            PlayerLoopSubscriptionController.Initialize(this, playerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
            GameTransform = cameraTransform;
            RegisterAsTransformOwner();
        }

        public void Shutdown()
        {
            PlayerLoopSubscriptionController.Shutdown();
            StopTracking();
            DisposeTransform();
            GameTransform = null;
            Target = null;
        }
        
        public void LookAt(Vector3 point)
        {
            GameTransform.LookAt(point, Vector3.up);
            var angles = GameTransform.rotation.eulerAngles;
            angles.y = 0;
            angles.z = 0;
            GameTransform.rotation = Quaternion.Euler(angles);
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
        
        
        #region ITracker implementation

        public ITrackable Target { get; private set; }
        
        
        public void StartTracking(ITrackable target)
        {
            if (IsTracking)
                StopTracking();

            if (target != null)
            {
                Target = target;
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
        
        
        
        #endregion


        

        



    }
}