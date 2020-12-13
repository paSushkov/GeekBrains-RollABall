using UnityEngine;


namespace LabirinthGame.Common
{
    public sealed class Rotator : MonoBehaviour
    {
        #region PrivateData

        private Transform _transform;

        #endregion


        #region Fields

        public float xSpeed = 1f;
        public float ySpeed = 1f;
        public float zSpeed = 1f;
        public bool isUsingGlobal;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _transform = transform;
        }

        private void FixedUpdate()
        {
            if (isUsingGlobal)
            {
                var rotation = _transform.rotation;
                rotation *= Quaternion.AngleAxis(xSpeed*Time.deltaTime, Vector3.right);
                rotation *= Quaternion.AngleAxis(ySpeed*Time.deltaTime, Vector3.up);
                rotation *= Quaternion.AngleAxis(zSpeed*Time.deltaTime, Vector3.forward);
                transform.rotation = rotation;
            }
            else
            {
                var rotation = _transform.rotation;
                rotation *= Quaternion.AngleAxis(xSpeed*Time.deltaTime, _transform.right);
                rotation *= Quaternion.AngleAxis(ySpeed*Time.deltaTime, _transform.up);
                rotation *= Quaternion.AngleAxis(zSpeed*Time.deltaTime, _transform.forward);
                transform.rotation = rotation;
            }
            
        }

        #endregion
    }
}