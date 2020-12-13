using LabirinthGame.Common;
using LabirinthGame.Common.Interfaces;
using UnityEngine;

namespace LabirinthGame.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Public data

        [SerializeField] private float moveSpeed = 0.5f;
        [SerializeField] private float maxDistance = 2f;

        #endregion
        

        #region Private data

        private Vector3 _desiredPosition;
        public Vector3 _offset = new Vector3(0, 20f, -5f);
        private Tracker _tracker = null;

        #endregion


        #region Properties

        public Transform CachedTransform { get; private set; }

        #endregion


        #region Unity events

        private void Awake()
        {
            AwakeInitialize();
        }

        private void OnDestroy()
        {
            _tracker.UnsubscribeFromTarget();
        }

        private void LateUpdate()
        {
            HandleMovement();
        }

        #endregion


        #region Public methods

        public void TrackTarget(ITrackable target)
        {
            _tracker.SetTarget(target);
            _tracker.SubscribeProcess(GetDesiredPosition);
        }

        #endregion


        #region Private methods

        private void HandleMovement()
        {
            var direction = _desiredPosition - CachedTransform.position;
            var sqrDistance = Vector3.SqrMagnitude(direction);

            if (sqrDistance > float.Epsilon)
            {
                if (sqrDistance > maxDistance * maxDistance)
                {
                    CachedTransform.position = _desiredPosition - direction.normalized * maxDistance;
                }

                MoveTowards(_desiredPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                CachedTransform.position = _desiredPosition;
            }
        }

        private void AwakeInitialize()
        {
            CachedTransform = transform;
            _tracker = new Tracker();
        }

        private void GetDesiredPosition(Vector3 position)
        {
            _desiredPosition = position + _offset;
        }

        #endregion

        public void MoveTowards(Vector3 position, float speed)
        {
            CachedTransform.position = Vector3.Lerp(CachedTransform.position, position, speed);
        }

        #if UNITY_EDITOR	
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(_desiredPosition, 1f);
        }
        #endif
    }
}