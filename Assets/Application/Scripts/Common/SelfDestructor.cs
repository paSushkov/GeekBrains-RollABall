using UnityEngine;


namespace LabirinthGame.Common
{
    public sealed  class SelfDestructor : MonoBehaviour
    {
        #region Fields

        public float lifeTime = 3f;
        public bool shouldDestroyOnAwake = true;
        public bool shouldDestroyOnStart = false;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            if (shouldDestroyOnAwake)
                Destroy(gameObject, lifeTime);
        }

        private void Start()
        {
            if (shouldDestroyOnStart)
                Destroy(gameObject, lifeTime);
        }

        #endregion
    }
}