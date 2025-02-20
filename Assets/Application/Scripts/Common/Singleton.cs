﻿using UnityEngine;

namespace LabyrinthGame.Common
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Private data

        private static bool _shuttingDown = false;
        private static object _lock = new object();
        private static T _instance;

        #endregion


        #region Properties

        public static T Instance
        {
            get
            {
                if (_shuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Search for existing instance.
                        _instance = (T) FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (_instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return _instance;
                }
            }
        }

        #endregion


        #region Unity events

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        protected virtual void OnDestroy()
        {
            _shuttingDown = true;
        }

        #endregion
    }
}